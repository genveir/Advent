using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2023.Shared;

namespace Advent2023.Advent14;

public class Solution : ISolution
{
    public Dictionary<long, List<Rock>> ByX { get; set; } = new();
    public Dictionary<long, List<Rock>> ByY { get; set; } = new();
    public HashSet<Rock> MovingRocks = new();

    long MaxY;

    public Solution(string input)
    {
        var grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            ByY[y] = new();
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (y == 0) ByX[x] = new();

                var coord = new Coordinate2D(x, y);
                Rock rock;
                if (grid[y][x] == 'O')
                {
                    rock = new Rock(coord, true);
                }
                else if (grid[y][x] == '#')
                {
                    rock = new Rock(coord, false);
                }
                else continue;

                AddRock(rock);
            }
        }

        ByX[-1] = new();
        ByX[grid[0].Length] = new();
        ByY[-1] = new();
        ByY[grid.Length] = new();

        for (int x = -1; x <= grid[0].Length; x++)
        {
            var above = new Coordinate2D(x, -1);
            AddRock(new(above, false));

            var below = new Coordinate2D(x, grid.Length);
            AddRock(new(below, false));
        }

        for (int y = 0; y < grid.Length; y++)
        {
            var left = new Coordinate2D(-1, y);
            AddRock(new(left, false));

            var right = new Coordinate2D(grid[0].Length, y);
            AddRock(new(right, false));
        }

        ReOrderLists();

        MaxY = grid.Length;
    }
    public Solution() : this("Input.txt") { }

    public void AddRock(Rock rock)
    {
        var coord = rock.Position;

        if (!rock.IsStatic) MovingRocks.Add(rock);

        ByX[coord.X].Add(rock);
        ByY[coord.Y].Add(rock);
    }

    public class Rock
    {
        public Coordinate2D Position;
        public bool IsStatic;

        public long LastMoved = -1;

        public Rock(Coordinate2D position, bool canMove)
        {
            Position = position;
            IsStatic = !canMove;
        }

        public override string ToString()
        {
            return $"{(IsStatic ? '#' : 'O')} {Position}";
        }
    }

    public void ReOrderLists()
    {
        foreach (var key in ByX.Keys)
        {
            ByX[key] = ByX[key].OrderBy(r => r.Position.Y).ToList();
        }

        foreach (var key in ByY.Keys)
        {
            ByY[key] = ByY[key].OrderBy(r => r.Position.X).ToList();
        }
    }

    long cycleNum = 0;
    public void RunCycle()
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        TiltEast();

        cycleNum++;
    }

    public void TiltNorth()
    {
        foreach (var key in ByX.Keys)
        {
            var rockRow = ByX[key].ToArray();

            for (int byY = 1; byY < rockRow.Length - 1; byY++)
            {
                var toMove = rockRow[byY];
                var destination = rockRow[byY - 1].Position.ShiftY(1);

                MoveRock(toMove, destination);
            }
        }

        ReOrderLists();
    }

    public void TiltSouth()
    {
        foreach (var key in ByX.Keys)
        {
            var rockRow = ByX[key].ToArray();

            for (int byY = rockRow.Length - 2; byY > 0; byY--)
            {
                var toMove = rockRow[byY];
                var destination = rockRow[byY + 1].Position.ShiftY(-1);

                MoveRock(toMove, destination);
            }
        }

        ReOrderLists();
    }

    public void TiltWest()
    {
        foreach (var key in ByY.Keys)
        {
            var rockRow = ByY[key].ToArray();

            for (int byX = 1; byX < rockRow.Length - 1; byX++)
            {
                var toMove = rockRow[byX];
                var destination = rockRow[byX - 1].Position.ShiftX(1);

                MoveRock(toMove, destination);
            }
        }

        ReOrderLists();
    }

    public void TiltEast()
    {
        foreach (var key in ByY.Keys)
        {
            var rockRow = ByY[key].ToArray();

            for (int byX = rockRow.Length - 2; byX > 0; byX--)
            {
                var toMove = rockRow[byX];
                var destination = rockRow[byX + 1].Position.ShiftX(-1);

                MoveRock(toMove, destination);
            }
        }

        ReOrderLists();
    }

    public void MoveRock(Rock rock, Coordinate2D destination)
    {
        if (rock.IsStatic) return;
        if (rock.Position == destination) return;

        ByX[rock.Position.X].Remove(rock);
        ByY[rock.Position.Y].Remove(rock);

        rock.Position = destination;
        ByX[rock.Position.X].Add(rock);
        ByY[rock.Position.Y].Add(rock);
    }

    public void FindPattern()
    {
        long[] last5 = new long[5] { 0, 0, 0, 0, 0 };

        HashSet<string> lastFives = new();
        while (true)
        {
            RunCycle();
            last5[0] = last5[1];
            last5[1] = last5[2];
            last5[2] = last5[3];
            last5[3] = last5[4];
            last5[4] = Load;

            var asString = string.Join(',', last5);

            if (lastFives.Contains(asString)) break;
            else lastFives.Add(asString);
        }
    }

    public long FindPatternLength()
    {
        var currentLoad = Load;
        var currentCycle = cycleNum;

        while(true)
        {
            RunCycle();

            if (Load == currentLoad) return cycleNum - currentCycle;
        }
    }

    public object GetResult1()
    {
        TiltNorth();

        return Load;
    }

    public object GetResult2()
    {
        FindPattern();
        var length = FindPatternLength();
        long cyclesToGo = 1_000_000_000 - cycleNum;
        var cyclesToSpool = cyclesToGo % length;

        for (int n = 0; n < cyclesToSpool; n++) RunCycle();

        return Load;
    }

    public long Load => MovingRocks.Sum(r => MaxY - r.Position.Y);

    public override string ToString()
    {
        var maxX = ByX.Keys.Max();
        var maxY = ByY.Keys.Max();

        StringBuilder builder = new();
        for (int y = 0; y < maxY; y++)
        {
            var row = ByY[y];
            for (int x = 0; x < maxX; x++)
            {
                var rock = row.SingleOrDefault(r => r.Position.X == x);

                if (rock != null)
                    builder.Append(rock.IsStatic ? '#' : 'O');
                else builder.Append('.');
            }
            builder.AppendLine();
        }

        return builder.ToString().TrimEnd();
    }
}
