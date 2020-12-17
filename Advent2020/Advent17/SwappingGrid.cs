using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent17
{
    public class SwappingGrid
    {
        public SwappingGrid()
        {
            Front = new _Front();
        }

        private int activeGrid = 0;

        private InfiniteAdjacencyGrid<int, _4DGridPosition>[] grids = new InfiniteAdjacencyGrid<int, _4DGridPosition>[2];
        public InfiniteAdjacencyGrid<int, _4DGridPosition> Current
        {
            get { return grids[activeGrid]; }
            set { grids[activeGrid] = value; }
        }
        public InfiniteAdjacencyGrid<int, _4DGridPosition> Next
        {
            get { return grids[1 - activeGrid]; }
        }

        public _Front Front;

        public class _Front
        {
            private HashSet<_4DGridPosition>[] fronts = new HashSet<_4DGridPosition>[2];

            public int activeFront;

            public _Front()
            {
                for (int n = 0; n < 2; n++)
                {
                    fronts[n] = new HashSet<_4DGridPosition>();
                }
            }

            public void Add(_4DGridPosition pos)
            {
                fronts[1 - activeFront].Add(pos);
            }

            public IEnumerable<_4DGridPosition> Get()
            {
                return fronts[activeFront];
            }

            public void Swap()
            {
                activeFront = 1 - activeFront;
                fronts[1 - activeFront].Clear();
            }
        }

        public void Swap()
        {
            if (Current != null) Current.Clear();
            activeGrid = 1 - activeGrid;
            Front.Swap();
        }
    }
}
