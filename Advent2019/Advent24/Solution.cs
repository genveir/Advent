using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent24
{
    public class Solution : ISolution
    {
        HashSet<Coordinate> startBugs;

        HashSet<Coordinate> bugs;
        HashSet<Coordinate> front;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            startBugs = Parse(lines);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Setup(bool withLevels)
        {
            bugs = new HashSet<Coordinate>(startBugs);

            _neighbours = new Dictionary<Coordinate, List<Coordinate>>();
            front = new HashSet<Coordinate>();
            foreach (var coord in bugs)
            {
                var neighbours = GetNeighbours(coord, withLevels);

                front.Add(coord);
                foreach (var neighbour in neighbours) front.Add(neighbour);
            }
        }

        public HashSet<Coordinate> Parse(string[] lines)
        {
            var res = new HashSet<Coordinate>();
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y].Trim();
                for (int x = 0; x < 5; x++)
                {
                    if(line[x] == '#') res.Add(new Coordinate(x, y, 0));
                }
            }

            return res;
        }

        public bool GetAt(Coordinate toGet)
        {
            return bugs.Contains(toGet);
        }

        public List<Coordinate> SimpleNeighbours(Coordinate coord)
        {
            var result = new List<Coordinate>();
            var x = coord.X;
            var y = coord.Y;
            var level = coord.Z;

            if (x < 4) result.Add(new Coordinate(x + 1, y, level));
            if (x > 0) result.Add(new Coordinate(x - 1, y, level));
            if (y < 4) result.Add(new Coordinate(x, y + 1, level));
            if (y > 0) result.Add(new Coordinate(x, y - 1, level));

            return result;
        }

        private Dictionary<Coordinate, List<Coordinate>> _neighbours;
        public List<Coordinate> GetNeighbours(Coordinate coord, bool withLevels)
        {
            List<Coordinate> neighbours;
            _neighbours.TryGetValue(coord, out neighbours);

            if (neighbours == null)
            {
                neighbours = SimpleNeighbours(coord);
                if (withLevels)
                {
                    var x = coord.X;
                    var y = coord.Y;
                    var level = coord.Z;

                    var middle = neighbours.Where(n => n.X == 2 && n.Y == 2).SingleOrDefault();
                    if (middle != null)
                    {
                        neighbours.Remove(middle);
                        if (x == 1)
                        {
                            for (int y2 = 0; y2 < 5; y2++) neighbours.Add(new Coordinate(0, y2, level + 1));
                        }
                        else if (x == 3)
                        {
                            for (int y2 = 0; y2 < 5; y2++) neighbours.Add(new Coordinate(4, y2, level + 1));
                        }
                        else if (y == 1)
                        {
                            for (int x2 = 0; x2 < 5; x2++) neighbours.Add(new Coordinate(x2, 0, level + 1));
                        }
                        else
                        {
                            for (int x2 = 0; x2 < 5; x2++) neighbours.Add(new Coordinate(x2, 4, level + 1));
                        }
                    }
                    else
                    {
                        if (x == 0) neighbours.Add(new Coordinate(1, 2, level - 1));
                        if (x == 4) neighbours.Add(new Coordinate(3, 2, level - 1));
                        if (y == 0) neighbours.Add(new Coordinate(2, 1, level - 1));
                        if (y == 4) neighbours.Add(new Coordinate(2, 3, level - 1));
                    }
                }
                _neighbours.Add(coord, neighbours);
            }

            return neighbours;
        }



        public void Step(bool withLevels)
        {
            var next = new HashSet<Coordinate>();
            var nextFront = new HashSet<Coordinate>();

            foreach (var coord in front)
            {
                var current = bugs.Contains(coord);
                var neighbours = GetNeighbours(coord, withLevels);

                var sum = neighbours.Where(neighbour => bugs.Contains(neighbour)).Count();

                bool aliveInNext;
                if (current)
                {
                    aliveInNext = (sum == 1);
                }
                else
                {
                    aliveInNext = (sum == 1 || sum == 2);
                }

                if (aliveInNext)
                {
                    next.Add(coord);
                    nextFront.Add(coord);
                    foreach (var neighbour in neighbours) nextFront.Add(neighbour);
                }
            }

            bugs = next;
            front = nextFront;
        }


        private long[] powers = null;
        private long GetBioDiversity()
        {
            if (powers == null)
            {
                powers = new long[25];
                powers[0] = 1;
                for (int n = 1; n < 25; n++)
                {
                    powers[n] = powers[n - 1] * 2;
                }
            }

            long result = 0;
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (bugs.Contains(new Coordinate(x, y, 0))) result += powers[y * 5 + x];
                }
            }

            return result;
        }

        public void Print()
        {
            var minZ = (int)front.Min(b => b.Z.Value);
            var maxZ = (int)front.Max(b => b.Z.Value);

            for (int z = minZ; z <= maxZ; z++)
            {
                Console.WriteLine("Depth " + z);
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if (front.Contains(new Coordinate(x, y, z))) Console.ForegroundColor = ConsoleColor.Red;
                        if (bugs.Contains(new Coordinate(x, y, z))) Console.Write('#');
                        else Console.Write('.');
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
            }
            var bioDiv = GetBioDiversity();
            Console.WriteLine(bioDiv.ToString());
            Console.WriteLine();

            if (readLineOnPrint) Console.ReadLine();
        }

        public static bool readLineOnPrint = true;

        public string GetResult1()
        {
            Setup(false);

            var seen = new HashSet<long>();

            var bioDiv = GetBioDiversity();
            seen.Add(bioDiv);

            while (true)
            {
                Step(false);
                bioDiv = GetBioDiversity();

                if (seen.Contains(bioDiv)) return bioDiv.ToString();
                seen.Add(bioDiv);
            }

            // not 524800
        }

        public long GetBugsAfter(int numMinutes)
        {
            for (int n = 0; n < numMinutes; n++)
            {
                Step(true);
            }

            return bugs.Count;
        }

        public string GetResult2()
        {
            Setup(true);

            //1922 too low

            return GetBugsAfter(200).ToString();
        }
    }
}
