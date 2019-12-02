using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent2
{
    public class Solution : ISolution
    {
        static int[] startintProgram;
        static int[] program;
        static bool stop = false;

        public bool replace = true;

        public Solution(Input.InputMode inputMode, string input)
        {
            startintProgram = Input.GetInputLines(inputMode, input, new char[] { ',' }).Select(num => int.Parse(num)).ToArray();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class Op
        {
            public static void Execute(int index)
            {
                switch(program[index])
                {
                    case 1: program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]]; break;
                    case 2: program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]]; break;
                    case 99: stop = true; break;
                    default: stop = true; break;
                }
            }
        }



        public string GetResult1()
        {
            program = startintProgram.DeepCopy();

            if (replace)
            {
                program[1] = 12;
                program[2] = 2;
            }

            int cursor = 0;
            while (!stop)
            {
                Op.Execute(cursor);
                cursor += 4;
            }

            return program[0].ToString();
        }

        public string GetResult2()
        {
            for (int first = 0; first < 100; first++)
            {
                for (int second = 0; second < 100; second++)
                {
                    program = startintProgram.DeepCopy();
                    stop = false;

                    program[1] = first;
                    program[2] = second;

                    int cursor = 0;
                    while (!stop)
                    {
                        Op.Execute(cursor);
                        cursor += 4;
                    }

                    if (program[0] == 19690720) return (100 * first + second).ToString();
                }
            }

            return "no result";
        }
    }
}
