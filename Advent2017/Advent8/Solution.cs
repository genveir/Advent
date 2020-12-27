using Advent2017.OpCode;
using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent8
{
    public class Solution : ISolution
    {
        Machine machine;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            machine = new Parser().Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            machine.Reset();
            machine.Run();

            return machine.Registers.Max(r => r.Value);
        }

        public object GetResult2()
        {
            machine.Reset();

            long highest = 0;
            for (int n = 0; n < machine.Instructions.Length; n++)
            {
                machine.Step();
                var currentHighest = machine.Registers.Max(r => r.Value);

                if (currentHighest > highest) highest = currentHighest;
            }

            return highest;
        }
    }
}
