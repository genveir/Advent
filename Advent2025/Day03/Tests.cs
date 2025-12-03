using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Day03;

class Tests
{
    [TestCase(example, 357)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 3121910778619)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"987654321111111
811111111111119
234234234234278
818181911112111";

    public const string example2 = example;
}
