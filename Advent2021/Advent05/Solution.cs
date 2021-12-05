using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent05
{
    public class Solution : ISolution
    {
        List<LineDef> lineDefs;

        public long[][] grid = new long[1000][];

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new ComplexParser<LineDef>("begin -> end");

            lineDefs = inputParser.Parse(lines);

            for (int n = 0; n < 1000; n++)
            {
                grid[n] = new long[1000];
            }
        }
        public Solution() : this("Input.txt") { }

        public class LineDef
        {
            Coordinate Begin;
            Coordinate End;

            public LineDef(long[] begin, long[] end)
            {
                Begin = new Coordinate(begin);
                End = new Coordinate(end);
            }

            public bool ForPart1 => Begin.X == End.X || Begin.Y == End.Y;

            public void Draw(long[][] grid)
            {
                var xDir = End.X.CompareTo(Begin.X);
                var yDir = End.Y.CompareTo(Begin.Y);

                var length = Math.Max(
                    Math.Abs(End.X - Begin.X),
                    Math.Abs(End.Y - Begin.Y));

                for (int n = 0; n <= length; n++)
                {
                    var xCoord = Begin.X + (n * xDir);
                    var yCoord = Begin.Y + (n * yDir);

                    grid[xCoord][yCoord]++;
                }
            }

            public override string ToString()
            {
                return $"{Begin}, {End}";
            }
        }

        public object GetResult1()
        {
            var relevantLines = lineDefs.Where(m => m.ForPart1);

            foreach (var line in relevantLines) line.Draw(grid);

            long result = 0;
            for (int n = 0; n < 1000; n++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (grid[n][i] > 1) result++;
                }
            }

            return result;
        }

        public object GetResult2()
        {
            var linesToAdd = lineDefs.Where(m => !m.ForPart1);

            foreach (var line in linesToAdd) line.Draw(grid);

            long result = 0;
            for (int n = 0; n < 1000; n++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (grid[n][i] > 1) result++;
                }
            }

            return result;
        }
    }
}
