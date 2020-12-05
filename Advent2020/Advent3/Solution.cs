using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent3
{
    public class Solution : ISolution
    {
        List<SlopeLevel> slopeLevels;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            slopeLevels = new List<SlopeLevel>();
            for (int n = 0; n < lines.Length; n++)
            {
                slopeLevels.Add(new SlopeLevel(lines[n]));
            }
        }
        public Solution() : this("Input.txt") { }

        public class SlopeLevel
        {
            string rawInput;

            public SlopeLevel(string input)
            {
                rawInput = input;
            }

            public bool HasTree(int x)
            {
                return rawInput[x % rawInput.Length] == '#';
            }
        }

        private long RunSlope(int xMod, int yMod)
        {
            int x = 0;
            int y = 0;
            int trees = 0;

            for (int n = 0; n < slopeLevels.Count; n++)
            {
                x += xMod;
                y += yMod;
                if (y < slopeLevels.Count && slopeLevels[y].HasTree(x)) trees++;
            }

            return trees;
        }

        public object GetResult1()
        {
            return RunSlope(3, 1).ToString();
        }

        public object GetResult2()
        {
            return
                (RunSlope(1, 1) * RunSlope(3, 1) * RunSlope(5, 1) * RunSlope(7, 1) * RunSlope(1, 2)).ToString();
        }
    }
}
