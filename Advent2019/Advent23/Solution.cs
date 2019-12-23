using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent23
{
    public class Solution : ISolution
    {
        public OpCode.Program program;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            var executor = new Executor(startProg);
            program = executor.startProgram;
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public string GetResult1()
        {
            Executor[] executors = new Executor[50];

            for (int n = 0; n < 50; n++)
            {
                executors[n] = new Executor(program);
                executors[n].Execute();
                executors[n].AddInput(n);
            }

            while(true)
            {
                for (int n = 0; n < 50; n++)
                {
                    var outputs = executors[n].program.output;
                    while (outputs.Count > 0)
                    {
                        int dest = int.Parse(outputs.Dequeue());
                        long x = long.Parse(outputs.Dequeue());
                        long y = long.Parse(outputs.Dequeue());

                        if (dest == 255) return y.ToString();

                        executors[dest].AddInput(x);
                        executors[dest].AddInput(y);
                    }

                    if (executors[n].program.Blocked) executors[n].AddInput(-1);
                }
            }
        }

        public string GetResult2()
        {
            long natX = 0;
            long natY = 0;

            long lastYSent = -1;

            Executor[] executors = new Executor[50];

            for (int n = 0; n < 50; n++)
            {
                executors[n] = new Executor(program);
                executors[n].Execute();
                executors[n].AddInput(n);
            }

            while (true)
            {
                int numIdle = 0;
                for (int n = 0; n < 50; n++)
                {
                    bool isIdle = true;
                    var outputs = executors[n].program.output;
                    while (outputs.Count > 0)
                    {
                        int dest = int.Parse(outputs.Dequeue());
                        long x = long.Parse(outputs.Dequeue());
                        long y = long.Parse(outputs.Dequeue());

                        if (dest == 255)
                        {
                            natX = x;
                            natY = y;
                        }
                        else
                        {
                            executors[dest].AddInput(x);
                            executors[dest].AddInput(y);
                        }

                        isIdle = false;
                    }

                    if (executors[n].program.Blocked)
                    {
                        executors[n].AddInput(-1);
                    }

                    if (isIdle) numIdle++;
                }

                if (numIdle == 50)
                {
                    if (natY == lastYSent) return natY.ToString();

                    executors[0].AddInput(natX);
                    executors[0].AddInput(natY);
                    lastYSent = natY;
                }
            }
        }
    }
}
