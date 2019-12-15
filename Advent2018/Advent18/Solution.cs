using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent18
{
    class Solution : ISolution
    {
        private Dictionary<(int x, int y), Tile> TilesByPosition;

        public enum InputMode { String, File }

        public Solution() : this("Input", InputMode.File) { }
        public Solution(string input, InputMode mode)
        {
            if (mode == InputMode.File) ParseInput(input);
            else
            {
                _ParseInput(input);
            }
        }

        private void ParseInput(string fileName)
        {
            string resourceName = "Advent2018.Advent18." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        public void _ParseInput(string input)
        {
            TilesByPosition = new Dictionary<(int x, int y), Tile>();

            var lines = input.Split('\n').Select(s => s.Trim()).Select(s => s.Replace("\r", "")).ToArray();

            for (int y = 0; y < lines.Length; y ++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    Tile.TileType type = Tile.TileType.Floor;

                    switch(lines[y][x])
                    {
                        case '.': type = Tile.TileType.Floor; break;
                        case '|': type = Tile.TileType.Forest; break;
                        case '#': type = Tile.TileType.Lumberjard; break;
                        default: throw new NotSupportedException(lines[y][x] + " is not a supported character");
                    }

                    Tile NW, N, NE, W;
                    TilesByPosition.TryGetValue((x - 1, y - 1), out NW);
                    TilesByPosition.TryGetValue((x, y - 1), out N);
                    TilesByPosition.TryGetValue((x + 1, y - 1), out NE);
                    TilesByPosition.TryGetValue((x - 1, y), out W);

                    TilesByPosition.Add((x, y), new Tile(type, NW, N, NE, W));
                }
            }
        }

        private class Tile
        {
            private List<Tile> Neighbours;

            public enum TileType { Floor, Forest, Lumberjard}
            public TileType[] Types;

            public Tile(TileType type, Tile NW, Tile N, Tile NE, Tile W)
            {
                this.Types = new TileType[] { type, type };
                this.Neighbours = new List<Tile>();
                
                if (NW != null) { Neighbours.Add(NW); NW.Neighbours.Add(this); }
                if (N != null) { Neighbours.Add(N); N.Neighbours.Add(this); }
                if (NE != null) { Neighbours.Add(NE); NE.Neighbours.Add(this); }
                if (W != null) { Neighbours.Add(W); W.Neighbours.Add(this); }
            }

            public void Tick(int round)
            {
                var index = (round % 2 == 0) ? 1 : 0;

                Types[1 - index] = NextType(index);
            }

            private TileType NextType(int index)
            {
                if (Types[index] == TileType.Floor)
                {
                    if (HasAtLeastNNeighboursWithType(index, 3, TileType.Forest)) return TileType.Forest;
                }
                else if (Types[index] == TileType.Forest)
                {
                    if (HasAtLeastNNeighboursWithType(index, 3, TileType.Lumberjard)) return TileType.Lumberjard;
                }
                else if (Types[index] == TileType.Lumberjard)
                {
                    if (HasAtLeastNNeighboursWithType(index, 1, TileType.Lumberjard) &&
                        HasAtLeastNNeighboursWithType(index, 1, TileType.Forest)) return TileType.Lumberjard;
                    return TileType.Floor;
                }
                return Types[index];
            }

            private bool HasAtLeastNNeighboursWithType(int index, int n, TileType type)
            {
                return Neighbours
                    .Where(neighbour => neighbour.Types[index] == type)
                    .Count() >= n;

            }
        }

        public void Update(int round)
        {
            foreach(var tile in TilesByPosition.Values)
            {
                tile.Tick(round);
            }
        }

        public void WriteResult()
        {
            int n = 0;
            while(n < 10)
            {
                Update(n++);
            }

            var result = TilesByPosition.Values.Where(v => v.Types[1 - (n % 2)] == Tile.TileType.Forest).Count() *
                         TilesByPosition.Values.Where(v => v.Types[1 - (n % 2)] == Tile.TileType.Lumberjard).Count();

            Console.WriteLine("part1: " + result);

            var notHashed = new Dictionary<int, List<Dictionary<(int x, int y), Tile.TileType>>>();
            var InCycle = new List<int>();
            while (true)
            {
                Update(n++);

                result = TilesByPosition.Values.Where(v => v.Types[1 - (n % 2)] == Tile.TileType.Forest).Count() *
                         TilesByPosition.Values.Where(v => v.Types[1 - (n % 2)] == Tile.TileType.Lumberjard).Count();

                var newDict = new Dictionary<(int x, int y), Tile.TileType>();
                foreach (var kv in TilesByPosition)
                {
                    newDict.Add(kv.Key, kv.Value.Types[n % 2]);
                }

                if (InCycle.Contains(result))
                {
                    List<Dictionary<(int x, int y), Tile.TileType>> sameResult;
                    notHashed.TryGetValue(result, out sameResult);

                    foreach (var element in sameResult)
                    {
                        bool areTheSame = true;
                        foreach (var kv in newDict)
                        {
                            if (element[kv.Key] != kv.Value) areTheSame = false;
                        }
                        if (areTheSame) goto whateverditwerkt;
                    }
                }
                else
                {
                    notHashed.Add(result, new List<Dictionary<(int x, int y), Tile.TileType>>());
                }

                notHashed[result].Add(newDict);
                InCycle.Add(result);
            }
            whateverditwerkt:

            var cycleLength = InCycle.Count() - InCycle.IndexOf(result);
            var cycleStart = n - cycleLength;
            InCycle = InCycle.TakeLast(cycleLength).ToList();

            int numToFind = 1_000_000_000;

            // the cycle starts at cycleStart and has a length of cycleLength, all and only all elements of the cycle are in InCycle
            var cycleNormalized = numToFind - cycleStart;
            var cycleIndex = cycleNormalized % cycleLength;

            Console.WriteLine("part2: " + InCycle[cycleIndex]);
        }
    }
}
