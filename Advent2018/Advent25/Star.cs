using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent25
{
    class Star
    {
        public int X;
        public int Y;
        public int Z;
        public int T;

        public int DistanceTo(Star star)
        {
            return
                Math.Abs(X - star.X) +
                Math.Abs(Y - star.Y) +
                Math.Abs(Z - star.Z) +
                Math.Abs(T - star.T);
        }
    }
}
