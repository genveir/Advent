using System.Collections.Concurrent;

namespace Advent2024.Day06;

public class BetterSolver
{
    public char[][] Grid { get; set; }
    public Coordinate2D Start { get; set; }

    public Dictionary<long, List<long>> WallsByX { get; set; }
    public Dictionary<long, List<long>> WallsByY { get; set; }

    public HashSet<Coordinate2D> VisitedByGuard { get; set; } = [];
    public ConcurrentDictionary<Coordinate2D, bool> LoopSpots { get; set; } = new();

    public BetterSolver(char[][] grid, Coordinate2D start, Dictionary<long, List<long>> wallsByX, Dictionary<long, List<long>> wallsByY)
    {
        Grid = grid;
        Start = start;
        WallsByX = wallsByX;
        WallsByY = wallsByY;
    }

    public void Solve()
    {
        if (VisitedByGuard.Count > 0)
        {
            return;
        }

        var guard = new Mover(Start, 0);

        List<Task> tasks = [];

        while (IsInBounds(guard.Position))
        {
            tasks.Add(WalkGuard(guard));
        }

        Task.WaitAll(tasks);
    }

    public Task WalkGuard(Mover guard)
    {
        var forward = guard.Forward();

        VisitedByGuard.Add(guard.Position);

        Task task = Task.CompletedTask;
        var currentData = guard.Data;
        if (!VisitedByGuard.Contains(forward))
        {
            task = Task.Run(() => RunGhost(currentData, forward));
        }

        if (IsInBounds(forward) && Grid[forward.Y][forward.X] is '#' or 'O')
        {
            guard.Direction = (guard.Direction + 1) % 4;
        }
        else
        {
            guard.Position = guard.Forward();
        }

        return task;
    }

    private void RunGhost(MoverData guardData, Coordinate2D blocked)
    {
        var ghost = new Mover(guardData.Position, (guardData.Direction + 1) % 4);

        if (WalkDoesNotLeadToOutOfBounds(ghost, blocked))
        {
            LoopSpots.AddOrUpdate(blocked, true, (_, _) => true);
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

            ghost.JumpForwardToWall(WallsByX, WallsByY, blocked);
        }

        return false;
    }

    public bool IsInBounds(Coordinate2D pos)
    {
        return pos.X >= 0 && pos.X < Grid[0].Length && pos.Y >= 0 && pos.Y < Grid.Length;
    }
}