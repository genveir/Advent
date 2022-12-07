using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent06
{
    public class Solution : ISolution
    {
        public string signal;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            signal = lines[0];
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            for (int n = 0; n < signal.Length; n++)
            {
                var hs = new HashSet<char>
                {
                    signal[n],
                    signal[n + 1],
                    signal[n + 2],
                    signal[n + 3]
                };

                if (hs.Count == 4) return n + 4;
            }

            return "";
        }

        public object GetResult2()
        {
            for (int n = 0; n < signal.Length; n++)
            {
                var hs = new HashSet<char>
                {
                    signal[n],
                    signal[n + 1],
                    signal[n + 2],
                    signal[n + 3],
                    signal[n + 4],
                    signal[n + 5],
                    signal[n + 6],
                    signal[n + 7],
                    signal[n + 8],
                    signal[n + 9],
                    signal[n + 10],
                    signal[n + 11],
                    signal[n + 12],
                    signal[n + 13]
                };

                if (hs.Count == 14) return n + 14;
            }

            return "";
        }
    }
}
