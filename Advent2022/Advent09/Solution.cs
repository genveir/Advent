using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent09
{
    public class Solution : ISolution
    {
        public List<Move> moves;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<Move>("direction amount");

            moves = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public enum Direction { Right, Left, Up, Down }

        public class Move
        {
            public long xShift, yShift;
            public long Amount;

            [ComplexParserConstructor]
            public Move(char direction, long amount)
            {
                xShift = direction switch
                {
                    'R' => 1,
                    'L' => -1,
                    _ => 0
                };
                yShift = direction switch
                {
                    'U' => 1,
                    'D' => -1,
                    _ => 0
                };

                if (xShift == 0 && yShift == 0) throw new ArgumentException("invalid direction");

                Amount = amount;
            }

            public void Execute(Rope rope)
            {
                for (int n = 0; n < Amount; n++)
                {
                    rope.MoveHead(xShift, yShift);
                }
            }
        }

        public class Rope
        {
            public Coordinate[] knots;
            
            public HashSet<Coordinate> TailPositions;

            public Rope(int numKnots)
            {
                knots = new Coordinate[numKnots];
                for (int n = 0; n < knots.Length; n++) knots[n] = new(0, 0);
                
                TailPositions = new() { new(0,0) };
            }

            public void MoveHead(long xShift, long yShift)
            {
                MoveKnot(0, xShift, yShift);
            }

            public void MoveKnot(int index, long xShift, long yShift)
            {
                knots[index] = knots[index].Shift(xShift, yShift);

                if (knots.Length - 1 == index) TailPositions.Add(knots[index]);
                else
                {
                    MoveKnotToTouch(index + 1);
                }
            }

            public void MoveKnotToTouch(int index)
            {
                var difference = knots[index - 1] - knots[index];

                if (difference.AbsX < 2 && difference.AbsY < 2)
                    return;

                if (difference.AbsX == 2)
                {
                    difference.X = difference.X / 2;
                }
                if (difference.AbsY == 2)
                {
                    difference.Y = difference.Y / 2;
                }

                MoveKnot(index, difference.X, difference.Y);
            }
        }

        public object GetResult1()
        {
            var rope = new Rope(2);

            foreach(var move in moves)
            {
                move.Execute(rope);
            }

            return rope.TailPositions.Count;
        }

        public object GetResult2()
        {
            var rope = new Rope(10);

            foreach (var move in moves)
            {
                move.Execute(rope);
            }

            return rope.TailPositions.Count;
        }
    }
}
