using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent14
{
    class Tests
    {
        const string sample = @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";

        const string sample2 = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";

        const string evenSmaller = @"mask = 000000000000000000000000000000X1001X
mem[42] = 100";

        [TestCase(sample, 165)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(sample, 1735166787584)]
        [TestCase(sample2, 208)]
        [TestCase(evenSmaller, 400)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void EvenKijkenHoor()
        {
            var sol = new Solution();

            Assert.AreEqual(72978, sol.RunProgram(2).memRoot.GetTreeSize());
        }
    }
}
