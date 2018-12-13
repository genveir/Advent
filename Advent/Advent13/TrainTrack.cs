using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent13
{
    abstract class TrainTrack
    {
        static Dictionary<XYCoord, TrainTrack> AllPositions = new Dictionary<XYCoord, TrainTrack>();

        public XYCoord coord;

        public static TrainTrack Parse(XYCoord coord, char input)
        {
            TrainTrack trackNorth;
            TrainTrack trackWest;
            AllPositions.TryGetValue(new XYCoord(coord.X, coord.Y - 1), out trackNorth);
            AllPositions.TryGetValue(new XYCoord(coord.X - 1, coord.Y), out trackWest);

            switch (input)
            {
                case '|': return new NorthSouth(coord, trackNorth);
                case '\\': return new Corner(coord, trackNorth, trackWest);
                case '/': return new Corner(coord, trackNorth, trackWest);
                case '-': return new EastWest(coord, trackWest);
                case '+': return new Intersection(coord, trackNorth, trackWest);
                default: throw new InvalidCastException("case " + input + " vergeten");
            }
        }

        public static void LinkCarts(IEnumerable<Cart> carts)
        {
            foreach (var cart in carts)
            {
                cart.track = AllPositions[cart.start];
            }
        }

        public TrainTrack(XYCoord coord)
        {
            this.coord = coord;
            AllPositions.Add(coord, this);
            Neighbours = new Dictionary<Direction, TrainTrack>();
        }

        protected Dictionary<Direction, TrainTrack> Neighbours;

        public void Link(TrainTrack track, Direction direction, bool backLink)
        {
            Neighbours[direction] = track;
            if (backLink) track.Link(this, direction.Opposite(), false);
        }

        public abstract void Move(Cart cart);
    }

    class Straight : TrainTrack
    {
        public Straight(XYCoord coord, TrainTrack north, TrainTrack west) : base(coord)
        {
            if (north != null) Link(north, Direction.North, true);
            if (west != null) Link(west, Direction.West, true);
        }

        public override void Move(Cart cart)
        {
            cart.track = Neighbours[cart.Facing];
        }
    }

    class NorthSouth : Straight
    {
        public NorthSouth(XYCoord coord, TrainTrack north) : base(coord, north, null) { }
    }

    class EastWest : Straight
    {
        public EastWest(XYCoord coord, TrainTrack west) : base(coord, null, west) { }
    }

    class Intersection : Straight
    {
        public Intersection(XYCoord coord, TrainTrack north, TrainTrack west) : base(coord, north, west) { }

        public override void Move(Cart cart)
        {
            cart.MakeTurn();

            base.Move(cart);
        }
    }

    class Corner : TrainTrack
    {
        private bool HasWest;
        private bool HasNorth;

        public Corner(XYCoord coord, TrainTrack north, TrainTrack west) : base(coord)
        {
            if (west is EastWest || west is Intersection) { Link(west, Direction.West, true); HasWest = true; }
            if (north is NorthSouth || north is Intersection) { Link(north, Direction.North, true); HasNorth = true; }
        }

        public override void Move(Cart cart)
        {
            switch (cart.Facing)
            {
                case Direction.North:
                case Direction.South:
                    if (HasWest) { cart.track = Neighbours[Direction.West]; cart.Facing = Direction.West; }
                    else { cart.track = Neighbours[Direction.East]; cart.Facing = Direction.East; }
                    break;
                case Direction.East:
                case Direction.West:
                    if (HasNorth) { cart.track = Neighbours[Direction.North]; cart.Facing = Direction.North; }
                    else { cart.track = Neighbours[Direction.South]; cart.Facing = Direction.South; }
                    break;
                default: throw new Exception("kannie");
            }
        }
    }
}
