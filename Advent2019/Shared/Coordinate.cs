using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Shared
{
    public class Coordinate
    {
        public Coordinate(long x, long y, long? z = null)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public long X;
        public long Y;
        public long? Z;

        public long IntegerDistance(Coordinate second) { return (long)Distance(second); }
        public double Distance(Coordinate second)
        {
            return Distance(second.X, second.Y, second.Z);
        }

        public long IntegerDistance(long x, long y, long z) { return (long)Distance(x, y, z); }
        public double Distance(long x, long y, long? z)
        {
            long zVal = z.HasValue ? z.Value : 0;
            long ZVal = Z.HasValue ? Z.Value : 0;
            var squared = Math.Abs(X - x) + Math.Abs(Y - y) + Math.Abs(ZVal - zVal);

            return Math.Sqrt(squared);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y +
                (Z.HasValue ? (", " + Z.Value) : "") +
                ")";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + (Z ?? 0).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coordinate;
            if (other == null) return false;
            return other.X == X && other.Y == Y && other.Z == Z;
        }
    }

}
