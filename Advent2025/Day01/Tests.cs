using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Day01;

class Tests
{
    [TestCase(example, 3)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 6)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [TestCase(-168, -2)]
    [TestCase(-1, -1)]
    [TestCase(0, 0)]
    [TestCase(10, 0)]
    [TestCase(168, 1)]
    public void CanGetCorrectFace(int rotation, int face)
    {
        Solution.GetFace(rotation).Should().Be(face);
    }

    public const string example = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";

    public const string example2 = example;
}
