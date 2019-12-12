using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent12
{
    class Tests
    {
        const string testCase1 = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";

        const string testCase2 = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";

        [TestCase(testCase1, "179", 10)]
        [TestCase(testCase2, "1940", 100)]
        public void Test1(string input, string output, int numSteps)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);
            sol.numSteps = numSteps;

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(testCase1, "2772")]
        [TestCase(testCase2, "4686774924")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void QuickCheck()
        {
            var sol = new Solution(Shared.Input.InputMode.String, testCase1);

            foreach (var moon in Solution.moons) Assert.That(moon.MatchesStart);
        }
    }
}
