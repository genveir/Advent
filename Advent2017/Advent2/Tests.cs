using Advent2017.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent2
{
    class Tests
    {
        const string sample = @"5 1 9 5
7 5 3
2 4 6 8";

        [TestCase(sample, 18)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        const string sample2 = @"5 9 2 8
9 4 7 3
3 8 6 5";

        [TestCase(sample2, 9)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
