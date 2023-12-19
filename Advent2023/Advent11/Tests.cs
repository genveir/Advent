using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent11;

class Tests
{
    [TestCase(example, 374)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 82000210)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";

    public const string example2 = example;
}
