using Advent2024.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day6;

internal class Tests
{
    [TestCase(example, 41)]
    [TestCase("Input.txt", 5101)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 6)]
    [TestCase("Input.txt", 1951)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [TestCase("3,6", example, 3, 6)]
    [TestCase("6,7", example, 6, 7)]
    [TestCase("7,7", example, 7, 7)]
    [TestCase("1,8", example, 1, 8)]
    [TestCase("3,8", example, 3, 8)]
    [TestCase("7,9", example, 7, 9)]
    public void TestActualSpots(string id, string input, int x, int y)
    {
        var sol = new Solution(input);

        sol.GetResult2();

        sol.loopSpots.Should().Contain(new Coordinate2D(x, y));
    }

    public const string example = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

    public const string example2 = example;
}