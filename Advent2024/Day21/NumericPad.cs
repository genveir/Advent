namespace Advent2024.Day21;


public class NumericPad
{
    public Dictionary<char, Coordinate2D> buttons = new()
    {
        {'7', new(0,0) },
        {'8', new(1,0) },
        {'9', new(2,0) },
        {'4', new(0,1) },
        {'5', new(1,1) },
        {'6', new(2,1) },
        {'1', new(0,2) },
        {'2', new(1,2) },
        {'3', new(2,2) },
        {'0', new(1,3) },
        {'A', new(2,3) }
    };

    public Route[] GetRoutesForCode(string code)
    {
        var actual = "A" + code;

        Route[] result = null;
        foreach (var (first, second) in actual.PairWise())
        {
            var from = buttons[first];
            var to = buttons[second];

            var routes = GetRoutes(from, to)
                .Select(ToRoute);

            if (result == null) result = routes.ToArray();
            else
            {
                var newResult = new List<Route>();
                foreach (var r1 in routes)
                {
                    foreach (var r2 in result)
                    {
                        newResult.Add(r2 + r1);
                    }
                }
                result = newResult.ToArray();
            }
        }

        return result;
    }

    public Route ToRoute(string input)
    {
        var route = new Route()
        {
            InitialValue = input
        };

        var actual = "A" + input;

        foreach (var (first, second) in actual.PairWise())
        {
            var index = moves[(first, second)];

            route.Steps[index]++;
        }

        return route;
    }

    public string[] GetRoutes(Coordinate2D from, Coordinate2D to)
    {
        if (from == to)
            return ["A"];

        List<string> results = [];
        if (from.X < to.X)
        {
            results.AddRange(GetRoutes(from.ShiftX(1), to).Select(r => ">" + r));
        }

        if (from.X > to.X && (from != new Coordinate2D(1, 3)))
        {
            results.AddRange(GetRoutes(from.ShiftX(-1), to).Select(r => "<" + r));
        }

        if (from.Y < to.Y && (from != new Coordinate2D(0, 2)))
        {
            results.AddRange(GetRoutes(from.ShiftY(1), to).Select(r => "v" + r));
        }

        if (from.Y > to.Y)
        {
            results.AddRange(GetRoutes(from.ShiftY(-1), to).Select(r => "^" + r));
        }

        return results.ToArray();
    }

    public Dictionary<(char, char), int> moves = new()
    {
        {('<', '<'), Route.LL },
        {('<', '^'), Route.LU },
        {('<', 'v'), Route.LD },
        {('<', 'A'), Route.LA },

        {('^', '<'), Route.UL },
        {('^', '^'), Route.UU },
        {('^', '>'), Route.UR },
        {('^', 'A'), Route.UA },

        {('v', '<'), Route.DL },
        {('v', 'v'), Route.DD },
        {('v', '>'), Route.DR },
        {('v', 'A'), Route.DA },

        {('>', '^'), Route.RU },
        {('>', 'v'), Route.RD },
        {('>', '>'), Route.RR },
        {('>', 'A'), Route.RA },

        {('A', '<'), Route.AL },
        {('A', '^'), Route.AU },
        {('A', 'v'), Route.AD },
        {('A', '>'), Route.AR },
        {('A', 'A'), Route.AA },
    };
}