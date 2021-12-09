using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent09
{
    public class Solution : ISolution
    {
        List<Tile> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).Select(l => l.AsDigits()).ToArray();

            var inputParser = new InputParser<Tile>("line");

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    new Tile(x, y, lines[y][x]);
                }
            }

            foreach (var kvp in Tile.allTiles)
            {
                kvp.Value.SetNeighbours();
            }
        }
        public Solution() : this("Input.txt") { }

        public class Tile
        {
            public static Dictionary<Coordinate, Tile> allTiles = new Dictionary<Coordinate, Tile>();

            public Coordinate coordinate;
            public long value;

            [ComplexParserConstructor]
            public Tile(int x, int y, long value)
            {
                this.coordinate = new Coordinate(x, y);

                allTiles.Add(coordinate, this);

                this.value = value;
            }

            private List<Tile> neighbours = new List<Tile>();
            public void SetNeighbours()
            {
                Tile above, right, below, left;

                var x = coordinate.X;
                var y = coordinate.Y;

                if (allTiles.TryGetValue(new Coordinate(x - 1, y), out left)) neighbours.Add(left);
                if (allTiles.TryGetValue(new Coordinate(x, y - 1), out above)) neighbours.Add(above);
                if (allTiles.TryGetValue(new Coordinate(x + 1, y), out right)) neighbours.Add(right);
                if (allTiles.TryGetValue(new Coordinate(x, y + 1), out below)) neighbours.Add(below);
            }

            public bool AmLowPoint()
            {
                return neighbours.All(n => this.value < n.value);
            }

            public void PropagateBasin(Coordinate basin)
            {
                if (this.value == 9) return;

                this.IsInBasin = basin;

                var neighboursNotInBasin = neighbours.Where(n => n.IsInBasin == null);

                foreach (var n in neighboursNotInBasin) n.PropagateBasin(basin);
            }

            public Coordinate IsInBasin = null;

            public long Risk => value + 1;
        }

        public object GetResult1()
        {
            return Tile.allTiles.Where(t => t.Value.AmLowPoint()).Select(t => t.Value.Risk).Sum();
        }

        public object GetResult2()
        {
            var lowPoints = Tile.allTiles.Where(t => t.Value.AmLowPoint()).Select(t => t.Value);

            foreach (var t in lowPoints) t.PropagateBasin(t.coordinate);

            var basins = Tile.allTiles.Select(t => t.Value).Where(t => t.IsInBasin != null).GroupBy(t => t.IsInBasin);
                var sizes = basins.Select(g => g.Count()).OrderByDescending(g => g).ToArray();

            return sizes[0] * sizes[1] * sizes[2];
        }
    }
}
