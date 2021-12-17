using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent17
{
    class Tests
    {
        [TestCase(example, 45)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 112)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [TestCase(7, 2, 0, 0, 0)]
        [TestCase(7, 2, 1, 7, 2)]
        [TestCase(7, 2, 2, 13, 3)]
        [TestCase(7, 2, 3, 18, 3)]

        [TestCase(6, 3, 0, 0, 0)]
        [TestCase(6, 3, 1, 6, 3)]
        [TestCase(6, 3, 2, 11, 5)]
        [TestCase(6, 3, 3, 15, 6)]
        [TestCase(6, 3, 4, 18, 6)]
        [TestCase(6, 3, 5, 20, 5)]
        [TestCase(6, 3, 6, 21, 3)]
        [TestCase(6, 3, 7, 21, 0)]
        [TestCase(6, 3, 8, 21, -4)]
        [TestCase(6, 3, 9, 21, -9)]
        public void CalculationWorks(int xVelocity, int yVelocity, int turns, int x, int y)
        {
            var sol = new Solution(example);

            var pos = sol.target.CalculatePosition(xVelocity, yVelocity, turns);

            Assert.AreEqual(new Coordinate(x, y), pos);
        }

        [TestCase(7, 2, 0, -1)]
        [TestCase(7, 2, 1, -1)]
        [TestCase(7, 2, 2, -1)]
        [TestCase(7, 2, 3, -1)]
        [TestCase(7, 2, 4, -1)]
        [TestCase(7, 2, 5, -1)]
        [TestCase(7, 2, 6, -1)]
        [TestCase(7, 2, 7, 0)]
        [TestCase(7, 2, 8, 1)]
        [TestCase(7, 2, 9, 1)]
        [TestCase(7, 2, 10, 1)]

        [TestCase(6, 3, 8, -1)]
        [TestCase(6, 3, 9, 0)]
        [TestCase(6, 3, 10, 1)]
        public void DetectionWorks(int xVelocity, int yVelocity, int turns, long expected)
        {
            var sol = new Solution(example);

            var pos = sol.target.ValidatePosition(xVelocity, yVelocity, turns);

            Assert.AreEqual(expected, pos);
        }

        [TestCase(7, 2, true)]
        [TestCase(6, 3, true)]
        [TestCase(9, 0, true)]
        [TestCase(17, -4, false)]
        public void CanDetectHit(int xVelocity, int yVelocity, bool expected)
        {
            var sol = new Solution(example);

            var hit = sol.target.IsValidShot(xVelocity, yVelocity);

            Assert.AreEqual(expected, hit);
        }

        [TestCase(7, 2, 3)]
        [TestCase(6, 3, 6)]
        [TestCase(9, 0, 0)]
        [TestCase(17, -4, 0)]
        public void CanGetHighestY(int xVelocity, int yVelocity, int expected)
        {
            var sol = new Solution(example);

            var maxY = sol.target.getMaxY(xVelocity, yVelocity);

            Assert.AreEqual(expected, maxY);
        }

        public const string example = @"target area: x=20..30, y=-10..-5";
    }
}
