using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent15
{
    public class Solution : ISolution
    {
        int[,] spoken;
        int turn;
        int last;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            spoken = new int[30000000,2];

            for (turn = 1; turn < nums.Length; turn++)
            {
                PushNum(nums[turn - 1]);
            }
            last = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public int PushNum(long num)
        {
            spoken[num, 1] = spoken[num, 0];
            spoken[num, 0] = turn;

            if (spoken[num, 1] == 0) return 0;
            else return spoken[num, 0] - spoken[num, 1];
        }

        public void SimulateSteps(int targetTurn)
        {
            while(turn < targetTurn)
            {
                last = PushNum(last);
                turn++;
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
