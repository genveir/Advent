using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent21
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
            executor.Execute();
            
            executor.AddAscInput(@"
NOT C J
NOT A T
OR T J
NOT B T
OR T J
AND D J
WALK");

            var output = executor.GetAscOutput();

            Console.WriteLine(output);



            return "";
        }

        public string GetResult2()
        {
            executor.Reset();
            executor.Execute();

            
            executor.AddAscInput(@"
NOT C J
NOT A T
OR T J
NOT B T
OR T J
AND D J
NOT H T
NOT T T
OR E T
AND T J
RUN");

            var output = executor.GetAscOutput();

            Console.WriteLine(output);



            return "";
        }
    }
}
