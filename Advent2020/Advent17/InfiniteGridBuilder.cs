using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public class InfiniteAdjacencyGridBuilder<ElementType, CoordinateType> where CoordinateType : AdjacencyCoordinateType<CoordinateType>
    {
        protected List<Func<InfiniteAdjacencyGrid<ElementType, CoordinateType>>> LayerFactoryFunctions =
            new List<Func<InfiniteAdjacencyGrid<ElementType, CoordinateType>>>();

        public void AddLayer<CoordType>(Func<CoordinateType, CoordType> dimensionCoord, Action<CoordinateType, CoordType> updateFunc) 
            where CoordType : ValueWithAdjacentValues<CoordType>
        {
            if (LayerFactoryFunctions.Count == 0)
            {
                LayerFactoryFunctions.Add(() => new InfiniteAdjacencyPointList<ElementType, CoordType, CoordinateType>(dimensionCoord, updateFunc));
            }
            else
            {
                int index = LayerFactoryFunctions.Count - 1;

                Func<InfiniteAdjacencyGrid<ElementType, CoordinateType>> newLayer =
                    () => new InfiniteAdjacencyDimension<ElementType, CoordType, CoordinateType>(dimensionCoord, updateFunc, LayerFactoryFunctions[index]);

                InfiniteAdjacencyDimension<ElementType, CoordType, CoordinateType>.BustCache();

                LayerFactoryFunctions.Add(newLayer);
            }
        }

        public InfiniteAdjacencyGrid<ElementType, CoordinateType> Complete()
        {
            return LayerFactoryFunctions.Last()();
        }
    }

    public class InfiniteGridBuilder<ElementType, CoordinateType> 
    {
        protected Func<InfiniteGrid<ElementType, CoordinateType>> topLayer;

        public void AddLayer<CoordType>(Func<CoordinateType, CoordType> dimensionCoord)
        {
            if (topLayer == null)
            {
                topLayer = () => new InfinitePointList<ElementType, CoordType, CoordinateType>(dimensionCoord);
            }
            else
            {
                topLayer = () => new InfiniteDimension<ElementType, CoordType, CoordinateType>(dimensionCoord, topLayer);
            }
        }

        public InfiniteGrid<ElementType, CoordinateType> Complete()
        {
            return topLayer();
        }
    }
}
