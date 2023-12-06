using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent06;

class Tests
{
    [TestCase(example, 288)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 71503)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"Time:      7  15   30
Distance:  9  40  200";

    public const string example2 = example;
}
