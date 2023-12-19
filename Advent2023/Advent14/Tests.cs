using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent14;

class Tests
{
    [TestCase(example, 136)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 64)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void CanRollNorth()
    {
        var sol = new Solution(simpleTest);

        sol.TiltNorth();

        sol.ToString().Should().Be(@".O.
...
...");
    }

    [Test]
    public void CanRollSouth()
    {
        var sol = new Solution(simpleTest);

        sol.TiltSouth();

        sol.ToString().Should().Be(@"...
...
.O.");
    }

    [Test]
    public void CanRollEast()
    {
        var sol = new Solution(simpleTest);

        sol.TiltEast();

        sol.ToString().Should().Be(@"...
..O
...");
    }

    [Test]
    public void CanRollWest()
    {
        var sol = new Solution(simpleTest);

        sol.TiltWest();

        sol.ToString().Should().Be(@"...
O..
...");
    }

    [TestCase(1, cycle1)]
    [TestCase(2, cycle2)]
    [TestCase(3, cycle3)]
    public void CanDoCycles(int num, string expected)
    {
        var sol = new Solution(example);

        for (int n = 0; n < num; n++)
        {
            sol.RunCycle();
        }

        sol.ToString().Should().Be(expected);
    }

    public const string simpleTest = @"...
.O.
...";

    public const string example = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";

    public const string example2 = example;

    public const string cycle1 = @".....#....
....#...O#
...OO##...
.OO#......
.....OOO#.
.O#...O#.#
....O#....
......OOOO
#...O###..
#..OO#....";

    public const string cycle2 = @".....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#..OO###..
#.OOO#...O";

    public const string cycle3 = @".....#....
....#...O#
.....##...
..O#......
.....OOO#.
.O#...O#.#
....O#...O
.......OOO
#...O###.O
#.OOO#...O";
}
