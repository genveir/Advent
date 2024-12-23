using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day22;

class Tests
{
    [TestCase(example, 37327623)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [Test]
    public void TestSingle()
    {
        var sol = new Solution("123");

        sol.Calculate(10);

        sol.bestPrice.Should().Be(6);
    }

    [TestCase(example2, 23)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"1
10
100
2024";

    public const string example2 = @"1
2
3
2024";
}
