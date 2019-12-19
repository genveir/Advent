using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent19
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


        public bool IsInField(int x, int y)
        {
            executor.Reset();
            executor.AddInput(x);
            executor.AddInput(y);
            executor.Execute();

            var output = executor.program.output.Dequeue();

            return output == "1";
        }

        public string GetResult1()
        {
            int num = 0;
            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    num += IsInField(x, y) ? 1 : 0;
                }
            }

            return num.ToString();
        }

        public bool CheckSquare(int x, int y, int squareSize)
        {
            int leftX = x - squareSize + 1;
            int bottomY = y + squareSize - 1;

            return IsInField(leftX, bottomY);
        }

        public string GetResult2()
        {
            int squareCount = 1;
            int x = 10;
            int y = 6;


            int output = 0;
            while (squareCount < 100)
            {
                while (!IsInField(x, y)) y++;

                if (CheckSquare(x, y, squareCount + 1))
                {
                    squareCount++;
                    output = 10000 * (x - squareCount + 1) + y;
                }

                x++;
            }

            return output.ToString();
        }
    }
}
