using Advent2023.AdventActive;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent16;

class Tests
{
    [TestCase(example, 46)]
    [TestCase(test, 5)]
    [TestCase(test2, 8)]
    [TestCase(test3, 8)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input.Trim());

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 51)]
    [TestCase(test, 5)]
    [TestCase(test2, 8)]
    [TestCase(test3, 8)]
    [TestCase(topIsBest, 5)]
    [TestCase(leftIsBest, 5)]
    [TestCase(rightIsBest, 5)]
    [TestCase(bottomIsBest, 5)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input.Trim());

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void TestContamination()
    {
        var sol = new Solution(example);

        sol.GetResult1().Should().Be(46);
        sol.GetResult2().Should().Be(51);
    }

    public const string example = @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";

    public const string example2 = example;

    public const string test = "..../";

    public const string test2 = @"
...\
\../";

    public const string test3 = @"
\..\
\../";

    public const string topIsBest = @"
...
\./
...";

    public const string bottomIsBest = @"
...
/.\
...";

    public const string leftIsBest = @"
.\.
...
./.";

    public const string rightIsBest = @"
./.
...
.\.";
}
