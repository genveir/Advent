using Advent2017.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent1
{
    class Tests
    {
        [TestCase("1122", 3)]
        [TestCase("1111", 4)]
        [TestCase("1234", 0)]
        [TestCase("91212129", 9)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("1212", 6)]
        [TestCase("1221", 0)]
        [TestCase("123425", 4)]
        [TestCase("123123", 12)]
        [TestCase("12131415", 4)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
