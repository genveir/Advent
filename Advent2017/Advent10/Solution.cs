using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent10
{
    public class Solution : ISolution
    {
        private long[] numbers = Enumerable.Range(0, 256).Select(i => (long)i).ToArray();
        private int currentPosition = 0;
        private int skipSize = 0;
        private long[] lengths;

        public Solution(string input)
        {
            lengths = Input.GetNumbers(input).ToArray();            
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
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
