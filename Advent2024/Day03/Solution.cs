using System.Text.RegularExpressions;

namespace Advent2024.Day3;

public partial class Solution : ISolution
{
    public List<Mul> muls = [];
    public List<IInstruction> instructions = [];

    public Solution(string input)
    {
        var inputString = Input.GetInput(input);

        var matches = MulRegex().Matches(inputString);
        foreach (Match match in matches)
        {
            var mul = new Mul(match.Index, match.Groups[1].Value, match.Groups[2].Value);

            muls.Add(mul);
            instructions.Add(mul);
        }

        var dosMatches = DoRegex().Matches(inputString);
        foreach (Match match in dosMatches)
        {
            var doi = new Do() { Index = match.Index };
            instructions.Add(doi);
        }

        var dontsMatches = DontRegex().Matches(inputString);
        foreach (Match match in dontsMatches)
        {
            var dont = new Dont() { Index = match.Index };
            instructions.Add(dont);
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public interface IInstruction
    {
        public int Index { get; }
    }

    public class Do : IInstruction
    {
        public int Index { get; init; }
    }

    public class Dont : IInstruction
    {
        public int Index { get; init; }
    }

    public class Mul : IInstruction
    {
        public int Index { get; }
        public int mul1;
        public int mul2;

        public Mul(int index, string m1, string m2)
        {
            this.Index = index;

            mul1 = int.Parse(m1);
            mul2 = int.Parse(m2);
        }

        public long Value => mul1 * mul2;
    }

    public object GetResult1()
    {
        return muls.Sum(m => m.Value);
    }

    public object GetResult2()
    {
        var inOrder = instructions.OrderBy(i => i.Index).ToList();

        long sum = 0;
        bool enabled = true;
        foreach (var instruction in inOrder)
        {
            switch (instruction)
            {
                case Do _:
                    enabled = true;
                    break;

                case Dont _:
                    enabled = false;
                    break;

                case Mul mul:
                    if (enabled)
                    {
                        sum += mul.Value;
                    }
                    break;
            }
        }

        return sum;
    }

    [GeneratedRegex("mul\\(([0-9]{1,3}),([0-9]{1,3})\\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex("do\\(\\)")]
    private static partial Regex DoRegex();

    [GeneratedRegex("don't\\(\\)")]
    private static partial Regex DontRegex();
}