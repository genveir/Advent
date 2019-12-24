using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent24
{
    class Tests
    {
        public const string test1 = @"....#
#..#.
#..##
..#..
#....";

        public const string actual = @"#.##.
###.#
#...#
##..#
.#...";

        [TestCase(test1, "2129920")]
        [TestCase(actual, "32505887")]
        public void Test1(string input, string output)
        {
            Solution.readLineOnPrint = false;
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(test1, "99", 10)]
        public void Test2(string input, string output, int steps)
        {
            Solution.readLineOnPrint = false;
            var sol = new Solution(Shared.Input.InputMode.String, input);
            sol.Setup(true);

            Assert.AreEqual(output, sol.GetBugsAfter(steps).ToString());
        }
    }
}
