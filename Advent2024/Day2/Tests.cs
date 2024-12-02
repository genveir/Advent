using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day2;

internal class Tests
{
    [TestCase(example, 2)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase("7 6 4 2 1", 1)]
    [TestCase("1 2 7 8 9", 0)]
    [TestCase("9 7 6 2 1", 0)]
    [TestCase("1 3 2 4 5", 1)]
    [TestCase("8 6 4 4 1", 1)]
    [TestCase("1 3 6 7 9", 1)]
    [TestCase(example2, 4)]
    [TestCase("9 1 2 3 4", 1)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

    public const string example2 = example;
}