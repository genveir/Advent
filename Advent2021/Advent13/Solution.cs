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

            var coordParser = new InputParser<Coordinate>("coord");
            for (int n = 0; n < lines[0].Length; n++)
            {
                var coord = coordParser.Parse(lines[0][n]);

                dots.Add(coord);
            }

            var instructionParser = new InputParser<Instruction>(false, 2, "fold along ", "=");

            for (int n = 0; n < lines[1].Length; n++)
            {
                var instruction = instructionParser.Parse(lines[1][n]);

                instructions.Add(instruction);
            }
        }
        public Solution() : this("Input.txt") { }

        public class Instruction
        {
            public long index;
            public Func<Coordinate, Coordinate> flip;

            public Instruction(long index) => this.index = index;

            [ComplexParserConstructor]
            public Instruction (char axis, long index)
            {
                this.index = index;
                this.flip = (axis == 'x') ? XFlip : YFlip;
            }

            public HashSet<Coordinate> Fold(HashSet<Coordinate> dots)
            {
                HashSet<Coordinate> newCoords = new HashSet<Coordinate>();
                foreach (var dot in dots)
                {
                    var newCoord = flip(dot);

                    if (!newCoords.Contains(newCoord)) newCoords.Add(newCoord);
                }

                return newCoords;
            }

            public long NewValue(long currentValue) => 
                (index < currentValue)
                    ? currentValue - 2 * Math.Abs(currentValue - index)
                    : currentValue;

            public Coordinate XFlip(Coordinate dot) => new Coordinate(NewValue(dot.X), dot.Y);
            public Coordinate YFlip(Coordinate dot) => new Coordinate(dot.X, NewValue(dot.Y));
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
