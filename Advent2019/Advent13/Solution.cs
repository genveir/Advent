using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent13
{
    public class Solution : ISolution
    {
        public Executor executor;

        public Dictionary<Coordinate, int> tiles;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        


        public string GetResult1()
        {
            executor.Reset();
            executor.Execute();

            int outputs = 0;
            while(executor.program.output.Count > 0)
            {
                var x = executor.program.output.Dequeue();
                var y = executor.program.output.Dequeue();
                var output = executor.program.output.Dequeue();

                if (output != "0")
                {
                    outputs++;
                }
            }


            // not 880 (empty)
            // not 277 (?)
            return outputs.ToString();
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
