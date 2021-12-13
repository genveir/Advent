using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent13
{
    public class Solution : ISolution
    {
        public HashSet<Coordinate> dots = new HashSet<Coordinate>();
        public List<Instruction> instructions = new List<Instruction>();

        public Solution(string input)
        {
            var lines = Input.GetBlockLines(input).ToArray();

            for (int n = 0; n < lines[0].Length; n++)
            {
                var line = lines[0][n];

                var parsed = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                var coord = new Coordinate(parsed[0], parsed[1]);

                dots.Add(coord);
            }

            for (int n = 0; n < lines[1].Length; n++)
            {
                var line = lines[1][n].Substring("fold along ".Length);

                var parsed = line.Split('=', StringSplitOptions.RemoveEmptyEntries).ToArray();

                var instruction = new Instruction(parsed[0].ToCharArray()[0], long.Parse(parsed[1]));

                instructions.Add(instruction);
            }
        }
        public Solution() : this("Input.txt") { }

        public class Instruction
        {
            public char axis;
            public long index;

            [ComplexParserConstructor]
            public Instruction(char axis, long index)
            {
                this.axis = axis;
                this.index = index;
            }

            public HashSet<Coordinate> Fold(HashSet<Coordinate> dots)
            {
                HashSet<Coordinate> newCoords = new HashSet<Coordinate>();
                foreach(var dot in dots)
                {
                    Coordinate newCoord = null;
                    if (axis == 'x')
                    {
                        var higher = dot.X - index > 0;
                        var distance = Math.Abs(dot.X - index);

                        if (higher) newCoord = new Coordinate(dot.X - 2 * distance, dot.Y);
                        else newCoord = new Coordinate(dot.X, dot.Y);
                    }
                    if (axis == 'y')
                    {
                        var higher = dot.Y - index > 0;
                        var distance = Math.Abs(dot.Y - index);

                        if (higher) newCoord = new Coordinate(dot.X, dot.Y - 2 * distance);
                        else newCoord = new Coordinate(dot.X, dot.Y);
                    }
                    if (!newCoords.Contains(newCoord)) newCoords.Add(newCoord);
                }

                return newCoords;
            }
        }

        public string Display(HashSet<Coordinate> coordinates)
        {
            var maxX = coordinates.Max(c => c.X);
            var maxY = coordinates.Max(c => c.Y);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (coordinates.Contains(new Coordinate(x, y))) sb.Append(Helper.BLOCK);
                    else sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public object GetResult1()
        {
            var newDots = instructions.First().Fold(dots);

            return newDots.Count;
        }

        public object GetResult2()
        {
            var newDots = dots;
            foreach(var i in instructions)
            {
                newDots = i.Fold(newDots);
            }

            var result = Display(newDots);
            return result;
        }
    }
}
