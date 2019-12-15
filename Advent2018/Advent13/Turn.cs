using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent13
{
    interface ITurn
    {
        void MakeTurn(Cart cart);
    }

    class TurnLeft : ITurn
    {
        private static TurnLeft _singleton;
        public static TurnLeft Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new TurnLeft();
                return _singleton;
            }
        }

        public void MakeTurn(Cart cart)
        {
            if (cart.Facing == Direction.North) cart.Facing = Direction.West;
            else if (cart.Facing == Direction.East) cart.Facing = Direction.North;
            else if (cart.Facing == Direction.South) cart.Facing = Direction.East;
            else cart.Facing = Direction.South;

            cart.TurnDirection = Next();
        }

        public ITurn Next() { return StraightOn.Singleton; }
    }

    class StraightOn : ITurn
    {
        private static StraightOn _singleton;
        public static StraightOn Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new StraightOn();
                return _singleton;
            }
        }

        public void MakeTurn(Cart cart)
        {
            cart.TurnDirection = Next();
        }

        public ITurn Next() { return TurnRight.Singleton; }
    }

    class TurnRight : ITurn
    {
        private static TurnRight _singleton;
        public static TurnRight Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new TurnRight();
                return _singleton;
            }
        }

        public void MakeTurn(Cart cart)
        {
            if (cart.Facing == Direction.North) cart.Facing = Direction.East;
            else if (cart.Facing == Direction.East) cart.Facing = Direction.South;
            else if (cart.Facing == Direction.South) cart.Facing = Direction.West;
            else cart.Facing = Direction.North;

            cart.TurnDirection = Next();
        }

        public ITurn Next() { return TurnLeft.Singleton; }
    }
}
