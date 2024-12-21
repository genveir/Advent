using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.AdventActive;

class Tests
{
    [TestCase(example, 126384)]
    [TestCase("Input.txt", 184718)]
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

        var route = numericPad.GetShortestRouteForCode(code);

        string[] options = ["<A^A>^^AvvvA", "<A^A^>^AvvvA", "<A^A^^>AvvvA"];

        options.Should().Contain(route);
    }

    [TestCase("<A^A>^^AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")]
    //[TestCase("<A^A^>^AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")] aha
    [TestCase("<A^A^^>AvvvA", "v<<A>>^A<A>AvA<^AA>A<vAAA>^A")]
    [TestCase("v<<A>>^A<A>AvA<^AA>A<vAAA>^A", "<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A")]
    public void TestDirectionalPadRoutes(string from, string to)
    {
        var directionalPad = new DirectionalPad();

        var route = directionalPad.GetOneRouteForRoute(from);

        route.Should().HaveLength(to.Length);

        //   <    A  ^  A  >   ^   ^  A   v v v   A
        //v<<A >^>A <A >A <A >vA ^<A >A v<A A A ^>A

        //   <    A  ^  A  ^   > ^  A   v v v   A
        //v<<A >>^A <A >A vA <^A A >A <vA A A >^A
    }

    [Test]
    public void TestNoDirectionalPreference()
    {
        var directions = new[] { "^", ">", "v", "<" };

        for (int n = 0; n < directions.Length; n++)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                var directionalPad = new DirectionalPad();
                var route = directionalPad.GetOneRouteForRoute(directions[n] + directions[i] + 'A');
                var inverse = directionalPad.GetOneRouteForRoute(directions[i] + directions[n] + 'A');

                route.Length.Should().Be(inverse.Length);
            }
        }
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

        // long / mine
        // 3     7   9    A
        //^A ^^<<A >>A vvvA

        // ^  A  ^ ^   < <    A  > >  A   v v v   A
        //<A >A <A A v<A A >>^A vA A ^A v<A A A >^A

        //    <    A  >  A    <   A A   v  <    A A  > >   ^  A   v   A A  ^  A   v  <    A A A  >   ^  A
        // v<<A >>^A vA ^A v<<A>>^A A v<A <A >>^A A vA A <^A >A v<A >^A A <A >A v<A <A >>^A A A vA <^A >A

        // short / theirs
        //  3     7   9    A
        // ^A <<^^A >>A vvvA

        //  ^   A    < <   ^ ^  A  > >  A   v v v   A 
        // <A  >A v<<A A >^A A >A vA A ^A <vA A A >^A

        //    <    A  >  A   v  < <    A A  >   ^  A A  >  A   v   A A  ^  A    <  v   A A A  >   ^  A
        // <v<A >>^A vA ^A <vA <A A >>^A A vA <^A >A A vA ^A <vA >^A A <A >A <v<A >A >^A A A vA <^A >A

        route.Should().HaveLength(length);
    }

    // ^<<A ^^A >>A vvvA

    [TestCase("<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A", "<A>Av<<AA>^AA>AvAA^A<vAAA>^A")]
    [TestCase("<Av<AA>>^A<AA>AvAA^A<vAAA>^A", "")]
    public void Revert(string input, string output)
    {
        var directionalPad = new DirectionalPad();

        var result = directionalPad.Revert(input);

        result.Should().Be(output);
    }

    public const string example = @"029A
980A
179A
456A
379A";

    public const string example2 = example;
}
