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
                case '\\': return new Corner(coord, trackWest, trackNorth);
                case '/': return new Corner(coord, trackWest, trackNorth);
                case '-': return new EastWest(coord, trackWest);
                case '+': return new Intersection(coord, trackWest, trackNorth);
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
        }

        public TrainTrack North;
        public TrainTrack West;
        public TrainTrack South;
        public TrainTrack East;

        public void LinkNorth(TrainTrack track, bool backLink) { North = track; if (backLink) track.LinkSouth(this, false); }
        public void LinkEast(TrainTrack track, bool backLink) { East = track; if (backLink) track.LinkWest(this, false); }
        public void LinkSouth(TrainTrack track, bool backLink) { South = track; if (backLink) track.LinkNorth(this, false); }
        public void LinkWest(TrainTrack track, bool backLink) { West = track; if (backLink) track.LinkEast(this, false); }

        public abstract void Move(Cart cart);
    }

    class NorthSouth : TrainTrack
    {
        public NorthSouth(XYCoord coord, TrainTrack north) : base(coord) { LinkNorth(north, true); }

        public override void Move(Cart cart)
        {
            switch (cart.Facing)
            {
                case Direction.North: cart.track = North; break;
                case Direction.South: cart.track = South; break;
                default: throw new Exception("kannie");
            }
        }
    }

    class Corner : TrainTrack
    {
        private bool HasWest;
        private bool HasNorth;

        public Corner(XYCoord coord, TrainTrack west, TrainTrack north) : base(coord)
        {
            if (west is EastWest || west is Intersection) { LinkWest(west, true); HasWest = true; }
            if (north is NorthSouth || north is Intersection) { LinkNorth(north, true); HasNorth = true; }
        }

        public override void Move(Cart cart)
        {
            switch (cart.Facing)
            {
                case Direction.North:
                case Direction.South:
                    if (HasWest) { cart.track = West; cart.Facing = Direction.West; }
                    else { cart.track = East; cart.Facing = Direction.East; }
                    break;
                case Direction.East:
                case Direction.West:
                    if (HasNorth) { cart.track = North; cart.Facing = Direction.North; }
                    else { cart.track = South; cart.Facing = Direction.South; }
                    break;
                default: throw new Exception("kannie");
            }
        }
    }

    class EastWest : TrainTrack
    {
        public EastWest(XYCoord coord, TrainTrack west) : base(coord) { LinkWest(west, true); }

        public override void Move(Cart cart)
        {
            switch (cart.Facing)
            {
                case Direction.East: cart.track = East; break;
                case Direction.West: cart.track = West; break;
                default: throw new Exception("kannie");
            }
        }
    }

    class Intersection : TrainTrack
    {
        public Intersection(XYCoord coord, TrainTrack west, TrainTrack north) : base(coord) { LinkWest(west, true); LinkNorth(north, true); }

        public override void Move(Cart cart)
        {
            cart.MakeTurn();

            switch (cart.Facing)
            {
                case Direction.North: cart.track = North; break;
                case Direction.East: cart.track = East; break;
                case Direction.South: cart.track = South; break;
                case Direction.West: cart.track = West; break;
                default: throw new Exception("kannie");
            }
        }
    }
}
