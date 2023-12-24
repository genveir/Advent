using FluentAssertions;
using NUnit.Framework;
using System;

namespace Advent2023.Advent24;

class Tests
{
    [TestCase(example, 7, 27, 2)]
    public void Test1(string input, long minTest, long maxTest, object output)
    {
        var sol = new Solution(input);
        sol.TestAreaStart = minTest;
        sol.TestAreaEnd = maxTest;

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 47)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string verticalLine = @"0, 0, 0 @ 0, 1, 0";

    public const string example = @"19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3";

    public const string example2 = example;
}
