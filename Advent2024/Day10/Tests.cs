using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day10;

internal class Tests
{
    [TestCase(example, 36)]
    [TestCase(line, 1)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 81)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";

    public const string line = "0123456789";

    public const string example2 = example;
}