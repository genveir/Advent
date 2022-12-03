using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tests
{
    internal class CoordinateTests
    {
        private static object[][] TwoPositionTestCaseSource =>
            new object[][]
            {
                new object[] { 0, 0 },
                new object[] { 1, 2 },
                new object[] { -1, -2 },
                new object[] { 1, -1 },
                new object[] { -1, 1 },
                new object[] { 1, 0 },
                new object[] { 0, 1 },
                new object[] { long.MaxValue, long.MaxValue },
                new object[] { long.MinValue, long.MinValue }
            };

        private static object[][] ThreePositionTestCaseSource =>
            new object[][]
            {
                        new object[] { 0, 0, 0},
                        new object[] { 1, 2, 3},
                        new object[] { long.MaxValue, long.MaxValue, long.MaxValue},
                        new object[] { -1, -1, -1},
                        new object[] { long.MinValue, long.MinValue, long.MinValue},
                        new object[] { -5, -3, 1},
                        new object[] { 1, 0, 0},
                        new object[] { 0, 1, 0},
                        new object[] { 0, 0, 1 }
            };

        [TestCaseSource(nameof(TwoPositionTestCaseSource))]
        public void CanConstructCoordinateWithTwoParameters(long x, long y)
        {
            var coord = new Coordinate(x, y);

            Assert.AreEqual(x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(null, coord.Z);
        }

        [TestCaseSource(nameof(ThreePositionTestCaseSource))]
        public void CanConstructCoordinateWithThreeParameters(long x, long y, long z)
        {
            var coord = new Coordinate(x, y, z);

            Assert.AreEqual(x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(z, coord.Z);
        }

        [TestCaseSource(nameof(TwoPositionTestCaseSource))]
        public void CanConstructCoordinateFromTwoPositionalArray(long x, long y)
        {
            var values = new long[] { x, y };

            var coord = new Coordinate(values);

            Assert.AreEqual(x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(null, coord.Z);
        }

        [TestCaseSource(nameof(ThreePositionTestCaseSource))]
        public void CanConstructCoordinateFromThreePositionalArray(long x, long y, long z)
        {
            var values = new long[] { x, y, z };

            var coord = new Coordinate(values);

            Assert.AreEqual(x, coord.X);
            Assert.AreEqual(y, coord.Y);
            Assert.AreEqual(z, coord.Z);
        }

        [Test]
        public void CannotConstructCoordinateWithOneValue()
        {
            var values = new long[] { 1 };

            Assert.Throws<ArgumentException>(() => new Coordinate(values));
        }

        [Test]
        public void CannotConstructCoorinateWithFourValues()
        {
            var values = new long[] { 1, 2, 3, 4 };

            Assert.Throws<ArgumentException>(() => new Coordinate(values));
        }

        [TestCaseSource(nameof(TwoPositionTestCaseSource))]
        public void CanShiftXPositionOnTwoValueCoordinate(long x, long y)
        {
            var coord = new Coordinate(x, y);

            var newCoord = coord.ShiftX(1);

            Assert.AreEqual(1 + coord.X, newCoord.X);
            Assert.AreEqual(coord.Y, newCoord.Y);
            Assert.AreEqual(coord.Z, newCoord.Z);
        }

        [TestCaseSource(nameof(TwoPositionTestCaseSource))]
        public void CanShiftYPositionOnTwoValueCoordinate(long x, long y)
        {
            var coord = new Coordinate(x, y);

            var newCoord = coord.ShiftY(1);

            Assert.AreEqual(coord.X, newCoord.X);
            Assert.AreEqual(1 + coord.Y, newCoord.Y);
            Assert.AreEqual(coord.Z, newCoord.Z);
        }

        [TestCaseSource(nameof(TwoPositionTestCaseSource))]
        public void ShiftingZPositionOnTwoValueCoordinateCreatesThreeValueCoordinate(long x, long y)
        {
            var coord = new Coordinate(x, y);

            var newCoord = coord.ShiftZ(1);

            Assert.AreEqual(coord.X, newCoord.X);
            Assert.AreEqual(coord.Y, newCoord.Y);
            Assert.AreEqual(1, newCoord.Z);
        }
    }
}
