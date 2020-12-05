using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent5
{
    class Tests
    {
        [TestCase("FBFBBFFRLR", "357")]
        [TestCase("BFFFBBFRRR", "567")]
        [TestCase("FFFBBBFRRR", "119")]
        [TestCase("BBFFBBFRLL", "820")]
        public void Test1(string input, string output)
        {
            var sol = new AltSolution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "")]
        public void Test2(string input, string output)
        {
            var sol = new AltSolution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
