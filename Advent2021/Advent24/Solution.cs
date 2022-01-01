using Advent2021.Advent24.Constraints;
using Advent2021.Advent24.Expressions.Operators;
using Advent2021.Advent24.Expressions.Values;
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
            var zero = new SetWithConstant(0);

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
                Registers.Add(n.ToString(), new Register(n.ToString(), new SetWithConstant(n)));
            }

            var lines = Input.GetInputLines(input).ToArray();

            foreach (var line in lines) ExpressionBuilder.Parse(Registers, line);
        }
        public Solution() : this("Input.txt") { }


        public class Register
        {
            public string Name;

            public Set Value;

            public Register(string name, Set value)
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
                            registers[data[2]] = new Register(val.ToString(), new Set(new[] { new Constant(val, Constraint.None()) }));
                        }
                    }

                    source = registers[data[2]];
                }
                var target = registers[data[1]];

                switch (data[0])
                {
                    case "inp": target.Value = new InputSet(inputCursor++); break;
                    case "add": target.Value = new Add(target.Value, source.Value).Apply(); break;
                    case "mul": target.Value = new Mul(target.Value, source.Value).Apply(); break;
                    case "div": target.Value = new Div(target.Value, source.Value).Apply(); break;
                    case "mod": target.Value = new Mod(target.Value, source.Value).Apply(); break;
                    case "eql": target.Value = new Eql(target.Value, source.Value).Apply(); break;
                };

                Console.WriteLine("applied " + line);
            }
        }

        public object GetResult1()
        {


            return "";
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
