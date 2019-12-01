using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent1
{
    public class Solution : ISolution
    {
        IEnumerable<ParsedInput> modules;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            modules = ParsedInput.Parse(lines);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class ParsedInput
        {
            public int mass;

            public static IEnumerable<ParsedInput> Parse(IEnumerable<string> line)
            {
                return line.Select(l =>
                {
                    return new ParsedInput() { mass = int.Parse(l) };
                });
            }
        }

        public string GetResult1()
        {
            int total = 0;
            foreach (var mod in modules) total += (mod.mass / 3) - 2;

            return total.ToString();
        }

        public string GetResult2()
        {
            int total = 0;
            foreach (var mod in modules)
            {
                int required = (mod.mass / 3) - 2;
                while (required > 0)
                {
                    total += required;
                    required = (required / 3) - 2;
                }
            }

            return total.ToString();
        }
    }
}
