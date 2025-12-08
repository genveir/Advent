using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Day08;

class Tests
{
    [TestCase(example, 40)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);
        Solution.NumLinksToMake = 10;

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 25272)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"162,817,812
57,618,57
906,360,560
592,479,940
352,342,300
466,668,158
542,29,236
431,825,988
739,650,466
52,470,668
216,146,977
819,987,18
117,168,530
805,96,715
346,949,466
970,615,88
941,993,340
862,61,35
984,92,344
425,690,689";

    public const string example2 = example;
}
