using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent13
{
    interface ITurn
    {
        void MakeTurn(Cart cart);
    }

    class Left : ITurn
    {
        private static Left _singleton;
        public static Left Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new Left();
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

        public ITurn Next() { return Straight.Singleton; }
    }

    class Straight : ITurn
    {
        private static Straight _singleton;
        public static Straight Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new Straight();
                return _singleton;
            }
        }

        public void MakeTurn(Cart cart)
        {
            cart.TurnDirection = Next();
        }

        public ITurn Next() { return Right.Singleton; }
    }

    class Right : ITurn
    {
        private static Right _singleton;
        public static Right Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new Right();
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

        public ITurn Next() { return Left.Singleton; }
    }
}
