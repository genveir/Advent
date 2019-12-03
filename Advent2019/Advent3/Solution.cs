using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent3
{
    public class Solution : ISolution
    {
        Dictionary<(int x, int y), int> steps;
        HashSet<(int x, int y)>[] wires;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            steps = new Dictionary<(int x, int y), int>();
            wires = ParsedInput.Parse(lines, steps);
            
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class ParsedInput
        {
            public static HashSet<(int x, int y)>[] Parse(string[] lines, Dictionary<(int x, int y), int> steps)
            {
                var result = new HashSet<(int x, int y)>[lines.Length];

                for (int num = 0; num < lines.Length; num++)
                {
                    var split = lines[num].Replace(" ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);

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
                            bool first = true;
                            switch(direction)
                            {
                                case 'R': x++; if (wire.Contains((x, y))) first = false; else wire.Add((x, y)); break;
                                case 'L': x--; if (wire.Contains((x, y))) first = false; else wire.Add((x, y)); break;
                                case 'U': y++; if (wire.Contains((x, y))) first = false; else wire.Add((x, y)); break;
                                case 'D': y--; if (wire.Contains((x, y))) first = false; else wire.Add((x, y)); break;
                            }
                            totalDist++;

                            if (!steps.ContainsKey((x, y))) steps[(x, y)] = totalDist;
                            else if (first) steps[(x, y)] = steps[(x, y)] + totalDist;
                        }
                    }

                    result[num] = wire;
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
