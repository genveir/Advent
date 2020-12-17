using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public class _4DGridPosition : AdjacencyCoordinateType<_4DGridPosition>
    {
        public _4DGridPosition(X_Value x, Y_Value y, Z_Value z, W_Value w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public W_Value W;
        public X_Value X;
        public Y_Value Y;
        public Z_Value Z;

        public _4DGridPosition Copy()
        {
            return new _4DGridPosition(X, Y, Z, W);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z}, {W})";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as _4DGridPosition;
            return other.X == this.X &&
                other.Y == this.Y &&
                other.Z == this.Z &&
                other.W == this.W;
        }
    }

    public class W_Value : IntValue<W_Value> { }
    public class X_Value : IntValue<X_Value> { }
    public class Y_Value : IntValue<Y_Value> { }
    public class Z_Value : IntValue<Z_Value> { }
}
