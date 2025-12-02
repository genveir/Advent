using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Shared.Tests;

internal class HelperTests
{
    [Test]
    public void CanPivotSquareArray()
    {
        var input = new int[2][];
        input[0] = [0, 1];
        input[1] = [2, 3];

        // 0 1            0 2
        // 2 3  pivots to 1 3

        var pivotted = input.Pivot();

        pivotted[0][0].Should().Be(0);
        pivotted[0][1].Should().Be(2);
        pivotted[1][0].Should().Be(1);
        pivotted[1][1].Should().Be(3);
    }

    [Test]
    public void CanPivotRectangularArray()
    {
        var input = new int[2][];
        input[0] = [0, 1, 2];
        input[1] = [3, 4, 5];

        // 0 1 2            0 3
        // 3 4 5  pivots to 1 4
        //                  2 5

        var pivotted = input.Pivot();

        pivotted[0][0].Should().Be(0);
        pivotted[0][1].Should().Be(3);
        pivotted[1][0].Should().Be(1);
        pivotted[1][1].Should().Be(4);
        pivotted[2][0].Should().Be(2);
        pivotted[2][1].Should().Be(5);
    }
}