using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent10;

class Tests
{
    [TestCase(example, 8)]
    [TestCase(test1, 4)]
    [TestCase(test2, 4)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 10)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

    public const string test1 = @".....
F-7
S.|
L-J";

    public const string test2 = @".....
F-7
|.S
L-J";

    public const string example2 = @"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L";
}
