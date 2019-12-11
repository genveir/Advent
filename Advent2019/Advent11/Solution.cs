using Advent2019.OpCode;
using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent11
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

        static Dictionary<(int X, int Y), bool> panels = new Dictionary<(int X, int Y), bool>();
        static HashSet<Coordinate> paintedAtAll = new HashSet<Coordinate>();

        public class Coordinate
        {
            public Coordinate(int x, int y) { this.X = x; this.Y = y; }

            public int X;
            public int Y;

            public override int GetHashCode()
            {
                return X.GetHashCode() + Y.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as Coordinate;
                if (other == null) return false;
                return other.X == X && other.Y == Y;
            }
        }

        public class Bot
        {
            public const int FACING_UP = 0;
            public const int FACING_RIGHT = 1;
            public const int FACING_DOWN = 2;
            public const int FACING_LEFT = 3;

            public int facing = 0;
            public int x = 0;
            public int y = 0;

            public void TurnRight()
            {
                facing = (facing + 1) % 4;
            }

            public void TurnLeft()
            {
                facing = (facing + 3) % 4;
            }

            public void MoveForward()
            {
                switch(facing)
                {
                    case FACING_UP: y--; break;
                    case FACING_RIGHT: x++; break;
                    case FACING_DOWN: y++; break;
                    case FACING_LEFT: x--; break;
                }
            }

            public bool GetPanelIsWhite()
            {
                bool value;
                panels.TryGetValue((x, y), out value);
                return value;
            }

            public void PaintPanel(bool white)
            {
                panels[(x, y)] = white;
                paintedAtAll.Add(new Coordinate(x, y));
            }

            public void Run(Executor executor, bool? firstInput)
            {
                while(true)
                {
                    if (firstInput == null)
                    {
                        executor.program.inputs.Enqueue(GetPanelIsWhite() ? "1" : "0");
                    }
                    else
                    {
                        executor.program.inputs.Enqueue(firstInput.Value ? "1" : "0");
                        firstInput = null;
                    }

                    executor.ExecuteToOutput();
                    if (executor.program.Stop) break;
                    var paintColor = executor.program.output.Dequeue() == "1";

                    executor.ExecuteToOutput();
                    if (executor.program.Stop) break;
                    var turnRight = executor.program.output.Dequeue() == "1";

                    PaintPanel(paintColor);
                    if (turnRight) TurnRight();
                    else TurnLeft();

                    MoveForward();
                }
            }
        }

        public string GetResult1()
        {
            executor.Reset();
            panels = new Dictionary<(int X, int Y), bool>();
            paintedAtAll = new HashSet<Coordinate>();

            var bot = new Bot();
            bot.Run(executor, false);

            return paintedAtAll.Count.ToString();
        }

        public string GetResult2()
        {
            executor.Reset();
            panels = new Dictionary<(int X, int Y), bool>();
            paintedAtAll = new HashSet<Coordinate>();

            var bot = new Bot();
            
            bot.Run(executor, true);
            ;
            var byY = paintedAtAll.GroupBy(paa => paa.Y).OrderBy(group => group.Key);
            for (int y = byY.First().Key; y <= byY.Last().Key; y++)
            {
                var yGroup = byY.Where(group => group.Key == y).SingleOrDefault();
                if (yGroup == null) continue;

                var byX = yGroup.OrderBy(val => val.X);
                for (int x =0; x <= byX.Last().X; x++)
                {
                    bool value;
                    panels.TryGetValue((x, y), out value);
                    if (value) Console.Write("X");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }

            return "";
        }
    }
}
