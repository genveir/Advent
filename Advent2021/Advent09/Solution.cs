using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent09
{
    public class Solution : ISolution
    {
        public List<Tile> allTiles;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).Select(l => l.AsDigits()).ToArray();

            var inputParser = new InputParser<Tile>("line");

            var tileMap = new Dictionary<Coordinate, Tile>();
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    var coord = new Coordinate(x, y);
                    var tile = new Tile(coord, lines[y][x]);

                    tileMap.Add(coord, tile);
                }
            }

            foreach (var kvp in tileMap)
            {
                kvp.Value.SetNeighbours(tileMap);
            }

            allTiles = tileMap.Select(kvp => kvp.Value).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class Tile
        {
            public Coordinate coordinate;
            public long value;

            [ComplexParserConstructor]
            public Tile(Coordinate coordinate, long value)
            {
                this.coordinate = coordinate;

                this.value = value;
            }

            private List<Tile> neighbours = new List<Tile>();
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

            public bool IsLowPoint => neighbours.All(n => this.value < n.value);

            public long Risk => value + 1;

            public Coordinate IsInBasin = null;
            public void PropagateBasin(Coordinate basin)
            {
                if (IsInBasin != null) return;
                if (this.value == 9) return;

                this.IsInBasin = basin;

                var neighboursNotInBasin = neighbours.Where(n => n.IsInBasin == null);

                foreach (var n in neighboursNotInBasin) n.PropagateBasin(basin);
            }
        }

        public object GetResult1()
        {
            return allTiles.Where(t => t.IsLowPoint).Select(t => t.Risk).Sum();
        }

        public object GetResult2()
        {
            var lowPoints = allTiles.Where(t => t.IsLowPoint);

            foreach (var t in lowPoints) t.PropagateBasin(t.coordinate);

            var basins = allTiles
                .Where(t => t.IsInBasin != null)
                .GroupBy(t => t.IsInBasin)
                .Select(g => g.Count())
                .OrderByDescending(g => g).ToArray();

            return basins[0] * basins[1] * basins[2];
        }
    }
}
