using System.Text;

namespace Advent2024.AdventActive;

public class DirectionalPad
{
    public Dictionary<(char, char), string> moves = new()
    {
        {('<', '<'), "A" },
        {('<', '^'), ">^A" },
        {('<', 'v'), ">A" },
        {('<', '>'), ">>A" },
        {('<', 'A'), ">>^A" },

        {('^', '<'), "<vA" },
        {('^', '^'), "A" },
        {('^', 'v'), "vA" },
        {('^', '>'), "v>A" },
        {('^', 'A'), ">A" },

        {('v', '<'), "<A" },
        {('v', '^'), "^A" },
        {('v', 'v'), "A" },
        {('v', '>'), ">A" },
        {('v', 'A'), ">^A" },

        {('>', '<'), "<<A" },
        {('>', '^'), "<^A" },
        {('>', 'v'), "<A" },
        {('>', '>'), "A" },
        {('>', 'A'), "^A" },

        {('A', '<'), "<<vA" },
        {('A', '^'), "<A" },
        {('A', 'v'), "<vA" },
        {('A', '>'), "vA" },
        {('A', 'A'), "A" },

    };

    public DirectionalPad()
    {

    }

    public string GetOneRouteForRoute(string route)
    {
        route = 'A' + route;

        var oneShortestBuilder = new StringBuilder();
        for (int n = 0; n < route.Length - 1; n++)
        {
            var pair = route.Substring(n, 2);
            var routes = moves[(pair[0], pair[1])];
            oneShortestBuilder.Append(routes);
        }

        return oneShortestBuilder.ToString();
    }

    public string Revert(string route)
    {
        var steps = route.Split('A');
        steps = steps.Take(steps.Length - 1).ToArray();

        var current = 'A';
        var builder = new StringBuilder();
        foreach (var step in steps)
        {
            var toFind = step + "A";

            var found = moves
                .Where(x => x.Key.Item1 == current)
                .Single(x => x.Value == toFind);

            builder.Append(found.Key.Item2);
            current = found.Key.Item2;
        }
        return builder.ToString();
    }

    public string[] GetShortestRoutesForRoutes(string[] routes)
    {
        return routes.Select(GetOneRouteForRoute).ToArray();
    }
}
