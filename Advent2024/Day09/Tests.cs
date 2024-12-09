using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day09;

internal class Tests
{
    [TestCase(example, 1928)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 2858)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void Test2Example()
    {
        var sol = new Solution(example);

        sol.Defragment();

        var expected = new Solution.Block[]
        {
            new(0, 0, 2),
            new(9, 2, 2),
            new(2, 4, 1),
            new(1, 5, 3),
            new(7, 8, 3),
            new(4, 12, 2),
            new(3, 15, 3),
            new(5, 22, 4),
            new(6, 27, 4),
            new(8, 36, 4)
        };

        sol.Blocks.Should().BeEquivalentTo(expected);
    }

    public const string example = @"2333133121414131402";

    public const string example2 = example;
}