using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent10
{
    class Tests
    {
        const string testCase1 = @".#..#
.....
#####
....#
...##";

        const string testCase2 = @"......#.#.
#..#.#....
..#######.
.#.#.###..
.#..#.....
..#....#.#
#..#....#.
.##.#..###
##...#..#.
.#....####";

        const string testCase3 = @".#..#..###
####.###.#
....###.#.
..###.##.#
##.##.#.#.
....###..#
..#.#..#.#
#..#.#.###
.##...##.#
.....#.#..";

        const string testCase4 = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

        const string testCase5 = @".#....#####...#..
##...##.#####..##
##...#...#.#####.
..#.....X...###..
..#.#.....#....##";

        [TestCase(testCase1, "8")]
        [TestCase(testCase2, "33")]
        [TestCase(testCase3, "41")]
        [TestCase(testCase4, "210")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [Test]
        public void GCDTest()
        {
            var sol = new Solution();

            Assert.AreEqual(4, sol.GetGCD(100, 4));
            Assert.AreEqual(4, sol.GetGCD(4, 100));
            Assert.AreEqual(4, sol.GetGCD(0, 4));
            Assert.AreEqual(21, sol.GetGCD(1071, 462));
        }

        [Test]
        public void AngleTest()
        {
            var sol = new Solution();

            Assert.AreEqual(0.0d * Math.PI, sol.GetAngle(0, -1));
            Assert.AreEqual(0.5d * Math.PI, sol.GetAngle(1, 0));
            Assert.AreEqual(1.0d * Math.PI, sol.GetAngle(0, 1));
            Assert.AreEqual(1.5d * Math.PI, sol.GetAngle(-1, 0));
        }

        [TestCase(testCase4, "802")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
