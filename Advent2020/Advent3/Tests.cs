using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent3
{
    class Tests
    {
        [TestCase("", "0")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "0")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
