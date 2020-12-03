using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent3
{
    public class Solution : ISolution
    {
        List<ParsedInput> slopes;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            slopes = new List<ParsedInput>();
            for (int n = 0; n < lines.Length; n++)
            {
                slopes.Add(new ParsedInput(lines[n]));
            }
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            string rawInput;

            public ParsedInput(string input)
            {
                rawInput = input;
            }

            public bool HasTree(int x)
            {
                return rawInput[x % rawInput.Length] == '#';
            }
        }

        public string GetResult1()
        {
            return RunSlope(3, 1).ToString();
        }

        private long RunSlope(int xMod, int yMod)
        {
            int x = 0;
            int y = 0;
            int trees = 0;

            for (int n = 0; n < slopes.Count; n++)
            {
                x += xMod;
                y += yMod;
                if (y < slopes.Count && slopes[y].HasTree(x)) trees++;
            }

            return trees;
        }

        public string GetResult2()
        {
            return
                (RunSlope(1, 1) * RunSlope(3, 1) * RunSlope(5, 1) * RunSlope(7, 1) * RunSlope(1, 2)).ToString();
        }
    }
}
