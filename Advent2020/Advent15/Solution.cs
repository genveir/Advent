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
        int spokenLastTurn;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            spoken = new int[30000000];

            for (turn = 1; turn < nums.Length; turn++)
            {
                spokenLastTurn = nums[turn - 1];

                spoken[spokenLastTurn] = turn;
            }
            spokenLastTurn = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public unsafe void RunUntilTurn(int targetTurn)
        {
            fixed (int* spokenPtr = spoken)
            {
                for (; turn < targetTurn; turn++)
                {
                    int theLastTimeThisWasSaid = spokenPtr[spokenLastTurn];

                    spokenPtr[spokenLastTurn] = turn;

                    spokenLastTurn = (theLastTimeThisWasSaid == 0) ? 0 : turn - theLastTimeThisWasSaid;
                }
            }
        }

        public object GetResult1()
        {
            RunUntilTurn(2020);

            return spokenLastTurn;
        }

        public object GetResult2()
        {
            RunUntilTurn(30000000);

            return spokenLastTurn;
        }
    }
}
