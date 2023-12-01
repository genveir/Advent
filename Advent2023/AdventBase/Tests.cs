using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.AdventBase;

class Tests
{
    [TestCase(example, "")]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        output.Should().Be(sol.GetResult1());
    }

    [TestCase(example, "")]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        output.Should().Be(sol.GetResult2());
    }

    public const string example = @"";
}
