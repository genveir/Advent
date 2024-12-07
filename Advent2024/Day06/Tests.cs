using FluentAssertions;
using NUnit.Framework;

namespace Advent2024.Day06;

internal class Tests
{
    [TestCase(example, 41)]
    [TestCase("Input.txt", 5101)]
    public void Test1(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 6)]
    [TestCase(sidePocket, 1)]
    [TestCase("Input.txt", 1951)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    [TestCase(example2, 6)]
    [TestCase(sidePocket, 1)]
    [TestCase("Input.txt", 1951)]
    public void TestBetter2(string input, long output)
    {
        var sol = new Solution(input);

        new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY)
            .Solve()
            .Should().Be(output);
    }

    [TestCase("3,6", example, 3, 6)]
    [TestCase("6,7", example, 6, 7)]
    [TestCase("7,7", example, 7, 7)]
    [TestCase("1,8", example, 1, 8)]
    [TestCase("3,8", example, 3, 8)]
    [TestCase("7,9", example, 7, 9)]
#pragma warning disable IDE0060
    public void TestBetterSpots(string id, string input, int x, int y)
    {
        var sol = new Solution(input);
        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        solver.Solve();

        solver.LoopSpots.Should().Contain(new Coordinate2D(x, y));
    }

    [TestCase("3,6", example, 3, 6)]
    [TestCase("6,7", example, 6, 7)]
    [TestCase("7,7", example, 7, 7)]
    [TestCase("1,8", example, 1, 8)]
    [TestCase("3,8", example, 3, 8)]
    [TestCase("7,9", example, 7, 9)]
    public void TestActualSpots(string id, string input, int x, int y)
    {
        var sol = new Solution(input);

        sol.GetResult2();

        sol.loopSpots.Should().Contain(new Coordinate2D(x, y));
    }
#pragma warning restore IDE0060

    [TestCase(single, singleMovers)]
    [TestCase(square, squareMovers)]
    [TestCase(walledSquare, walledSquareMovers)]
    [TestCase(crossSquare, crossSquareMovers)]
    [TestCase(doubleOptions, doubleOptionsMovers)]
    [TestCase(sidePocket, sidePocketMovers)]
    public void MoverConstruction(string input, string expected)
    {
        var sol = new Solution(input);

        var movers = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY).Movers;

        var moverString = string.Join(
            Environment.NewLine,
            movers
                .OrderBy(m => m.Position.X)
                .ThenBy(m => m.Position.Y)
                .ThenBy(m => m.Direction));

        moverString.Should().Be(expected);
    }

    [TestCase(single, singleStepped)]
    [TestCase(square, squareStepped)]
    [TestCase(crossSquare, crossSquareStepped)]
    [TestCase(doubleOptions, doubleOptionsStepped)]
    public void MoverStep(string input, string expected)
    {
        var sol = new Solution(input);

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        solver.StepMovers();

        var moverString = string.Join(
            Environment.NewLine,
            solver.Movers
                .OrderBy(m => m.Position.X)
                .ThenBy(m => m.Position.Y)
                .ThenBy(m => m.Direction));

        moverString.Should().Be(expected);
    }

    [Test]
    public void CrossSteps()
    {
        var sol = new Solution(crossSquare);

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        string[] expecteds = [
            crossSquareStepped,
            crossSquareStepped2,
            crossSquareStepped3,
            ""
        ];

        for (int n = 0; n < expecteds.Length; n++)
        {
            solver.StepMovers();

            var moverString = string.Join(
                Environment.NewLine,
                solver.Movers
                    .OrderBy(m => m.Position.X)
                    .ThenBy(m => m.Position.Y)
                    .ThenBy(m => m.Direction));

            moverString.Should().Be(expecteds[n]);
        }
    }

    [Test]
    public void OptionSteps()
    {
        var sol = new Solution(doubleOptions);

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        string[] expecteds = [
            doubleOptionsStepped,
            doubleOptionsStepped2,
            doubleOptionsStepped3,
            doubleOptionsStepped4,
            ""
        ];

        for (int n = 0; n < expecteds.Length; n++)
        {
            solver.StepMovers();
            var moverString = string.Join(
                Environment.NewLine,
                solver.Movers
                    .OrderBy(m => m.Position.X)
                    .ThenBy(m => m.Position.Y)
                    .ThenBy(m => m.Direction));
            moverString.Should().Be(expecteds[n]);
        }
    }

    [Test]
    public void SidePocketSteps()
    {
        var sol = new Solution(sidePocket);

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);

        string[] expecteds = [
            sidePocketStepped,
            sidePocketStepped2,
            sidePockedStepped3,
            sidePocketStepped4,
            ""
        ];

        for (int n = 0; n < expecteds.Length; n++)
        {
            solver.StepMovers();
            var moverString = string.Join(
                Environment.NewLine,
                solver.Movers
                    .OrderBy(m => m.Position.X)
                    .ThenBy(m => m.Position.Y)
                    .ThenBy(m => m.Direction));
            moverString.Should().Be(expecteds[n]);
        }
    }

    [Test]
    public void BruteForceFindsNoLoopThatIsNotInBetter()
    {
        var sol = new Solution("Input.txt");
        sol.GetResult2();
        var bruteForceSpots = sol.loopSpots;

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);
        solver.Solve();
        var betterSpots = solver.LoopSpots;

        List<Coordinate2D> spots = [];
        foreach (var spot in bruteForceSpots)
        {
            if (!betterSpots.Contains(spot))
            {
                spots.Add(spot);
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, spots));
        spots.Count.Should().Be(0);
    }

    [Test]
    public void BetterFindsNoLoopThatIsNotInBruteForce()
    {
        var sol = new Solution("Input.txt");
        sol.GetResult2();
        var bruteForceSpots = sol.loopSpots;

        var solver = new BetterSolver(sol.grid, sol.start, sol.wallsByX, sol.wallsByY);
        solver.Solve();
        var betterSpots = solver.LoopSpots;

        List<Coordinate2D> spots = [];
        foreach (var spot in betterSpots)
        {
            if (!bruteForceSpots.Contains(spot))
            {
                spots.Add(spot);
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, spots));
        spots.Count.Should().Be(0);
    }

    public const string example = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

    public const string example2 = example;

    #region single
    public const string single = @"^";

    public const string singleMovers = @"(0, 0) 0
(0, 0) 1
(0, 0) 2
(0, 0) 3";

    public const string singleStepped = @"";
    #endregion single

    #region square
    public const string square = @"...
.^.
...";

    public const string squareMovers = @"(0, 0) 0
(0, 0) 3
(0, 1) 3
(0, 2) 2
(0, 2) 3
(1, 0) 0
(1, 2) 2
(2, 0) 0
(2, 0) 1
(2, 1) 1
(2, 2) 1
(2, 2) 2";

    public const string squareStepped = @"(0, 1) 0
(0, 1) 2
(1, 0) 1
(1, 0) 3
(1, 1) 0
(1, 1) 1
(1, 1) 2
(1, 1) 3
(1, 2) 1
(1, 2) 3
(2, 1) 0
(2, 1) 2";
    #endregion square

    #region walledSquare
    public const string walledSquare = @"###
#^#
###";

    public const string walledSquareMovers = "";
    #endregion walledSquare

    #region crossSquare
    public const string crossSquare = @"#.#
.^.
#.#";

    public const string crossSquareMovers = @"(0, 1) 3
(1, 0) 0
(1, 2) 2
(2, 1) 1";

    public const string crossSquareStepped = @"(0, 1) 2
(1, 0) 3
(1, 1) 0
(1, 1) 1
(1, 1) 2
(1, 1) 3
(1, 2) 1
(2, 1) 0";

    public const string crossSquareStepped2 = @"(0, 1) 1
(1, 0) 2
(1, 2) 0
(2, 1) 3";

    public const string crossSquareStepped3 = @"(0, 1) 0
(1, 0) 1
(1, 2) 3
(2, 1) 2";
    #endregion crossSquare

    #region doubleOptions
    public const string doubleOptions = @"###
.^.
#.#";

    public const string doubleOptionsMovers = @"(0, 1) 3
(1, 2) 2
(2, 1) 1";

    public const string doubleOptionsStepped = @"(0, 1) 2
(1, 1) 1
(1, 1) 2
(1, 1) 3
(1, 2) 1
(2, 1) 0";

    public const string doubleOptionsStepped2 = @"(0, 1) 1
(1, 1) 0
(2, 1) 3";

    public const string doubleOptionsStepped3 = @"(0, 1) 0
(1, 2) 0
(2, 1) 2";

    public const string doubleOptionsStepped4 = @"(1, 2) 3";
    #endregion doubleOptions

    #region sidePocket
    public const string sidePocket = @"#.###
#...#
#^#.#
#.###";

    public const string sidePocketMovers = @"(1, 0) 0
(1, 3) 2";

    public const string sidePocketStepped = @"(1, 0) 3
(1, 1) 0
(1, 2) 2
(1, 3) 1";

    public const string sidePocketStepped2 = @"(1, 1) 2
(1, 1) 3
(1, 2) 0
(1, 2) 1";

    public const string sidePockedStepped3 = @"(1, 0) 2
(1, 2) 3
(1, 3) 0
(2, 1) 3";

    public const string sidePocketStepped4 = @"(1, 0) 1
(1, 3) 3
(2, 1) 2
(3, 1) 3";

    #endregion sidePocket
}