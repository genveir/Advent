using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.AdventActive;

class Tests
{
    [TestCase(example, 126384)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [Test]
    public void TestNumPadRoutes()
    {
        var numericPad = new NumericPad();
        var code = "029A";

        var routes = numericPad.GetShortestRoutesForCode(code);

        string[] options = ["<A^A>^^AvvvA", "<A^A^>^AvvvA", "<A^A^^>AvvvA"];

        options.Should().BeEquivalentTo(routes);
    }

    [TestCase("<A^A>^^AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")]
    //[TestCase("<A^A^>^AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")] aha
    [TestCase("<A^A^^>AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")]
    [TestCase("v<<A>>^A<A>AvA<^AA>A<vAAA>^A", "<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A")]
    public void TestDirectionalPadRoutes(string from, string to)
    {
        var directionalPad = new DirectionalPad();

        var route = directionalPad.GetOneRouteForRoute(from);

        route.Length.Should().Be(to.Length);
    }

    [TestCase("^", "<A")]
    [TestCase("<", "v<<A")]
    [TestCase("v", "v<A")]
    [TestCase(">", "vA")]
    [TestCase("<<", "v<<AA")]
    public void DirPadTests(string route, string expected)
    {
        var directionalPad = new DirectionalPad();
        var result = directionalPad.GetOneRouteForRoute(route);

        result.Should().Be(expected);
    }

    [TestCase("029A", 68)]
    [TestCase("980A", 60)]
    [TestCase("179A", 68)]
    [TestCase("456A", 64)]
    [TestCase("379A", 64)]
    public void TestLengthsForCodes(string code, int length)
    {
        var sol = new Solution(example);

        var route = sol.GetRouteForCode(code, 2);

        route.Length.Should().Be(length);
    }

    [TestCase(example2, "")]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    public const string example = @"029A
980A
179A
456A
379A";

    public const string example2 = example;
}
