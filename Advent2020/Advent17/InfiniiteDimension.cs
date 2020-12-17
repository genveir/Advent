using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public class InfiniteDimension<ElementType, CoordType, CoordinateType> : InfiniteGrid<ElementType, CoordinateType>
    {
        protected Dictionary<CoordType, InfiniteGrid<ElementType, CoordinateType>> nextLayers = new Dictionary<CoordType, InfiniteGrid<ElementType, CoordinateType>>();

        protected Func<CoordinateType, CoordType> getCoordFunc;
        protected Func<InfiniteGrid<ElementType, CoordinateType>> createLayerFunc;

        public InfiniteDimension(Func<CoordinateType, CoordType> getCoordFunc, Func<InfiniteGrid<ElementType, CoordinateType>> createLayerFunc)
        {
            this.getCoordFunc = getCoordFunc;
            this.createLayerFunc = createLayerFunc;
        }

        public ElementType Get(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            InfiniteGrid<ElementType, CoordinateType> nextLayer;
            if (!nextLayers.TryGetValue(coord, out nextLayer)) return default(ElementType);
            else return nextLayer.Get(cs);
        }

        public void Set(CoordinateType cs, ElementType value)
        {
            var coord = getCoordFunc(cs);

            InfiniteGrid<ElementType, CoordinateType> nextLayer;
            if (!nextLayers.TryGetValue(coord, out nextLayer))
            {
                nextLayer = createLayerFunc();
                nextLayers.Add(coord, nextLayer);
            }

            nextLayer.Set(cs, value);
        }

        public void Update(CoordinateType cs, Func<ElementType, ElementType> updateFunc)
        {
            var coord = getCoordFunc(cs);

            InfiniteGrid<ElementType, CoordinateType> nextLayer;
            if (!nextLayers.TryGetValue(coord, out nextLayer))
            {
                nextLayer = createLayerFunc();
                nextLayers.Add(coord, nextLayer);
            }

            nextLayer.Update(cs, updateFunc);
        }

        public (bool isNowEmpty, ElementType element) Remove(CoordinateType cs)
        {
            var coord = getCoordFunc(cs);

            InfiniteGrid<ElementType, CoordinateType> nextLayer;
            if (!nextLayers.TryGetValue(coord, out nextLayer)) return (false, default(ElementType));
            else
            {
                var (nextLayerIsEmpty, result) = nextLayer.Remove(cs);
                if (nextLayerIsEmpty) nextLayers.Remove(coord);

                return (nextLayers.Count == 0, result);
            }
        }

        public void Clear()
        {
            nextLayers.Clear();
        }

        public long Count()
        {
            return nextLayers.Values.Select(v => v.Count()).Sum();
        }
    }

    public class InfiniteAdjacencyDimension<ElementType, CoordType, CoordinateType> : 
        InfiniteDimension<ElementType, CoordType, CoordinateType>,
        InfiniteAdjacencyGrid<ElementType, CoordinateType>
        where CoordType : ValueWithAdjacentValues<CoordType>
        where CoordinateType : AdjacencyCoordinateType<CoordinateType>
    {
        private Action<CoordinateType, CoordType> updateCoordFunc;

        public InfiniteAdjacencyDimension(
            Func<CoordinateType, CoordType> getCoordFunc, 
            Action<CoordinateType, CoordType> updateCoordFunc,
            Func<InfiniteAdjacencyGrid<ElementType, CoordinateType>> createLayerFunc)
            : base(getCoordFunc, createLayerFunc)
        {
            this.updateCoordFunc = updateCoordFunc;
        }

        public static void BustCache()
        {
            _adjacentPositions = new Dictionary<CoordinateType, IEnumerable<CoordinateType>>();
            _localNeighbours = new Dictionary<CoordinateType, List<CoordinateType>>();
        }

        public IEnumerable<ElementType> GetAdjacentMembers(CoordinateType cs)
        {
            var localNeighbours = _GetLocalNeighbours(cs);
            localNeighbours.Add(cs);

            var results = new List<ElementType>();

            foreach (var ln in localNeighbours)
            {
                var lnCoord = getCoordFunc(ln);

                InfiniteGrid<ElementType, CoordinateType> nextLayer;
                if (nextLayers.TryGetValue(lnCoord, out nextLayer))
                {
                    // safe cast since next layers are built with the createLayerFunc from the constructor
                    var adjNextLayer = nextLayer as InfiniteAdjacencyGrid<ElementType, CoordinateType>;
                    results.AddRange(adjNextLayer.GetAdjacentMembers(ln));
                }
            }

            return results;
        }

        private static Dictionary<CoordinateType, IEnumerable<CoordinateType>> _adjacentPositions = new Dictionary<CoordinateType, IEnumerable<CoordinateType>>();
        public IEnumerable<CoordinateType> GetAdjacentPositions(CoordinateType cs)
        {
            IEnumerable<CoordinateType> adjacentPositions;
            if (!_adjacentPositions.TryGetValue(cs, out adjacentPositions))
            {
                var localNeighbours = _GetLocalNeighbours(cs);
                localNeighbours.Add(cs);

                var results = new List<CoordinateType>();

                foreach (var ln in localNeighbours)
                {
                    var lnCoord = getCoordFunc(ln);

                    InfiniteGrid<ElementType, CoordinateType> nextLayer;
                    if (!nextLayers.TryGetValue(lnCoord, out nextLayer))
                    {
                        nextLayer = createLayerFunc();
                        nextLayers.Add(lnCoord, nextLayer);
                    }

                    // safe cast since next layers are built with the createLayerFunc from the constructor
                    var adjNextLayer = nextLayer as InfiniteAdjacencyGrid<ElementType, CoordinateType>;
                    results.AddRange(adjNextLayer.GetAdjacentPositions(ln));
                }

                adjacentPositions = results;
                _adjacentPositions.Add(cs, adjacentPositions);
            }

            return adjacentPositions;
        }

        private static Dictionary<CoordinateType, List<CoordinateType>> _localNeighbours = new Dictionary<CoordinateType, List<CoordinateType>>();
        private List<CoordinateType> _GetLocalNeighbours(CoordinateType cs)
        {
            List<CoordinateType> localNeighbours;
            if (!_localNeighbours.TryGetValue(cs, out localNeighbours))
            {
                var coord = getCoordFunc(cs);

                localNeighbours = new List<CoordinateType>();

                var coordNeighbours = coord.GetAdjacentValues();
                foreach (var coordNeighbour in coordNeighbours)
                {
                    var csCopy = cs.Copy();
                    updateCoordFunc(csCopy, coordNeighbour);

                    localNeighbours.Add(csCopy);
                }

                _localNeighbours.Add(cs, localNeighbours);
            }

            return localNeighbours;
        }

        public override string ToString()
        {
            return $"Dimension of {typeof(CoordType).Name}";
        }
    }
}
