using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent15
{
    public class Solution : ISolution
    {
        int[] spoken2;
        int turn;
        int last;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            spoken2 = new int[30000000];

            for (turn = 1; turn < nums.Length; turn++)
            {
                last = nums[turn - 1];

                spoken2[last] = turn;
            }
            last = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public unsafe void go(int targetTurn)
        {
            fixed (int* spoken2Zero = spoken2)
            {
                for (; turn < targetTurn; turn++)
                {
                    int value = spoken2Zero[last];

                    spoken2Zero[last] = turn;

                    last = (value == 0) ? 0 : turn - value;
                }
            }
        }

        public object GetResult1()
        {
            go(2020);

            return last;
        }

        public object GetResult2()
        {
            go(30000000);

            return last;
        }
    }
}
