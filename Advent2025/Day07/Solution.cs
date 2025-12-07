namespace Advent2025.Day07;

public class Solution
{
    public Grid<char> grid;
    public Coordinate2D start;

    public Solution(string input)
    {
        grid = new Grid<char>(Input.GetLetterGrid(input).ToArray());

        for (int y = 0; y < grid.Cells.Length; y++)
        {
            for (int x = 0; x < grid.Cells[y].Length; x++)
            {
                if (grid[x, y] == 'S')
                {
                    start = new(x, y);
                }
            }
        }
    }
    public Solution() : this("Input.txt") { }

    public long Simulate()
    {
        long numSplits = 0;

        HashSet<Coordinate2D> visited = [];
        Queue<Coordinate2D> toVisit = new();
        toVisit.Enqueue(start);

        while (toVisit.Count > 0)
        {
            var current = toVisit.Dequeue();

            if (visited.Contains(current)) continue;
            visited.Add(current);

            if (!grid.IsInBounds(current))
                continue;

            switch (grid[current])
            {
                case '.' or 'S':
                    toVisit.Enqueue(current.ShiftY(1)); break;
                case '^':
                    toVisit.Enqueue(current.ShiftX(1));
                    toVisit.Enqueue(current.ShiftX(-1));
                    numSplits++;
                    break;
            }
        }

        return numSplits;
    }

    public long DynamicSim(Coordinate2D coord, Dictionary<Coordinate2D, long> visited)
    {
        if (visited.ContainsKey(coord))
        {
            return visited[coord];
        }
        if (!grid.IsInBounds(coord))
        {
            return 1;
        }
        long numPaths = 0;

        switch (grid[coord])
        {
            case '.' or 'S':
                numPaths += DynamicSim(coord.ShiftY(1), visited);
                break;
            case '^':
                numPaths += DynamicSim(coord.ShiftX(1), visited);
                numPaths += DynamicSim(coord.ShiftX(-1), visited);
                break;
        }

        visited[coord] = numPaths;
        return numPaths;
    }

    public object GetResult1()
    {
        return Simulate();
    }

    public object GetResult2()
    {
        return DynamicSim(start, new());
    }
}
