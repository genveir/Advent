using System.Text;

namespace Advent2024.AdventActive;


public class NumericPad
{
    public List<NumPadNode> Nodes { get; set; } = [];
    public class NumPadNode
    {
        public int Value { get; set; }

        public NumPadNode Up { get; set; }
        public NumPadNode Right { get; set; }
        public NumPadNode Down { get; set; }
        public NumPadNode Left { get; set; }
    }

    public void LinkNode(int node, int? up, int? right, int? down, int? left)
    {
        var toLink = Nodes.Single(n => n.Value == node);
        toLink.Up = Nodes.SingleOrDefault(n => n.Value == up);
        toLink.Right = Nodes.SingleOrDefault(n => n.Value == right);
        toLink.Down = Nodes.SingleOrDefault(n => n.Value == down);
        toLink.Left = Nodes.SingleOrDefault(n => n.Value == left);
    }

    public NumericPad()
    {
        for (int n = 0; n < 10; n++)
        {
            var node = new NumPadNode { Value = n };
            Nodes.Add(node);
        }
        Nodes.Add(new NumPadNode { Value = 'A' });

        LinkNode(7, null, 8, 4, null);
        LinkNode(8, null, 9, 5, 7);
        LinkNode(9, null, null, 6, 8);
        LinkNode(4, 7, 5, 1, null);
        LinkNode(5, 8, 6, 2, 4);
        LinkNode(6, 9, null, 3, 5);
        LinkNode(1, 4, 2, null, null);
        LinkNode(2, 5, 3, 0, 1);
        LinkNode(3, 6, null, 'A', 2);
        LinkNode(0, 2, 'A', null, null);
        LinkNode('A', 3, null, null, 0);
    }

    public string[] GetShortestRoutesForCode(string code)
    {
        code = 'A' + code;

        string[][] shortestForPairs = new string[code.Length - 1][];
        for (int n = 0; n < code.Length - 1; n++)
        {
            var pair = code.Substring(n, 2);

            var routes = GetShortestRoutesForPair(ToInt(pair[0]), ToInt(pair[1]));

            shortestForPairs[n] = routes;
        }

        var numResults = shortestForPairs.Select(sp => sp.Length).Aggregate((a, b) => a * b);

        var results = new HashSet<string>();

        for (int c1 = 0; c1 < shortestForPairs[0].Length; c1++)
        {
            for (int c2 = 0; c2 < shortestForPairs[1].Length; c2++)
            {
                for (int c3 = 0; c3 < shortestForPairs[2].Length; c3++)
                {
                    for (int c4 = 0; c4 < shortestForPairs[3].Length; c4++)
                    {
                        results.Add(shortestForPairs[0][c1] + shortestForPairs[1][c2] + shortestForPairs[2][c3] + shortestForPairs[3][c4]);
                    }
                }
            }
        }

        return results.ToArray();
    }

    public int ToInt(char c)
    {
        if (c == 'A') return 'A';

        return c - 48;
    }

    public class SearchNode
    {
        public NumPadNode Node { get; set; }
        public string Route { get; set; }
        public List<int> Visited { get; set; } = [];
    }

    Dictionary<(int, int), string[]> Routes { get; set; } = [];
    public string[] GetShortestRoutesForPair(int first, int second)
    {
        if (Routes.TryGetValue((first, second), out var routes))
        {
            return routes;
        }

        var queue = new Queue<SearchNode>();
        var firstNode = Nodes.Single(n => n.Value == first);
        var searchNode = new SearchNode { Node = firstNode, Route = "", Visited = new List<int> { first } };
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

    public SearchNode BuildNode(SearchNode origin, char direction, NumPadNode target)
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