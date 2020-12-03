﻿using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent4
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<string>("line");

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

        public string GetResult1()
        {
            return "123";
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
