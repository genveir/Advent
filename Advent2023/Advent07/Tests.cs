using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent07;

class Tests
{
    [TestCase(example, 6440)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 5905)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [Test]
    public void CanOrderOnJacks()
    {
        var sol = new Solution(jackTest);

        sol.Hands[0].P2Value.Should().Be(long.Parse("9" + "0012121212"));
        sol.Hands[1].P2Value.Should().Be(long.Parse("9" + "0000000013"));
        sol.Hands[2].P2Value.Should().Be(long.Parse("9" + "0202020202"));
    }

    public const string jackTest = @"JQQQQ 10
JJJJK 10
22222 10";

    public const string example = @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";

    public const string example2 = example;
}
