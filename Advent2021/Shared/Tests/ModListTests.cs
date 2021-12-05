using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Shared.Tests
{
    class ModListTests
    {
        [Test]
        public void ModListIsCircular()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 10; n++) modList.Add(n);

            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(1, modList[11]);
            Assert.AreEqual(1, modList[-9]);
        }

        [Test]
        public void ModListIndexCanBeHigherThanMaxInt()
        {
            var modList = new ModList<int>();
            modList.Add(1);

            Assert.AreEqual(1, modList[500_000_000_000_000]);
        }

        [Test]
        public void CanInsertIntoModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithNegativeStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(-1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithHighStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(500_000_000_000_001, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithVeryHighStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(21, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertLargeDataIntoModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            var newList = new List<int>();
            for (int n = 0; n < 100000; n++) newList.Add(n);

            modList.Insert(1, newList);

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(0, modList[1]);
            Assert.AreEqual(1, modList[-1]);
            Assert.AreEqual(99999, modList[-2]);
        }

        [Test]
        public void CanReversePartOfModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(1, 3);

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(1, modList[3]);
            Assert.AreEqual(4, modList[4]);
        }

        [Test]
        public void CanReversePastEndOfModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(3, 3);

            Assert.AreEqual(3, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(0, modList[3]);
            Assert.AreEqual(4, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithNegativeIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(-1, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithHighIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(9, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithVeryHighIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(500_000_000_000_004, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

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
