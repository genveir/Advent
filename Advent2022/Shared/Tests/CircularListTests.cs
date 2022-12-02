using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tests
{
    class CircularListTests
    {
        [Test]
        public void CircularListIsCircular()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 10; n++) CircularList.Add(n);

            Assert.AreEqual(1, CircularList[1]);
            Assert.AreEqual(1, CircularList[11]);
            Assert.AreEqual(1, CircularList[-9]);
        }

        [Test]
        public void CircularListIndexCanBeHigherThanMaxInt()
        {
            var CircularList = new CircularList<int>();
            CircularList.Add(1);

            Assert.AreEqual(1, CircularList[500_000_000_000_000]);
        }

        [Test]
        public void CanInsertIntoCircularList()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 2; n++) CircularList.Add(n);

            CircularList.Insert(1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(3, CircularList[1]);
            Assert.AreEqual(4, CircularList[2]);
            Assert.AreEqual(1, CircularList[3]);
        }

        [Test]
        public void CanInsertIntoCircularListWithNegativeStartIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 2; n++) CircularList.Add(n);

            CircularList.Insert(-1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(3, CircularList[1]);
            Assert.AreEqual(4, CircularList[2]);
            Assert.AreEqual(1, CircularList[3]);
        }

        [Test]
        public void CanInsertIntoCircularListWithHighStartIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 2; n++) CircularList.Add(n);

            CircularList.Insert(500_000_000_000_001, new List<int>() { 3, 4 });

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(3, CircularList[1]);
            Assert.AreEqual(4, CircularList[2]);
            Assert.AreEqual(1, CircularList[3]);
        }

        [Test]
        public void CanInsertIntoCircularListWithVeryHighStartIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 2; n++) CircularList.Add(n);

            CircularList.Insert(21, new List<int>() { 3, 4 });

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(3, CircularList[1]);
            Assert.AreEqual(4, CircularList[2]);
            Assert.AreEqual(1, CircularList[3]);
        }

        [Test]
        public void CanInsertLargeDataIntoCircularList()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 2; n++) CircularList.Add(n);

            var newList = new List<int>();
            for (int n = 0; n < 100000; n++) newList.Add(n);

            CircularList.Insert(1, newList);

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(0, CircularList[1]);
            Assert.AreEqual(1, CircularList[-1]);
            Assert.AreEqual(99999, CircularList[-2]);
        }

        [Test]
        public void CanReversePartOfCircularList()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 5; n++) CircularList.Add(n);

            CircularList.ReverseRange(1, 3);

            Assert.AreEqual(0, CircularList[0]);
            Assert.AreEqual(3, CircularList[1]);
            Assert.AreEqual(2, CircularList[2]);
            Assert.AreEqual(1, CircularList[3]);
            Assert.AreEqual(4, CircularList[4]);
        }

        [Test]
        public void CanReversePastEndOfCircularList()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 5; n++) CircularList.Add(n);

            CircularList.ReverseRange(3, 3);

            Assert.AreEqual(3, CircularList[0]);
            Assert.AreEqual(1, CircularList[1]);
            Assert.AreEqual(2, CircularList[2]);
            Assert.AreEqual(0, CircularList[3]);
            Assert.AreEqual(4, CircularList[4]);
        }

        [Test]
        public void CanStartReverseOfCircularListWithNegativeIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 5; n++) CircularList.Add(n);

            CircularList.ReverseRange(-1, 2);

            Assert.AreEqual(4, CircularList[0]);
            Assert.AreEqual(1, CircularList[1]);
            Assert.AreEqual(2, CircularList[2]);
            Assert.AreEqual(3, CircularList[3]);
            Assert.AreEqual(0, CircularList[4]);
        }

        [Test]
        public void CanStartReverseOfCircularListWithHighIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 5; n++) CircularList.Add(n);

            CircularList.ReverseRange(9, 2);

            Assert.AreEqual(4, CircularList[0]);
            Assert.AreEqual(1, CircularList[1]);
            Assert.AreEqual(2, CircularList[2]);
            Assert.AreEqual(3, CircularList[3]);
            Assert.AreEqual(0, CircularList[4]);
        }

        [Test]
        public void CanStartReverseOfCircularListWithVeryHighIndex()
        {
            var CircularList = new CircularList<int>();
            for (int n = 0; n < 5; n++) CircularList.Add(n);

            CircularList.ReverseRange(500_000_000_000_004, 2);

            Assert.AreEqual(4, CircularList[0]);
            Assert.AreEqual(1, CircularList[1]);
            Assert.AreEqual(2, CircularList[2]);
            Assert.AreEqual(3, CircularList[3]);
            Assert.AreEqual(0, CircularList[4]);
        }
    }
}
