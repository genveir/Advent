using Advent2023.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2023.Advent18;
public class Points
{
    public List<Point> InOrderOfAdding = new();

    public Dictionary<long, SortedList<long, Point>> ByX = new();
    public Dictionary<long, SortedList<long, Point>> ByY = new();

    public Dictionary<Coordinate, List<Line>> LinesByCoord = new();
    public List<Line> Lines { get; set; } = new();

    public void Add(Point coord)
    {
        if (!ByX.ContainsKey(coord.X))
            ByX[coord.X] = new();
        ByX[coord.X].Add(coord.Y, coord);

        if (!ByY.ContainsKey(coord.Y))
            ByY[coord.Y] = new();
        ByY[coord.Y].Add(coord.X, coord);

        InOrderOfAdding.Add(coord);
    }

    public bool TryGetPoint(Coordinate coordinate, out Point point)
    {
        point = null;
        if (!ByY.TryGetValue(coordinate.Y, out var list)) return false;
        return list.TryGetValue(coordinate.X, out point);
    }

    public void Add(Line line)
    {
        Lines.Add(line);

        if (!LinesByCoord.ContainsKey(line.First.Location))
            LinesByCoord[line.First.Location] = new();
        LinesByCoord[line.First.Location].Add(line);

        if (!LinesByCoord.ContainsKey(line.Second.Location))
            LinesByCoord[line.Second.Location] = new();
        LinesByCoord[line.Second.Location].Add(line);
    }

    public void Remove(Line line)
    {
        Lines.Remove(line);

        LinesByCoord[line.First.Location].Remove(line);
        LinesByCoord[line.Second.Location].Remove(line);
    }

    public void SplitLine(Line line, Point point)
    {
        Remove(line);

        Add(new Line(line.First, point, line.IsTrench));
        Add(new Line(point, line.Second, line.IsTrench));
    }

    public record CollisionData(Line line, Coordinate coord, long Distance);

    public void SetCollisionPoint(Line line)
    {
        // determine the actual point
        var firstIsPoint = TryGetPoint(line.First.Location, out _);
        var actualPoint = firstIsPoint ? line.First : line.Second;

        // grab all collisions on the line
        var collisions = new List<CollisionData>();
        foreach (var l in Lines)
        {
            if (l.TryGetCollisionCoords(line, out List<Coordinate> coords))
            {
                foreach (var c in coords) collisions.Add(new(l, c, c.ManhattanDistance(actualPoint.Location)));
            }
        }

        collisions = collisions.Where(c => c.coord != actualPoint.Location).ToList();

        if (collisions.Count() == 0)
            throw new InvalidOperationException("a collision has to exist");

        // only the nearest are relevant
        var closestDist = collisions.Min(c => c.Distance);
        var closest = collisions.Where(c => c.Distance == closestDist).ToList();

        // if there is one, we make a new Point
        // if there are more, a point has to exist
        Point point;
        if (closest.Count() == 1)
        {
            var colData = closest.Single();

            point = new Point(colData.coord);
            SplitLine(colData.line, point);

            point.IsInside = !LinesByCoord[point.Location]
                .Any(l => l.IsTrench);

            Add(point);
        }
        else
        {
            TryGetPoint(closest.First().coord, out point);
        }

        var newLine = new Line(actualPoint, point, false);
        Add(newLine);
    }

    public int Count() => ByX.Sum(kvp => kvp.Value.Count);

    public Point TopLeftest => ByY[ByY.Keys.Min()].First().Value;

    public string Print()
    {
        var minX = ByX.Keys.Min();
        var maxX = ByX.Keys.Max();
        var minY = ByY.Keys.Min();
        var maxY = ByY.Keys.Max();

        var grid = new char[maxY - minY + 1][];
        for (int n = 0; n < grid.Length; n++) grid[n] = new char[maxX - minX + 1];
        for (int y = 0; y < grid.Length; y++)
            for (int x = 0; x < grid[y].Length; x++)
                grid[y][x] = '.';

        foreach (var line in Lines)
        {
            var loc = line.First.Location;
            var representation = line.IsTrench ? '#' : line.IsVertical ? '|' : '-';
            Func<Coordinate, Coordinate> shift = line.IsVertical ?
                ((Coordinate c) => c.ShiftY(1)) :
                ((Coordinate c) => c.ShiftX(1));

            while (loc != line.Second.Location)
            {
                grid[loc.Y - minY][loc.X - minX] = representation;

                loc = shift(loc);
            }
        }

        foreach (var point in InOrderOfAdding)
        {
            var loc = point.Location;
            var representation = point.LetterCode;

            grid[loc.Y - minY][loc.X - minX] = representation;
        }

        var sb = new StringBuilder();

        var topNums = new char[4][];
        for (int n = 0; n < 4; n++)
            topNums[n] = new char[grid[0].Length];

        for (int n = 0; n < grid[0].Length; n++)
        {
            var lineNum = (n + minX).ToString().PadLeft(4);
            for (int i = 0; i < 4; i++)
                topNums[i][n] = lineNum[i];
        }

        for (int n = 0; n < 4; n++)
            sb.AppendLine("     " + new string(topNums[n]));

        for (int n = 0; n < grid.Length; n++)
        {
            var lineNum = (n + minY).ToString().PadLeft(4);

            sb.AppendLine(lineNum + " " + new string(grid[n]));
        }
        return sb.ToString();
    }
}