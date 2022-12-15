using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Advent2022.Advent15.Solution;

namespace Advent2022.Advent15
{
    class Tests
    {
        [TestCase(example, 10, 26)]
        public void Test1(string input, long row, object output)
        {
            var sol = new Solution(input);
            sol.Row = row;

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 6, -3, null, null)]
        [TestCase(example, 6, -2, 8, 8)]
        [TestCase(example, 6, -1, 7, 9)]
        [TestCase(example, 6, 7, -1, 17)]
        public void TestSegments(string input, int sensorIndex, long row, long? expectedLeft, long? expectedRight)
        {
            var sol = new Solution(input);

            var result = sol.sensors[sensorIndex].GetSegmentOnRow(row);

            RowSegment expected = expectedLeft == null ?
                new RowSegment(false, row, 0, 0) :
                new RowSegment(true, row, expectedLeft.Value, expectedRight.Value);
            Assert.AreEqual(expected, result);
        }

        [TestCase(example, 56000011)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string example = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";
    }
}
