using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent1
{
    public class Solution : ISolution
    {
        long[] nums;

        public Solution(string input)
        {
            nums = Input.GetNumbers(input);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            long sum = 0;
            for (int n = 0; n < nums.Length - 1; n++)
            {
                if (nums[n] == nums[n + 1]) sum += nums[n];
            }
            if (nums[nums.Length - 1] == nums[0]) sum += nums[0];
            return sum;
        }

        public object GetResult2()
        {
            long sum = 0;
            for (int n = 0; n < nums.Length; n++)
            {
                var compare = (n + (nums.Length / 2)) % nums.Length;
                if (nums[n] == nums[compare]) sum += nums[n];
            }
            return sum;
        }
    }
}
