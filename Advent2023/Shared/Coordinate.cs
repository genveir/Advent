using Advent2023.Shared.InputParsing;
using System;
using System.Collections.Generic;

namespace Advent2023.Shared;

public class Coordinate
{
    public Coordinate(long x, long y, long? z = null)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    [ComplexParserConstructor("coords")]
    public Coordinate(long[] coords)
    {
        if (coords.Length < 2)
        {
            throw new ArgumentException("coord input must be of length 2 or 3");
        }
        if (coords.Length >= 2)
        {
            this.X = coords[0];
            this.Y = coords[1];
        }
        if (coords.Length == 3)
        {
            this.Z = coords[2];
        }
        if (coords.Length > 3)
        {
            throw new ArgumentException("coord input must be of length 2 or 3");
        }
    }

    public long X;
    public long Y;
    public long? Z;

    public long AbsX => Math.Abs(X);
    public long AbsY => Math.Abs(Y);
    public long? AbsZ => Z == null ? null : Math.Abs(Z.Value);

    public Coordinate ShiftX(long shift) => new Coordinate(this.X + shift, this.Y, this.Z);
    public Coordinate ShiftY(long shift) => new Coordinate(this.X, this.Y + shift, this.Z);
    public Coordinate ShiftZ(long shift)
    {
        if (this.Z == null)
        {
            if (shift == 0)
                return new Coordinate(this.X, this.Y);
            else
                return new Coordinate(this.X, this.Y, shift);
        }
        else return new Coordinate(this.X, this.Y, this.Z + shift);
    }

    public Coordinate Shift(long shiftX, long shiftY, long shiftZ = 0) =>
        ShiftX(shiftX).ShiftY(shiftY).ShiftZ(shiftZ);

    private IEnumerable<Coordinate> _neighbours;
    public IEnumerable<Coordinate> GetNeighbours(bool orthogonalOnly = false)
    {
        if (_neighbours == null)
        {
            var neighbours = new List<Coordinate>();

            for (int xShift = -1; xShift <= 1; xShift++)
            {
                for (int yShift = -1; yShift <= 1; yShift++)
                {
                    if (Z.HasValue)
                    {
                        for (int zShift = -1; zShift <= 1; zShift++)
                        {
                            var zeroCount = (xShift == 0 ? 1 : 0) + (yShift == 0 ? 1 : 0) + (zShift == 0 ? 1 : 0);
                            if (zeroCount == 3) continue;
                            if (orthogonalOnly && zeroCount != 2) continue;

                            neighbours.Add(new Coordinate(X + xShift, Y + yShift, Z.Value + zShift));
                        }
                    }
                    else
                    {
                        var zeroCount = (xShift == 0 ? 1 : 0) + (yShift == 0 ? 1 : 0);
                        if (zeroCount == 2) continue;
                        if (orthogonalOnly && zeroCount != 1) continue;

                        neighbours.Add(new Coordinate(X + xShift, Y + yShift));
                    }
                }
            }
            _neighbours = neighbours;
        }
        return _neighbours;
    }


    public long IntegerDistance(Coordinate second) { return (long)Distance(second); }
    public double Distance(Coordinate second)
    {
        return Distance(second.X, second.Y, second.Z);
    }

    public long ManhattanDistance(Coordinate second)
    {
        var diff = Difference(second);

        return diff.AbsX + diff.AbsY + (diff.AbsZ ?? 0);
    }

    public long IntegerDistance(long x, long y, long z) { return (long)Distance(x, y, z); }
    public double Distance(long x, long y, long? z)
    {
        long zVal = z.HasValue ? z.Value : 0;
        long ZVal = Z.HasValue ? Z.Value : 0;

        var squared =
            Math.Abs(X - x) * Math.Abs(X - x) +
            Math.Abs(Y - y) * Math.Abs(Y - y) +
            Math.Abs(ZVal - zVal) * Math.Abs(ZVal - zVal);

        return Math.Sqrt(squared);
    }
    public static Coordinate operator +(Coordinate first, Coordinate second) =>
        first.Sum(second);

    public Coordinate Sum(Coordinate second) =>
        new(X + second.X, Y + second.Y, Z + second.Z);

    public static Coordinate operator -(Coordinate first, Coordinate second) =>
        first.Difference(second);

    public static bool operator ==(Coordinate first, Coordinate second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null)) return false;
        if (ReferenceEquals(second, null)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(Coordinate first, Coordinate second) =>
        !(first == second);

    public Coordinate Difference(Coordinate second) =>
        new(X - second.X, Y - second.Y, Z - second.Z);

    public override string ToString()
    {
        return "(" + X + ", " + Y +
            (Z.HasValue ? (", " + Z.Value) : "") +
            ")";
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() + Y.GetHashCode() + (Z ?? 0).GetHashCode();
    }

    public override bool Equals(object obj)
    {
        var other = obj as Coordinate;
        if (ReferenceEquals(other, null)) return false;
        return other.X == X && other.Y == Y && other.Z == Z;
    }
}
