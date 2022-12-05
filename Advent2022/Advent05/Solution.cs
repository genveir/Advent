using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent05
{
    public class Solution : ISolution
    {
        private readonly string input;

        public List<Stack> stacks = new();
        public List<Move> moves = new();

        public Solution(string input)
        {
            this.input = input;

            Initialize();
        }

        private void Initialize()
        {
            stacks = new();
            moves = new();

            var blocks = Input.GetBlockLines(input).ToArray();

            var stackLines = blocks[0]
                .Take(blocks[0].Length - 1);
            var moveLines = blocks[1];

            char[][] boxes = stackLines
                .Select(sl => sl.Parts(elementLength: 3, dividerLength: 1)
                    .Select(p => p.Parse<char>(pattern: "[a]", defaultOnEmpty: () => ' '))
                    .ToArray()
                ).ToArray();

            for (int n = 0; n < boxes[0].Length; n++)
            {
                var stackChars = boxes.Select(b => b[n]);
                var withoutEmpty = stackChars.Where(sc => sc != ' ');
                var reversedToArray = withoutEmpty.Reverse().ToArray();

                stacks.Add(new Stack(reversedToArray));
            }

            for (int n = 0; n < moveLines.Length; n++)
            {
                var moveNums = moveLines[n].Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",").Split(',')
                    .Select(s => int.Parse(s))
                    .ToArray();

                moves.Add(new Move(moveNums));
            }
        }
        public Solution() : this("Input.txt") { }

        public class Stack
        {
            public Stack<char> items;

            [ComplexParserConstructor]
            public Stack(char[] chars)
            {
                items = new();

                for (int n = 0; n < chars.Length; n++)
                {
                    items.Push(chars[n]);
                }
            }

            public override string ToString()
            {
                return items.ToList().Select(i => "[" + i + "] ").Aggregate((a, b) => a + b);
            }
        }

        public class Move
        {
            public static int moveNum = 0;
            public int index;

            public int From;
            public int To;
            public int Num;

            public Move(int[] nums)
            {
                index = moveNum++;

                Num = nums[0];
                From = nums[1];
                To = nums[2];
            }
        }

        public void ExecuteMove(Move move)
        {
            for (int n = 0; n < move.Num; n++)
            {
                var item = stacks[move.From - 1].items.Pop();
                stacks[move.To - 1].items.Push(item);
            }
        }

        public void ExecuteMove2(Move move)
        {
            List<char> items = new();

            for (int n = 0; n < move.Num; n++)
            {
                items.Add(stacks[move.From - 1].items.Pop());
            }

            for (int n = move.Num - 1; n >= 0; n--)
            {
                stacks[move.To - 1].items.Push(items[n]);
            }
        }

        public object GetResult1()
        {
            foreach (var move in moves)
            {
                ExecuteMove(move);
            }

            StringBuilder result = new();
            for (int n = 0; n < stacks.Count; n++)
            {
                result.Append(stacks[n].items.Peek());
            }

            return result.ToString();
        }

        public object GetResult2()
        {
            Initialize();

            foreach (var move in moves)
            {
                ExecuteMove2(move);
            }

            StringBuilder result = new();
            for (int n = 0; n < stacks.Count; n++)
            {
                result.Append(stacks[n].items.Peek());
            }

            return result.ToString();
        }
    }
}
