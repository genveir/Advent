using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent2
{
    class Tests
    {
        const string example = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";

        [TestCase(example, "2")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, "1")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
