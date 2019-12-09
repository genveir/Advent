using Advent2019.OpCode;
using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent5
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
            Console.WriteLine("result1: ");
            executor.program.Verbose = true;
            executor.Execute();
            return "";
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
