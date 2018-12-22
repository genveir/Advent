using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Advent22
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
            var explored = new ConcurrentDictionary<SearchNode, byte>();

            var baseTile = Tiles.GetTile(0, 0);
            var baseNode = new SearchNode(explored, 0, baseTile, Tool.Torch);

            var minDist = int.MaxValue - 1000;

            var pQueue = new Advent22PriorityQueue(baseNode.Priority);
            pQueue.Enqueue(baseNode);
            while (pQueue.Count > 0)
            {
                List<SearchNode> atPrio = new List<SearchNode>();

                (var nodes, var num) = pQueue.DequeueLowestPrio(100);

                for(int n = 0; n < num; n++)
                {
                    if (nodes[n].tile.coord.Equals(target) && nodes[n].tool == Tool.Torch)
                    {
                        return nodes[n].time;
                    }

                    // limit search to nodes that don't detour too far. 
                    // Search space is random, we'll be able to move in a fairly straight line
                    if (nodes[n].tile.HeuristicDistance > minDist + 75) continue;
                    atPrio.Add(nodes[n]);
                }

                var currentPrio = nodes[0].Priority;
                //Console.WriteLine("dequeued " + atPrio.Count + " at prio: " + currentPrio + " min dist: " + minDist);

                if (atPrio.Count > 0)
                {
                    var batchMin = atPrio.Min(sn => sn.tile.HeuristicDistance);
                    if (batchMin < minDist) minDist = batchMin;

                    Parallel.ForEach(atPrio, node => node.Explore());

                    pQueue.Enqueue(atPrio.SelectMany(ap => ap.ExploreResult));
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
