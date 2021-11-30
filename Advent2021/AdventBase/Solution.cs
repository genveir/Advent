using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.AdventBase
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<string>(true, 2, " ");

            modules = lines.Select(line =>
            {
                var pi = new ParsedInput();
                //(pi) = inputParser.Parse(line);
                return pi;
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
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
