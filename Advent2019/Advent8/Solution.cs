﻿using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent8
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
