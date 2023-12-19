using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Advent17;

class Tests
{
    [TestCase(example, 102)]
    [TestCase(subset, 20)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example, 94)]
    [TestCase(example2, 71)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533";

    public const string subset = @"241343
321545";

    public const string example2 = @"111111111111
999999999991
999999999991
999999999991
999999999991";
}
