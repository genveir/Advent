using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.AdventInfi
{
    public class Solution : ISolution
    {
        public List<Move> moves;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            moves = lines
                .Select(l => l.Split(' '))
                .Select<string[], Move>(s => s[0] switch
            {
                "draai" => new Draai(s[1]),
                "loop" => new Loop(s[1]),
                "spring" => new Spring(s[1]),
                _ => throw new NotSupportedException()
            })
                .ToList();
        }
        public Solution() : this("Input.txt") { }

        public class Santa
        {
            public ModNum Direction = new ModNum(0, 360);
            public Coordinate Position = new Coordinate(0, 0);

            public HashSet<Coordinate> positionsReached = new();

            public void Move(int amount)
            {
                var newPos = Direction.number switch
                {
                    0 => Position.ShiftY(amount),
                    45 => Position.Shift(amount, amount, 0),
                    90 => Position.ShiftX(amount),
                    135 => Position.Shift(amount, -amount, 0),
                    180 => Position.ShiftY(-amount),
                    225 => Position.Shift(-amount, -amount, 0),
                    270 => Position.ShiftX(-amount),
                    315 => Position.Shift(-amount, amount, 0),
                    _ => throw new NotSupportedException()
                };

                Position = newPos;
                if (!positionsReached.Contains(Position)) positionsReached.Add(Position);
            }
        }

        public abstract class Move
        {
            protected int amount;

            public abstract void Execute(Santa santa);
        }

        public class Draai : Move
        {
            public Draai(string amount)
            {
                this.amount = int.Parse(amount);
            }

            public override void Execute(Santa santa)
            {
                santa.Direction += amount;
            }
        }

        public class Loop : Move
        {
            public Loop(string amount)
            {
                this.amount = int.Parse(amount);
            }

            public override void Execute(Santa santa)
            {
                for (int n = 0; n < amount; n++)
                {
                    santa.Move(1);
                }
            }
        }

        public class Spring : Move
        {
            public Spring(string amount)
            {
                this.amount = int.Parse(amount);
            }

            public override void Execute(Santa santa)
            {
                santa.Move(amount);
            }
        }

        public object GetResult1()
        {
            var santa = new Santa();

            foreach (var move in moves)
            {
                move.Execute(santa);
            }

            Print(santa.positionsReached);

            return santa.Position.ManhattanDistance(new(0, 0));
        }

        public object GetResult2()
        {
            return "";
        }

        public void Print(HashSet<Coordinate> coords)
        {
            var minX = (int)coords.Min(c => c.X);
            var maxX = (int)coords.Max(c => c.X);
            var minY = (int)coords.Min(c => c.Y);
            var maxY = (int)coords.Max(c => c.Y);

            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Console.Write(coords.Contains(new(x, y)) ? Helper.cBLOCK : ' ');
                }
                Console.WriteLine();
            }
        }
    }
}
