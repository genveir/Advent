
namespace Advent2024.Day06;

public class Solution : ISolution
{
    public char[][] grid;
    public Coordinate2D start;
    public Coordinate2D guard;
    public int direction = 0;
    public Dictionary<Coordinate2D, List<int>> visited = [];
    public HashSet<Coordinate2D> loopSpots = [];

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '^')
                {
                    guard = new Coordinate2D(x, y);
                    start = new Coordinate2D(x, y);
                }
            }
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public bool Move()
    {
        if (!visited.TryGetValue(guard, out List<int> directions))
        {
            directions = [];
            visited.Add(guard, directions);
        }
        if (directions.Contains(direction)) return true;
        directions.Add(direction);

        var nextPos = direction switch
        {
            0 => guard.ShiftY(-1),
            1 => guard.ShiftX(1),
            2 => guard.ShiftY(1),
            3 => guard.ShiftX(-1),
            _ => throw new Exception("Invalid direction")
        };

        bool turned = false;
        if (IsInBounds(nextPos))
        {
            if (grid[nextPos.Y][nextPos.X] == '#')
            {
                turned = true;
                direction = (direction + 1) % 4;
            }
            //else
            //{
            //    if (CheckLoop(direction))
            //    {
            //        loopSpots.Add(nextPos);
            //    }
            //}
        }

        if (!turned) guard = nextPos;

        return false;
    }

    // this doesn't work for the main problem, but why not? <-- because loops don't have to be on the existing path
    public bool CheckLoop(int loopDir)
    {
        loopDir = (loopDir + 1) % 4;

        var pos = guard;

        while (IsInBounds(pos))
        {
            if (grid[pos.Y][pos.X] == '#') return false;

            if (visited.TryGetValue(pos, out var directions))
            {
                if (directions.Contains(loopDir))
                {
                    return true;
                }
            }

            pos = loopDir switch
            {
                0 => pos.ShiftY(-1),
                1 => pos.ShiftX(1),
                2 => pos.ShiftY(1),
                3 => pos.ShiftX(-1),
                _ => throw new Exception("Invalid direction")
            };
        }

        return false;
    }

    public bool IsInBounds(Coordinate2D pos)
    {
        return pos.X >= 0 && pos.X < grid[0].Length && pos.Y >= 0 && pos.Y < grid.Length;
    }

    public object GetResult1()
    {
        while (IsInBounds(guard))
        {
            Move();
        }

        return visited.Count;
    }

    // not 492, not 443
    public object GetResult2()
    {
        while (IsInBounds(guard))
        {
            Move();
        }

        var checkSpots = visited.Keys.ToList();

        foreach (var spot in checkSpots)
        {
            var (x, y) = spot;

            var current = grid[y][x];

            grid[y][x] = '#';

            guard = start;
            direction = 0;
            visited = [];

            while (IsInBounds(guard))
            {
                var exitedBecauseOfLoop = Move();

                if (exitedBecauseOfLoop)
                {
                    loopSpots.Add(new(x, y));
                    break;
                }
            }

            grid[y][x] = current;
        }

        return loopSpots.Count;
    }
}