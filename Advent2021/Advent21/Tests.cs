using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent21
{
    class Tests
    {
        [TestCase(example, 739785)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(4, 8);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 444356092776315)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(4, 8);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void TestWithVerySimpleState()
        {
            // if p1 rolls 3 p2 wins, otherwise p1 wins. Chance for 1,1,1 = 1/27 so should be (26,1)
            // no, p2 also rolls, so p1 wins in 26 universes, p2 wins in 1 * 27 = 27
            var state = new Solution.State()
            {
                p1Pos = 10,
                p2Pos = 10,
                p1Score = 17,
                p2Score = 20,
                p1sTurn = true
            };

            var calc = new Solution().CalculateWins(state);

            Assert.AreEqual(26, calc.p1Wins);
            Assert.AreEqual(27, calc.p2Wins);
        }

        public const string example = @"";
    }
}
