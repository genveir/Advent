using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent02
{
    public class Solution : ISolution
    {
        List<Move> moves;

        public class Position
        {
            public long hor;
            public long ver;
            public long aim;
        }

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();
            var inputParser = new InputParser<string, int>("direction amount");

            moves = lines.Select(line =>
            {
                var (direction, amount) = inputParser.Parse(line);
                return new Move(direction, amount);
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class Move
        {
            private bool updown;
            private int amount;

            public Move(string direction, int amount)
            {
                switch (direction.Trim())
                {
                    case "up": updown = true; this.amount = -amount; break;
                    case "down": updown = true; this.amount = amount; break;
                    case "forward": updown = false; this.amount = amount; break;
                    default: throw new InvalidOperationException();
                }
            }

            public void Go(Position pos)
            {
                if (updown) pos.ver += amount;
                else pos.hor += amount;
            }

            public void Go2(Position pos)
            {
                if (updown) pos.aim += amount;
                else
                {
                    pos.hor += amount;
                    pos.ver += pos.aim * amount;
                }
            }
        }

        public object GetResult1()
        {
            var pos = new Position();
            foreach (var move in moves) move.Go(pos);

            return pos.hor * pos.ver;
        }

        public object GetResult2()
        {
            var pos = new Position();
            foreach (var move in moves) move.Go2(pos);

            return pos.hor * pos.ver;
        }
    }
}
