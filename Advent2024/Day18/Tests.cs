using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day18;

internal class Tests
{
    [TestCase(example, 22)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input)
        {
            GridMax = 6,
            FallAmount = 12
        };

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, "6,1")]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input)
        {
            GridMax = 6,
            FallAmount = 12
        };

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";

    public const string example2 = example;
}