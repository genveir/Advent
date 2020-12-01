using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent1
{
    public class Solution : ISolution
    {
        List<int> numbers;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            numbers = lines.Select(l => int.Parse(l)).ToList();
        }
        public Solution() : this("Input.txt") { }

        

        public string GetResult1()
        {
            for (int n = 0; n < numbers.Count; n++)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (numbers[n] + numbers[i] == 2020) return "" + (numbers[n] * numbers[i]);
                }
            }

            return "no solution";
        }

        public string GetResult2()
        {
            for (int n = 0; n < numbers.Count; n++)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    for (int x = 0; x < numbers.Count; x++)
                    {
                        if (numbers[n] + numbers[i] + numbers[x] == 2020) return "" + (numbers[n] * numbers[i] * numbers[x]);
                    }
                    
                }
            }

            return "no solution";
        }
    }
}
