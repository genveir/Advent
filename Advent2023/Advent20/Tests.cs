using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent20;

class Tests
{
    [TestCase(example, 32000000)]
    [TestCase(example2, 11687500)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase("Input.txt", 237878264003759)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a";

    public const string example2 = @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output";
}
