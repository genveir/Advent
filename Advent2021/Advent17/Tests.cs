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
        [TestCase(testFar, 478731)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 112)]
        [TestCase(testFar, 33370)]
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

        [Test]
        public void CheckAnswer()
        {
            var sol = new Solution(testFar);

            var results = sol.target.EnumeratePossibleHits();

            foreach(var result in results)
            {
                var hit = sol.target.CalculatePosition(result.xVelocity, result.yVelocity, result.turn);

                Assert.That(hit.X >= sol.target.xMin);
                Assert.That(hit.X <= sol.target.xMax);
                Assert.That(hit.Y >= sol.target.yMin);
                Assert.That(hit.Y <= sol.target.yMax);
            }
        }

        public const string example = @"target area: x=20..30, y=-10..-5";
        public const string testFar = @"target area: x=2000000..2000100, y=-200..0";

        public const string answerExample2 = @"23,-10  25,-9   27,-5   29,-6   22,-6   21,-7   9,0     27,-7   24,-5
25,-7   26,-6   25,-5   6,8     11,-2   20,-5   29,-10  6,3     28,-7
8,0     30,-6   29,-8   20,-10  6,7     6,4     6,1     14,-4   21,-6
26,-10  7,-1    7,7     8,-1    21,-9   6,2     20,-7   30,-10  14,-3
20,-8   13,-2   7,3     28,-8   29,-9   15,-3   22,-5   26,-8   25,-8
25,-6   15,-4   9,-2    15,-2   12,-2   28,-9   12,-3   24,-6   23,-7
25,-10  7,8     11,-3   26,-7   7,1     23,-9   6,0     22,-10  27,-6
8,1     22,-8   13,-4   7,6     28,-6   11,-4   12,-4   26,-9   7,4
24,-10  23,-8   30,-8   7,0     9,-1    10,-1   26,-5   22,-9   6,5
7,5     23,-6   28,-10  10,-2   11,-1   20,-9   14,-2   29,-7   13,-3
23,-5   24,-8   27,-9   30,-7   28,-5   21,-10  7,9     6,6     21,-5
27,-10  7,2     30,-9   21,-8   22,-7   24,-9   20,-6   6,9     29,-5
8,-2    27,-8   30,-5   24,-7";
    }
}
