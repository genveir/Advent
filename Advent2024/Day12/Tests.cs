using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day12;

internal class Tests
{
    [TestCase(example, 1930)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

#pragma warning disable IDE0060 // Remove unused parameter
    [TestCase("example2", example2, 1206)]
    [TestCase("square", square, 16)]
    [TestCase("line", line, 8)]
    [TestCase("cross", cross, 4 * 4 + 5 * 3 * 4)]
    [TestCase("small", small, 80)]
    [TestCase("bigger", bigger, 236)]
    [TestCase("third", third, 368)]
    [TestCase("inside", inside, 4 + 8 * 8)]
    [TestCase("multipleInside", multipleInside, 4 + 4 + 8 * 10)]
    [TestCase("smallThird", smallThird, 14 * 12 + 8)]
    public void Test2(string id, string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }
#pragma warning restore IDE0060 // Remove unused parameter

    public const string example = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE";

    public const string small = @"AAAA
BBCD
BBCC
EEEC";

    public const string bigger = @"EEEEE
EXXXX
EEEEE
EXXXX
EEEEE";

    public const string third = @"AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA";

    public const string smallThird = @"AAAA
AABA
ABAA
AAAA";

    public const string square = @"AA
AA";

    public const string line = @"AA";

    public const string cross = @"XAX
AAA
XAX";

    public const string inside = @"AAA
AXA
AAA";

    public const string multipleInside = @"AAA
AXA
ARA
AAA";

    public const string example2 = example;
}