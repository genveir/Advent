using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day06;

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
    [TestCase(sidePocket, 1)]
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
#pragma warning disable IDE0060
    public void TestBetterSpots(string id, string input, int x, int y)
    {
        var sol = new Solution(input);
        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        solver.Solve();

        solver.LoopSpots.Should().ContainKey(new Coordinate2D(x, y));
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

        sol.solver.LoopSpots.Should().ContainKey(new Coordinate2D(x, y));
    }
#pragma warning restore IDE0060

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

    public const string sidePocket = @"#.###
#...#
#^#.#
#.###";
}