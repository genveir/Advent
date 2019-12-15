﻿using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent15
{
    public class Solution : ISolution
    {
        public Executor executor;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);

            Explored = new HashSet<(long, long)>();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        HashSet<(long, long)> Explored;

        public class Bot
        {
            public OpCode.Program program;
            public Coordinate coordinate;
            public bool AtTarget;

            public Bot(Coordinate coordinate, OpCode.Program program, bool atTarget = false)
            {
                this.coordinate = coordinate;
                this.program = program.Copy();
                this.AtTarget = atTarget;
            }

            public Bot Move(int direction, HashSet<(long, long)> explored)
            {
                var executor = new Executor(program);

                var nextCoord = new Coordinate(coordinate.X, coordinate.Y, coordinate.Z.Value + 1);
                switch(direction)
                {
                    case 1: nextCoord.Y++; break;
                    case 2: nextCoord.Y--; break;
                    case 3: nextCoord.X++; break;
                    case 4: nextCoord.X--; break;
                }

                if (explored.Contains((nextCoord.X, nextCoord.Y))) return null;
                explored.Add((nextCoord.X, nextCoord.Y));

                executor.AddInput(direction);
                var output = executor.program.output.Dequeue();

                if (output == "0") return null;
                if (output == "2") return new Bot(nextCoord, executor.program, true);
                if (output == "1") return new Bot(nextCoord, executor.program);
                else throw new Exception("unexpected output");
            }
        }

        public string GetResult1()
        {
            executor.Execute();

            var positions = new Queue<Bot>();

            var bot = new Bot(new Coordinate(0, 0, 0), executor.program);

            positions.Enqueue(bot);
            while(positions.Count > 0)
            {
                var head = positions.Dequeue();

                for (int n = 1; n <= 4; n++)
                {
                    var newBot = head.Move(n, Explored);
                    if (newBot == null) continue;
                    if (newBot.AtTarget) return newBot.coordinate.Z.ToString();

                    positions.Enqueue(newBot);
                }
            }

            return "no result";
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
