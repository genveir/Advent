using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent09;

class Tests
{
    [TestCase(example, 114)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 2)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45";

    public const string example2 = example;
}
