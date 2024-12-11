using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day11;

internal class Tests
{
    [TestCase(example, 55312)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase("0 1 10 99 999", 7)]
    public void SingleBlink(string input, object output)
    {
        var sol = new Solution(input)
        {
            NumBlinksPt1 = 1
        };

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 65601038650482)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"125 17";

    public const string example2 = example;
}