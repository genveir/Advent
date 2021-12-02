using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent03
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<string, string>("var1 var2");

            modules = lines.Select(line =>
            {
                var (var1, var2) = inputParser.Parse(line);
                return new ParsedInput();
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public ParsedInput()
            {

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
