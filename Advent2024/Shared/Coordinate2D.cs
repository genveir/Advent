namespace Advent2024.Shared;

public class Coordinate2D : IEquatable<Coordinate2D>
{
    public Coordinate2D(long x, long y)
    {
        X = x;
        Y = y;
    }

    [ComplexParserTarget("coords")]
    public Coordinate2D(long[] coords)
    {
        if (coords.Length == 2)
        {
            X = coords[0];
            Y = coords[1];
        }
        else
        {
            throw new ArgumentException("coord2D input must be of length 2");
        }
    }

    public void Deconstruct(out long x, out long y)
    {
        x = X;
        y = Y;
    }

    public long X;
    public long Y;

    public long AbsX => Math.Abs(X);
    public long AbsY => Math.Abs(Y);

    public Coordinate2D ShiftX(long shift) => new(X + shift, Y);

    public Coordinate2D ShiftY(long shift) => new(X, Y + shift);

    public Coordinate3D ShiftZ(long shift) => new(X, Y, shift);

    public Coordinate2D Shift(long shiftX, long shiftY) =>
        ShiftX(shiftX).ShiftY(shiftY);

    public Coordinate3D ToCoordinate3D(long z = 0) => new(X, Y, z);

    private IEnumerable<Coordinate2D> _neighbours;

    public IEnumerable<Coordinate2D> GetNeighbours(bool orthogonalOnly = false)
    {
        if (_neighbours == null)
        {
            var neighbours = new List<Coordinate2D>();

            for (int xShift = -1; xShift <= 1; xShift++)
            {
                for (int yShift = -1; yShift <= 1; yShift++)
                {
                    var zeroCount = (xShift == 0 ? 1 : 0) + (yShift == 0 ? 1 : 0);
                    if (zeroCount == 2) continue;
                    if (orthogonalOnly && zeroCount != 1) continue;

                    neighbours.Add(new Coordinate2D(X + xShift, Y + yShift));
                }
            }
            _neighbours = neighbours;
        }
        return _neighbours;
    }

    public long IntegerDistance(Coordinate2D second) =>
        (long)Distance(second);

    public long IntegerDistance(long x, long y) =>
        (long)Distance(x, y);

    public double Distance(Coordinate2D second) =>
        Distance(second.X, second.Y);

    public double Distance(long x, long y)
    {
        var squared =
            Math.Abs(X - x) * Math.Abs(X - x) +
            Math.Abs(Y - y) * Math.Abs(Y - y);

        return Math.Sqrt(squared);
    }

    public long ManhattanDistance(Coordinate2D second)
    {
        var diff = Difference(second);

        return diff.AbsX + diff.AbsY;
    }

    public Coordinate2D Difference(Coordinate2D second) =>
        new(X - second.X, Y - second.Y);

    public bool IsInBounds(long minX, long maxX, long minY, long maxY) =>
    X >= minX && X <= maxX && Y >= minY && Y <= maxY;

    public bool IsInBounds(Coordinate2D first, Coordinate2D second)
    {
        var minX = Math.Min(first.X, second.X);
        var maxX = Math.Max(first.X, second.X);

        var minY = Math.Min(first.Y, second.Y);
        var maxY = Math.Max(first.Y, second.Y);

        return IsInBounds(minX, maxX, minY, maxY);
    }

    public bool IsInBounds<T>(T[][] grid) =>
        Y >= 0 && Y < grid.Length && X >= 0 && X < grid[Y].Length;

    public static Coordinate2D operator +(Coordinate2D first, Coordinate2D second) =>
        first.Sum(second);

    public Coordinate2D Sum(Coordinate2D second) =>
        new(X + second.X, Y + second.Y);

    public static Coordinate2D operator -(Coordinate2D first, Coordinate2D second) =>
        first.Difference(second);

    public static bool operator ==(Coordinate2D first, Coordinate2D second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (first is null) return false;
        return first.Equals(second);
    }

    public static bool operator !=(Coordinate2D first, Coordinate2D second) =>
        !(first == second);

    public override string ToString() => $"({X}, {Y})";

    public override int GetHashCode()
    {
        return X.GetHashCode() + 3719 * Y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Coordinate2D other) return false;
        return other.X == X && other.Y == Y;
    }

    public bool Equals(Coordinate2D other) => other.X == X && other.Y == Y;
}