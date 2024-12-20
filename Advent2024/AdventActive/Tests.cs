using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.AdventActive;

internal class Tests
{
    [TestCase(2, example, 44)]
    [TestCase(4, example, 30)]
    [TestCase(8, example, 14)]
    [TestCase(6, example, 16)]
    [TestCase(10, example, 10)]
    [TestCase(12, example, 8)]
    [TestCase(20, example, 5)]
    [TestCase(36, example, 4)]
    [TestCase(38, example, 3)]
    [TestCase(40, example, 2)]
    [TestCase(64, example, 1)]
    [TestCase(63, example, 1)]
    [TestCase(100, example, 0)]
    [TestCase(4, simpleTest, 1)]
    [TestCase(5, simpleTest, 0)]
    public void Test1(int timeToSave, string input, object output)
    {
        var sol = new Solution(input)
        {
            TimeToSave = timeToSave
        };

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(285, example2, 50)]
    [TestCase(253, example2, 52)]
    [TestCase(222, example2, 54)]
    [TestCase(193, example2, 56)]
    [TestCase(154, example2, 58)]
    [TestCase(129, example2, 60)]
    [TestCase(106, example2, 62)]
    [TestCase(86, example2, 64)]
    [TestCase(67, example2, 66)]
    [TestCase(55, example2, 68)]
    [TestCase(41, example2, 70)]
    [TestCase(29, example2, 72)]
    [TestCase(7, example2, 74)]
    [TestCase(3, example2, 76)]
    public void Test2(int timeToSave, string input, object output)
    {
        var sol = new Solution(input)
        {
            TimeToSave = timeToSave
        };

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void FindReachableInDistanceShouldFindRoute()
    {
        var sol = new Solution(simpleTest);
        sol.MapPath().Should().BeTrue();

        var reachable = sol.FindReachableInDistance(sol.start, 0, 2);

        reachable.Should().Contain(4);
        reachable.Length.Should().Be(1);
    }

    [TestCase(example, 9, 7)]
    public void ShowOnMap(string input, int x, int y)
    {
        var sol = new Solution(input);

        var grid = sol.grid;

        grid[y][x] = 'X';

        foreach (var row in grid)
        {
            Console.WriteLine(row);
        }
    }

    [TestCase(7, 1, 12)]
    [TestCase(9, 7, 20)]
    public void CanFindExampleCuts(long x, long y, int timeSaved)
    {
        var sol = new Solution(example);
        sol.MapPath().Should().BeTrue();

        var start = new Coordinate2D(x, y);

        var reachable = sol.FindReachableInDistance(start, 0, 2);

        var startIndex = sol.pathIndex[start];

        var cuts = reachable.Select(r => r - startIndex).ToList();

        cuts.Should().Contain(timeSaved);
    }

    public const string example = @"###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############";

    public const string simpleTest = @"#####
#...#
#.#.#
#S#E#
#####";

    public const string example2 = example;
}