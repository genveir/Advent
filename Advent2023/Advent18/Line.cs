using Advent2023.Shared;
using System;
using System.Collections.Generic;

namespace Advent2023.Advent18;
public class Line
{
    public Point First { get; set; }
    public Point Second { get; set; }

    public bool IsVertical { get; set; }

    public bool IsTrench { get; set; }

    public long Length { get; set; }

    public Line(Point first, Point second, bool isTrench, bool isRay = false)
    {
        bool swap;
        if (first.X == second.X)
        {
            IsVertical = true;
            if (!isRay) Length = Math.Abs(first.Y - second.Y) + 1;
            swap = first.Y > second.Y;
        }
        else
        {
            IsVertical = false;
            if (!isRay) Length = Math.Abs(first.X - second.X) + 1;
            swap = first.X > second.X;
        }
        First = swap ? second : first;
        Second = swap ? first : second;
        IsTrench = isTrench;
    }

    public bool TryGetCollisionCoords(Line line, out List<Coordinate2D> coords)
    {
        coords = new();

        return (IsVertical, line.IsVertical) switch
        {
            (true, false) => IntersectVerticalWithHorizontal(this, line, out coords),
            (false, true) => IntersectVerticalWithHorizontal(line, this, out coords),
            (true, true) => IntersectVerticalLines(this, line, out coords),
            (false, false) => IntersectHorizontalLines(this, line, out coords)
        };
    }

    public static bool IntersectVerticalWithHorizontal(Line vertical, Line horizontal, out List<Coordinate2D> intersection)
    {
        intersection = new();
        var prospect = new Coordinate2D(vertical.First.X, horizontal.First.Y);

        if (prospect.Y >= vertical.First.Y && prospect.Y <= vertical.Second.Y &&
            prospect.X >= horizontal.First.X && prospect.X <= horizontal.Second.X)
        {
            intersection.Add(prospect);
        }

        return intersection.Count > 0;
    }

    public static bool IntersectVerticalLines(Line existing, Line large, out List<Coordinate2D> intersection)
    {
        intersection = new();
        if (existing.First.X != large.First.X) return false;

        if (existing.First.Y >= large.First.Y && existing.First.Y <= large.Second.Y)
            intersection.Add(existing.First.Location);
        if (existing.Second.Y >= large.First.Y && existing.Second.Y <= large.Second.Y)
            intersection.Add(existing.Second.Location);

        return intersection.Count > 0;
    }

    public static bool IntersectHorizontalLines(Line existing, Line large, out List<Coordinate2D> intersection)
    {
        intersection = new();
        if (existing.First.Y != large.First.Y) return false;

        if (existing.First.X >= large.First.X && existing.First.X <= large.Second.X)
            intersection.Add(existing.First.Location);
        if (existing.Second.X >= large.First.X && existing.Second.X <= large.Second.X)
            intersection.Add(existing.Second.Location);

        return intersection.Count > 0;
    }

    public override string ToString()
    {
        var letterCode = IsTrench ? "T " : "";

        return $"{letterCode}({First}) - ({Second})";
    }
}