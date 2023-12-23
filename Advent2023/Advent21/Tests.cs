using Advent2023.Shared;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2023.Advent21;

class Tests
{
    [TestCase(example, 1, 2)]
    [TestCase(example, 2, 4)]
    [TestCase(example, 3, 6)]
    [TestCase(example, 6, 16)]
    public void Test1(string input, long stepsNum, object output)
    {
        var sol = new Solution(input);
        sol.StepsNum = stepsNum;

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example, 6, 16)]
    [TestCase(example, 10, 50)]
    [TestCase(example, 50, 1594)]
    [TestCase(example, 100, 6536)]
    [TestCase(example, 500, 167004)]
    [TestCase(example, 1000, 668697)]
    //[TestCase(example, 5000, 16733044)]
    public void TestWalk(string input, long stepsNum, object output)
    {
        var sol = new Solution(input);
        sol.Quadratic = false;
        sol.LoopStepsNum = stepsNum;

        sol.GetResult2().Should().Be(output);
    }

    [TestCase(exampleWithProp, 6, 36)]
    [TestCase(exampleWithProp, 10, 90)]
    [TestCase(exampleWithProp, 50, 1940)]
    [TestCase(exampleWithProp, 100, 7645)]
    [TestCase(exampleWithProp, 500, 188756)]
    public void Test2(string input, long stepsNum, object output)
    {
        var sol = new Solution(input);
        sol.LoopStepsNum = stepsNum;

        sol.GetResult2().Should().Be(output);
    }

    [TestCase(0, 11, example, example)]
    [TestCase(-11, 22, example, big)]
    public void GridIsCorrect(int from, int to, string input, string output)
    {
        var sol = new Solution(input);
        sol.Reset();

        var gridFromIsPlot = new char[to - from][];
        var start = sol.Start;
        for (int y = from; y < to; y++)
        {
            gridFromIsPlot[y - from] = new char[to - from];
            for (int x = from; x < to; x++)
            {
                var coord = Solution.FromCoordinates(x, y);

                gridFromIsPlot[y - from][x - from] = sol.IsPlot(coord, true) ? '.' : '#';
                if (coord == start) gridFromIsPlot[y - from][x - from] = 'S';
            }
        }

        var fromPlotString = ConvertGridToString(gridFromIsPlot);

        fromPlotString.Should().Be(output);
    }

    private string ConvertGridToString(char[][] grid)
    {
        List<string> lines = new();
        for (int y = 0; y < grid.Length; y++)
        {
            lines.Add(new string(grid[y]));
        }
        return string.Join(Environment.NewLine, lines);
    }

    /*      01234567890
     *    0  ...........
          1  .....###.#.
          2  .###.##..#.
          3  ..#.#...#..
          4  ....#.#....
          5  .##..S####.
          6  .##..#...#.
          7  .......##..
          8  .##.#.####.
          9  .##..##.##.
          0  ...........
    */

    public const string example = @"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........";

    public const string exampleWithProp = @"...........
......##.#.
.###..#..#.
..#.#...#..
....#.#....
.....S.....
.##......#.
.......##..
.##.#.####.
.##...#.##.
...........";

    public const string big = @".................................
.....###.#......###.#......###.#.
.###.##..#..###.##..#..###.##..#.
..#.#...#....#.#...#....#.#...#..
....#.#........#.#........#.#....
.##...####..##...####..##...####.
.##..#...#..##..#...#..##..#...#.
.......##.........##.........##..
.##.#.####..##.#.####..##.#.####.
.##..##.##..##..##.##..##..##.##.
.................................
.................................
.....###.#......###.#......###.#.
.###.##..#..###.##..#..###.##..#.
..#.#...#....#.#...#....#.#...#..
....#.#........#.#........#.#....
.##...####..##..S####..##...####.
.##..#...#..##..#...#..##..#...#.
.......##.........##.........##..
.##.#.####..##.#.####..##.#.####.
.##..##.##..##..##.##..##..##.##.
.................................
.................................
.....###.#......###.#......###.#.
.###.##..#..###.##..#..###.##..#.
..#.#...#....#.#...#....#.#...#..
....#.#........#.#........#.#....
.##...####..##...####..##...####.
.##..#...#..##..#...#..##..#...#.
.......##.........##.........##..
.##.#.####..##.#.####..##.#.####.
.##..##.##..##..##.##..##..##.##.
.................................";
}
