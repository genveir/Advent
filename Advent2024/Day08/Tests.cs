using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day08;

internal class Tests
{
    [TestCase(example, 14)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 34)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void ExampleTest1()
    {
        var sol = new Solution(example);

        sol.CalculateAntiNodes();

        sol.PrintGrid().Should().Be(exampleExpected);
    }

    public const string example = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";

    public const string example2 = example;

    public const string exampleExpected = @"......#....#
...#....0...
....#0....#.
..#....0....
....0....#..
.#....A.....
...#........
#......#....
........A...
.........A..
..........#.
..........#.";
}