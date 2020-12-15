using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent15
{
    public class Solution : ISolution
    {
        int[] s2;
        int[] s1;
        int t;
        int l;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int[]>("line");

            var nums = inputParser.Parse(lines[0]);

            s2 = new int[30000000];
            s1 = new int[30000000];

            for (t = 1; t < nums.Length; t++)
            {
                l = nums[t - 1];

                s1[l] = s2[l];
                s2[l] = t;

                if (s1[l] == 0) l = 0;
                else l = s2[l] - s1[l];
            }
            l = nums.Last();
        }
        public Solution() : this("Input.txt") { }

        public unsafe void go(int xt)
        {
            fixed (int* s10 = s2, s20 = s1)
            {
                for (; t < xt; t++)
                {
                    int v = s10[l];

                    s20[l] = v; s10[l] = t;

                    l = (v == 0) ? 0 : t - v;
                }
            }
        }

        public object GetResult1()
        {
            go(2020);

            return l;
        }

        public object GetResult2()
        {
            go(30000000);

            return l;
        }
    }
}
