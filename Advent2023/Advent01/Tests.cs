using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent01;

class Tests
{
    [TestCase(example, 142)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 281)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";

    public const string example2 = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen";
}
