namespace Advent2024.Day16;

using NodeData = AStar<Solution.SearchNode>.NodeData;
public class Solution
{
    public char[][] grid;

    public Coordinate2D Start;
    public Coordinate2D End;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input).ToArray();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == 'S')
                {
                    Start = new Coordinate2D(x, y);
                }
                else if (grid[y][x] == 'E')
                {
                    End = new Coordinate2D(x, y);
                }
            }
        }
    }

    public class SearchNode : IEquatable<SearchNode>
    {
        public SearchNode DiscoveredBy;
        public long Cost;

        public Coordinate2D Position;
        public int Direction;

        public void AddCoordinates(HashSet<Coordinate2D> foundNodes)
        {
            if (DiscoveredBy != null)
            {
                DiscoveredBy.AddCoordinates(foundNodes);
            }
            foundNodes.Add(Position);
        }

        public bool Equals(SearchNode other)
        {
            return other.Position == Position && other.Direction == Direction;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is SearchNode node && Equals(node);
        }

        public override string ToString()
        {
            var directionString = Direction switch
            {
                0 => "Up",
                1 => "Right",
                2 => "Down",
                3 => "Left",
                _ => throw new Exception("Invalid direction")
            };

            return $"{Position} {directionString}";
        }
    }

    public IEnumerable<SearchNode> FindNeighbourFunction(SearchNode node)
    {
        var forward = node.Direction switch
        {
            0 => node.Position.ShiftY(-1),
            1 => node.Position.ShiftX(1),
            2 => node.Position.ShiftY(1),
            3 => node.Position.ShiftX(-1),
            _ => throw new Exception("Invalid direction")
        };

        if (grid[forward.Y][forward.X] != '#')
        {
            yield return new SearchNode() { Position = forward, Direction = node.Direction, DiscoveredBy = node, Cost = node.Cost + 1 };
        }

        yield return new SearchNode() { Position = node.Position, Direction = (node.Direction + 1) % 4, DiscoveredBy = node, Cost = node.Cost + 1000 };
        yield return new SearchNode() { Position = node.Position, Direction = (node.Direction + 3) % 4, DiscoveredBy = node, Cost = node.Cost + 1000 };
    }

    public long TransitionCostFunction(SearchNode from, SearchNode to)
    {
        if (from.Direction == to.Direction)
        {
            return 1;
        }
        else
        {
            return 1000;
        }
    }

    public long HeuristicCostFunction(SearchNode node)
    {
        return node.Position.ManhattanDistance(End);
    }

    public Solution() : this("Input.txt")
    {
    }

    public NodeData GetShortest()
    {
        var startNode = new SearchNode()
        {
            Position = Start,
            Direction = 1
        };

        var endNodeFunc = (SearchNode sn) => sn.Position == End;

        var search = new AStar<SearchNode>(
            startNode,
            endNodeFunc,
            FindNeighbourFunction,
            TransitionCostFunction,
            HeuristicCostFunction);

        return search.FindShortest();
    }

    public object GetResult1()
    {
        return GetShortest().Cost;
    }

    public object GetResult2()
    {
        var shortest = (long)GetResult1();

        return GetRoutes(shortest);
    }

    public long GetRoutes(long shortest)
    {
        var startNode = new SearchNode()
        {
            Position = Start,
            Direction = 1
        };

        PriorityQueue<SearchNode, long> queue = new();
        queue.Enqueue(startNode, 0);

        HashSet<Coordinate2D> foundNodes = [];

        HashSet<SearchNode> visited = [];
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            visited.Add(node);

            if (node.Cost > shortest)
            {
                break;
            }

            if (node.Position == End && node.Cost == shortest)
            {
                node.AddCoordinates(foundNodes);
            }
            else
            {
                var neighbours = FindNeighbourFunction(node);

                foreach (var neighbor in neighbours)
                {
                    if (visited.Contains(neighbor))
                    {
                        continue;
                    }

                    queue.Enqueue(neighbor, neighbor.Cost);
                }
            }
        }

        return foundNodes.Count;
    }
}