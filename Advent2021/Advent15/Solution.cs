using Advent2021.Shared;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent15
{
    public class Solution : ISolution
    {
        public int[][] inputVals;

        public List<Tile> allTiles;
        public Tile start;
        public Coordinate target;

        public Solution(string input)
        {
            inputVals = Input.GetInputLines(input).Select(l => l.AsDigits()).ToArray();

            ParseMap(inputVals);
        }

        public void ParseMap(int[][] inputVals) 
        { 
            var tileMap = new Dictionary<Coordinate, Tile>();
            for (int y = 0; y < inputVals.Length; y++)
            {
                for (int x = 0; x < inputVals[0].Length; x++)
                {
                    var coord = new Coordinate(x, y);
                    var tile = new Tile(coord, inputVals[y][x]);

                    tileMap.Add(coord, tile);
                }
            }

            start = tileMap[new Coordinate(0, 0)];

            foreach (var kvp in tileMap)
            {
                kvp.Value.SetNeighbours(tileMap);
            }

            allTiles = tileMap.Select(kvp => kvp.Value).ToList();
            var targetX = allTiles.Max(t => t.coordinate.X);
            var targetY = allTiles.Max(t => t.coordinate.Y);
            target = new Coordinate(targetX, targetY);
        }
        public Solution() : this("Input.txt") { }

        public class Tile
        {
            public Coordinate coordinate;
            public long value;
            public bool explored;

            [ComplexParserConstructor]
            public Tile(Coordinate coordinate, long value)
            {
                this.coordinate = coordinate;

                this.value = value;
            }

            public List<Tile> neighbours = new List<Tile>();
            public void SetNeighbours(Dictionary<Coordinate, Tile> allTiles)
            {
                Tile above, right, below, left;

                var x = coordinate.X;
                var y = coordinate.Y;

                if (allTiles.TryGetValue(new Coordinate(x - 1, y), out left)) neighbours.Add(left);
                if (allTiles.TryGetValue(new Coordinate(x, y - 1), out above)) neighbours.Add(above);
                if (allTiles.TryGetValue(new Coordinate(x + 1, y), out right)) neighbours.Add(right);
                if (allTiles.TryGetValue(new Coordinate(x, y + 1), out below)) neighbours.Add(below);
            }
        }

        public class SearchNode1 : IComparable<SearchNode1>
        {
            public Tile Current;
            public Coordinate target;
            public long totalValue;

            public long HeuristicValue;

            public SearchNode1(Tile start, Coordinate target)
            {
                this.Current = start;
                this.totalValue = 0;
                this.target = target;
            }

            public SearchNode1(SearchNode1 parent, Tile newCurrent, Coordinate target)
            {
                this.Current = newCurrent;
                this.target = target;
                this.totalValue = parent.totalValue + newCurrent.value;

                this.HeuristicValue = totalValue + target.ManhattanDistance(Current.coordinate);
            }

            public IEnumerable<SearchNode1> Explore()
            {
                foreach (var neighbour in Current.neighbours.Where(n => !n.explored))
                {
                    neighbour.explored = true;
                    yield return new SearchNode1(this, neighbour, target);
                }
            }

            public int CompareTo(SearchNode1 obj)
            {
                return HeuristicValue.CompareTo(obj.HeuristicValue);
            }

            public bool isWin => Current.coordinate.Equals(target);
        }

        public SearchNode1 GetPath1()
        {
            var node = new SearchNode1(start, target);

            var queue = new PriorityQueue<SearchNode1>();
            queue.Add(node);

            while (queue.Count() > 0)
            {
                var popped = queue.Poll();

                if (popped.isWin) return popped;
                else
                {
                    var newNodes = popped.Explore();
                    foreach (var newNode in newNodes) queue.Add(newNode);
                }
            }

            return null;
        }

        public object GetResult1()
        {
            return GetPath1().totalValue;
        }

        public object GetResult2()
        {
            var rewrittenMap = RewriteMap(inputVals);

            ParseMap(rewrittenMap);

            return GetResult1();
        }

        public static int[][] RewriteMap(int[][] vals)
        {
            int[][] output;

            output = new int[vals.Length * 5][];
            for (int y = 0; y < vals.Length; y++)
            {
                output[y] = RewriteLine(vals, y);
            }

            for (int x = 0; x < output[0].Length; x++)
            {
                RewriteColumn(vals, output, x);
            }

            return output;
        }

        public static int[] RewriteLine(int[][] vals, int index)
        {
            int[] result = new int[vals[0].Length * 5];
            for (int it = 0; it < 5; it++)
            {
                for (int x = 0; x < vals[index].Length; x++)
                {
                    var val = vals[index][x] + it;
                    if (val > 9) val = val - 9;

                    result[it * vals[index].Length + x] = val;
                }
            }
            return result;
        }

        public static void RewriteColumn(int[][] vals, int[][] output, int x)
        {
            for (int it = 0; it < 5; it++)
            {
                for (int y = 0; y < vals.Length; y++)
                {
                    if (output[vals.Length * it + y] == null) output[vals.Length * it + y] = new int[vals[0].Length * 5];

                    var val = output[y][x] + it;
                    if (val > 9) val = val - 9;

                    output[vals.Length * it + y][x] = val;
                }
            }
        }
    }
}
