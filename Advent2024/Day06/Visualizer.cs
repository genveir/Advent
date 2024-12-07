namespace Advent2024.Day06;
internal class Visualizer
{
    public static void Visualize(BetterSolver solver, Coordinate2D[] actualLoopSpots)
    {
        solver.RunMovers();

        var guard = new Mover(solver.Start, 0);
        long loopSpots = 0;

        Coordinate2D runTo = null;
        string ghostFeedback = "";
        while (true)
        {
            if (runTo is not null)
            {
                while (IsInBounds(guard.Position, solver.Grid) && guard.Forward() != runTo)
                {
                    solver.WalkGuard(guard, ref loopSpots);
                }
                runTo = null;
            }

            Console.Clear();

            Print(guard, solver);

            if (ghostFeedback is not "")
            {
                Console.WriteLine(ghostFeedback);
                ghostFeedback = "";
            }

            Console.WriteLine("[S]tep e[X]it [R]unTo [T]urn [G]host [W]all");
            var line = Console.ReadLine();

            if (line is "" or "s")
            {
                solver.WalkGuard(guard, ref loopSpots);
            }

            if (line is "x")
            {
                break;
            }

            if (line is "r")
            {
                Console.WriteLine("X?");
                var x = long.Parse(Console.ReadLine());

                Console.WriteLine("Y?");
                var y = long.Parse(Console.ReadLine());

                runTo = new(x, y);
            }

            if (line is "t")
            {
                guard.Direction = (guard.Direction + 1) % 4;
            }

            if (line is "g")
            {
                var ghost = new Mover(guard.Position, (guard.Direction + 1) % 4);

                var doesNotLeadOutOfBounds = solver.WalkDoesNotLeadToOutOfBounds(ghost, guard.Forward());

                ghostFeedback = doesNotLeadOutOfBounds ? "Does not lead to out of bounds" : "Leads to out of bounds";
            }

            if (line is "w")
            {
                var forward = guard.Forward();

                if (IsInBounds(forward, solver.Grid))
                {
                    solver.Grid[forward.Y][forward.X] = 'O';
                }
            }
        }
    }

    private static void Print(Mover guard, BetterSolver solver)
    {
        var grid = solver.Grid;

        // displays an area of 60x20 around the guard
        for (long y = guard.Position.Y - 10; y < guard.Position.Y + 10; y++)
        {
            for (long x = guard.Position.X - 30; x < guard.Position.X + 30; x++)
            {
                if (IsInBounds(new(x, y), grid))
                {
                    if (x == guard.Position.X && y == guard.Position.Y)
                    {
                        Console.Write('x');
                    }
                    else
                    {
                        Console.Write(grid[y][x]);
                    }
                }
            }
            Console.WriteLine();
        }

        var directionString = guard.Direction switch
        {
            0 => "Up",
            1 => "Right",
            2 => "Down",
            3 => "Left",
            _ => throw new Exception("Invalid direction")
        };

        Console.WriteLine($"Guard is at {guard.Position} facing {directionString}");
        if (solver.LeadsToOutOfBounds.ContainsKey(guard.Data))
        {
            Console.WriteLine("Guard will go out of bounds at " + solver.LeadsToOutOfBounds[guard.Data]);
        }
        else
        {
            Console.WriteLine("Guard will not go out of bounds");
        }

        if (solver.LoopSpots.Contains(guard.Forward()))
        {
            Console.WriteLine("Guard is at a loop spot");
        }
    }

    private static bool IsInBounds(Coordinate2D pos, char[][] grid)
    {
        return pos.X >= 0 && pos.X < grid[0].Length && pos.Y >= 0 && pos.Y < grid.Length;
    }
}
