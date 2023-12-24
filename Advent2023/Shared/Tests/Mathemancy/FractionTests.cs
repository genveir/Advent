using Advent2023.Shared.Mathemancy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023.Shared.Tests.Mathemancy;
public class FractionTests
{
    [TestCase(1, 5)]
    [TestCase(1, 1)]
    [TestCase(5, 1)]
    [TestCase(10, 2)]
    [TestCase(-1, 5)]
    [TestCase(1, -5)]
    [TestCase(0, -5)]
    public void CanCreateFraction(long top, long bottom)
    {
        Assert.DoesNotThrow(() => _ = new Fraction(1, 5));
    }

    [Test]
    public void CannotCreateFractionWithZeroDenominator()
    {
        Assert.Throws<ArgumentException>(() => _ = new Fraction(10, 0));
    }

    [TestCase(1, 5, 1, 5)]
    [TestCase(2, 10, 1, 5)]
    [TestCase(10, 2, 5, 1)]
    [TestCase(-2, 10, -1, 5)]
    [TestCase(2, -10, -1, 5)]
    [TestCase(4, 10, 2, 5)]
    [TestCase(-4, -10, 2, 5)]
    [TestCase(0, -10, 0, 1)]
    public void FractionIsNormalized(long top, long bottom, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top, bottom);

        frac.Top.Should().Be(expectedTop);
        frac.Bottom.Should().Be(expectedBottom);
    }

    [Test]
    public void FractionRenormalizesWhenUpdatingTop()
    {
        var frac = new Fraction(1, 10);
        frac.Top = 2;

        frac.Top.Should().Be(1);
        frac.Bottom.Should().Be(5);
    }

    [Test]
    public void FractionRenormalizesWhenUpdatingBottom()
    {
        var frac = new Fraction(10, 1);
        frac.Bottom = 2;

        frac.Top.Should().Be(5);
        frac.Bottom.Should().Be(1);
    }

    [Test]
    public void NumeratorGetsTop()
    {
        var frac = new Fraction(5, 1);
        frac.Numerator.Should().Be(frac.Top);

        frac.Top = 10;
        frac.Bottom = 13;
        frac.Numerator.Should().Be(frac.Top);
    }

    [Test]
    public void NumeratorSetsTop()
    {
        var frac = new Fraction(5, 1);

        frac.Numerator = 10;
        frac.Bottom = 13;
        frac.Top.Should().Be(10);
    }

    [Test]
    public void DenominatorGetsBottom()
    {
        var frac = new Fraction(1, 5);
        frac.Denominator.Should().Be(frac.Bottom);

        frac.Top = 10;
        frac.Bottom = 13;
        frac.Denominator.Should().Be(frac.Bottom);
    }

    [Test]
    public void DenominatorSetsBottom()
    {
        var frac = new Fraction(1, 5);

        frac.Top = 10;
        frac.Denominator = 13;
        frac.Bottom.Should().Be(13);
    }

    [TestCase(1, 5, false)]
    [TestCase(10, 1, true)]
    [TestCase(10, 2, true)]
    [TestCase(10, -1, true)]
    [TestCase(-10, 1, true)]
    [TestCase(10, 3, false)]
    public void FractionCanReportIsInteger(long top, long bottom, bool isInteger)
    {
        new Fraction(top, bottom).IsInteger.Should().Be(isInteger);
    }

    [TestCase(5, 1, 5)]
    [TestCase(1, 5, 0)]
    [TestCase(4, 5, 0)]
    [TestCase(6, 5, 1)]
    [TestCase(-1, 5, 0)]
    [TestCase(-4, 5, 0)]
    [TestCase(-5, 5, -1)]
    [TestCase(-6, 5, -1)]
    public void FractionCanConvertToLong(long top, long bottom, long expected)
    {
        new Fraction(top, bottom).ToLong().Should().Be(expected);
    }

    [TestCase(1, 2, 0.5d)]
    [TestCase(1, 4, 0.25d)]
    [TestCase(5, 1, 5.0d)]
    [TestCase(6, 5, 1.2d)]
    [TestCase(6, -5, -1.2d)]
    [TestCase(-6, 5, -1.2d)]
    public void FractionCanConvertToDouble(long top, long bottom, double expected)
    {
        new Fraction(top, bottom).ToDouble().Should().Be(expected);
    }

    [Test]
    public void FractionImplicitlyConvertsFromInt()
    {
        Fraction test = 5;
        test.Top.Should().Be(5);
        test.Bottom.Should().Be(1);
    }

    [Test]
    public void FractionImplicitlyConvertsFromLong()
    {
        Fraction test = 5L;
        test.Top.Should().Be(5);
        test.Bottom.Should().Be(1);
    }

    [Test]
    public void CanCastFractionToLong()
    {
        Fraction test = new(6, 5);
        long converted = (long)test;

        converted.Should().Be(1L);
    }

    [Test]
    public void CanCastFractionToDouble()
    {
        Fraction test = new(6, 5);
        double converted = (double)test;

        converted.Should().Be(1.2d);
    }

    [Test]
    public void CanCastFractionToInt()
    {
        Fraction test = new(6, 5);
        int converted = (int)test;

        converted.Should().Be(1);
    }

    [TestCase(1, 6, -1, 6)]
    [TestCase(-1, 6, 1, 6)]
    [TestCase(1, -6, 1, 6)]
    [TestCase(-1, -6, -1, 6)]
    public void CanNegateFraction(long top, long bottom, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top, bottom);
        var neg = -frac;

        neg.Top.Should().Be(expectedTop);
        neg.Bottom.Should().Be(expectedBottom);
    }

    [TestCase(1, 2, 1, 4, 1, 8)]
    [TestCase(11, 5, 7, 3, 77, 15)]
    [TestCase(-11, 5, 7, 3, -77, 15)]
    [TestCase(11, -5, 7, 3, -77, 15)]
    [TestCase(11, 5, -7, 3, -77, 15)]
    [TestCase(11, 5, 7, -3, -77, 15)]
    [TestCase(11, -5, 7, -3, 77, 15)]
    public void CanMultiplyFractions(long top1, long bottom1, long top2, long bottom2, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top1, bottom1) * new Fraction(top2, bottom2);

        frac.Top.Should().Be(expectedTop);
        frac.Bottom.Should().Be(expectedBottom);
    }

    [TestCase(1, 1, 1, 4, 4, 1)]
    [TestCase(2, 1, 1, 4, 8, 1)]
    [TestCase(2, -1, 1, 4, -8, 1)]
    [TestCase(11, 5, 7, 3, 33, 35)]
    public void CanDivideFractions(long top1, long bottom1, long top2, long bottom2, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top1, bottom1) / new Fraction(top2, bottom2);

        frac.Top.Should().Be(expectedTop);
        frac.Bottom.Should().Be(expectedBottom);
    }

    [TestCase(11, 5, 7, 3, 68, 15)]
    [TestCase(-11, 5, 7, 3, 2, 15)]
    public void CanAddFractions(long top1, long bottom1, long top2, long bottom2, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top1, bottom1) + new Fraction(top2, bottom2);

        frac.Top.Should().Be(expectedTop);
        frac.Bottom.Should().Be(expectedBottom);
    }

    [TestCase(11, 5, 7, 3, -2, 15)]
    [TestCase(-11, 5, 7, 3, -68, 15)]
    public void CanSubtractFractions(long top1, long bottom1, long top2, long bottom2, long expectedTop, long expectedBottom)
    {
        var frac = new Fraction(top1, bottom1) - new Fraction(top2, bottom2);

        frac.Top.Should().Be(expectedTop);
        frac.Bottom.Should().Be(expectedBottom);
    }

    [Test]
    public void CanTestFractionEquality()
    {
        (new Fraction(1, 5) == new Fraction(2, 10)).Should().BeTrue();
        (new Fraction(1, 5) == new Fraction(1, 10)).Should().BeFalse();
        (null == new Fraction(1, 10)).Should().BeFalse();
        (new Fraction(1, 5) == null).Should().BeFalse();
        ((Fraction)null == null).Should().BeTrue();
    }

    [Test]
    public void CanTestFractionInequality()
    {
        (new Fraction(1, 5) != new Fraction(2, 10)).Should().BeFalse();
        (new Fraction(1, 5) != new Fraction(1, 10)).Should().BeTrue();
        (null != new Fraction(1, 10)).Should().BeTrue();
        (new Fraction(1, 5) != null).Should().BeTrue();
        ((Fraction)null != null).Should().BeFalse();
    }

    [TestCase(1, 5, 1, 2, false)]
    [TestCase(1, 2, 1, 5, true)]
    [TestCase(-1, 2, 1, 5, false)]
    [TestCase(-1, 2, -1, 5, false)]
    [TestCase(1, 5, 1, 5, false)]
    public void CanTestFractionGreaterThan(long top1, long bottom1, long top2, long bottom2, bool expected)
    {
        (new Fraction(top1, bottom1) > new Fraction(top2, bottom2)).Should().Be(expected);
    }

    [TestCase(1, 5, 1, 2, false)]
    [TestCase(1, 2, 1, 5, true)]
    [TestCase(-1, 2, 1, 5, false)]
    [TestCase(-1, 2, -1, 5, false)]
    [TestCase(1, 5, 1, 5, true)]
    public void CanTestFractionEqualOrGreaterThan(long top1, long bottom1, long top2, long bottom2, bool expected)
    {
        (new Fraction(top1, bottom1) >= new Fraction(top2, bottom2)).Should().Be(expected);
    }

    [TestCase(1, 5, 1, 2, true)]
    [TestCase(1, 2, 1, 5, false)]
    [TestCase(-1, 2, 1, 5, true)]
    [TestCase(-1, 2, -1, 5, true)]
    [TestCase(1, 5, 1, 5, false)]
    public void CanTestFractionLesserThan(long top1, long bottom1, long top2, long bottom2, bool expected)
    {
        (new Fraction(top1, bottom1) < new Fraction(top2, bottom2)).Should().Be(expected);
    }

    [TestCase(1, 5, 1, 2, true)]
    [TestCase(1, 2, 1, 5, false)]
    [TestCase(-1, 2, 1, 5, true)]
    [TestCase(-1, 2, -1, 5, true)]
    [TestCase(1, 5, 1, 5, true)]
    public void CanTestFractionEqualOrLesserThan(long top1, long bottom1, long top2, long bottom2, bool expected)
    {
        (new Fraction(top1, bottom1) <= new Fraction(top2, bottom2)).Should().Be(expected);
    }

    [Test]
    public void FractionEqualityWorksWithFrameworkEqualitySystem()
    {
        var frac1 = new Fraction(1, 5);
        var frac2 = new Fraction(1, 5);

        new HashSet<Fraction>() { frac1 }.Contains(frac2).Should().BeTrue();
    }
}
