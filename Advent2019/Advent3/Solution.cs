using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent3
{
    public class Solution : ISolution
    {
        static Dictionary<(int x, int y), int> steps;
        HashSet<(int x, int y)>[] wires;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            wires = ParsedInput.Parse(lines);
            
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class ParsedInput
        {
            public static HashSet<(int x, int y)>[] Parse(IEnumerable<string> lines)
            {
                steps = new Dictionary<(int x, int y), int>();

                var result = new HashSet<(int x, int y)>[2];
                int num = 0;
                foreach(var line in lines)
                {
                    var split = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    split = split.Select(s => s.Trim()).ToArray();
                    HashSet<(int x, int y)> wire = new HashSet<(int x, int y)>();
                    int x = 0;
                    int y = 0;
                    int totalDist = 0;
                    foreach(var route in split)
                    {
                        var direction = route[0];
                        var distance = int.Parse(route.Substring(1));

                        for(int n = 0; n < distance; n++)
                        {
                            switch(direction)
                            {
                                case 'R': x++; wire.Add((x, y)); break;
                                case 'L': x--; wire.Add((x, y)); break;
                                case 'U': y++; wire.Add((x, y)); break;
                                case 'D': y--; wire.Add((x, y)); break;
                            }
                            totalDist++;

                            if (!steps.ContainsKey((x, y))) steps[(x, y)] = totalDist;
                            else steps[(x, y)] = steps[(x, y)] + totalDist;
                        }
                    }

                    result[num++] = wire;
                }

                return result;
            }
        }

        public string GetResult1()
        {
            var intersect = wires[0].Intersect(wires[1]);

            int minDist = int.MaxValue;
            foreach(var point in intersect)
            {
                var dist = Math.Abs(point.x) + Math.Abs(point.y);
                if (dist < minDist) minDist = dist;
            }

            return minDist.ToString();
        }

        public string GetResult2()
        {
            var intersect = wires[0].Intersect(wires[1]);

            int minDist = int.MaxValue;
            foreach (var point in intersect)
            {
                var dist = steps[(point.x, point.y)];
                if (dist < minDist) minDist = dist;
            }

            return minDist.ToString();
        }
    }
}
