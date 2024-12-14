namespace Advent2024.Day13;

public class Solution
{
    public List<Problem> problems = [];

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        foreach (var block in blocks)
        {
            var aSplit = block[0].Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries);
            var aCoord = new Coordinate2D(int.Parse(aSplit[1]), int.Parse(aSplit[3]));

            var bSplit = block[1].Split(new[] { ',', '+' }, StringSplitOptions.RemoveEmptyEntries);
            var bCoord = new Coordinate2D(int.Parse(bSplit[1]), int.Parse(bSplit[3]));

            var prize = block[2].Split(new[] { ',', '=' }, StringSplitOptions.RemoveEmptyEntries);
            var prizeCoord = new Coordinate2D(int.Parse(prize[1]), int.Parse(prize[3]));

            problems.Add(new Problem
            {
                ButtonA = aCoord,
                ButtonB = bCoord,
                Target = prizeCoord
            });
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public class Problem
    {
        public Coordinate2D ButtonA { get; set; }
        public Coordinate2D ButtonB { get; set; }

        public Coordinate2D Target { get; set; }

        public AB Solve()
        {
            // oja, hoe doe je ook al weer wiskunde
            var x = new Function(Target.X, ButtonA.X, ButtonB.X);
            var y = new Function(Target.Y, ButtonA.Y, ButtonB.Y);

            var xWithBEqualToY = x.Times(y.B / x.B);

            var zeroB = y.Minus(xWithBEqualToY);

            var a = zeroB.Target / zeroB.A;

            var b = (x.Target - (x.A * a)) / x.B;

            if (a.Denominator == 1 && b.Denominator == 1)
            {
                return new AB { A = a.ToLong(), B = b.ToLong() };
            }

            return new AB { A = 0, B = 0 };
        }

        private class Function
        {
            public Function(Fraction target, Fraction a, Fraction b)
            {
                Target = target;
                A = a;
                B = b;
            }

            public Fraction Target { get; set; }
            public Fraction A { get; set; }
            public Fraction B { get; set; }

            public Function Times(Fraction fraction) =>
                new Function(
                    Target * fraction,
                    A * fraction,
                    B * fraction);

            public Function Minus(Function other) =>
                new Function(
                    Target - other.Target,
                    A - other.A,
                    B - other.B);
        }
    }

    public class AB
    {
        public long A { get; set; }
        public long B { get; set; }
    }

    public object GetResult1()
    {
        var results = problems
            .Select(p => p.Solve())
            .Select(ab => 3L * ab.A + ab.B)
            .ToArray();

        return results
            .Sum();
    }

    public object GetResult2()
    {
        var results = problems
            .Select(problem => new Problem
            {
                Target = new(problem.Target.X + 10000000000000, problem.Target.Y + 10000000000000),
                ButtonA = problem.ButtonA,
                ButtonB = problem.ButtonB
            })
            .Select(p => p.Solve())
            .Select(ab => 3L * ab.A + ab.B)
            .ToArray();

        return results
            .Sum();
    }
}