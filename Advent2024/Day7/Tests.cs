using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day7;

internal class Tests
{
    [TestCase(example, 3749)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 11387)]
    [TestCase("7290: 6 8 6 15", 7290)]
    [TestCase("192: 17 8 14", 192)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";

    public const string example2 = example;
}