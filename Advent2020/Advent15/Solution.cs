using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent15
{
    public class Solution : ISolution
    {
        int[] spoken;
        int turn;
        int spokenLast;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            spoken = new int[30000000];

            for (turn = 1; turn < nums.Length; turn++)
            {
                spokenLast = nums[turn - 1];

                spoken[spokenLast] = turn;
            }
            spokenLast = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public unsafe void RunUntilTurn(int targetTurn)
        {
            fixed (int* spokenPtr = spoken)
            {
                for (; turn < targetTurn; turn++)
                {
                    int value = spokenPtr[spokenLast];

                    spokenPtr[spokenLast] = turn;

                    spokenLast = (value == 0) ? 0 : turn - value;
                }
            }
        }

        public object GetResult1()
        {
            RunUntilTurn(2020);

            return spokenLast;
        }

        public object GetResult2()
        {
            RunUntilTurn(30000000);

            return spokenLast;
        }
    }
}
