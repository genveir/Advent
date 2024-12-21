namespace Advent2024.AdventActive;

public partial class Solution
{
    public string[] Codes { get; set; }

    public Solution(string input)
    {
        Codes = Input.GetInputLines(input).ToArray();
    }
    public Solution() : this("Input.txt") { }

    public string GetRouteForCode(string code, int steps)
    {
        var numericPad = new NumericPad();
        var directionalPad = new DirectionalPad();

        var route = numericPad.GetShortestRouteForCode(code);

        for (int n = 0; n < steps; n++)
        {
            Console.WriteLine(n);

            route = directionalPad.GetOneRouteForRoute(route);
        }

        return route;
    }

    public object GetResult1()
    {
        long sum = 0;
        foreach (var code in Codes)
        {
            var route = GetRouteForCode(code, 2);

            var numericPart = long.Parse(code.Substring(0, 3).TrimStart('0'));

            sum += numericPart * route.Length;
        }
        return sum;
    }

    public object GetResult2()
    {
        long sum = 0;
        foreach (var code in Codes)
        {
            var route = GetRouteForCode(code, 25);

            var numericPart = long.Parse(code.Substring(0, 3).TrimStart('0'));

            sum += numericPart * route.Length;
        }
        return sum;
    }
}
