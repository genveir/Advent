using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent18;

public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 }

public class Solution : ISolution
{
    public List<ParsedInput> dinges;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        dinges = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class ParsedInput
    {
        [ComplexParserConstructor("direction distance (#color)")]
        public ParsedInput(char direction, long distance, string color)
        {
            Direction = direction switch
            {
                'U' => Direction.Up,
                'R' => Direction.Right,
                'D' => Direction.Down,
                'L' => Direction.Left,
                _ => throw new InvalidOperationException("invalid direction")
            };
            Distance = distance;
            Color = color;
        }

        public Direction Direction { get; }
        public long Distance { get; }
        public string Color { get; }

        public ActualInstruction Pt1Instruction => new(Direction, Distance);

        public ActualInstruction Pt2Instruction
        {
            get
            {
                var value = long.Parse(Color.Substring(0, 5), System.Globalization.NumberStyles.HexNumber);
                var direction = Color[5] switch
                {
                    '0' => Direction.Right,
                    '1' => Direction.Down,
                    '2' => Direction.Left,
                    '3' => Direction.Up,
                    _ => throw new InvalidOperationException("invalid direction")
                };

                return new(direction, value);
            }
        }
    }

    public record ActualInstruction(Direction Direction, long Distance);

    public Points DrawShape(ActualInstruction[] instructions)
    {
        Points points = new();

        Coordinate current = new(0, 0);
        Point firstPoint = null;
        Point lastPoint = null;
        for (int n = 0; n < instructions.Length; n++)
        {
            var instruction = instructions[n];
            var dist = instruction.Distance;
            var direction = instruction.Direction;

            var newPoint = new Point(current, direction);
            if (firstPoint == null) firstPoint = newPoint;
            points.Add(newPoint);
            if (lastPoint != null)
            {
                points.Add(new Line(newPoint, lastPoint, true));
            }
            lastPoint = newPoint;

            Coordinate vector = direction switch
            {
                Direction.Left => new(-dist, 0),
                Direction.Right => new(dist, 0),
                Direction.Up => new(0, -dist),
                Direction.Down => new(0, dist),
                _ => throw new InvalidOperationException("invalid direction")
            };
            current = current + vector;
        }

        points.Add(new Line(firstPoint, lastPoint, true));

        if (current.X != 0 || current.Y != 0)
            throw new InvalidOperationException("shape is not enclosed");

        if (instructions.Length != points.Count())
            throw new InvalidOperationException("not all points were mapped");

        return points;
    }

    public void DetermineInsideCorners(Points points)
    {
        var topLeftest = points.TopLeftest;
        bool rightTurning = topLeftest.DirectionOfTrench == Direction.Right;

        var currentDirection = points.InOrderOfAdding[0].DirectionOfTrench;
        Direction newDirection;
        for (int n = 1; n < points.InOrderOfAdding.Count; n++)
        {
            var point = points.InOrderOfAdding[n];

            newDirection = point.DirectionOfTrench;
            if (newDirection == currentDirection)
                throw new InvalidOperationException("path is straight");

            point.IsInside = DetermineInside(currentDirection, newDirection, rightTurning);

            currentDirection = newDirection;
        }
        newDirection = points.InOrderOfAdding[0].DirectionOfTrench;
        points.InOrderOfAdding[0].IsInside = DetermineInside(currentDirection, newDirection, rightTurning);
    }

    public bool DetermineInside(Direction current, Direction newDirection, bool rightTurning)
    {
        var turn = (Direction)((newDirection - current + 4) % 4);

        return rightTurning && turn == Direction.Left
            || !rightTurning && turn == Direction.Right;
    }

    public void MakeSquares(Points points)
    {
        for (int n = 0; n < points.InOrderOfAdding.Count; n++)
        {
            var point = points.InOrderOfAdding[n];
            if (!point.IsInside) continue;

            var existingLines = points.LinesByCoord[point.Location];

            bool makeUp = false, makeRight = false, makeDown = false, makeLeft = false;
            if (LineDownFromPoint(point, existingLines) == null) makeDown = true;
            if (LineUpFromPoint(point, existingLines) == null) makeUp = true;
            if (LineRightFromPoint(point, existingLines) == null) makeRight = true;
            if (LineLeftFromPoint(point, existingLines) == null) makeLeft = true;

            var lines = new List<Line>();
            if (makeUp) lines.Add(MakeLine(point, Direction.Up));
            if (makeLeft) lines.Add(MakeLine(point, Direction.Left));
            if (makeDown) lines.Add(MakeLine(point, Direction.Down));
            if (makeRight) lines.Add(MakeLine(point, Direction.Right));

            foreach (var ray in lines)
            {
                points.SetCollisionPoint(ray);
            }
        }
    }

    public Line LineUpFromPoint(Point point, IEnumerable<Line> lines) =>
        lines.SingleOrDefault(l => l.IsVertical && l.Second.Location == point.Location);
    public Line LineDownFromPoint(Point point, IEnumerable<Line> lines) =>
        lines.SingleOrDefault(l => l.IsVertical && l.First.Location == point.Location);
    public Line LineRightFromPoint(Point point, IEnumerable<Line> lines) =>
        lines.SingleOrDefault(l => !l.IsVertical && l.First.Location == point.Location);
    public Line LineLeftFromPoint(Point point, IEnumerable<Line> lines) =>
        lines.SingleOrDefault(l => !l.IsVertical && l.Second.Location == point.Location);

    public Line MakeLine(Point source, Direction direction) =>
        direction switch
        {
            Direction.Up => new Line(source, new(new Coordinate(source.X, -long.MaxValue)), false, true),
            Direction.Right => new Line(source, new(new Coordinate(long.MaxValue, source.Y)), false, true),
            Direction.Down => new Line(source, new(new Coordinate(source.X, long.MaxValue)), false, true),
            Direction.Left => new Line(source, new(new Coordinate(-long.MaxValue, source.Y)), false, true),
            _ => throw new InvalidOperationException("invalid direction")
        };

    public void SetPointLines(Points points)
    {
        foreach (var point in points.InOrderOfAdding)
        {
            var lines = points.LinesByCoord[point.Location];

            var upLine = LineUpFromPoint(point, lines);
            var rightLine = LineRightFromPoint(point, lines);
            var downLine = LineDownFromPoint(point, lines);
            var leftLine = LineLeftFromPoint(point, lines);

            if (upLine != null) point.Lines.Add(Direction.Up, upLine);
            if (rightLine != null) point.Lines.Add(Direction.Right, rightLine);
            if (downLine != null) point.Lines.Add(Direction.Down, downLine);
            if (leftLine != null) point.Lines.Add(Direction.Left, leftLine);
        }
    }

    public long CalculateSquaresSum(Points points)
    {
        long sum = 0;
        foreach (var point in points.InOrderOfAdding)
        {
            sum += CalculateSquareValue(point);
        }
        return sum;
    }

    public long CalculateSquareValue(Point point)
    {
        if (!point.Lines.TryGetValue(Direction.Right, out var right)) return 0;
        if (!right.Second.Lines.TryGetValue(Direction.Down, out var down)) return 0;
        if (!down.Second.Lines.TryGetValue(Direction.Left, out var left)) return 0;
        if (!left.First.Lines.TryGetValue(Direction.Up, out var up)) return 0;

        if (up.First != point)
            throw new InvalidOperationException($"square starting at {point} does not close");

        var xLength = right.Length;
        if (left.Length != xLength) throw new InvalidOperationException("lines do not match");

        var yLength = up.Length;
        if (down.Length != yLength) throw new InvalidOperationException("lines do not match");

        return xLength * yLength;
    }

    public long CalculateInsideLineSum(Points points)
    {
        return points.Lines.Where(l => !l.IsTrench).Sum(l => l.Length);
    }

    public long DetermineSurface(Points points)
    {
        DetermineInsideCorners(points);
        MakeSquares(points);
        SetPointLines(points);

        var squaresSum = CalculateSquaresSum(points);
        var insideLineSum = CalculateInsideLineSum(points);
        var fullInsidePointCount = points.InOrderOfAdding.Count(p => p.IsCreated && p.IsInside);

        return squaresSum - insideLineSum + fullInsidePointCount;
    }

    public object GetResult1()
    {
        var instructions = dinges.Select(d => d.Pt1Instruction).ToArray();

        var points = DrawShape(instructions);

        return DetermineSurface(points);
    }

    public object GetResult2()
    {
        var instructions = dinges.Select(d => d.Pt2Instruction).ToArray();

        var points = DrawShape(instructions);

        return DetermineSurface(points);
    }
}