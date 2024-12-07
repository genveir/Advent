using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day01;

internal class Tests
{
    [TestCase(example, 11)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 31)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"3   4
4   3
2   5
1   3
3   9
3   3";

    public const string example2 = example;
}