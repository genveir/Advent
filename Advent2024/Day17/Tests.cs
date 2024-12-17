using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day17;

internal class Tests
{
    [TestCase(example, "4,6,3,5,6,3,5,2,1,0")]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [Test]
    public void ParseTest()
    {
        var sol = new Solution(example);

        sol.Original.A.Should().Be(729);
        sol.Original.B.Should().Be(0);
        sol.Original.C.Should().Be(0);

        sol.Original.Program[0].Should().Be(0);
        sol.Original.Program[1].Should().Be(1);
        sol.Original.Program[2].Should().Be(5);
        sol.Original.Program[3].Should().Be(4);
        sol.Original.Program[4].Should().Be(3);
        sol.Original.Program[5].Should().Be(0);
    }

    [Test]
    public void ExampleOp1()
    {
        var computer = new Solution.Computer(0, 0, 9, [2, 6]);

        computer.Step();

        computer.B.Should().Be(1);
    }

    [Test]
    public void ExampleOp2()
    {
        var computer = new Solution.Computer(10, 0, 0, [5, 0, 5, 1, 5, 4]);

        computer.Run();

        computer.Output.Should().BeEquivalentTo(new List<long> { 0, 1, 2 });
    }

    [Test]
    public void ExampleOp3()
    {
        var computer = new Solution.Computer(2024, 0, 0, [0, 1, 5, 4, 3, 0]);

        computer.Run();

        computer.A.Should().Be(0);
        computer.Output.Should().BeEquivalentTo(new List<long> { 4, 2, 5, 6, 7, 7, 7, 7, 3, 1, 0 });
    }

    [Test]
    public void CheckXORReasoning()
    {
        for (int n = 0; n < 8; n++)
        {
            var xor = n ^ 1;

            var expected = n switch
            {
                0 => 1,
                1 => 0,
                2 => 3,
                3 => 2,
                4 => 5,
                5 => 4,
                6 => 7,
                7 => 6,
                _ => throw new Exception()
            };

            xor.Should().Be(expected);
        }
    }

    public const string example = @"Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0";

    public const string example2 = example;
}