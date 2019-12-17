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
            for (int n = 0; n < c.Length; n++) c[n] = (char)int.Parse(outputs.Dequeue());

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

        //string routeByHand = "L,10,R,8,R,8,L,10,R,8,R,8,L,10,L,12,R,8,R,10,R,10,L,12,R,10,L,10,L,12,R,8,R,10,R,10,L,12,R,10,L,10,L,12,R,8,R,10,R,10,L,12,R,10,R,10,L,12,R,10,L,10,R,8,R,8";
        string routeByHand = "R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2";

        public string SplitHandRoute()
        {
            var splitRoute = routeByHand.Split(',');

            List<string> parts = new List<string>();
            foreach (var str in splitRoute)
            {
                if (str == "L") parts.Add("L");
                else if (str == "R") parts.Add("R");
                else
                {
                    var num = int.Parse(str);
                    for (int n = 0; n < num; n++) parts.Add("1");
                }
            }

            return string.Join(',', parts);
        }

        public string GetResult2()
        {
            SetScaffold();
            Console.WriteLine(scaffoldString);

            executor.Reset();
            executor.program.SetAt(0, "2");
            executor.Execute();

            var splitroute = SplitHandRoute();
            for (int first = 1; first < splitroute.Length; first++)
            {
                var newSplit = SplitHandRoute();
                var substr1 = splitroute.Substring(0, first).TrimEnd(',');

                newSplit = newSplit.Replace(substr1, "A");

                for (int second = 1; second < newSplit.Length; second++)
                {
                    var level2Copy = new string(newSplit);
                    var substr2 = level2Copy.Substring(first, second).Trim(',');

                    if (substr2.Length == 0) continue;
                    if (substr2.Contains("A")) break;

                    level2Copy = level2Copy.Replace(substr2, "B");

                    var thirdLevel = level2Copy.Split(new char[] { 'A', 'B' }, StringSplitOptions.RemoveEmptyEntries).Select(tl => tl.TrimEnd(','));
                    var tl1 = thirdLevel.First();
                    if (thirdLevel.All(tl => tl == tl1))
                    {
                        Console.WriteLine("match");
                        Console.WriteLine("A: " + substr1);
                        Console.WriteLine("B: " + substr2);
                        Console.WriteLine("C: " + tl1);
                    }
                }
            }

            return "";
        }
    }
}
