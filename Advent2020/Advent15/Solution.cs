using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent15
{
    public class Solution : ISolution
    {
        int[] spoken1;
        int[] spoken2;
        int turn;
        int last;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            spoken1 = new int[30000000];
            spoken2 = new int[30000000];

            for (turn = 1; turn < nums.Length; turn++)
            {
                last = nums[turn - 1];

                spoken2[last] = spoken1[last];
                spoken1[last] = turn;

                if (spoken2[last] == 0) last = 0;
                else last = spoken1[last] - spoken2[last];
            }
            last = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public unsafe void SimulateSteps(int targetTurn)
        {
            fixed (int* s10 = spoken1, s20 = spoken2)
            {
                while (turn < targetTurn)
                {
                    int v = s10[last];

                    s20[last] = v;
                    s10[last] = turn;

                    if (v == 0) last = 0;
                    else last = turn - v;

                    turn++;
                }
            }
        }

        public object GetResult1()
        {
            SimulateSteps(2020);

            return last;
        }

        public object GetResult2()
        {
            SimulateSteps(30000000);

            return last;
        }
    }
}
