using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent4
{
    class Tests
    {
        [TestCase(111111, true)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        public void Test1(int input, bool output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, "");

            Assert.AreEqual(output, sol.Test(input, false));
        }

        [TestCase(112233, true)]
        [TestCase(111122, true)]
        [TestCase(123444, false)]
        [TestCase(332211, false)]
        [TestCase(111222, false)]
        public void Test2(int input, bool output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, "");

            Assert.AreEqual(output, sol.Test(input, true));
        }
    }
}
