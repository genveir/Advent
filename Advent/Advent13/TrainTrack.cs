using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent13
{
    abstract class TrainTrack
    {
        public XYCoord coord;
        protected Dictionary<Direction, TrainTrack> Neighbours;

        public TrainTrack(XYCoord coord, TrainTrack north, TrainTrack west)
        {
            this.coord = coord;
            Neighbours = new Dictionary<Direction, TrainTrack>();

            if (north != null) Link(north, Direction.North, true);
            if (west != null) Link(west, Direction.West, true);
        }

        public void Link(TrainTrack track, Direction direction, bool backLink)
        {
            Neighbours[direction] = track;
            if (backLink) track.Link(this, direction.Opposite(), false);
        }

        public virtual void Move(Cart cart)
        {
            cart.track = Neighbours[cart.Facing];
        }
    }

    class NorthSouth : TrainTrack
    {
        public NorthSouth(XYCoord coord, TrainTrack north) : base(coord, north, null) { }
    }

    class EastWest : TrainTrack
    {
        public EastWest(XYCoord coord, TrainTrack west) : base(coord, null, west) { }
    }

    class Intersection : TrainTrack
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

        public Corner(XYCoord coord, TrainTrack north, TrainTrack west) : base(coord, north, west)
        {
            if (west is EastWest || west is Intersection) { Link(west, Direction.West, true); HasWest = true; }
            if (north is NorthSouth || north is Intersection) { Link(north, Direction.North, true); HasNorth = true; }
        }

        public override void Move(Cart cart)
        {
            if (cart.Facing == Direction.North || cart.Facing == Direction.South)
            {
                if (HasWest) cart.Facing = Direction.West;
                else cart.Facing = Direction.East;
            }
            else
            {
                if (HasNorth) cart.Facing = Direction.North;
                else cart.Facing = Direction.South;
            }

            base.Move(cart);
        }
    }
}
