using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent9
{
    public class Solution : ISolution
    {
        List<long> numbers;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            numbers = lines.Select(line => long.Parse(line)).ToList();
        }
        public Solution() : this("Input.txt") { }

        private bool IsSumOfPrevious(int index, int preamble)
        {
            for (int first = 1; first <= preamble; first++)
            {
                for (int second = first + 1; second <= preamble; second++)
                {
                    if (numbers[index] == numbers[index - first] + numbers[index - second]) return true;
                }
            }
            return false;
        }

        private (long smallest, long largest) SumsToN(int index, long numRequired)
        {
            long accumulated = 0;
            List<long> nums = new List<long>();
            while (accumulated < numRequired)
            {
                nums.Add(numbers[index]);
                accumulated += numbers[index++];
            }

            if (accumulated == numRequired && nums.Count > 1)
            {
                nums.Sort();
                return (nums.First(), nums.Last());
            }

            return (0, 0);
        }

        private long _result1;
        public object GetResult1()
        {
            if (_result1 == 0)
            {
                for (int n = 25; n < numbers.Count; n++)
                {
                    if (!IsSumOfPrevious(n, 25))
                    {
                        _result1 = numbers[n];
                    }
                }
            }

            if (_result1 == 0) return "no solution";
            else return _result1;
        }

        public object GetResult2()
        {
            var toFind = (long)GetResult1();
            for (int n = 0; n < numbers.Count; n++)
            {
                var vals = SumsToN(n, toFind);

                if (vals.largest != 0) return vals.smallest + vals.largest;
            }

            return "no solution";
        }
    }
}
