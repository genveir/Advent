using System.Text;

namespace Advent2024.AdventActive;

public class DirectionalPad
{
    public List<DirectionalPadNode> Nodes { get; set; } = [];
    public class DirectionalPadNode
    {
        public char Value { get; set; }
        public DirectionalPadNode Up { get; set; }
        public DirectionalPadNode Right { get; set; }
        public DirectionalPadNode Down { get; set; }
        public DirectionalPadNode Left { get; set; }
    }

    public void LinkNode(char node, char? up, char? right, char? down, char? left)
    {
        var toLink = Nodes.Single(n => n.Value == node);
        toLink.Up = Nodes.SingleOrDefault(n => n.Value == up);
        toLink.Right = Nodes.SingleOrDefault(n => n.Value == right);
        toLink.Down = Nodes.SingleOrDefault(n => n.Value == down);
        toLink.Left = Nodes.SingleOrDefault(n => n.Value == left);
    }

    public DirectionalPad()
    {
        Nodes.Add(new DirectionalPadNode { Value = '^' });
        Nodes.Add(new DirectionalPadNode { Value = '>' });
        Nodes.Add(new DirectionalPadNode { Value = 'v' });
        Nodes.Add(new DirectionalPadNode { Value = '<' });
        Nodes.Add(new DirectionalPadNode { Value = 'A' });

        LinkNode('^', null, 'A', 'v', null);
        LinkNode('A', null, null, '>', '^');
        LinkNode('<', null, 'v', null, null);
        LinkNode('v', '^', '>', null, '<');
        LinkNode('>', 'A', null, null, 'v');
    }

    public string GetOneRouteForRoute(string route)
    {
        route = 'A' + route;

        var oneShortestBuilder = new StringBuilder();
        for (int n = 0; n < route.Length - 1; n++)
        {
            var pair = route.Substring(n, 2);
            var routes = GetShortestRoutesForPair(pair[0], pair[1]);
            oneShortestBuilder.Append(routes.First());
        }

        return oneShortestBuilder.ToString();
    }

    public string[] GetShortestRoutesForRoutes(string[] routes)
    {
        HashSet<string> allRoutes = [];

        foreach (var route in routes)
        {
            var allRoutesForRoute = GetShortestRoutesForRoute(route);
            foreach (var r in allRoutesForRoute)
            {
                allRoutes.Add(r);
            }
        }

        var shortest = allRoutes.Min(r => r.Length);

        return allRoutes.Where(r => r.Length == shortest).ToArray();
    }

    public string[] GetShortestRoutesForRoute(string route)
    {
        route = 'A' + route;

        string[][] shortestForPairs = new string[route.Length - 1][];
        var results = new List<string>() { "" };
        for (int n = 0; n < route.Length - 1; n++)
        {
            var pair = route.Substring(n, 2);

            var routes = GetShortestRoutesForPair(pair[0], pair[1]);

            shortestForPairs[n] = routes;

            List<string> newResults = [];
            foreach (var result in results)
            {
                foreach (var r in routes)
                {
                    newResults.Add(result + r);
                }
            }
            results = newResults;
        }

        return results.ToArray();
    }

    public class SearchNode
    {
        public DirectionalPadNode Node { get; set; }
        public string Route { get; set; }
        public List<char> Visited { get; set; } = [];
    }

    Dictionary<(char, char), string[]> Routes { get; set; } = [];
    public string[] GetShortestRoutesForPair(char first, char second)
    {
        if (Routes.TryGetValue((first, second), out var routes))
        {
            return routes;
        }

        var queue = new Queue<SearchNode>();
        var firstNode = Nodes.Single(n => n.Value == first);
        var searchNode = new SearchNode { Node = firstNode, Route = "", Visited = new List<char> { first } };
        queue.Enqueue(searchNode);

        int? shortestRoute = null;
        var routeList = new List<string>();
        while (queue.Any())
        {
            var current = queue.Dequeue();

            if (current.Node.Value == second)
            {
                if (shortestRoute == null)
                {
                    shortestRoute = current.Route.Length;
                }
                else if (current.Route.Length > shortestRoute)
                {
                    break;
                }
                routeList.Add(current.Route + 'A');
                continue;
            }

            if (current.Node.Up != null && !current.Visited.Contains(current.Node.Up.Value))
            {
                queue.Enqueue(BuildNode(current, '^', current.Node.Up));
            }

            if (current.Node.Right != null && !current.Visited.Contains(current.Node.Right.Value))
            {
                queue.Enqueue(BuildNode(current, '>', current.Node.Right));
            }

            if (current.Node.Down != null && !current.Visited.Contains(current.Node.Down.Value))
            {
                queue.Enqueue(BuildNode(current, 'v', current.Node.Down));
            }

            if (current.Node.Left != null && !current.Visited.Contains(current.Node.Left.Value))
            {
                queue.Enqueue(BuildNode(current, '<', current.Node.Left));
            }
        }

        routes = routeList.ToArray();
        Routes[(first, second)] = routes;
        return routes;
    }

    public SearchNode BuildNode(SearchNode origin, char direction, DirectionalPadNode target)
    {
        var newNode = new SearchNode()
        {
            Node = target,
            Route = origin.Route + direction,
            Visited = origin.Visited.Append(origin.Node.Value).ToList()
        };
        return newNode;
    }
}
