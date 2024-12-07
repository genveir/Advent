namespace Advent2024.Day06;

internal class BetterSolver
{
    public char[][] Grid { get; set; }
    public Coordinate2D Start { get; set; }

    public Dictionary<long, List<long>> WallsByX { get; set; }
    public Dictionary<long, List<long>> WallsByY { get; set; }

    public List<Mover> Movers { get; set; }

    public Dictionary<MoverData, Coordinate2D> LeadsToOutOfBounds { get; set; } = [];
    public HashSet<Coordinate2D> VisitedByGuard { get; set; } = [];
    public HashSet<Coordinate2D> LoopSpots { get; set; } = [];

    public BetterSolver(char[][] grid, Coordinate2D start, Dictionary<long, List<long>> wallsByX, Dictionary<long, List<long>> wallsByY)
    {
        Grid = grid;
        Start = start;
        WallsByX = wallsByX;
        WallsByY = wallsByY;

        MakeMovers();
    }

    public long Solve()
    {
        RunMovers();

        var guard = new Mover(Start, 0);

        long loopSpots = 0;
        while (IsInBounds(guard.Position))
        {
            WalkGuard(guard, ref loopSpots);
        }

        return loopSpots;
    }

    public void WalkGuard(Mover guard, ref long loopSpots)
    {
        var forward = guard.Forward();
        var ghost = new Mover(guard.Position, (guard.Direction + 1) % 4);

        LeadsToOutOfBounds.Remove(guard.Data);
        VisitedByGuard.Add(guard.Position);
        if (!VisitedByGuard.Contains(forward) && WalkDoesNotLeadToOutOfBounds(ghost, forward))
        {
            LoopSpots.Add(forward);
            loopSpots++;
        }

        if (IsInBounds(forward) && Grid[forward.Y][forward.X] is '#' or 'O')
        {
            guard.Direction = (guard.Direction + 1) % 4;
        }
        else
        {
            guard.Position = guard.Forward();
        }
    }

    public bool WalkDoesNotLeadToOutOfBounds(Mover ghost, Coordinate2D blocked)
    {
        HashSet<MoverData> visitedByGhost = [];

        while (IsInBounds(ghost.Position))
        {
            if (visitedByGhost.Contains(ghost.Data))
            {
                return true;
            }
            visitedByGhost.Add(ghost.Data);

            if (!LeadsToOutOfBounds.ContainsKey(ghost.Data))
            {
                return true;
            }

            ghost.JumpForwardToWall(WallsByX, WallsByY, blocked);
        }

        return false;
    }

    private void MakeMovers()
    {
        Movers = [];

        for (int y = 0; y < Grid.Length; y++)
        {
            Movers.Add(new(new(0, y), 3));
            Movers.Add(new(new(Grid[y].Length - 1, y), 1));
        }

        for (int x = 0; x < Grid[0].Length; x++)
        {
            Movers.Add(new(new(x, 0), 0));
            Movers.Add(new(new(x, Grid.Length - 1), 2));
        }

        Movers = Movers.Where(m => Grid[m.Position.Y][m.Position.X] != '#').ToList();
    }

    public void RunMovers()
    {
        int maxSteps = 10000;

        while (Movers.Count > 0 && maxSteps > 0)
        {
            StepMovers();

            maxSteps--;
        }

        if (maxSteps == 0)
        {
            throw new Exception("Max steps reached");
        }
    }

    public void StepMovers()
    {
        List<Mover> newMovers = [];
        List<Mover> toRemove = [];
        foreach (var mover in Movers)
        {
            LeadsToOutOfBounds.Add(mover.Data, mover.StartPosition);

            if (ShouldCreateRotatedMover(mover))
            {
                Mover newMover = new(mover.StartPosition, mover.Position, (mover.Direction + 3) % 4);

                newMovers.Add(newMover);
            }

            if (ShouldStepBack(mover))
            {
                mover.Position = mover.Backward();
            }
            else
            {
                toRemove.Add(mover);
            }
        }

        Movers = Movers
            .Except(toRemove)
            .Concat(newMovers)
            .ToList();
    }

    public bool ShouldStepBack(Mover mover)
    {
        var backward = mover.Backward();

        return IsInBounds(backward) && Grid[backward.Y][backward.X] != '#';
    }

    public bool ShouldCreateRotatedMover(Mover mover)
    {
        var toTheLeft = mover.ToTheLeft();

        return IsInBounds(toTheLeft) && Grid[toTheLeft.Y][toTheLeft.X] == '#';
    }

    public bool IsInBounds(Coordinate2D pos)
    {
        return pos.X >= 0 && pos.X < Grid[0].Length && pos.Y >= 0 && pos.Y < Grid.Length;
    }
}