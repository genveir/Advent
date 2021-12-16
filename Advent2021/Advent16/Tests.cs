using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent16
{
    class Tests
    {
        [TestCase(example, "")]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("C200B40A82", 3)]
        [TestCase("04005AC33890", 54)]
        [TestCase("880086C3E88112", 7)]
        [TestCase("CE00C43D881120", 9)]
        [TestCase("F600BC2D8F", 0)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void huh ()
        {
            var p = new Solution.Packet("11101110000000001101010000001100100000100011000001100000", 0);

                ;
        }

        public const string example = @"";
    }
}
