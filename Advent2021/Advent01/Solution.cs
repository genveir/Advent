using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent01
{
    public class Solution : ISolution
    {
        List<long> nums = new List<long>();

        public Solution(string input)
        {
            nums = Input.GetNumberLines(input);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            int result = 0;
            long buffer = nums[0];
            for (int n = 1; n < nums.Count; n++)
            {
                if (nums[n] > buffer) result++;
                buffer = nums[n];
            }

            return result;
        }

        public object GetResult2()
        {
            int result = 0;
            long buffer = nums[0] + nums[1] + nums[2];
            for (int n = 3; n < nums.Count; n++)
            {
                if (nums[n] + nums[n - 1] + nums[n - 2] > buffer) result++;
                buffer = nums[n] + nums[n - 1] + nums[n - 2];
            }
            return result;
        }
    }
}
