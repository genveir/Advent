﻿using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.AdventBase
{
    public class Solution : ISolution
    {
        public List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<ParsedInput>("line");

            modules = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            [ComplexParserConstructor]
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
