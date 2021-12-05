﻿using Advent2021.Shared;
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

            var inputParser = new InputParser<long[], long[]>("begin -> end");

            lineDefs = lines.Select(line =>
            {
                (var begin, var end) = inputParser.Parse(line);

                return new LineDef(begin, end);
            }).ToList();

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

                if (Begin.X > End.X)
                {
                    var buffer = End;
                    End = Begin;
                    Begin = buffer;
                }
            }

            public bool ForPart1 => Begin.X == End.X || Begin.Y == End.Y;

            public void Draw(long[][] grid)
            {
                var xDir = 1;

                var yDir = Begin.Y < End.Y ? 1 : -1;
                if (Begin.Y == End.Y) yDir = 0;

                var length = End.X - Begin.X;
                if (length == 0)
                {
                    length = (End.Y - Begin.Y) * yDir;
                    xDir = 0;
                }

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
