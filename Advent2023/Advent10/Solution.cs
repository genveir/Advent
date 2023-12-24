using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent10;

public class Solution : ISolution
{
    public Dictionary<Coordinate2D, Pipe> Loop = new();
    public Pipe Start;

    public Solution(string input)
    {
        var grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
            for (int x = 0; x < grid[y].Length; x++)
                if (grid[y][x] == 'S')
                {
                    FindLoop(grid, new(x, y));
                }
    }
    public Solution() : this("Input.txt") { }

    public char GetCharFromGrid(char[][] grid, Coordinate2D toFind)
    {
        if (toFind.X < 0 || toFind.Y < 0 || toFind.Y >= grid.Length || toFind.X >= grid[toFind.Y].Length)
            return '.';

        return grid[toFind.Y][toFind.X];
    }

    public enum Direction { Up, Right, Down, Left }
    public bool FindLoop(char[][] grid, Coordinate2D start)
    {
        // we're not setting left and right on start. Can fix if it turns out to be necessary
        Loop.Add(start, new Pipe(Array.Empty<Coordinate2D>(), Array.Empty<Coordinate2D>(), false));

        var above = GetCharFromGrid(grid, start.ShiftY(-1));
        if (above is '|' or '7' or 'F') return FindLoop(grid, start.ShiftY(-1), Direction.Up, Loop);

        var below = GetCharFromGrid(grid, start.ShiftY(1));
        if (below is '|' or 'J' or 'L') return FindLoop(grid, start.ShiftY(1), Direction.Down, Loop);

        return FindLoop(grid, start.ShiftX(1), Direction.Right, Loop);
    }

    public bool FindLoop(char[][] grid, Coordinate2D current, Direction direction, Dictionary<Coordinate2D, Pipe> loop)
    {
        while (true)
        {
            var charToParse = GetCharFromGrid(grid, current);
            if (charToParse == 'S') return true;

            var pipe = ParsePipe(charToParse, current, direction);
            loop.Add(current, pipe);

            (current, direction) = FindNext(charToParse, current, direction);
        }
    }

    public Pipe ParsePipe(char charToParse, Coordinate2D current, Direction direction) =>
        charToParse switch
        {
            '|' => new Pipe(current.ShiftX(-1), current.ShiftX(1), direction != Direction.Up),
            '-' => new Pipe(current.ShiftY(-1), current.ShiftY(1), direction != Direction.Right),
            'J' => new Pipe(current.Shift(-1, -1), new[] { current.ShiftY(1), current.ShiftX(1) }, direction != Direction.Right),
            'L' => new Pipe(current.Shift(1, -1), new[] { current.ShiftX(-1), current.ShiftY(1) }, direction != Direction.Down),
            'F' => new Pipe(current.Shift(1, 1), new[] { current.ShiftX(-1), current.ShiftY(-1) }, direction != Direction.Left),
            '7' => new Pipe(current.Shift(-1, 1), new[] { current.ShiftX(1), current.ShiftY(-1) }, direction != Direction.Up),
            _ => throw new NotImplementedException("invalid pipe char")
        };

    public (Coordinate2D next, Direction newDirection) FindNext(char charToParse, Coordinate2D current, Direction direction)
    {
        var newDirection = charToParse switch
        {
            'J' => direction == Direction.Right ? Direction.Up : Direction.Left,
            'F' => direction == Direction.Left ? Direction.Down : Direction.Right,
            '7' => direction == Direction.Up ? Direction.Left : Direction.Down,
            'L' => direction == Direction.Left ? Direction.Up : Direction.Right,
            _ => direction
        };

        var newCoordinate = newDirection switch
        {
            Direction.Up => current.ShiftY(-1),
            Direction.Right => current.ShiftX(1),
            Direction.Down => current.ShiftY(1),
            Direction.Left => current.ShiftX(-1),
            _ => throw new NotImplementedException("invalid direction")
        };

        return (newCoordinate, newDirection);
    }

    public class Pipe
    {
        public Coordinate2D[] Left { get; set; }
        public Coordinate2D[] Right { get; set; }

        public Pipe(Coordinate2D[] left, Coordinate2D[] right, bool swap)
        {
            Left = swap ? right : left;
            Right = swap ? left : right;
        }

        public Pipe(Coordinate2D left, Coordinate2D[] right, bool swap) : this(new[] { left }, right, swap) { }
        public Pipe(Coordinate2D[] left, Coordinate2D right, bool swap) : this(left, new[] { right }, swap) { }
        public Pipe(Coordinate2D left, Coordinate2D right, bool swap) : this(new[] { left }, new[] { right }, swap) { }

        public Coordinate2D[] Inside(bool leftIsOutside) => leftIsOutside ? Right : Left;
    }

    public void Flood(bool leftIsOutside, HashSet<Coordinate2D> blockers)
    {
        Queue<Coordinate2D> coordinates = new();
        foreach(var pipe in Loop.Values)
        {
            var insideCoords = pipe.Inside(leftIsOutside);
            foreach(var coord in insideCoords) coordinates.Enqueue(coord);
        }

        while (coordinates.Count > 0)
        {
            var coord = coordinates.Dequeue();
            
            if (blockers.Contains(coord)) continue;
            blockers.Add(coord);

            coordinates.Enqueue(coord.ShiftX(1));
            coordinates.Enqueue(coord.ShiftX(-1));
            coordinates.Enqueue(coord.ShiftY(1));
            coordinates.Enqueue(coord.ShiftY(-1));
        }
    }

    public object GetResult1()
    {
        return Loop.Count / 2;
    }

    public object GetResult2()
    {
        var toppest = Loop.Keys.Min(c => c.Y);
        var leftest = Loop.Keys.Where(c => c.Y == toppest).Min(c => c.X);
        var topLeftCoord = new Coordinate2D(leftest, toppest);
        var topLeftPipe = Loop[topLeftCoord];

        bool leftIsOutside = topLeftPipe.Left.Contains(topLeftCoord.ShiftY(-1));
        var blockers = new HashSet<Coordinate2D>(Loop.Keys);

        Flood(leftIsOutside, blockers);

        return blockers.Count - Loop.Count;
    }
}
