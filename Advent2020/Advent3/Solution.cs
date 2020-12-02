using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent3
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            modules = ParsedInput.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public static List<ParsedInput> Parse(IEnumerable<string> lines)
            {
                var inputParser = new InputParser<string>("line");

                return lines.Select(line =>
                {
                    var pi = new ParsedInput();
                    //(pi) = inputParser.Parse(line);
                    return pi;
                }).ToList();
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
