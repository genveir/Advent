using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent1
{
    class Tests
    {
        [TestCase(@"1721
979
366
299
675
1456", "514579")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("0", "no solution")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
