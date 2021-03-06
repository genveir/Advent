﻿using Advent2019.Shared;
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

        const string testHorizontal = "########";
        const string testVertical = @"#
#
#
#
#
#";

        [TestCase(testCase1, "8")]
        [TestCase(testCase2, "33")]
        [TestCase(testCase3, "41")]
        [TestCase(testCase4, "210")]
        [TestCase(testHorizontal, "2")]
        [TestCase(testVertical, "2")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [Test]
        public void GCDTest()
        {
            Assert.AreEqual(4, Helper.GCD(100, 4));
            Assert.AreEqual(4, Helper.GCD(4, 100));
            Assert.AreEqual(4, Helper.GCD(0, 4));
            Assert.AreEqual(21, Helper.GCD(1071, 462));
        }

        [Test]
        public void AngleTest()
        {
            Assert.AreEqual(0.0d * Math.PI, Helper.GetAngle(0, -1));
            Assert.AreEqual(0.5d * Math.PI, Helper.GetAngle(1, 0));
            Assert.AreEqual(1.0d * Math.PI, Helper.GetAngle(0, 1));
            Assert.AreEqual(1.5d * Math.PI, Helper.GetAngle(-1, 0));
        }

        [TestCase(testCase4, "802")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
