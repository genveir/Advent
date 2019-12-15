using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent20
{
    class Solution : ISolution
    {
        private static Dictionary<(int x, int y), Room> TilesByPosition;
        private Room baseTile;
        private static string input;
        private static int cursor;

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
            string resourceName = "Advent.Advent20." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        private void _ParseInput(string input)
        {
            cursor = 1;
            TilesByPosition = new Dictionary<(int x, int y), Room>();
            baseTile = new Room((0, 0));
            baseTile.distance = 0;
            Solution.input = input;

            baseTile.LinkWithBase(baseTile);
        }

        private class Room
        {
            private HashSet<Room> Neighbours;
            private int x;
            private int y;
            public int distance = -1;

            public Room((int x, int y) position)
            {
                this.x = position.x;
                this.y = position.y;
                this.Neighbours = new HashSet<Room>();
            }

            private Room Link((int x, int y) coord)
            {
                Room tile;
                if (!TilesByPosition.ContainsKey(coord))
                {
                    tile = new Room(coord);
                    TilesByPosition.Add(coord, tile);
                }
                else tile = TilesByPosition[coord];

                if (!Neighbours.Contains(tile))
                {
                    this.Neighbours.Add(tile);
                    tile.Neighbours.Add(this);
                }

                tile.SetDistance(distance + 1);
                return tile;
            }

            private void SetDistance(int distance)
            {
                if (this.distance != -1 && this.distance < distance) return;
                else
                {
                    this.distance = distance;
                    foreach (var n in Neighbours) SetDistance(this.distance + 1);
                }
            }

            public void LinkWithBase(Room baseTile)
            {
                Room tile;
                switch (input[cursor++])
                {
                    case 'W': tile = Link((x - 1, y)); break;
                    case 'N': tile = Link((x, y - 1)); break;
                    case 'E': tile = Link((x + 1, y)); break;
                    case 'S': tile = Link((x, y + 1)); break;
                    case '$': return;
                    case ')': return;
                    case '(': LinkWithBase(this); LinkWithBase(baseTile); return;
                    case '|': baseTile.LinkWithBase(baseTile); return;
                    default: throw new NotImplementedException();
                }

                tile.LinkWithBase(baseTile);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}: distance {2}", x, y, distance);
            }
        }

        public int GetResult1()
        {
            return TilesByPosition.OrderBy(kv => kv.Value.distance).Last().Value.distance;
        }

        public int GetResult2()
        {
            return TilesByPosition.Where(kv => kv.Value.distance >= 1000).Count();
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetResult1());
            Console.WriteLine("part2: " + GetResult2());
        }


    }
}
