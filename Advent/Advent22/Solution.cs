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
        public static int depth;
        public static (int x, int y) target;
        public static ConcurrentDictionary<(int x, int y), Tile> Tiles;

        public Solution(int? depth = null, (int x, int y)? target = null)
        {
            Solution.depth = depth ?? 10647;
            Solution.target = target ?? (7, 770);

            Tiles = new ConcurrentDictionary<(int x, int y), Tile>();
            GetTile(0, 0);
        }

        private static Tile GetTile(int x, int y)
        {
            if (!Tiles.ContainsKey((x, y)))
            {
                Tile tile;
                if (x < 0 || y < 0) tile = new Tile((x, y), -1);
                else if (x == target.x && y == target.y) tile = new Tile((x, y), 0);
                else if (y == 0) tile = new Tile((x, y), x * 16807);
                else if (x == 0) tile = new Tile((x, y), y * 48271);
                else
                {
                    var X = GetTile(x - 1, y).erosionLevel;
                    var Y = GetTile(x, y - 1).erosionLevel;
                    tile = new Tile((x, y), (X * Y));
                }

                Tiles.GetOrAdd((x, y), tile);
            }

            return Tiles[(x, y)];
        }

        public int GetTotalRisk()
        {
            var totalRisk = 0;
            for (int y = 0; y <= target.y; y++)
            {
                for (int x = 0; x <= target.x; x++)
                {
                    Tile tile = GetTile(x, y);

                    totalRisk += tile.Type;
                }
            }

            return totalRisk;
        }

        public class Tile
        {
            public (int x, int y) coord;

            public Tile((int x, int y) coord, int multiplyOfOthers)
            {
                this.coord = coord;
                this.erosionLevel = (multiplyOfOthers + depth) % 20183;
                if (multiplyOfOthers == -1) this.erosionLevel = -1;
                HeuristicDistance = Math.Abs(coord.x - target.x) + Math.Abs(coord.y - target.y);
            }

            public int erosionLevel;
            public int Type
            {
                get
                {
                    return erosionLevel % 3;
                }
            }

            public List<Tile> Neighbours
            {
                get
                {
                    var neighbours = new List<Tile>();
                    neighbours.Add(GetTile(coord.x - 1, coord.y));
                    neighbours.Add(GetTile(coord.x, coord.y - 1));
                    neighbours.Add(GetTile(coord.x + 1, coord.y));
                    neighbours.Add(GetTile(coord.x, coord.y + 1));

                    return neighbours;
                }
            }

            public int HeuristicDistance { get; set; }

            public override int GetHashCode()
            {
                return 1000 * coord.y + coord.x;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Tile;
                return coord.x == other.coord.x && coord.y == other.coord.y;
            }

            public override string ToString()
            {
                string typeString;
                switch (Type)
                {
                    case 0: typeString = "Rocky"; break;
                    case 1: typeString = "Wet"; break;
                    case 2: typeString = "Narrow"; break;
                    default: typeString = "Impassable"; break;
                }
                return string.Format("({0}, {1}): {2}", coord.x, coord.y, typeString);
            }
        }

        public enum Tool { None = 0, Torch = 1, Gear = 2 };

        private static ConcurrentDictionary<SearchNode, byte> explored;
        private class SearchNode : IComparable<SearchNode>
        {
            public int time;
            public Tile tile;
            public Tool tool;

            public SearchNode(int time, Tile tile, Tool tool)
            {
                this.time = time;
                this.tile = tile;
                this.tool = tool;
            }

            public int Priority
            {
                get
                {
                    return time + tile.HeuristicDistance;
                }
            }

            public int CompareTo(SearchNode other)
            {
                if (this.Priority != other.Priority) return Priority.CompareTo(other.Priority);
                else return tile.HeuristicDistance.CompareTo(other.tile.HeuristicDistance);
            }

            public void Explore()
            {
                var neighbours = tile.Neighbours;

                foreach (var neighbour in neighbours)
                {
                    if (IsValidTool(neighbour, tool)) AddToResult(time + 1, neighbour, tool);
                }

                for (int n = 0; n < 3; n++)
                {
                    if (IsValidTool(tile, (Tool)n)) AddToResult(time + 7, tile, (Tool)n);
                }
            }

            public List<SearchNode> ExploreResult = new List<SearchNode>();

            private bool IsValidTool(Tile tile, Tool tool)
            {
                var type = tile.Type;
                switch (type)
                {
                    case 0: return tool != Tool.None;
                    case 1: return tool != Tool.Torch;
                    case 2: return tool != Tool.Gear;
                    default: return false;
                }
            }

            private void AddToResult(int time, Tile tile, Tool tool)
            {
                var node = new SearchNode(time, tile, tool);
                if (node.Priority < Priority) return;

                if (explored.TryAdd(node, 0))
                {
                    ExploreResult.Add(node);
                }
            }

            int hashCompares;
            public override int GetHashCode()
            {
                var tileHash = tile.GetHashCode() * 179;
                var toolHash = (int)(tool + 1) * 997300;
                var timeHash = time * 46370000;
                return tileHash + toolHash + timeHash;
            }

            int equalityCompares;
            public override bool Equals(object obj)
            {
                var other = obj as SearchNode;
                var areEqual = tile.Equals(other.tile) && tool == other.tool && time.Equals(other.time);

                return areEqual;
            }

            public override string ToString()
            {
                return string.Format("{0} / {3}: {1}, {2}", time, tile, tool.ToString(), Priority);
            }
        }

        public static Solution current;
        public int FindFastest()
        {
            explored = new ConcurrentDictionary<SearchNode, byte>();
            var pQueue = new PriorityQueue<SearchNode>();
            var nodes = new SearchNode[8];

            var baseTile = GetTile(0, 0);
            var baseNode = new SearchNode(0, baseTile, Tool.Torch);

            var minDist = int.MaxValue - 1000;

            pQueue.Enqueue(baseNode);
            while (pQueue.Count() > 0)
            {
                var currentPrio = pQueue.Peek().Priority;
                
                List<SearchNode> atPrio = new List<SearchNode>();
                int prioCount = 0;
                while (pQueue.Count() > 0 && pQueue.Peek().Priority == currentPrio && prioCount < 1000)
                { 
                    var node = pQueue.Dequeue();

                    if (node.tile.coord.Equals(target) && node.tool == Tool.Torch)
                    {
                        return node.time;
                    }

                    if (node.tile.HeuristicDistance > minDist + 75) continue;
                    atPrio.Add(node);
                    prioCount++;
                }
                var batchMin = atPrio.Min(sn => sn.tile.HeuristicDistance);
                if (batchMin < minDist) minDist = batchMin;

                //Console.WriteLine("dequeued " + atPrio.Count + " at prio: " + currentPrio + " min dist: " + minDist);

                Parallel.ForEach(atPrio, node => node.Explore());

                foreach (var node in atPrio)
                {
                    var newNodes = node.ExploreResult;
                    foreach (var newNode in newNodes)
                    {
                        pQueue.Enqueue(newNode);
                    }
                }
            }

            return -1;
        }

        public void WriteResult()
        {
            current = this;

            GetTotalRisk();
            Console.WriteLine("part1: " + GetTotalRisk());
            Console.WriteLine("part2: " + FindFastest());
        }
    }
}
