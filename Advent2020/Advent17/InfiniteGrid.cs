using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public interface InfiniteAdjacencyGrid<ElementType, CoordinateType> : 
        InfiniteGrid<ElementType, CoordinateType>
    {
        IEnumerable<ElementType> GetAdjacentMembers(CoordinateType cs);

        IEnumerable<CoordinateType> GetAdjacentPositions(CoordinateType cs);
    }

    public interface InfiniteGrid<ElementType, CoordinateType> : InfiniteGridElement<ElementType, CoordinateType>
    {

    }

    public interface InfiniteGridElement<ElementType, CoordinateType>
    {
        ElementType Get(CoordinateType cs);

        void Set(CoordinateType cs, ElementType value);

        void Update(CoordinateType cs, Func<ElementType, ElementType> updateFunc);

        (bool isNowEmpty, ElementType element) Remove(CoordinateType cs);

        void Clear();

        long Count();
    }
}
