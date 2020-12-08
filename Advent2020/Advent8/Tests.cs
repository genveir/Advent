using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent8
{
    class Tests
    {
        [TestCase("", "")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6", 8)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
