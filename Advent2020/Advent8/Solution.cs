using Advent2020.OpCode;
using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent8
{
    public class Solution : ISolution
    {
        Executor executor;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            executor = new Executor(lines);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            executor.Reset();

            RunToLoopOrCompletion();

            return executor.accumulator;
        }

        private bool RunToLoopOrCompletion()
        {
            HashSet<int> pointers = new HashSet<int>();
            while (!pointers.Contains(executor.instructionIndex))
            {
                pointers.Add(executor.instructionIndex);
                bool completed = !executor.ExecuteStep();

                if (completed) return true;
            }
            return false;
        }

        public object GetResult2()
        {
            for (int n = 0; n < executor.operators.Count; n++)
            {
                if (executor.operators[n] is Nop)
                {
                    executor.Reset();
                    executor.operators[n] = new Jmp(n, executor.operators[n].Argument);
                }
                else if (executor.operators[n] is Jmp)
                {
                    executor.Reset();
                    executor.operators[n] = new Nop(n, executor.operators[n].Argument);
                }
                else continue;

                var ranToCompletion = RunToLoopOrCompletion();

                if (ranToCompletion) return executor.accumulator;
            }
            return "no solution";
        }
    }
}
