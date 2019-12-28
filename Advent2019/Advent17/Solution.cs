using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent17
{
    public class Solution : ISolution
    {
        public Executor executor;
        public OpCode.Program startProgram;


        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);

            this.startProgram = executor.startProgram.Copy();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        string scaffoldString;
        char[][] scaffold;

        public char GetPos(int x, int y)
        {
            if (x < 0 || y < 0 || x >= scaffold[y].Length || y >= scaffold.Length) return ' ';
            else return scaffold[y][x];
        }

        public void SetScaffold()
        {
            var scaffoldExec = new Executor(startProgram);
            scaffoldExec.Execute();

            var outputs = scaffoldExec.program.output;

            char[] c = new char[outputs.Count];
            for (int n = 0; n < c.Length; n++) c[n] = (char)outputs.Dequeue();

            scaffoldString = new string(c);
            scaffold = scaffoldString.Split('\n').Select(s => s.ToCharArray()).ToArray();
        }

        public string GetResult1()
        {
            SetScaffold();

            int sum = 0;
            for (int y = 0; y < scaffold.Length; y++)
            {
                for (int x = 0; x < scaffold[y].Length; x++)
                {
                    var left = GetPos(x - 1, y);
                    var up = GetPos(x, y - 1);
                    var right = GetPos(x + 1, y);
                    var down = GetPos(x, y + 1);

                    var here = GetPos(x, y);
                    var all = new char[] { left, up, right, down, here };
                    if (all.All(ch => ch == '#'))
                    {
                        sum += x * y;
                    }
                }
            }
            

            return sum.ToString();
        }

        public string GetResult2()
        {
            executor.Reset();
            executor.program.SetAt(0, 2);
            executor.Execute();

            var A = "L,10,R,8,R,8\n";
            var B = "L,10,L,12,R,8,R,10\n";
            var C = "R,10,L,12,R,10\n";

            var total = "A,A,B,C,B,C,B,C,C,A\n";

            InputString(total);
            InputString(A);
            InputString(B);
            InputString(C);

            executor.AddInput('n');
            executor.AddInput('\n');

            var outputs = executor.program.output;

            return outputs.Last().ToString();
        }

        public void InputString(string input)
        {
            if (!executor.program.Blocked) throw new Exception("huh hij moet input pakken");
            foreach (var ch in input.ToCharArray()) executor.AddInput(ch);
        }

        // L,10,R,8,R,8,L,10,R,8,R,8,L,10,L,12,R,8,R,10,R,10,L,12,R,10,L,10,L,12,R,8,R,10,R,10,L,12,R,10,L,10,L,12,R,8,R,10,R,10,L,12,R,10,R,10,L,12,R,10,L,10,R,8,R,8

        //L,10, P
        //R,8,  Q
        //R,8,  Q

        //L,10, P
        //R,8,  Q
        //R,8,  Q

        //L,10, P
        //L,12, R
        //R,8,  Q
        //R,10, S

        //R,10, S
        //L,12, R
        //R,10, S

        //L,10, P
        //L,12, R
        //R,8,  Q
        //R,10, S

        //R,10, S
        //L,12, R
        //R,10, S

        //L,10, P
        //L,12, R
        //R,8,  Q
        //R,10, S

        //R,10, S
        //L,12, R
        //R,10, S

        //R,10, S
        //L,12, R
        //R,10, S

        //L,10, P
        //R,8,  Q
        //R,8   Q

        //PQQ PQQ PRQS SRS PRQS SRS PRQS SRS SRS PQQ
    }
}
