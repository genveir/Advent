using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Shared.Tests;
internal class ModNumTests
{
    [TestCase(5, 3, 2)]
    [TestCase(-5, 3, 1)]
    [TestCase(121, 121, 0)]
    [TestCase(2404, 101, 2404 % 101)]
    public void CanGetModNum(long number, long modulo, long expected)
    {
        var modNum = new ModNum(number, modulo);
        modNum.number.Should().Be(expected);
        modNum.modulo.Should().Be(modulo);
    }
}
