using Advent2017.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent9
{
    class Tests
    {
        [TestCase("{}", 1)]
        [TestCase("{{{}}}", 6)]
        [TestCase("{{},{}}", 5)]
        [TestCase("{{{},{},{{}}}}", 16)]
        [TestCase("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9)]
        [TestCase("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9)]
        [TestCase("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("{<>}", 0)]
        [TestCase("{<random characters>}", 17)]
        [TestCase("{<<<<>}", 3)]
        [TestCase("{<{!>}>}", 2)]
        [TestCase("{<!!> }", 0)]
        [TestCase("{<!!!>> }", 0)]
        [TestCase("{<{o\"i!a,<{i<a>}", 10)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
