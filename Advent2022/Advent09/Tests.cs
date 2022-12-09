using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent09
{
    class Tests
    {
        [TestCase(example, 13)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example2, 36)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string example = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

        public const string example2 = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
    }
}
