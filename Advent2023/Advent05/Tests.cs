using FluentAssertions;
using NUnit.Framework;
using System.Linq;

namespace Advent2023.Advent05;

class Tests
{
    [TestCase(example, 35)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 46)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void p2TestAnswerMapsCorrectly()
    {
        var sol = new Solution(example);

        sol.MapSeed(82).Should().Be(46);
    }

    [Test]
    public void p2TestAnswerMapsAsRange()
    {
        var sol = new Solution(example);

        var result = sol.MapSeedRange(82, 1).Should().Be(46);   
    }

    public const string example = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

    public const string example2 = example;
}
