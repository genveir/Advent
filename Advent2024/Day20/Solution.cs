namespace Advent2024.Day20;

public class Solution
{
    public char[][] grid;
    public Dictionary<Coordinate2D, int> pathIndex = [];

    public Coordinate2D start;
    public Coordinate2D end;
    public int TimeToSave { get; set; } = 100;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == 'S')
                {
                    start = new Coordinate2D(x, y);
                    grid[y][x] = '.';
                }
                else if (grid[y][x] == 'E')
                {
                    end = new Coordinate2D(x, y);
                    grid[y][x] = '.';
                }
            }
        }

        if (start == null || end == null)
        {
            throw new Exception("No start or end found");
        }
    }

    public bool MapPath()
    {
        var visited = new HashSet<Coordinate2D>();
        var queue = new Queue<Coordinate2D>();

        int index = 0;

        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            pathIndex[current] = index++;

            if (current == end)
            {
                return true;
            }
            visited.Add(current);

            var neighbours = current.GetNeighbours(true);

            var visitedNeighbourCount = neighbours.Where(visited.Contains).Count();

            if (visitedNeighbourCount > 1)
                return false;

            foreach (var neighbor in neighbours)
            {
                if (!neighbor.IsInBounds(grid))
                    continue;

                if (visited.Contains(neighbor))
                    continue;

                if (grid[neighbor.Y][neighbor.X] == '#')
                    continue;

                queue.Enqueue(neighbor);
            }
        }
        return false;
    }

    public Coordinate2D[] GetPathArray()
    {
        var pathArray = new Coordinate2D[pathIndex.Count];

        foreach (var item in pathIndex)
        {
            pathArray[item.Value] = item.Key;
        }

        return pathArray;
    }

    public Cut[][] FindShortcuts(int maxDistance)
    {
        var isSinglePath = MapPath();

        if (!isSinglePath) throw new Exception("not a single path");

        var pathArray = GetPathArray();

        var shortcuts = new Cut[pathArray.Length][];
        for (int n = 0; n < pathArray.Length; n++)
        {
            shortcuts[n] = FindPossibleCuts(n, pathArray, maxDistance);
        }
        return shortcuts;
    }

    public class Cut
    {
        public long Gain { get; set; }
        public long Distance { get; set; }
    }

    public Cut[] FindPossibleCuts(int targetIndex, Coordinate2D[] pathArray, int maxDistance)
    {
        var target = pathArray[targetIndex];

        List<Cut> possibleCuts = [];
        for (int n = 0; n < targetIndex; n++)
        {
            var manhattanDistance = pathArray[n].ManhattanDistance(target);

            if (manhattanDistance > maxDistance)
                continue;

            var gain = pathIndex[target] - pathIndex[pathArray[n]] - manhattanDistance;

            if (gain > 0)
            {
                possibleCuts.Add(new Cut { Distance = manhattanDistance, Gain = gain });
            }
        }
        return possibleCuts.ToArray();
    }

    public Solution() : this("Input.txt")
    {
    }

    // not 1276
    public object GetResult1()
    {
        return FindShortcuts(2)
            .SelectMany(c => c)
            .Count(c => c.Gain >= TimeToSave);
    }

    public object GetResult2()
    {
        return FindShortcuts(20)
            .SelectMany(c => c)
            .Count(c => c.Gain >= TimeToSave);
    }
}