using Advent2021.Advent24.Expressions;
using Advent2021.Shared;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent24
{
    public class Solution : ISolution
    {
        public Register W, X, Y, Z;

        public Solution(string input)
        {
            var zero = new Constant(0);

            W = new Register("W", zero);
            X = new Register("X", zero);
            Y = new Register("Y", zero);
            Z = new Register("Z", zero);

            var Registers = new Dictionary<string, Register>()
            {
                { "w", W },
                { "x", X },
                { "y", Y },
                { "z", Z },
                { "0", new Register("0", zero) }
            };

            for (int n = 1; n < 10; n++)
            {
                Registers.Add(n.ToString(), new Register(n.ToString(), new Constant(n)));
            }

            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<string, char, string>("op arg1 arg2");

            foreach (var line in lines) ExpressionBuilder.Parse(Registers, line);
        }
        public Solution() : this("Input.txt") { }


        public class Register
        {
            public string Name;

            public Expression Value;

            public Register(string name, Expression value)
            {
                this.Name = name;
                this.Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public class ExpressionBuilder
        {
            static int inputCursor = 0;
            public static void Parse(Dictionary<string, Register> registers, string line)
            {
                var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Register source = null;
                if (data.Length == 3)
                {
                    if (int.TryParse(data[2], out int val))
                    {
                        if (!registers.ContainsKey(data[2]))
                        {
                            registers[data[2]] = new Register(val.ToString(), new Constant(val));
                        }
                    }

                    source = registers[data[2]];
                }
                var target = registers[data[1]];

                switch (data[0])
                {
                    case "inp": target.Value = new InputSet(inputCursor++); break;
                    case "add": target.Value = new Add(target.Value, source.Value).Simplify(); break;
                    case "mul": target.Value = new Mul(target.Value, source.Value).Simplify(); break;
                    case "div": target.Value = new Div(target.Value, source.Value).Simplify(); break;
                    case "mod": target.Value = new Mod(target.Value, source.Value).Simplify(); break;
                    case "eql": target.Value = new Eql(target.Value, source.Value).Simplify(); break;
                };
            }
        }

        public object GetResult1()
        {
            var numNodes = Z.Value.Count();
            var depth = Z.Value.Depth();
            var numExpressions = Z.Value.UniqueSimplifyableExpressionCount();

            return "";
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
