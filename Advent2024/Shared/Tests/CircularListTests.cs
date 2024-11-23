using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Advent2024.Shared.Tests;

internal class CircularListTests
{
    [Test]
    public void CircularListIsCircular()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 10; n++) circularlist.Add(n);

        circularlist[1].Should().Be(1);
        circularlist[11].Should().Be(1);
        circularlist[-9].Should().Be(1);
    }

    [Test]
    public void CircularListIndexCanBeHigherThanMaxInt()
    {
        var circularlist = new CircularList<int> { 1 };

        circularlist[500_000_000_000_000].Should().Be(1);
    }

    [Test]
    public void CanInsertIntoCircularList()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 2; n++) circularlist.Add(n);

        circularlist.Insert(1, [3, 4]);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(3);
        circularlist[2].Should().Be(4);
        circularlist[3].Should().Be(1);
    }

    [Test]
    public void CanInsertIntoCircularListWithNegativeStartIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 2; n++) circularlist.Add(n);

        circularlist.Insert(-1, [3, 4]);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(3);
        circularlist[2].Should().Be(4);
        circularlist[3].Should().Be(1);
    }

    [Test]
    public void CanInsertIntoCircularListWithHighStartIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 2; n++) circularlist.Add(n);

        circularlist.Insert(500_000_000_000_001, [3, 4]);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(3);
        circularlist[2].Should().Be(4);
        circularlist[3].Should().Be(1);
    }

    [Test]
    public void CanInsertIntoCircularListWithVeryHighStartIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 2; n++) circularlist.Add(n);

        circularlist.Insert(21, [3, 4]);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(3);
        circularlist[2].Should().Be(4);
        circularlist[3].Should().Be(1);
    }

    [Test]
    public void CanInsertLargeDataIntoCircularList()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 2; n++) circularlist.Add(n);

        var newList = new List<int>();
        for (int n = 0; n < 100000; n++) newList.Add(n);

        circularlist.Insert(1, newList);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(0);
        circularlist[-1].Should().Be(1);
        circularlist[-2].Should().Be(99999);
    }

    [Test]
    public void CanReversePartOfCircularList()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 5; n++) circularlist.Add(n);

        circularlist.ReverseRange(1, 3);

        circularlist[0].Should().Be(0);
        circularlist[1].Should().Be(3);
        circularlist[2].Should().Be(2);
        circularlist[3].Should().Be(1);
        circularlist[4].Should().Be(4);
    }

    [Test]
    public void CanReversePastEndOfCircularList()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 5; n++) circularlist.Add(n);

        circularlist.ReverseRange(3, 3);

        circularlist[0].Should().Be(3);
        circularlist[1].Should().Be(1);
        circularlist[2].Should().Be(2);
        circularlist[3].Should().Be(0);
        circularlist[4].Should().Be(4);
    }

    [Test]
    public void CanStartReverseOfCircularListWithNegativeIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 5; n++) circularlist.Add(n);

        circularlist.ReverseRange(-1, 2);

        circularlist[0].Should().Be(4);
        circularlist[1].Should().Be(1);
        circularlist[2].Should().Be(2);
        circularlist[3].Should().Be(3);
        circularlist[4].Should().Be(0);
    }

    [Test]
    public void CanStartReverseOfCircularListWithHighIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 5; n++) circularlist.Add(n);

        circularlist.ReverseRange(9, 2);

        circularlist[0].Should().Be(4);
        circularlist[1].Should().Be(1);
        circularlist[2].Should().Be(2);
        circularlist[3].Should().Be(3);
        circularlist[4].Should().Be(0);
    }

    [Test]
    public void CanStartReverseOfCircularListWithVeryHighIndex()
    {
        var circularlist = new CircularList<int>();
        for (int n = 0; n < 5; n++) circularlist.Add(n);

        circularlist.ReverseRange(500_000_000_000_004, 2);

        circularlist[0].Should().Be(4);
        circularlist[1].Should().Be(1);
        circularlist[2].Should().Be(2);
        circularlist[3].Should().Be(3);
        circularlist[4].Should().Be(0);
    }
}