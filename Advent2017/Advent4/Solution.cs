using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent4
{
    public class Solution : ISolution
    {
        string[][] words;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            words = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            return words
                .Where(w => w.Count() == w.Distinct().Count())
                .Count();
        }

        public object GetResult2()
        {
            return words
                .Select(w => w.Select(chr => new string(chr.OrderBy(c => c).ToArray())))
                .Where(w => w.Count() == w.Distinct().Count())
                .Count();

        }
    }
}
