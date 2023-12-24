using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;
using Advent2023.Shared.Mathemancy;

namespace Advent2023.Advent24;

public class Solution : ISolution
{
    public List<Line> Lines;

    public long TestAreaStart = 200_000_000_000_000;
    public long TestAreaEnd = 400_000_000_000_000;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<Line>("line");

        Lines = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class Line
    {
        public Coordinate3D InitialCoordinate;
        public Coordinate3D Vector;

        public LinearEquation YFromX;
        public LinearEquation ZFromX;

        public bool AddedX = false;
        public bool AddedY = false;
        public bool AddedZ = false;

        [ComplexParserTarget("coord @ vector")]
        public Line(Coordinate3D coordinate, Coordinate3D vector)
        {
            // test assumption that no vector has a 0 component
            if (vector.X == 0 || vector.Y == 0 || vector.Z == 0)
                throw new ArgumentException("assumption about 0-components is wrong");

            InitialCoordinate = coordinate;
            Vector = vector;

            YFromX = new LinearEquation(coordinate.Y, coordinate.X, new Fraction(vector.Y, vector.X));
            ZFromX = new LinearEquation(coordinate.Z, coordinate.X, new Fraction(vector.Z, vector.X));
        }

        public bool IntersectsInArea(Line other, long lowerBound, long upperBound)
        {
            if (YFromX.TryFindIntersection(other.YFromX, out _, out var input, out var output))
            {
                return input >= lowerBound && input <= upperBound &&
                    output >= lowerBound && output <= upperBound &&
                    IsIntheFuture(input) && other.IsIntheFuture(input);
            }
            return false;
        }

        public bool IsIntheFuture(Fraction x) =>
            Vector.X < 0 && x < InitialCoordinate.X ||
            Vector.X > 0 && x > InitialCoordinate.X;

        public bool IsParallel(Line other)
        {
            return YFromX.A == other.YFromX.A &&
                ZFromX.A == other.ZFromX.A;
        }

        public override string ToString() => $"{InitialCoordinate} @ {Vector}";
    }

    public object GetResult1()
    {
        long num = 0;
        for (int n = 0; n < Lines.Count; n++)
        {
            var first = Lines[n];

            for (int i = n + 1; i < Lines.Count; i++)
            {
                var second = Lines[i];

                if (first.IntersectsInArea(second, TestAreaStart, TestAreaEnd))
                    num++;
            }
        }

        // 3337 too low
        return num;
    }

    public object GetResult2()
    {
        // there are no parallel lines
        // there are no hailstones with matching start x,y or z

        // there are lots of pairs of hailstones with a matching velocity in a dimension
        // there are no pairs of hailstones with two matching velocities

        // should be possible to find possible velocities for each based on the speeds you could use to hit them in integer intervals?
        var (xMatches, yMatches, zMatches) = FindMatchingVelocityGroups();

        var xMatchList = xMatches.Values.ToList();
        var yMatchList = yMatches.Values.ToList();
        var zMatchList = zMatches.Values.ToList();

        var xOptions = GetVelocityOptions(xMatchList[0], (l) => l.InitialCoordinate.X, (l) => l.Vector.X);
        for (int n = 1; n < xMatchList.Count; n++)
            xOptions = xOptions.Intersect(GetVelocityOptions(xMatchList[n], (l) => l.InitialCoordinate.X, (l) => l.Vector.X)).ToList();

        var yOptions = GetVelocityOptions(yMatchList[0], (l) => l.InitialCoordinate.Y, (l) => l.Vector.Y);
        for (int n = 1; n < yMatchList.Count; n++)
            yOptions = yOptions.Intersect(GetVelocityOptions(yMatchList[n], (l) => l.InitialCoordinate.Y, (l) => l.Vector.Y)).ToList();

        var zOptions = GetVelocityOptions(zMatchList[0], (l) => l.InitialCoordinate.Z, (l) => l.Vector.Z);
        for (int n = 1; n < zMatchList.Count; n++)
            zOptions = zOptions.Intersect(GetVelocityOptions(zMatchList[n], (l) => l.InitialCoordinate.Z, (l) => l.Vector.Z)).ToList();

        var vector = new Coordinate3D(xOptions.Single(), yOptions.Single(), zOptions.Single());

        // vector eraf geeft lijn voor de hit, doen voor 2 en intersectie?

        var l1 = new Line(Lines[0].InitialCoordinate, Lines[0].Vector - vector);
        var l2 = new Line(Lines[1].InitialCoordinate, Lines[1].Vector - vector);

        var intersection = l1.YFromX.TryFindIntersection(l2.YFromX, out _, out var XPos, out var YPos);
        var intersection2 = l1.ZFromX.TryFindIntersection(l2.ZFromX, out _, out var XCheck, out var ZPos);

        var val = (XPos + YPos + ZPos).ToBigint();

        return val;
    }

    private List<long> GetVelocityOptions(List<Line> lines, Func<Line, long> diffVal, Func<Line, long> vecVal)
    {
        List<long> options = GetVelocityOptions(lines[0], lines[1], diffVal, vecVal);
        for (int n = 0; n < lines.Count; n++)
        {
            for (int i = n + 1; i < lines.Count; i++)
            {
                options = options.Intersect(GetVelocityOptions(lines[n], lines[i], diffVal, vecVal)).ToList();
            }
        }

        return options;
    }

    public List<long> GetVelocityOptions(Line first, Line second, Func<Line, long> diffVal, Func<Line, long> vecVal)
    {
        List<long> options = new();

        var distance = diffVal(second) - diffVal(first);
        for (long n = -250; n < 250; n++)
        {
            if (n == vecVal(first)) continue;
            if (distance % (n - vecVal(first)) == 0)
                options.Add(n);
        }
        return options;
    }

    public (Dictionary<long, List<Line>> xMatches, Dictionary<long, List<Line>> yMatches, Dictionary<long, List<Line>> zMatches) FindMatchingVelocityGroups()
    {
        Dictionary<long, List<Line>> xMatches = new();
        Dictionary<long, List<Line>> yMatches = new();
        Dictionary<long, List<Line>> zMatches = new();
        for (int n = 0; n < Lines.Count; n++)
        {
            var first = Lines[n];

            for (int i = n + 1; i < Lines.Count; i++)
            {
                var second = Lines[i];

                if (first.Vector.X == second.Vector.X)
                {
                    if (!xMatches.ContainsKey(first.Vector.X))
                        xMatches[first.Vector.X] = new();

                    if (!first.AddedX) xMatches[first.Vector.X].Add(first);
                    if (!second.AddedX) xMatches[first.Vector.X].Add(second);

                    first.AddedX = true;
                    second.AddedX = true;
                }
                if (first.Vector.Y == second.Vector.Y)
                {
                    if (!yMatches.ContainsKey(first.Vector.Y))
                        yMatches[first.Vector.Y] = new();

                    if (!first.AddedY) yMatches[first.Vector.Y].Add(first);
                    if (!second.AddedY) yMatches[first.Vector.Y].Add(second);

                    first.AddedY = true;
                    second.AddedY = true;
                }
                if (first.Vector.Z == second.Vector.Z)
                {
                    if (!zMatches.ContainsKey(first.Vector.Z))
                    {
                        zMatches[first.Vector.Z] = new();
                    }
                    if (!first.AddedZ) zMatches[first.Vector.Z].Add(first);
                    if (!second.AddedZ) zMatches[first.Vector.Z].Add(second);

                    first.AddedZ = true;
                    second.AddedZ = true;
                }
            }
        }

        return (xMatches, yMatches, zMatches);
    }
}
