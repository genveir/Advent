using Advent2019.OpCode;
using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent9
{
    public class Solution : ISolution
    {
        public Executor executor;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public string GetResult1()
        {
            executor.Reset();
            executor.AddInput(1);
            executor.Execute();

            var sb = new StringBuilder();
            while (executor.program.output.Count > 0)
            {
                sb.Append(executor.program.output.Dequeue());
                sb.Append(","); // eh whatever
            }

            return sb.ToString();
        }

        public string GetResult2()
        {
            executor.Reset();
            executor.AddInput(2);
            executor.Execute();

            var sb = new StringBuilder();
            while (executor.program.output.Count > 0)
            {
                sb.Append(executor.program.output.Dequeue());
                sb.Append(","); // eh whatever
            }

            return sb.ToString();
        }
    }
}
