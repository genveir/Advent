using Advent2023.AdventActive;
using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2023.Advent18;

class Tests
{
    [TestCase(example, 62)]
    [TestCase(SimpleRectLeft, 50)]
    [TestCase(SimpleRectRight, 50)]
    [TestCase(LShape, 56)]
    [TestCase(TightLine, 20)]
    [TestCase(TightPartial, 15)]
    [TestCase(FullInsidePoint, 51)]
    public void Test1(string input, object output)
    {
        if (input.StartsWith(".")) input = Convert(input);

        var sol = new Solution(input);

        sol.GetResult1().Should().Be(output);
    }

    [TestCase(example2, 952408144115)]
    public void Test2(string input, object output)
    {
        var sol = new Solution(input);

        sol.GetResult2().Should().Be(output);
    }

    //[TestCase(example, @"D:\temp\aocTestOutput\example.txt")]
    //[TestCase("Input.txt", @"D:\temp\aocTestOutput\full.txt")]
    //[TestCase(FullInsidePoint, @"D:\temp\aocTestOutput\fip.txt")]
    public void PrintExampleDebug(string input, string filePath)
    {
        if (input.StartsWith(".")) input = Convert(input);

        var sol = new Solution(input);

        var points = sol.DrawShape(sol.dinges.Select(d => d.Pt1Instruction).ToArray());

        using (var writer = new StreamWriter(new FileStream(filePath, FileMode.Create)))
        {
            writer.WriteLine("Shape");
            writer.WriteLine(points.Print());

            sol.DetermineInsideCorners(points);

            writer.WriteLine("With Inside Corners");
            writer.WriteLine(points.Print());

            sol.MakeSquares(points);

            writer.WriteLine("Squares");
            writer.WriteLine(points.Print());
        }
    }

    public static string Convert(string input)
    {
        var split = input.Substring(1).Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        var sb = new StringBuilder();
        for (int n = 0; n < split.Length; n++)
        {
            sb.AppendLine($"{split[n][0]} {split[n].Substring(1)} (#000000)");
        }
        return sb.ToString().Trim();
    }

    public const string example = @"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)";

    public const string example2 = example;

    public const string SimpleRectRight = ".R9 D4 L9 U4";

    public const string SimpleRectLeft = ".D4 R9 U4 L9";

    public const string LShape = ".R4 D4 R3 U8 L7 D4";

    public const string TightLine = ".D9 R1 U9 L1";

    public const string TightPartial = ".D5 R2 U2 L1 U3 L1";

    public const string FullInsidePoint = ".R4 D2 R4 D4 L6 U2 L2 U4";
}
