using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day14;

internal class Tests
{
    [TestCase(example, 11, 7, 12)]
    public void Test1(string input, long width, long height, object output)
    {
        var sol = new Solution(input);
        sol.Width = width;
        sol.Height = height;

        sol.GetResult1().Should().Be(output);
    }

    [Test]
    public void SimulateMatchesExample()
    {
        var bot = new Solution.ParsedInput(new Coordinate2D(2, 4), new Coordinate2D(2, -3));

        bot.SimulateSteps(1, 11, 7).Should().Be(new Coordinate2D(4, 1));
        bot.SimulateSteps(1, 11, 7).Should().Be(new Coordinate2D(6, 5));
        bot.SimulateSteps(1, 11, 7).Should().Be(new Coordinate2D(8, 2));
        bot.SimulateSteps(1, 11, 7).Should().Be(new Coordinate2D(10, 6));
    }

    [TestCase(example)]
    [TestCase("Input.txt")]
    public void SimulateMatchesPosition(string input)
    {
        var sol = new Solution(input);
        var calculated = sol.modules.Select(m => m.PositionAt(100, sol.Width, sol.Height)).ToArray();
        var simulated = sol.modules.Select(m => m.SimulateSteps(100, sol.Width, sol.Height)).ToArray();

        foreach (var (calc, sim) in calculated.Zip(simulated))
        {
            calc.Should().Be(sim);
        }
    }

    [TestCase(11, 7, 4, 2)]
    [TestCase(103, 101, 50, 49)]
    [TestCase(3, 5, 0, 1)]
    public void TestQuadrants(long width, long height, long wExpect, long hExpect)
    {
        var sol = new Solution(example);
        sol.Width = width;
        sol.Height = height;

        var quadrants = sol.MakeQuadrants();

        quadrants[0].TopLeft.Should().Be(new Coordinate2D(0, 0));
        quadrants[0].BottomRight.Should().Be(new Coordinate2D(wExpect, hExpect));

        quadrants[1].TopLeft.Should().Be(new Coordinate2D(wExpect + 2, 0));
        quadrants[1].BottomRight.Should().Be(new Coordinate2D(width - 1, hExpect));

        quadrants[2].TopLeft.Should().Be(new Coordinate2D(0, hExpect + 2));
        quadrants[2].BottomRight.Should().Be(new Coordinate2D(wExpect, height - 1));

        quadrants[3].TopLeft.Should().Be(new Coordinate2D(wExpect + 2, hExpect + 2));
        quadrants[3].BottomRight.Should().Be(new Coordinate2D(width - 1, height - 1));
    }

    public const string example = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

    public const string example2 = example;
}