using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day20;

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

    [TestCase(50, example2, 285)]
    [TestCase(52, example2, 253)]
    [TestCase(54, example2, 222)]
    [TestCase(56, example2, 193)]
    [TestCase(58, example2, 154)]
    [TestCase(60, example2, 129)]
    [TestCase(62, example2, 106)]
    [TestCase(64, example2, 86)]
    [TestCase(66, example2, 67)]
    [TestCase(68, example2, 55)]
    [TestCase(70, example2, 41)]
    [TestCase(72, example2, 29)]
    [TestCase(74, example2, 7)]
    [TestCase(76, example2, 3)]
    public void Test2(int timeToSave, string input, object output)
    {
        var sol = new Solution(input)
        {
            TimeToSave = timeToSave
        };

        sol.GetResult2().Should().Be(output);
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



    [TestCase(9, 1, 12)]
    public void CanFindExampleCuts2(long x, long y, int timeSaved)
    {
        var sol = new Solution(example);
        sol.MapPath().Should().BeTrue();

        var target = new Coordinate2D(x, y);

        var pathArray = sol.GetPathArray();

        var possibleCuts = sol.FindPossibleCuts(target, pathArray);

        possibleCuts.Where(pc => pc.Distance <= 2).Select(pc => pc.Gain).Should().Contain(timeSaved);
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