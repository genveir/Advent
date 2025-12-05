using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Day05;

class Tests
{
    [TestCase(example, 3)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 14)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"3-5
10-14
16-20
12-18

1
5
8
11
17
32";

    public const string example2 = example;
}
