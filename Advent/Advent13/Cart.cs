using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent13
{
    class Cart
    {
        static HashSet<XYCoord> cartCoords;

        static Cart() { cartCoords = new HashSet<XYCoord>(); }

        public static void RemoveFrom(XYCoord coord)
        {
            cartCoords.Remove(coord);
        }

        public TrainTrack track;
        public Direction Facing;
        public ITurn TurnDirection;

        public Cart(XYCoord start, Direction facing)
        {
            cartCoords.Add(start);
            this.Facing = facing;
            TurnDirection = TurnLeft.Singleton;
        }

        public bool Move()
        {
            cartCoords.Remove(track.coord);
            track.Move(this);
            if (cartCoords.Contains(track.coord)) return true;

            cartCoords.Add(track.coord);
            return false;
        }

        public void MakeTurn()
        {
            TurnDirection.MakeTurn(this);
        }

        public override string ToString()
        {
            char charRep = 'X';
            switch (Facing)
            {
                case Direction.North: charRep = '^'; break;
                case Direction.East: charRep = '>'; break;
                case Direction.South: charRep = 'v'; break;
                case Direction.West: charRep = '<'; break;
            }

            return charRep + " " + track.coord + " " + TurnDirection.GetType().Name;
        }
    }
}
