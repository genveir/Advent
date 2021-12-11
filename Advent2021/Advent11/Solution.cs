using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advent2021.Advent11
{
    public class Solution : ISolution
    {
        Dictionary<Coordinate, Tile> tileMap;
        List<Tile> allTiles;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).Select(l => l.AsDigits()).ToArray();

            var inputParser = new InputParser<Tile>("line");

            tileMap = new Dictionary<Coordinate, Tile>();
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
            public long energy;

            [ComplexParserConstructor]
            public Tile(Coordinate coordinate, long value)
            {
                this.coordinate = coordinate;

                this.energy = value;
            }

            private List<Tile> neighbours = new List<Tile>();
            public void SetNeighbours(Dictionary<Coordinate, Tile> allTiles)
            {
                foreach (var n in coordinate.GetNeighbours())
                    if (allTiles.TryGetValue(n, out Tile newN)) neighbours.Add(newN);
            }

            public int turn = 0;
            public int lastFlash = -1;

            public static int FlashCount = 0;

            public void Turn()
            {
                turn++;
                energy++;
            }

            public void DoFlashIfRequired()
            {
                if (lastFlash != turn && energy > 9) Flash();
            }

            public void Flash()
            {
                energy = 0;
                FlashCount++;
                lastFlash = turn;
                foreach (var n in neighbours) n.RegisterFlash();
            }

            public void RegisterFlash()
            {
                energy++;
                DoFlashIfRequired();
            }

            public void DeflateIfNeeded()
            {
                if (turn == lastFlash) energy = 0;
            }
        }

        private void SimStep()
        {
            foreach (var octopus in allTiles)
            {
                octopus.Turn();
            }

            foreach (var octopus in allTiles)
            {
                octopus.DoFlashIfRequired();
            }

            foreach (var octopus in allTiles)
            {
                octopus.DeflateIfNeeded();
            }
        }

        int turn = 0;
        public object GetResult1()
        {
            for (; turn < 100; turn++)
            {
                SimStep();
            }

            return Tile.FlashCount;
        }

        public object GetResult2()
        {
            for (; true; turn++)
            {
                var flashCount = Tile.FlashCount;

                SimStep();

                var difference = Tile.FlashCount - flashCount;

                if (difference == 100) return turn + 1;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var coord = new Coordinate(x, y);
                    sb.Append(tileMap[coord].energy);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
