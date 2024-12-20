namespace Advent2024.AdventActive;

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

    public int[][] GetReachableArray(Coordinate2D[] pathArray)
    {
        var reachableArray = pathArray.Select(FindReachableInDistance2).ToArray();

        return reachableArray;
    }

    public int[][] GetCutArray(Coordinate2D[] pathArray, int[][] reachableArray)
    {
        var cutArray = new int[pathArray.Length][];
        for (int n = 0; n < pathArray.Length; n++)
        {
            cutArray[n] = new int[reachableArray[n].Length];
            for (int i = 0; i < reachableArray[n].Length; i++)
            {
                cutArray[n][i] = reachableArray[n][i] - pathIndex[pathArray[n]];
            }
        }

        return cutArray;
    }

    public int[][] FindShortcuts()
    {
        var isSinglePath = MapPath();

        if (!isSinglePath) throw new Exception("not a single path");

        var pathArray = GetPathArray();

        var reachableArray = GetReachableArray(pathArray);

        return GetCutArray(pathArray, reachableArray);
    }

    public int[] FindReachableInDistance2(Coordinate2D coordinate) => FindReachableInDistance(coordinate, 0, 2);

    public int[] FindReachableDistance20(Coordinate2D coordinate) => FindReachableInDistance(coordinate, 0, 20);

    public int[] FindReachableInDistance(Coordinate2D coordinate, int distance, int toDistance)
    {
        var current = coordinate;

        int[] ownValue;

        if (grid[current.Y][current.X] == '#')
            ownValue = [];
        else
        {
            var val = pathIndex[current] - distance;

            ownValue = [val];
        }

        if (distance == toDistance)
            return ownValue;

        var neighbours = current.GetNeighbours(true)
            .Where(c => c.IsInBounds(grid))
            .ToArray();

        var neighbourValues = neighbours
            .Select(c => FindReachableInDistance(c, distance + 1, toDistance));

        return ownValue
            .Concat(neighbourValues.SelectMany(c => c))
            .Where(c => c > 0)
            .ToArray();
    }

    public Solution() : this("Input.txt")
    {
    }

    // not 1276
    public object GetResult1()
    {
        return FindShortcuts()
            .SelectMany(c => c)
            .Count(c => c >= TimeToSave);
    }

    public object GetResult2()
    {
        return "";
    }
}