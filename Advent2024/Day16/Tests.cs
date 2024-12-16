using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day16;

internal class Tests
{
    [TestCase(test, 2002)]
    [TestCase(example, 7036)]
    [TestCase(exampleTwo, 11048)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase("example", example, 45)]
    [TestCase("exampleTwo", exampleTwo, 64)]
    [TestCase("test", test, 3)]
    [TestCase("twoWay", twoWay, 8)]
    [TestCase("simpleBranch", simpleBranch, 10)]
    public void Test2(string id, string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string test = @"#####
#E.S#
#####";

    public const string twoWay = @"#####
#...#
#S#E#
#...#
#####";

    public const string simpleBranch = @"#####
##S##
#...#
#.#.#
#...#
##E##
#####";

    public const string example = @"###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############";

    public const string exampleTwo = @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";

    public const string example2 = example;
}