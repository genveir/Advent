﻿using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent1
{
    public class Solution : ISolution
    {
        ParsedInput parsedInput;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            parsedInput = ParsedInput.Parse(lines);
        }
        public Solution() : this(Input.InputMode.File, "Input") { }

        private class ParsedInput
        {


            public static ParsedInput Parse(IEnumerable<string> line)
            {
                var toReturn = new ParsedInput();

                return toReturn;
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
