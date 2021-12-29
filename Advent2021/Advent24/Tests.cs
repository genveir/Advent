using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent24
{
    class Tests
    {
        [TestCase(example, "")]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            var str = sol.Z.Value.PrintToDepth(3);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, "")]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string example = @"inp w
mul x 0
add x z
mod x 26
div z 1
add x 11
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y 6
mul y x
add z y";
    }
}
