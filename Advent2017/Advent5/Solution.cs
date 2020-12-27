using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent5
{
    public class Solution : ISolution
    {
        long[] jumps;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            jumps = lines.Select(line => long.Parse(line)).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            var jumps = this.jumps.DeepCopy();

            long current = 0;
            long numSteps;
            for (numSteps = 0; current < jumps.Length && current >= 0; numSteps++)
            {
                var jump = jumps[current];
                jumps[current]++;

                current = current + jump;
            }

            return numSteps;
        }

        public object GetResult2()
        {
            var jumps = this.jumps.DeepCopy();

            long current = 0;
            long numSteps;
            for (numSteps = 0; current < jumps.Length && current >= 0; numSteps++)
            {
                var jump = jumps[current];
                if (jump >= 3) jumps[current]--;
                else jumps[current]++;

                current = current + jump;
            }

            return numSteps;
        }
    }
}
