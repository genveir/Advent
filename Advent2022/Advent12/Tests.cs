using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent12
{
    class Tests
    {
        [TestCase(example, 31)]
        [TestCase(example2, 29)]
        [TestCase(diagDiff, 26)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input.Trim());

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 29)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string example = @"
Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

        public const string diagDiff = @"
Saaaaaaaaaaaaaaaaaaaaaaaaa
abcdefghijklmnopqrstuvwxyE";

        public const string example2 = @"aabqponm
abcryxxl
accszExk
acctuvwj
Sbdefghi";
    }
}
