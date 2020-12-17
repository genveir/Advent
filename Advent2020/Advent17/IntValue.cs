using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public interface AdjacencyCoordinateType<CoordinateType>
        where CoordinateType : AdjacencyCoordinateType<CoordinateType>
    {
        CoordinateType Copy();
    }

    public interface ValueWithAdjacentValues<CoordType>
    {
        public IEnumerable<CoordType> GetAdjacentValues();
    }

    public class IntValue<CoordType> : ValueWithAdjacentValues<CoordType>, IComparable<IntValue<CoordType>> where CoordType : IntValue<CoordType>
    {
        private static Dictionary<int, CoordType> values = new Dictionary<int, CoordType>();

        public static CoordType Get(int val)
        {
            CoordType coord;
            if (!values.TryGetValue(val, out coord))
            {
                coord = (CoordType)Activator.CreateInstance(typeof(CoordType));
                coord.intValue = val;
                values.Add(val, coord);
            }
            return coord;
        }

        public int intValue;

        public IEnumerable<CoordType> GetAdjacentValues()
        {
            var result = new List<CoordType>();
            result.Add(Get(this.intValue - 1));
            result.Add(Get(this.intValue + 1));
            return result;
        }

        public override string ToString()
        {
            return intValue.ToString();
        }

        public int CompareTo(IntValue<CoordType> other)
        {
            return intValue.CompareTo(other.intValue);
        }
    }
}
