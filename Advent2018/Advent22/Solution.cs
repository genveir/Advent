using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018.Advent22
{
    class Solution : ISolution
    {
        public int depth;
        public (int x, int y) target;
        public TileCollection Tiles;

        public Solution(int? depth = null, (int x, int y)? target = null)
        {
            this.depth = depth ?? 10647;
            this.target = target ?? (7, 770);

            Tiles = new TileCollection(this.depth, this.target);
            Tiles.GetTile(0, 0);
        }

        public int GetTotalRisk()
        {
            var totalRisk = 0;
            for (int y = 0; y <= target.y; y++)
            {
                for (int x = 0; x <= target.x; x++)
                {
                    Tile tile = Tiles.GetTile(x, y);

                    totalRisk += tile.Type;
                }
            }

            return totalRisk;
        }
        
        public int FindFastest()
        {
            var expanded = new ConcurrentDictionary<SearchNode, int>();

            var baseTile = Tiles.GetTile(0, 0);
            var baseNode = new SearchNode(expanded, 0, baseTile, Tool.Torch);

            var dequeuesize = 100;
            var pQueue = new Advent22PriorityQueue(baseNode.Cost, dequeuesize);
            pQueue.Enqueue(baseNode);
            while (pQueue.Count > 0)
            {
                List<SearchNode> atCost = new List<SearchNode>();

                (var nodes, var num) = pQueue.DequeueLowestCost();

                for (int n = 0; n < num; n++)
                {
                    if (nodes[n].tile.coord.Equals(target) && nodes[n].tool == Tool.Torch)
                    {
                        return nodes[n].time;
                    }

                    atCost.Add(nodes[n]);
                }

                if (atCost.Count > 0)
                {
                    Parallel.ForEach(atCost, node => node.Expand());

                    pQueue.Enqueue(atCost.SelectMany(ap => ap.ExpandResult));
                }
            }

            return -1;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetTotalRisk());
            Console.WriteLine("part2: " + FindFastest());
        }
    }
}
