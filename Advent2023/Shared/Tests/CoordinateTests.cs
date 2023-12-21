using System;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Shared.Tests;

internal class CoordinateTests
{
    private static object[][] TwoPositionTestCaseSource =>
        new[]
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
        new[]
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

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().BeNull();
    }

    [TestCaseSource(nameof(ThreePositionTestCaseSource))]
    public void CanConstructCoordinateWithThreeParameters(long x, long y, long z)
    {
        var coord = new Coordinate(x, y, z);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(z);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanConstructCoordinateFromTwoPositionalArray(long x, long y)
    {
        var values = new[] { x, y };

        var coord = new Coordinate(values);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().BeNull();
    }

    [TestCaseSource(nameof(ThreePositionTestCaseSource))]
    public void CanConstructCoordinateFromThreePositionalArray(long x, long y, long z)
    {
        var values = new[] { x, y, z };

        var coord = new Coordinate(values);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(z);
    }

    [Test]
    public void CannotConstructCoordinateWithOneValue()
    {
        var values = new long[] { 1 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate(values));
    }

    [Test]
    public void CannotConstructCoorinateWithFourValues()
    {
        var values = new long[] { 1, 2, 3, 4 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate(values));
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanShiftXPositionOnTwoValueCoordinate(long x, long y)
    {
        var coord = new Coordinate(x, y);

        var newCoord = coord.ShiftX(1);
        
        newCoord.X.Should().Be(1 + coord.X);
        newCoord.Y.Should().Be(coord.Y);
        newCoord.Z.Should().Be(coord.Z);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanShiftYPositionOnTwoValueCoordinate(long x, long y)
    {
        var coord = new Coordinate(x, y);

        var newCoord = coord.ShiftY(1);

        newCoord.X.Should().Be(coord.X);
        newCoord.Y.Should().Be(1 + coord.Y);
        newCoord.Z.Should().Be(coord.Z);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void ShiftingZPositionOnTwoValueCoordinateCreatesThreeValueCoordinate(long x, long y)
    {
        var coord = new Coordinate(x, y);

        var newCoord = coord.ShiftZ(1);

        newCoord.X.Should().Be(coord.X);
        newCoord.Y.Should().Be(coord.Y);
        newCoord.Z.Should().Be(1);
    }

    [Test]
    public void CanGetNeighboursInTwoDimensions()
    {
        var coord = new Coordinate(10, -5);

        var neighbours = coord.GetNeighbours();

        neighbours.Should().HaveCount(8);
        neighbours.Should().Contain(new Coordinate(9, -6));
        neighbours.Should().Contain(new Coordinate(10, -6));
        neighbours.Should().Contain(new Coordinate(11, -6));
        neighbours.Should().Contain(new Coordinate(9, -5));
        neighbours.Should().Contain(new Coordinate(11, -5));
        neighbours.Should().Contain(new Coordinate(9, -4));
        neighbours.Should().Contain(new Coordinate(10, -4));
        neighbours.Should().Contain(new Coordinate(11, -4));
    }

    [Test]
    public void CanGetOrthogonalNeighboursInTwoDimensions()
    {
        var coord = new Coordinate(10, -5);

        var neighbours = coord.GetNeighbours(orthogonalOnly: true);

        neighbours.Should().HaveCount(4);
        neighbours.Should().Contain(new Coordinate(10, -6));
        neighbours.Should().Contain(new Coordinate(9, -5));
        neighbours.Should().Contain(new Coordinate(11, -5));
        neighbours.Should().Contain(new Coordinate(10, -4));
    }

    [Test]
    public void CanGetNeighboursInThreeDimensions()
    {
        var coord = new Coordinate(10, -5, 1000);

        var neighbours = coord.GetNeighbours();

        neighbours.Should().HaveCount(26);
        neighbours.Should().Contain(new Coordinate(9, -6 , 999));
        neighbours.Should().Contain(new Coordinate(10, -6, 999));
        neighbours.Should().Contain(new Coordinate(11, -6, 999));
        neighbours.Should().Contain(new Coordinate(9, -5 , 999));
        neighbours.Should().Contain(new Coordinate(10, -5, 999));
        neighbours.Should().Contain(new Coordinate(11, -5, 999));
        neighbours.Should().Contain(new Coordinate(9, -4 , 999));
        neighbours.Should().Contain(new Coordinate(10, -4, 999));
        neighbours.Should().Contain(new Coordinate(11, -4, 999));
        neighbours.Should().Contain(new Coordinate(9, -6 , 1000));
        neighbours.Should().Contain(new Coordinate(10, -6, 1000));
        neighbours.Should().Contain(new Coordinate(11, -6, 1000));
        neighbours.Should().Contain(new Coordinate(9, -5 , 1000));
        neighbours.Should().Contain(new Coordinate(11, -5, 1000));
        neighbours.Should().Contain(new Coordinate(9, -4 , 1000));
        neighbours.Should().Contain(new Coordinate(10, -4, 1000));
        neighbours.Should().Contain(new Coordinate(11, -4, 1000));
        neighbours.Should().Contain(new Coordinate(9, -6 , 1001));
        neighbours.Should().Contain(new Coordinate(10, -6, 1001));
        neighbours.Should().Contain(new Coordinate(11, -6, 1001));
        neighbours.Should().Contain(new Coordinate(9, -5 , 1001));
        neighbours.Should().Contain(new Coordinate(10, -5, 1001));
        neighbours.Should().Contain(new Coordinate(11, -5, 1001));
        neighbours.Should().Contain(new Coordinate(9, -4 , 1001));
        neighbours.Should().Contain(new Coordinate(10, -4, 1001));
        neighbours.Should().Contain(new Coordinate(11, -4, 1001));
    }

    [Test]
    public void CanGetOrthogonalNeighboursInThreeDimensions()
    {
        var coord = new Coordinate(10, -5, 1000);

        var neighbours = coord.GetNeighbours(orthogonalOnly: true);

        neighbours.Should().HaveCount(6);
        neighbours.Should().Contain(new Coordinate(10, -5, 999));
        neighbours.Should().Contain(new Coordinate(10, -6, 1000));
        neighbours.Should().Contain(new Coordinate(9, -5 , 1000));
        neighbours.Should().Contain(new Coordinate(11, -5, 1000));
        neighbours.Should().Contain(new Coordinate(10, -4, 1000));
        neighbours.Should().Contain(new Coordinate(10, -5, 1001));
    }
}
