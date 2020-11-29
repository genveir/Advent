using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.AdventBase
{
    public class Solution : ISolution
    {
        IEnumerable<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            modules = ParsedInput.Parse(lines);
        }
        public Solution() : this("Input") { }

        public class ParsedInput
        {

            public static IEnumerable<ParsedInput> Parse(IEnumerable<string> lines)
            {
                var parsedInputs = new List<ParsedInput>();

                foreach(var line in lines)
                {
                    var pi = new ParsedInput()
                    {

                    };

                    parsedInputs.Add(pi);
                }

                return parsedInputs;
            }
        }

        public string GetResult1()
        {
            return "";
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
