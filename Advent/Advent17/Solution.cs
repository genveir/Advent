using NUnit.Framework;
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
            PropDown(500, yMin);

            Console.WriteLine("part1: " + waterTiles.Count);
            Console.WriteLine("part2: " + waterTiles.Where(wt => wt.Value).Count());
        }

        private bool PropDown(int x, int y)
        {
            if (blockedTiles.Contains((x, y))) return true;
            if (waterTiles.ContainsKey((x, y))) return waterTiles[(x, y)];
            if (y == yMax + 1) return false;

            waterTiles.Add((x, y), false);

            var belowIsBlocked = false;
            var leftIsBlocked = false;
            var rightIsBlocked = false;

            belowIsBlocked = PropDown(x, y + 1);

            if (belowIsBlocked)
            {
                leftIsBlocked = PropLeft(x - 1, y);

                rightIsBlocked = PropRight(x + 1, y);
            }

            if (leftIsBlocked && rightIsBlocked)
            {
                PropSettled(x, y);
            }

            return leftIsBlocked && rightIsBlocked && belowIsBlocked;
        }

        private bool PropLeft(int x, int y)
        {
            if (blockedTiles.Contains((x, y))) return true;
            if (waterTiles.ContainsKey((x, y))) return waterTiles[(x, y)];

            waterTiles.Add((x, y), false);

            var belowIsBlocked = false;
            var leftIsBlocked = false;

            belowIsBlocked = PropDown(x, y + 1);

            if (belowIsBlocked)
            {
                leftIsBlocked = PropLeft(x - 1, y);
            }

            return leftIsBlocked && belowIsBlocked;
        }

        private bool PropRight(int x, int y)
        {
            if (blockedTiles.Contains((x, y))) return true;
            if (waterTiles.ContainsKey((x, y))) return waterTiles[(x, y)];

            waterTiles.Add((x, y), false);

            var belowIsBlocked = false;
            var rightIsBlocked = false;

            belowIsBlocked = PropDown(x, y + 1);

            if (belowIsBlocked)
            {
                rightIsBlocked = PropRight(x + 1, y);
            }

            return rightIsBlocked && belowIsBlocked;
        }

        private void PropSettled(int x, int y)
        {
            PropSettledLeft(x, y);
            PropSettledRight(x + 1, y);
        }

        private void PropSettledLeft(int x, int y)
        {
            if (blockedTiles.Contains((x, y))) return;
            waterTiles[(x, y)] = true;

            PropSettledLeft(x - 1, y);
        }

        private void PropSettledRight(int x, int y)
        {
            if (blockedTiles.Contains((x, y))) return;
            waterTiles[(x, y)] = true;

            PropSettledRight(x + 1, y);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int y = 0; y <= yMax; y++)
            {
                for (int x = xMin - 1; x <= xMax + 1; x++)
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
