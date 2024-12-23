namespace Advent2024.Day21;

public partial class Solution
{
    public string[] Codes { get; set; }

    public Solution(string input)
    {
        Codes = Input.GetInputLines(input).ToArray();
    }
    public Solution() : this("Input.txt") { }

    public Route GetRouteForCode(string code, int steps)
    {
        var numericPad = new NumericPad();

        var routes = numericPad.GetRoutesForCode(code);

        Route minRoute = null;
        for (int r = 0; r < routes.Length; r++)
        {
            var route = routes[r];

            for (int n = 0; n < steps; n++)
            {
                route.Iterate();
            }

            if (minRoute == null || route.Length < minRoute.Length)
            {
                minRoute = route;
            }
        }

        return minRoute;
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
