using Advent2019.OpCode;
using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent2
{
    public class Solution : ISolution
    {
        public Executor executor;

        public bool replace = true;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public string GetResult1()
        {
            executor.Reset();

            if (replace)
            {
                executor.program.SetAt(1, "12");
                executor.program.SetAt(2, "2");
            }

            executor.Execute();

            return executor.program.GetAt(0).ToString();
        }

        public string GetResult2()
        {
            for (int first = 0; first < 100; first++)
            {
                for (int second = 0; second < 100; second++)
                {
                    executor.Reset();

                    executor.program.ISetAt(1, first);
                    executor.program.ISetAt(2, second);

                    executor.Execute();

                    if (executor.program.IGetAt(0) == 19690720) return (100 * first + second).ToString();
                }
            }

            return "no result";
        }
    }
}
