namespace Advent2024.Day06;

public class Solution
{
    public char[][] grid;
    public HashSet<Coordinate2D> walls = [];
    public Dictionary<long, List<long>> wallsByX = [];
    public Dictionary<long, List<long>> wallsByY = [];

    public Coordinate2D start;

    public BetterSolver solver;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '^')
                {
                    start = new Coordinate2D(x, y);
                }

                if (grid[y][x] == '#')
                {
                    walls.Add(new Coordinate2D(x, y));
                }
            }

            walls.Add(new(-2, y));
            walls.Add(new(grid[y].Length + 1, y));
        }

        for (int x = 0; x < grid[0].Length; x++)
        {
            walls.Add(new(x, -2));
            walls.Add(new(x, grid.Length + 1));
        }

        foreach (var wall in walls)
        {
            if (!wallsByX.ContainsKey(wall.X)) wallsByX.Add(wall.X, []);
            if (!wallsByY.ContainsKey(wall.Y)) wallsByY.Add(wall.Y, []);

            wallsByX[wall.X].Add(wall.Y);
            wallsByY[wall.Y].Add(wall.X);
        }

        solver = new(grid, start, wallsByX, wallsByY);
    }

    public Solution() : this("Input.txt")
    {
    }


    public object GetResult1()
    {
        solver.Solve();

        return solver.VisitedByGuard.Count;
    }

    // not 492, not 443
    public object GetResult2()
    {
        solver.Solve();

        return solver.LoopSpots.Count;
    }
}