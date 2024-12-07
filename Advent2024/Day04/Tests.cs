using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day04;

internal class Tests
{
    [TestCase(example, 18)]
    [TestCase(up, 1)]
    [TestCase(down, 1)]
    [TestCase(left, 1)]
    [TestCase(right, 1)]
    [TestCase(upRight, 1)]
    [TestCase(upLeft, 1)]
    [TestCase(downRight, 1)]
    [TestCase(downLeft, 1)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 9)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

    public const string up = @"S
A
M
X";

    public const string down = @"X
M
A
S";

    public const string left = @"SAMX";
    public const string right = @"XMAS";

    public const string upRight = @"   S
  A
 M
X";

    public const string upLeft = @"S
 A
  M
   X";

    public const string downRight = @"X
 M
  A
   S";

    public const string downLeft = @"   X
  M
 A
S";

    public const string example2 = example;
}