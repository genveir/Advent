using FluentAssertions;
using NUnit.Framework;

namespace Advent2025.Day06;

class Tests
{
    [TestCase(example, 4277556)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 3263827)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ";

    public const string example2 = example;
}
