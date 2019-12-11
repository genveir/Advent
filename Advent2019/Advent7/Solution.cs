using Advent2019.OpCode;
using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent7
{
    public class Solution : ISolution
    {
        public Amplifier[] amplifiers;

        public Solution(Input.InputMode inputMode, string input)
        {
            var program = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();

            amplifiers = new Amplifier[5];
            for (int n = 0; n < 5; n++) amplifiers[n] = new Amplifier(program, "Amplifier " + n);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public class Amplifier
        {
            private Executor executor;
            private string name;

            public Amplifier(string[] program, string name)
            {
                executor = new Executor(program);
                this.name = name;
            }

            public void Start(int phaseSetting)
            {
                executor.Reset();
                executor.AddInput(phaseSetting);
                executor.program.Name = name + " running phase setting " + phaseSetting;
            }

            private int lastOutput = -1;
            public int GetOutput(int input)
            {
                executor.AddInput(input);
                executor.ExecuteToOutput();
                if (executor.program.output.Count > 0) lastOutput = int.Parse(executor.program.output.Dequeue());

                return lastOutput;
            }

            public bool IsStopped()
            {
                return executor.program.Stop;
            }

            public bool Verbose { set { executor.program.Verbose = value; } }
        }

        

        public string GetResult1()
        {
            int[] settings = new int[5] { 0, 1, 2, 3, 4 };
            int[][] toTry = settings.GetPermutations();

            int bestResult = 0;
            foreach (var permutation in toTry)
            {
                for (int n = 0; n < 5; n++) amplifiers[n].Start(permutation[n]);

                var first = amplifiers[0].GetOutput(0);
                var second = amplifiers[1].GetOutput(first);
                var third = amplifiers[2].GetOutput(second);
                var fourth = amplifiers[3].GetOutput(third);
                var fifth = amplifiers[4].GetOutput(fourth);

                if (fifth > bestResult) bestResult = fifth;
            }

            return bestResult.ToString();
        }

        public string GetResult2()
        {
            int[] settings = new int[5] { 5, 6, 7, 8, 9 };
            int[][] toTry = settings.GetPermutations();

            int bestResult = 0;
            amplifiers[4].Verbose = true;
            foreach (var permutation in toTry)
            {
                for (int n = 0; n < 5; n++) amplifiers[n].Start(permutation[n]);
                int fifth = 0;

                do
                {
                    var first = amplifiers[0].GetOutput(fifth);
                    var second = amplifiers[1].GetOutput(first);
                    var third = amplifiers[2].GetOutput(second);
                    var fourth = amplifiers[3].GetOutput(third);
                    fifth = amplifiers[4].GetOutput(fourth);
                } while (!amplifiers[4].IsStopped());

                if (fifth > bestResult) bestResult = fifth;
            }

            return bestResult.ToString();
        }
    }
}
