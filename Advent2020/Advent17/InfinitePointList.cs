using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public class InfinitePointList<ElementType, CoordType, CoordinateType> : InfiniteGrid<ElementType, CoordinateType>
    {
        protected Dictionary<CoordType, ElementType> points = new Dictionary<CoordType, ElementType>();

        protected Func<CoordinateType, CoordType> getCoordFunc;

        public InfinitePointList(Func<CoordinateType, CoordType> getCoordFunc)
        {
            this.getCoordFunc = getCoordFunc;
        }

        public ElementType Get(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            return _Get(coord);
        }

        protected ElementType _Get(CoordType coord)
        {
            ElementType element;
            if (!points.TryGetValue(coord, out element)) return default(ElementType);
            else return element;
        }

        public void Set(CoordinateType cs, ElementType value)
        {
            var coord = getCoordFunc(cs);

            points[coord] = value;
        }

        public void Update(CoordinateType cs, Func<ElementType, ElementType> updateFunc)
        {
            var currentValue = Get(cs);

            Set(cs, updateFunc(currentValue));
        }

        public (bool isNowEmpty, ElementType element) Remove(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            ElementType result;
            points.Remove(coord, out result);

            return (points.Count == 0, result);
        }

        public void Clear()
        {
            points.Clear();
        }

        public long Count()
        {
            return points.Count;
        }
    }

    public class InfiniteAdjacencyPointList<ElementType, CoordType, CoordinateType> :
        InfinitePointList<ElementType, CoordType, CoordinateType>, 
        InfiniteAdjacencyGrid<ElementType, CoordinateType>
        where CoordType : ValueWithAdjacentValues<CoordType>
        where CoordinateType : AdjacencyCoordinateType<CoordinateType>
    {
        private Action<CoordinateType, CoordType> updateCoordFunc;

        public InfiniteAdjacencyPointList(
            Func<CoordinateType, CoordType> getCoordFunc,
            Action<CoordinateType, CoordType> updateCoordFunc) : base(getCoordFunc)
        {
            this.updateCoordFunc = updateCoordFunc;
        }

        public IEnumerable<ElementType> GetAdjacentMembers(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            var adjacentCoords = coord.GetAdjacentValues();

            return adjacentCoords.Select(c => _Get(c));
        }

        public IEnumerable<CoordinateType> GetAdjacentPositions(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            var result = new List<CoordinateType>() { cs };

            var adjacentCoords = coord.GetAdjacentValues();
            foreach(var adjacentCoord in adjacentCoords)
            {
                var csCopy = cs.Copy();
                updateCoordFunc(csCopy, adjacentCoord);
                result.Add(csCopy);
            }

            return result;
        }
    }
}
