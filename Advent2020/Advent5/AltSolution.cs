using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2020.Advent5
{
    public class AltSolution : ISolution
    {
        List<int> seatIds;
        public AltSolution(string input)
        {
            var rawInput = Input.GetInput(input);
            rawInput = Regex.Replace(rawInput, "F|L", "0");
            rawInput = Regex.Replace(rawInput, "B|R", "1");
            var lines = rawInput.Split(Environment.NewLine);
            seatIds = lines.Select(line => Convert.ToInt32(line, 2)).ToList();
        }
        public AltSolution() : this("Input.txt") { }

        public object GetResult1() { return seatIds.Max(); }

        public object GetResult2() { return Enumerable.Range(seatIds.Min(), seatIds.Count + 1).Except(seatIds).Single(); }
    }
}
