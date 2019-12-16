using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent16
{
    public class Solution : ISolution
    {
        public long[] program;

        public Solution(Input.InputMode inputMode, string input)
        {
            program = Input.GetInputLines(inputMode, input).ToArray()[0].ToCharArray().Select(c => c - 48).Select(c => (long)c).ToArray();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class ParsedInput
        {

            public static IEnumerable<ParsedInput> Parse(IEnumerable<string> lines)
            {
                var parsedInputs = new List<ParsedInput>();

                foreach(var line in lines)
                {
                    var pi = new ParsedInput()
                    {

                    };

                    parsedInputs.Add(pi);
                }

                return parsedInputs;
            }
        }

        public long[] RunPhase()
        {
            var newProgram = new long[program.Length];

            for (int positionConsidered = 0; positionConsidered < program.Length; positionConsidered++)
            {
                newProgram[positionConsidered] = Calculate(program, positionConsidered);
            }

            Console.Write(".");
            return newProgram;
        }

        bool Verbose = false;

        public long Calculate(long[] program, long positionConsidered)
        {
            long result = 0;

            for (int position = 0; position < program.Length; position++)
            {
                var multiplier = GetMultiplierFor(positionConsidered, position);

                if (Verbose) Console.Write(program[position] + "*" + multiplier);
                if (Verbose && position != program.Length) Console.Write(" + ");

                var val = program[position] * multiplier;
                result += val;
            }

            if (Verbose) Console.WriteLine(" = " + Math.Abs(result % 10));
            if (Verbose) Console.ReadLine();

            return Math.Abs(result % 10);
        }

        public long GetMultiplierFor(long positionConsidered, int element)
        {
            // posCon0:  0, 1, 0, -1, elementen die 1 pakken: 0, 4, 8, 12, etc. = el mod 4 == 0 in (0)
            // posCon1:  0, 0, 1, 1, 0, 0, -1, -1, elementen die 1 pakken: 1, 2, 9, 10, etc = (el mod 8) in (1, 2)
            // posCon2:  0, 0, 0, 1, 1, 1, 0, 0, 0, -1, -1, -1, elementen die 1 pakken: 2, 3, 4, 14, 15, 16, etc => (el mod 12) in (2, 3, 4)

            var positionInNonRepeatingPattern = 
                (element % ((positionConsidered + 1) * 4)) + 1;

            bool conditionpos = 
                positionInNonRepeatingPattern >= (positionConsidered + 1) &&
                positionInNonRepeatingPattern < 2 * (positionConsidered + 1)
                ;
            bool conditionneg =
                positionInNonRepeatingPattern >= (positionConsidered + 1) * 3 &&
                positionInNonRepeatingPattern < 4 * (positionConsidered + 1);

            if (conditionpos) return 1;
            else if (conditionneg) return -1;
            else return 0;
        }

        public long[] pattern = new long[] { 0, 1, 0, -1 };

        public string GetResult1()
        {
            //Verbose = true;
            for (int n = 0; n < 100; n++)
            {
                program = RunPhase();
            }

            var result = new string(program.Take(8).Select(c => c + 48).Select(c => (char)c).ToArray());
            return result;
        }

        public string GetResult2()
        {
            var newProgram = new long[program.Length * 10000];
            for (int n = 0; n < 10000; n++)
            {
                Array.Copy(program, 0, newProgram, n * program.Length, program.Length);
            }
            program = newProgram;

            return GetResult1();
        }
    }
}
