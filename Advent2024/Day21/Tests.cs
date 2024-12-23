using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day21;

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
    public void TestNumPadCodes()
    {
        var numericPad = new NumericPad();

        var rA0 = numericPad.GetRoutes(numericPad.buttons['A'], numericPad.buttons['0']);
        var r02 = numericPad.GetRoutes(numericPad.buttons['0'], numericPad.buttons['2']);
        var r29 = numericPad.GetRoutes(numericPad.buttons['2'], numericPad.buttons['9']);
        var r9A = numericPad.GetRoutes(numericPad.buttons['9'], numericPad.buttons['A']);

        (rA0.Length * r02.Length * r29.Length * r9A.Length).Should().Be(3);

        List<string> results = [];
        foreach (var r in r29)
        {
            results.Add(rA0.Single() + r02.Single() + r + r9A.Single());
        }

        string[] options = ["<A^A>^^AvvvA", "<A^A^>^AvvvA", "<A^A^^>AvvvA"];

        results.Should().BeEquivalentTo(options);
    }

    [Test]
    public void TestConversion()
    {
        var numericPad = new NumericPad();

        var route = numericPad.ToRoute("<A^A>^^AvvvA");

        var correctSteps = new int[17];
        // A
        correctSteps[Route.AL]++; // <
        correctSteps[Route.LA]++; // A
        correctSteps[Route.AU]++; // ^
        correctSteps[Route.UA]++; // A
        correctSteps[Route.AR]++; // >
        correctSteps[Route.RU]++; // ^
        correctSteps[Route.UU]++; // ^
        correctSteps[Route.UA]++; // A
        correctSteps[Route.AD]++; // v
        correctSteps[Route.DD]++; // v
        correctSteps[Route.DD]++; // v
        correctSteps[Route.DA]++; // A

        route.Steps.Should().BeEquivalentTo(correctSteps);
    }

    [Test]
    public void TestFirstIteration()
    {
        var numericPad = new NumericPad();

        var route = numericPad.ToRoute("<A^A>^^AvvvA");

        route.Iterate();

        var correctSteps = new int[17];
        //A
        correctSteps[Route.AD]++; //v
        correctSteps[Route.DL]++; //<
        correctSteps[Route.LL]++; //<
        correctSteps[Route.LA]++; //A
        correctSteps[Route.AR]++; //>
        correctSteps[Route.RR]++; //>
        correctSteps[Route.RU]++; //^
        correctSteps[Route.UA]++; //A
        correctSteps[Route.AL]++; //<
        correctSteps[Route.LA]++; //A
        correctSteps[Route.AR]++; //>
        correctSteps[Route.RA]++; //A
        correctSteps[Route.AD]++; //v
        correctSteps[Route.DA]++; //A
        correctSteps[Route.AL]++; //<
        correctSteps[Route.LU]++; //^
        correctSteps[Route.UA]++; //A
        correctSteps[Route.AA]++; //A
        correctSteps[Route.AR]++; //>
        correctSteps[Route.RA]++; //A
        correctSteps[Route.AL]++; //<
        correctSteps[Route.LD]++; //v
        correctSteps[Route.DA]++; //A
        correctSteps[Route.AA]++; //A
        correctSteps[Route.AA]++; //A
        correctSteps[Route.AU]++; //^
        correctSteps[Route.UR]++; //>
        correctSteps[Route.RA]++; //A

        for (int n = 0; n < route.Steps.Length; n++)
        {
            route.Steps[n].Should().Be(correctSteps[n], "" + n);
        }
    }

    [TestCase("<A^A>^^AvvvA")]
    [TestCase("<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A")]
    public void TestLength(string toTest)
    {
        var numericPad = new NumericPad();

        var route = numericPad.ToRoute(toTest);

        route.Length.Should().Be(toTest.Length);
    }

    [Test]
    public void TestNumPadRoutes()
    {
        var numericPad = new NumericPad();
        var code = "029A";

        var routes = numericPad.GetRoutesForCode(code);

        string[] options = ["<A^A>^^AvvvA", "<A^A^>^AvvvA", "<A^A^^>AvvvA"];
        Route[] converted = options.Select(numericPad.ToRoute).ToArray();

        foreach (var r in converted)
        {
            routes.Should().Contain(r);
        }
    }

    [TestCase("029A", 0, 12)]
    [TestCase("029A", 1, 28)]
    [TestCase("029A", 2, 68)]
    [TestCase("980A", 2, 60)]
    [TestCase("179A", 2, 68)]
    [TestCase("456A", 2, 64)]
    [TestCase("379A", 2, 64)]
    public void TestLengthsForCodes(string code, int steps, int length)
    {
        var sol = new Solution(example);

        var route = sol.GetRouteForCode(code, steps);

        route.Length.Should().Be(length);
    }

    public const string example = @"029A
980A
179A
456A
379A";

    public const string example2 = example;
}
