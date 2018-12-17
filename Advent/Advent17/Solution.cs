using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent17
{
    public class Solution : ISolution
    {
        HashSet<(int x, int y)> blockedTiles;
        Dictionary<(int x, int y), bool> waterTiles;
        int yMin;
        int yMax;
        int xMin;
        int xMax;

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
            string resourceName = "Advent.Advent17." + fileName + ".txt";
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
            blockedTiles = new HashSet<(int x, int y)>();
            waterTiles = new Dictionary<(int x, int y), bool>();
            this.yMin = int.MaxValue;
            this.yMax = int.MinValue;
            this.xMin = int.MaxValue;
            this.xMax = int.MinValue;

            var lines = input.Split('\n');

            foreach (var line in lines)
            {
                var splitLine = line.Split(", ");
                var xLine = splitLine
                    .Where(l => l.StartsWith("x"))
                    .Single()
                    .Replace("x=", "")
                    .Split("..");

                var yLine = splitLine
                    .Where(l => l.StartsWith("y"))
                    .Single()
                    .Replace("y=", "")
                    .Split("..");

                IEnumerable<int> xRange, yRange;
                if (xLine.Length == 1) xRange = new List<int>() { int.Parse(xLine.Single()) };
                else
                {
                    var xMin = int.Parse(xLine[0]);
                    var xMax = int.Parse(xLine[1]);
                    var xRangeList = new List<int>();
                    for(int x = xMin; x <= xMax; x++)
                    {
                        xRangeList.Add(x);
                    }
                    xRange = xRangeList;
                }

                if (yLine.Length == 1) yRange = new List<int>() { int.Parse(yLine.Single()) };
                else
                {
                    var yMin = int.Parse(yLine[0]);
                    var yMax = int.Parse(yLine[1]);

                    var yRangeList = new List<int>();
                    for (int y = yMin; y <= yMax; y++)
                    {
                        yRangeList.Add(y);
                    }
                    yRange = yRangeList;
                }

                foreach (int x in xRange)
                {
                    if (x < xMin) xMin = x;
                    if (x > xMax) xMax = x;
                    foreach (int y in yRange)
                    {
                        if (y < yMin) yMin = y;
                        if (y > yMax) yMax = y;
                        blockedTiles.Add((x, y));
                    }
                }
            }
        }

        public void WriteResult()
        {
            xMin = 495;
            Console.WriteLine(xMin); // returnt 1 ????!?!

            var water = Propagate(500, yMin, new (int x, int y)[0], 0);

            Console.WriteLine(this);
            Console.WriteLine("part1: " + water.numWater);
        }

        private FlowResult Propagate(int x, int y, (int x, int y)[] soFar, int propDir)
        {
            // base case
            if (blockedTiles.Contains((x, y))) return FlowResult.BaseCase;
            if (waterTiles.ContainsKey((x, y))) return new FlowResult() { blocked = waterTiles[(x, y)] };
            if (y == yMax) return FlowResult.DropCase;

            // propagate
            (int x, int y)[] withThis = new (int x, int y)[soFar.Length + 1];
            soFar.CopyTo(withThis, 0);
            withThis[soFar.Length] = (x, y);

            var below = new FlowResult();
            var left = new FlowResult();
            var right = new FlowResult();

            below = Propagate(x, y + 1, withThis, 0);
            
            if (below.blocked)
            {
                if (propDir != 1) left = Propagate(x - 1, y, withThis, -1);
                else left = FlowResult.BaseCase;

                if (propDir != -1) right = Propagate(x + 1, y, withThis, 1);
                else right = FlowResult.BaseCase;
            }

            bool settled = (left.blocked && right.blocked);
            
            var  result = new FlowResult()
            {
                blocked = left.blocked && right.blocked && below.blocked,
                numSettled = left.numSettled + right.numSettled + below.numSettled + ((settled) ? 1 : 0),
                numWater = left.numWater + right.numWater + below.numWater + 1
            };
            waterTiles.Add((x, y), result.blocked);

            return result;
        }

        private class FlowResult
        {
            public static FlowResult BaseCase
            {
                get
                {
                    return new FlowResult()
                    {
                        blocked = true,
                        numSettled = 0,
                        numWater = 0
                    };
                }
            }

            public static FlowResult DropCase
            {
                get
                {
                    return new FlowResult()
                    {
                        blocked = false,
                        numSettled = 0,
                        numWater = 0
                    };
                }
            }

            public int numWater = 0;
            public int numSettled = 0;
            public bool blocked = false;

            public override string ToString()
            {
                return "flow " + (blocked ? "blocked" : "open") + " (" + numSettled + " / " + numWater;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int y = 0; y <= yMax; y++)
            {
                for (int x = xMin = 1; x <= xMax + 1; x++)
                {
                    if (blockedTiles.Contains((x, y))) builder.Append('#');
                    else if (waterTiles.ContainsKey((x, y))) builder.Append(waterTiles[(x, y)] ? '~' : '|');
                    else builder.Append('.');
                }
                if (y != yMax) builder.Append('\n');
            }
            return builder.ToString();
        }
    }
}
