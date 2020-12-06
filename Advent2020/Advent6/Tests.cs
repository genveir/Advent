using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent6
{
    class Tests
    {
        [TestCase(@"abc

a
b
c

ab
ac

a
a
a
a

b", 11)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "")]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
