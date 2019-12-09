using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent9
{
    class Tests
    {
        [TestCase("104,1125899906842624,99", "1125899906842624,")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
