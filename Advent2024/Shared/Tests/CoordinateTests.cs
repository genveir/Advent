using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Shared.Tests;

internal class CoordinateTests
{
    private static object[][] TwoPositionTestCaseSource =>
        [
            [0, 0],
            [1, 2],
            [-1, -2],
            [1, -1],
            [-1, 1],
            [1, 0],
            [0, 1],
            [long.MaxValue, long.MaxValue],
            [long.MinValue, long.MinValue]
        ];

    private static object[][] ThreePositionTestCaseSource =>
        [
                    [0, 0, 0],
                    [1, 2, 3],
                    [long.MaxValue, long.MaxValue, long.MaxValue],
                    [-1, -1, -1],
                    [long.MinValue, long.MinValue, long.MinValue],
                    [-5, -3, 1],
                    [1, 0, 0],
                    [0, 1, 0],
                    [0, 0, 1]
        ];

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanConstructCoordinate2DWithTwoParameters(long x, long y)
    {
        var coord = new Coordinate2D(x, y);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanConstructCoordinate3DWithTwoParameters(long x, long y)
    {
        var coord = new Coordinate3D(x, y);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(0);
    }

    [TestCaseSource(nameof(ThreePositionTestCaseSource))]
    public void CanConstructCoordinateWithThreeParameters(long x, long y, long z)
    {
        var coord = new Coordinate3D(x, y, z);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(z);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanConstructCoordinate2DFromTwoPositionalArray(long x, long y)
    {
        var values = new[] { x, y };

        var coord = new Coordinate2D(values);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanConstructCoordinate3DFromTwoPositionalArray(long x, long y)
    {
        var values = new[] { x, y };

        var coord = new Coordinate3D(values);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(0);
    }

    [TestCaseSource(nameof(ThreePositionTestCaseSource))]
    public void CanConstructCoordinate3DFromThreePositionalArray(long x, long y, long z)
    {
        var values = new[] { x, y, z };

        var coord = new Coordinate3D(values);

        coord.X.Should().Be(x);
        coord.Y.Should().Be(y);
        coord.Z.Should().Be(z);
    }

    [Test]
    public void CannotConstructCoordinate2DWithOneValue()
    {
        var values = new long[] { 1 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate2D(values));
    }

    [Test]
    public void CannotConstructCoordinate2DWithThreeValues()
    {
        var values = new long[] { 1, 2, 3 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate2D(values));
    }

    [Test]
    public void CannotConstructCoordinate3DWithOneValue()
    {
        var values = new long[] { 1 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate3D(values));
    }

    [Test]
    public void CannotConstructCoordinate3DWithFourValues()
    {
        var values = new long[] { 1, 2, 3, 4 };

        Assert.Throws<ArgumentException>(() => _ = new Coordinate3D(values));
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanShiftXPositionOnTwoValueCoordinate(long x, long y)
    {
        var coord = new Coordinate2D(x, y);

        var newCoord = coord.ShiftX(1);

        newCoord.X.Should().Be(1 + coord.X);
        newCoord.Y.Should().Be(coord.Y);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void CanShiftYPositionOnTwoValueCoordinate(long x, long y)
    {
        var coord = new Coordinate2D(x, y);

        var newCoord = coord.ShiftY(1);

        newCoord.X.Should().Be(coord.X);
        newCoord.Y.Should().Be(1 + coord.Y);
    }

    [TestCaseSource(nameof(TwoPositionTestCaseSource))]
    public void ShiftingZPositionOnTwoValueCoordinateCreatesThreeValueCoordinate(long x, long y)
    {
        var coord = new Coordinate2D(x, y);

        var newCoord = coord.ShiftZ(1);

        newCoord.X.Should().Be(coord.X);
        newCoord.Y.Should().Be(coord.Y);
        newCoord.Z.Should().Be(1);
    }

    [Test]
    public void CanGetNeighboursInTwoDimensions()
    {
        var coord = new Coordinate2D(10, -5);

        var neighbours = coord.GetNeighbours();

        neighbours.Should().HaveCount(8);
        neighbours.Should().Contain(new Coordinate2D(9, -6));
        neighbours.Should().Contain(new Coordinate2D(10, -6));
        neighbours.Should().Contain(new Coordinate2D(11, -6));
        neighbours.Should().Contain(new Coordinate2D(9, -5));
        neighbours.Should().Contain(new Coordinate2D(11, -5));
        neighbours.Should().Contain(new Coordinate2D(9, -4));
        neighbours.Should().Contain(new Coordinate2D(10, -4));
        neighbours.Should().Contain(new Coordinate2D(11, -4));
    }

    [Test]
    public void CanGetOrthogonalNeighboursInTwoDimensions()
    {
        var coord = new Coordinate2D(10, -5);

        var neighbours = coord.GetNeighbours(orthogonalOnly: true);

        neighbours.Should().HaveCount(4);
        neighbours.Should().Contain(new Coordinate2D(10, -6));
        neighbours.Should().Contain(new Coordinate2D(9, -5));
        neighbours.Should().Contain(new Coordinate2D(11, -5));
        neighbours.Should().Contain(new Coordinate2D(10, -4));
    }

    [Test]
    public void CanGetNeighboursInThreeDimensions()
    {
        var coord = new Coordinate3D(10, -5, 1000);

        var neighbours = coord.GetNeighbours();

        neighbours.Should().HaveCount(26);
        neighbours.Should().Contain(new Coordinate3D(9, -6, 999));
        neighbours.Should().Contain(new Coordinate3D(10, -6, 999));
        neighbours.Should().Contain(new Coordinate3D(11, -6, 999));
        neighbours.Should().Contain(new Coordinate3D(9, -5, 999));
        neighbours.Should().Contain(new Coordinate3D(10, -5, 999));
        neighbours.Should().Contain(new Coordinate3D(11, -5, 999));
        neighbours.Should().Contain(new Coordinate3D(9, -4, 999));
        neighbours.Should().Contain(new Coordinate3D(10, -4, 999));
        neighbours.Should().Contain(new Coordinate3D(11, -4, 999));
        neighbours.Should().Contain(new Coordinate3D(9, -6, 1000));
        neighbours.Should().Contain(new Coordinate3D(10, -6, 1000));
        neighbours.Should().Contain(new Coordinate3D(11, -6, 1000));
        neighbours.Should().Contain(new Coordinate3D(9, -5, 1000));
        neighbours.Should().Contain(new Coordinate3D(11, -5, 1000));
        neighbours.Should().Contain(new Coordinate3D(9, -4, 1000));
        neighbours.Should().Contain(new Coordinate3D(10, -4, 1000));
        neighbours.Should().Contain(new Coordinate3D(11, -4, 1000));
        neighbours.Should().Contain(new Coordinate3D(9, -6, 1001));
        neighbours.Should().Contain(new Coordinate3D(10, -6, 1001));
        neighbours.Should().Contain(new Coordinate3D(11, -6, 1001));
        neighbours.Should().Contain(new Coordinate3D(9, -5, 1001));
        neighbours.Should().Contain(new Coordinate3D(10, -5, 1001));
        neighbours.Should().Contain(new Coordinate3D(11, -5, 1001));
        neighbours.Should().Contain(new Coordinate3D(9, -4, 1001));
        neighbours.Should().Contain(new Coordinate3D(10, -4, 1001));
        neighbours.Should().Contain(new Coordinate3D(11, -4, 1001));
    }

    [Test]
    public void CanGetOrthogonalNeighboursInThreeDimensions()
    {
        var coord = new Coordinate3D(10, -5, 1000);

        var neighbours = coord.GetNeighbours(orthogonalOnly: true);

        neighbours.Should().HaveCount(6);
        neighbours.Should().Contain(new Coordinate3D(10, -5, 999));
        neighbours.Should().Contain(new Coordinate3D(10, -6, 1000));
        neighbours.Should().Contain(new Coordinate3D(9, -5, 1000));
        neighbours.Should().Contain(new Coordinate3D(11, -5, 1000));
        neighbours.Should().Contain(new Coordinate3D(10, -4, 1000));
        neighbours.Should().Contain(new Coordinate3D(10, -5, 1001));
    }
}