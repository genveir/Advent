namespace Advent2024.Day06;

internal class BetterSolver
{
    public char[][] Grid { get; set; }
    public Coordinate2D Start { get; set; }

    public List<Mover> Movers { get; set; }

    public HashSet<MoverData> LeadsToOutOfBounds { get; set; } = [];
    public HashSet<Coordinate2D> LoopSpots { get; set; } = [];

    public BetterSolver(char[][] grid, Coordinate2D start)
    {
        Grid = grid;
        Start = start;

        MakeMovers();
    }

    public long Solve()
    {
        RunMovers();

        var guard = new Mover(Start, 0);

        long loopSpots = 0;
        while (IsInBounds(guard.Position))
        {
            var forward = guard.Forward();
            var ghost = new Mover(guard.Position, (guard.Direction + 1) % 4);

            if (WalkDoesNotLeadToOutOfBounds(ghost))
            {
                LoopSpots.Add(forward);
                loopSpots++;
            }
            LeadsToOutOfBounds.Remove(guard.Data);

            if (IsInBounds(forward) && Grid[forward.Y][forward.X] == '#')
            {
                guard.Direction = (guard.Direction + 1) % 4;
            }
            else
            {
                guard.Position = guard.Forward();
            }
        }

        return loopSpots;
    }

    private bool WalkDoesNotLeadToOutOfBounds(Mover ghost)
    {
        while (IsInBounds(ghost.Position))
        {
            if (!LeadsToOutOfBounds.Contains(ghost.Data))
            {
                return true;
            }

            var forward = ghost.Forward();
            if (IsInBounds(forward) && Grid[forward.Y][forward.X] == '#')
            {
                ghost.Direction = (ghost.Direction + 1) % 4;
            }
            else
            {
                ghost.Position = ghost.Forward();
            }
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
            LeadsToOutOfBounds.Add(mover.Data);

            if (ShouldCreateRotatedMover(mover))
            {
                Mover newMover = new(mover.Position, (mover.Direction + 3) % 4);

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