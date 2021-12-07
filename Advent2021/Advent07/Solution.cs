using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent07
{
    public class Solution : ISolution
    {
        List<long> numbers;

        public Solution(string input)
        {
            numbers = Input.GetNumbers(input, ',').ToList();
        }
        public Solution() : this("Input.txt") { }

        

        public object GetResult1()
        {
            long minCost = long.MaxValue;
            
            for (int n = 0; n < 10000; n++)
            {
                long cost = 0;
                for (int i = 0; i < numbers.Count; i++)
                {
                    cost += Math.Abs(numbers[i] - n);
                }
                if (cost < minCost) minCost = cost;
            }
            return minCost;
        }

        public object GetResult2()
        {
            long minCost = long.MaxValue;

            for (int n = 0; n < 10000; n++)
            {
                long cost = 0;
                for (int i = 0; i < numbers.Count; i++)
                {
                    var x = Math.Abs(numbers[i] - n);

                    var triangle = x * (x + 1) / 2;

                    cost += triangle;
                }
                if (cost < minCost) minCost = cost;
            }
            return minCost;
        }
    }
}
