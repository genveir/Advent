using Advent2023.Shared.Mathemancy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023.Shared.Tests.Mathemancy;
public class LinearEquationTests
{
    [TestCase(5, 9, 1, 3)] // y = 5/9x + 1/3
    [TestCase(1, 1, 0, 1)] // y = x + 0
    [TestCase(-1, 1, 0, 1)] // y = -x + 1
    [TestCase(1, 1, -1, 3)] // y = x - 1/3
    [TestCase(0, 1, 0, 1)] // y = 0
    public void CanCreateLinearEquationFromAAndB(long aTop, long aBottom, long bTop, long bBottom)
    {
        var equation = new LinearEquation(new Fraction(aTop, aBottom), new Fraction(bTop, bBottom));

        equation.ToString().Should().Be($"out = {aTop} / {aBottom} in + {bTop} / {bBottom}");
    }

    [TestCase(12, 7, 5, 9, 5, 9, 1, 3)]     // (12, 7) @ 5, 9       => y = 5/9x + 1/3
    [TestCase(12, 12, 1, 1, 1, 1, 0, 1)]    // (12, 12) @ 1, 1      => y = x + 0
    [TestCase(5, 5, 1, 1, 1, 1, 0, 1)]      // (5, 5) @ 1,1         => y = x + 0
    [TestCase(-5, 5, 1, 1, 1, 1, 10, 1)]    // (-5, 5) @ 1,1        => y = x + 10
    [TestCase(5, -5, 1, 1, 1, 1, -10, 1)]   // (5, -5) @ 1,1        => y = x - 10
    [TestCase(5, 5, -1, 1, -1, 1, 10, 1)]   // (5, 5) @ -1,0        => y = -x + 10
    [TestCase(5, 5, 1, -1, -1, 1, 10, 1)]   // (5, 5) @ -1,-1       => y = x + 10
    [TestCase(5, 5, 0, 1, 0, 1, 5, 1)]      // (5, 5) @ 1,0         => y = 5
    public void CanCreateLinearEquationFromValuesAndVector(long x, long y, long dX, long dY,
        long expectedATop, long expectedABottom, long expectedBTop, long expectedBBottom)
    {
        var equation = new LinearEquation(y, x, new(dX, dY));

        var expected = new LinearEquation(new Fraction(expectedATop, expectedABottom), new Fraction(expectedBTop, expectedBBottom));

        equation.Should().Be(expected);
    }

    [TestCase(12, 7, 5, 9, 5, 9, 1, 3)]     // (12, 7) @ 5, 9       => y = 5/9x + 1/3
    [TestCase(12, 12, 1, 1, 1, 1, 0, 1)]    // (12, 12) @ 1, 1      => y = x
    [TestCase(5, 5, 1, 1, 1, 1, 0, 1)]      // (5, 5) @ 1,1         => y = x
    [TestCase(-5, 5, 1, 1, 1, 1, 10, 1)]    // (-5, 5) @ 1,1        => y = x + 10
    [TestCase(5, -5, 1, 1, 1, 1, -10, 1)]   // (5, -5) @ 1,1        => y = x - 10
    [TestCase(5, 5, -1, 1, -1, 1, 10, 1)]   // (5, 5) @ -1,0        => y = -x + 10
    [TestCase(5, 5, 1, -1, -1, 1, 10, 1)]   // (5, 5) @ -1,-1       => y = x + 10
    [TestCase(5, 5, 0, 1, 0, 1, 5, 1)]      // (5, 5) @ 1,0         => y = 5
    public void CanCreateLinearEquationFromCoordinateAndVector(long x, long y, long dX, long dY,
        long expectedATop, long expectedABottom, long expectedBTop, long expectedBBottom)
    {
        var equation = new LinearEquation(new Coordinate2D(x, y), new Fraction(dX, dY));

        var expected = new LinearEquation(new Fraction(expectedATop, expectedABottom), new Fraction(expectedBTop, expectedBBottom));

        equation.Should().Be(expected);
    }

    [TestCase(12, 7, 30, 17, 5, 9, 1, 3)]   // (12,7) - (30,17)     => y = 5/9x + 1/3
    [TestCase(1, 1, 2, 2, 1, 1, 0, 1)]      // (1,1) - (2,2)        => y = x
    [TestCase(1, 1, -1, -1, 1, 1, 0, 1)]    // (1,1) - (-1,-1)      => y = x
    [TestCase(-1, 1, 1, 1, 0, 1, 1, 1)]     // (-1,1) - (1,1)       => y = 1
    [TestCase(-1, 1, -2, 2, -1, 1, 0, 1)]   // (-1,1) - (-2,2)      => y = -x
    public void CanCreateLinearEquationFromTwoCoordinates(long xOne, long yOne, long xTwo, long yTwo,
        long expectedATop, long expectedABottom, long expectedBTop, long expectedBBottom)
    {
        var equation = new LinearEquation(new Coordinate2D(xOne, yOne), new Coordinate2D(xTwo, yTwo));

        var expected = new LinearEquation(new Fraction(expectedATop, expectedABottom), new Fraction(expectedBTop, expectedBBottom));

        equation.Should().Be(expected);
    }

    [Test]
    public void CannotCreateVerticalLineFromTwoCoordinates()
    {
        Assert.Throws<ArgumentException>(() => new LinearEquation(new Coordinate2D(0, 0), new Coordinate2D(0, 1)));
    }

    [TestCase(5, 9, 1, 3, 12, 7, 1)]        // y = 5/9x + 1/3 at 7 => 12
    [TestCase(5, 9, 1, 3, 30, 17, 1)]       // y = 5/9x + 1/3 at 30 => 17
    [TestCase(1, 10, 0, 1, 15, 3, 2)]       // y = 1/10x at 15 => 3/2
    [TestCase(-1, 10, 0, 1, 15, -3, 2)]     // y = -1/10x at 15 => -3/2
    [TestCase(-1, 10, 3, 2, 15, 0, 1)]      // y = -1/10x + 3/2 at 15 => 0
    public void CanGetValueAtLong(long aTop, long aBottom, long bTop, long bBottom, long input, long outTop, long outBottom)
    {
        var equation = new LinearEquation(new Fraction(aTop, aBottom), new Fraction(bTop, bBottom));

        var value = equation.ValueAt(input);
        value.Top.Should().Be(outTop);
        value.Bottom.Should().Be(outBottom);
    }

    [TestCase(5, 9, 1, 3, 12, 1, 7, 1)]        // y = 5/9x + 1/3 at 7 => 12
    [TestCase(5, 9, 1, 3, 30, 1, 17, 1)]       // y = 5/9x + 1/3 at 30 => 17
    [TestCase(1, 10, 0, 1, 15, 1, 3, 2)]       // y = 1/10x at 15 => 3/2
    [TestCase(-1, 10, 0, 1, 15, 1, -3, 2)]     // y = -1/10x at 15 => -3/2
    [TestCase(-1, 10, 3, 2, 15, 1, 0, 1)]      // y = -1/10x + 3/2 at 15 => 0
    [TestCase(5, 9, 1, 3, 132, 5, 15, 1)]        // y = 5/9x + 1/3 at 132/5 => 15
    public void CanGetValueAtFraction(long aTop, long aBottom, long bTop, long bBottom, long inTop, long inBottom, long outTop, long outBottom)
    {
        var equation = new LinearEquation(new Fraction(aTop, aBottom), new Fraction(bTop, bBottom));

        var input = new Fraction(inTop, inBottom);

        var value = equation.ValueAt(input);
        value.Top.Should().Be(outTop);
        value.Bottom.Should().Be(outBottom);
    }

    [TestCase(5, 9, 1, 3, 12, 1, 7, 1)]        // y = 5/9x + 1/3 for 12 => 7
    [TestCase(5, 9, 1, 3, 30, 1, 17, 1)]       // y = 5/9x + 1/3 for 17 => 30
    [TestCase(1, 10, 0, 1, 15, 1, 3, 2)]       // y = 1/10x for 3/2 => 15
    [TestCase(-1, 10, 0, 1, 15, 1, -3, 2)]     // y = -1/10x for -3/2 => 15
    [TestCase(-1, 10, 3, 2, 15, 1, 0, 1)]      // y = -1/10x + 3/2 for 0 => 15
    [TestCase(5, 9, 1, 3, 132, 5, 15, 1)]      // y = 5/9x + 1/3 for 15 => 132/5
    public void CanGetInputForOutput(long aTop, long aBottom, long bTop, long bBottom, long inTop, long inBottom, long outTop, long outBottom)
    {
        var equation = new LinearEquation(new Fraction(aTop, aBottom), new Fraction(bTop, bBottom));

        var output = new Fraction(outTop, outBottom);

        var value = equation.InputFor(output);
        value.Top.Should().Be(inTop);
        value.Bottom.Should().Be(inBottom);
    }

    [Test]
    public void CanTestEquality()
    {
        var equation = new LinearEquation(new Fraction(5, 9), new Fraction(1, 3));
        var equation2 = new LinearEquation(new Coordinate2D(12, 7), new Coordinate2D(30, 17));
        var equation3 = new LinearEquation(new Coordinate2D(1, 1), new Fraction(1, 1));

        (equation == equation2).Should().BeTrue();
        (equation == equation3).Should().BeFalse();
    }

    [Test]
    public void CanTestInequality()
    {
        var equation = new LinearEquation(new Fraction(5, 9), new Fraction(1, 3));
        var equation2 = new LinearEquation(new Coordinate2D(12, 7), new Coordinate2D(30, 17));
        var equation3 = new LinearEquation(new Coordinate2D(1, 1), new Fraction(1, 1));

        (equation != equation2).Should().BeFalse();
        (equation != equation3).Should().BeTrue();
    }

    [Test]
    public void EqualityWorksWithFrameworkEqualitySystem()
    {
        var equation = new LinearEquation(new Fraction(5, 9), new Fraction(1, 3));
        var equation2 = new LinearEquation(new Coordinate2D(12, 7), new Coordinate2D(30, 17));

        HashSet<LinearEquation> equations = new() { equation };

        equations.Contains(equation2).Should().BeTrue();
    }
}
