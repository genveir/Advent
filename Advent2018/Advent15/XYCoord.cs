using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent15
{
    class XYCoord : IComparable
    {
        public int X;
        public int Y;

        public XYCoord(int x, int y) { this.X = x; this.Y = y; }

        public override int GetHashCode() { return X + Y; }
        public override bool Equals(object obj)
        {
            var other = obj as XYCoord;
            return X == other.X && Y == other.Y;
        }

        public int CompareTo(object obj)
        {
            var other = obj as XYCoord;
            var yCompare = Y.CompareTo(other.Y);
            if (yCompare != 0) return yCompare;
            return X.CompareTo(other.X);
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}
