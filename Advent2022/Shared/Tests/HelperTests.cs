using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tests
{
    internal class HelperTests
    {
        [Test]
        public void CanPivotSquareArray()
        {
            var input = new int[2][];
            input[0] = new int[] { 0, 1 };
            input[1] = new int[] { 2, 3 };

            // 0 1            0 2
            // 2 3  pivots to 1 3

            var pivotted = input.Pivot();

            Assert.AreEqual(0, pivotted[0][0]);
            Assert.AreEqual(2, pivotted[0][1]);
            Assert.AreEqual(1, pivotted[1][0]);
            Assert.AreEqual(3, pivotted[1][1]);
        }

        [Test]
        public void CanPivotRectangularArray()
        {
            var input = new int[2][];
            input[0] = new int[] { 0, 1, 2 };
            input[1] = new int[] { 3, 4, 5 };

            // 0 1 2            0 3
            // 3 4 5  pivots to 1 4
            //                  2 5

            var pivotted = input.Pivot();

            Assert.AreEqual(0, pivotted[0][0]);
            Assert.AreEqual(3, pivotted[0][1]);
            Assert.AreEqual(1, pivotted[1][0]);
            Assert.AreEqual(4, pivotted[1][1]);
            Assert.AreEqual(2, pivotted[2][0]);
            Assert.AreEqual(5, pivotted[2][1]);
        }
    }
}
