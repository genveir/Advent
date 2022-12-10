using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent10
{
    public class Solution : ISolution
    {
        public Computer computer;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var operators = lines.Select(Operator.Parse);

            computer= new Computer(operators);
        }
        public Solution() : this("Input.txt") { }

        public class Computer
        {
            public List<Operator> Operators { get; set; }
            public long X { get; set; }
            public Monitor monitor;

            public Computer(IEnumerable<Operator> operators)
            {
                Operators = operators.ToList();
                monitor = new();
                Reset();
            }

            public void Reset()
            {
                X = 1;
                _cycle = 1;
                _operatorIndex = 0;
                IsDone = false;

                Operators[0].Start();
            }

            public long _cycle { get; set; }
            public int _operatorIndex { get; set; }
            public long SignalStrength => X * _cycle;

            public void Cycle()
            {
                if (IsDone) throw new NotSupportedException("no operator to execute");

                var op = Operators[_operatorIndex];

                op.Cycle(this);
                if (op.IsDone)
                {
                    _operatorIndex++;

                    if (_operatorIndex == Operators.Count)
                        IsDone = true;
                    else 
                        Operators[_operatorIndex].Start();
                }

                monitor.Draw(this);

                _cycle++;
            }

            public bool IsDone { get; set; }
        }

        public abstract class Operator
        {
            public static Operator Parse(string line)
            {
                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                return split[0] switch
                {
                    "noop" => new Noop(),
                    "addx" => new AddX(split),
                    _ => throw new NotSupportedException($"unknown operator {split[0]}")
                };
            }

            public abstract void Start();
            public abstract void Cycle(Computer computer);
            public bool IsDone { get; set; }
        }

        public class Noop : Operator
        {
            public override string ToString() => "Noop";

            public override void Start() => IsDone = false;
            public override void Cycle(Computer computer) => IsDone = true;
        }

        public class AddX : Operator
        {
            public long value;

            public AddX(string[] split)
            {
                value = long.Parse(split[1]);
            }

            public override string ToString() => $"AddX {value}";

            private int _cycle = 0;
            public override void Start()
            {
                _cycle = 0;
                IsDone = false;
            }

            public override void Cycle(Computer computer)
            {
                _cycle++;

                if (_cycle == 2)
                {
                    computer.X += value;
                    IsDone = true;
                }
            }
        }

        public class Monitor
        {
            public void Draw(Computer computer)
            {
                var position = computer._cycle % 40;

                if (position == 0) Console.WriteLine();
                if (Math.Abs(position - computer.X) <= 1) Console.Write(Helper.BLOCK);
                else Console.Write(' ');
            }
        }

        private long RunProgram()
        {
            computer.Reset();
            computer.monitor.Draw(computer); // hacks!

            long result = 0;

            while (!computer.IsDone)
            {
                if (computer._cycle % 40 == 20) result += (computer.SignalStrength);

                computer.Cycle();
            }

            return result;
        }

        public object GetResult1()
        {
            return RunProgram();
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
