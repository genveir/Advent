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

        static Dictionary<(long X, long Y), bool> panels = new Dictionary<(long X, long Y), bool>();
        static HashSet<Coordinate> paintedAtAll = new HashSet<Coordinate>();

        

        public class Bot
        {
            public const long FACING_UP = 0;
            public const long FACING_RIGHT = 1;
            public const long FACING_DOWN = 2;
            public const long FACING_LEFT = 3;

            public long facing = 0;
            public long x = 0;
            public long y = 0;

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
                        executor.AddInput(GetPanelIsWhite() ? 1 : 0);
                    }
                    else
                    {
                        executor.AddInput(firstInput.Value ? 1 : 0);
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
            panels = new Dictionary<(long X, long Y), bool>();
            paintedAtAll = new HashSet<Coordinate>();

            var bot = new Bot();
            bot.Run(executor, false);

            return paintedAtAll.Count.ToString();
        }

        public string GetResult2()
        {
            executor.Reset();
            panels = new Dictionary<(long X, long Y), bool>();
            paintedAtAll = new HashSet<Coordinate>();

            var bot = new Bot();
            
            bot.Run(executor, true);

            long minX = paintedAtAll.Select(c => c.X).Min() - 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            var byY = paintedAtAll.GroupBy(paa => paa.Y).OrderBy(group => group.Key);
            for (long y = byY.First().Key; y <= byY.Last().Key; y++)
            {
                var yGroup = byY.Where(group => group.Key == y).SingleOrDefault();
                if (yGroup == null) continue;

                var byX = yGroup.OrderBy(val => val.X);
                for (long x = minX; x <= byX.Last().X; x++)
                {
                    bool value;
                    panels.TryGetValue((x, y), out value);
                    if (value) sb.Append(Helper.BLOCK);
                    else sb.Append(" ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
