namespace Advent2024.Shared;

public class Coordinate3D
{
    public Coordinate3D(long x, long y) : this(x, y, 0)
    {
    }

    public Coordinate3D(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    [ComplexParserTarget("coords")]
    public Coordinate3D(long[] coords)
    {
        if (coords.Length < 2)
        {
            throw new ArgumentException("coord3D input must be of length 2 or 3");
        }
        if (coords.Length >= 2)
        {
            X = coords[0];
            Y = coords[1];
        }
        if (coords.Length == 3)
        {
            Z = coords[2];
        }
        if (coords.Length > 3)
        {
            throw new ArgumentException("coord3D input must be of length 2 or 3");
        }
    }

    public void Deconstruct(out long x, out long y, out long z)
    {
        x = X;
        y = Y;
        z = Z;
    }

    public long X;
    public long Y;
    public long Z;

    public long AbsX => Math.Abs(X);
    public long AbsY => Math.Abs(Y);
    public long AbsZ => Math.Abs(Z);

    public Coordinate3D ShiftX(long shift) => new(X + shift, Y, Z);

    public Coordinate3D ShiftY(long shift) => new(X, Y + shift, Z);

    public Coordinate3D ShiftZ(long shift) => new(X, Y, Z + shift);

    public Coordinate3D Shift(long shiftX, long shiftY, long shiftZ) =>
        ShiftX(shiftX).ShiftY(shiftY).ShiftZ(shiftZ);

    private IEnumerable<Coordinate3D> _neighbours;

    public IEnumerable<Coordinate3D> GetNeighbours(bool orthogonalOnly = false)
    {
        if (_neighbours == null)
        {
            var neighbours = new List<Coordinate3D>();

            for (int xShift = -1; xShift <= 1; xShift++)
            {
                for (int yShift = -1; yShift <= 1; yShift++)
                {
                    for (int zShift = -1; zShift <= 1; zShift++)
                    {
                        var zeroCount = (xShift == 0 ? 1 : 0) + (yShift == 0 ? 1 : 0) + (zShift == 0 ? 1 : 0);
                        if (zeroCount == 3) continue;
                        if (orthogonalOnly && zeroCount != 2) continue;

                        neighbours.Add(new Coordinate3D(X + xShift, Y + yShift, Z + zShift));
                    }
                }
            }
            _neighbours = neighbours;
        }
        return _neighbours;
    }

    public long IntegerDistance(Coordinate3D second) =>
        (long)Distance(second);

    public long IntegerDistance(long x, long y, long z) =>
        (long)Distance(x, y, z);

    public double Distance(Coordinate3D second) =>
        Distance(second.X, second.Y, second.Z);

    public double Distance(long x, long y, long z)
    {
        var squared =
            Math.Abs(X - x) * Math.Abs(X - x) +
            Math.Abs(Y - y) * Math.Abs(Y - y) +
            Math.Abs(Z - z) * Math.Abs(Z - z);

        return Math.Sqrt(squared);
    }

    public long ManhattanDistance(Coordinate3D second)
    {
        var diff = Difference(second);

        return diff.AbsX + diff.AbsY + diff.AbsZ;
    }

    public Coordinate3D Difference(Coordinate3D second) =>
        new(X - second.X, Y - second.Y, Z - second.Z);

    public bool IsInBounds(long minX, long maxX, long minY, long maxY, long minZ, long maxZ) =>
        X >= minX && X <= maxX && Y >= minY && Y <= maxY && Z >= minZ && Z <= maxZ;

    public bool IsInBounds(Coordinate3D first, Coordinate3D second)
    {
        var minX = Math.Min(first.X, second.X);
        var maxX = Math.Max(first.X, second.X);

        var minY = Math.Min(first.Y, second.Y);
        var maxY = Math.Max(first.Y, second.Y);

        var minZ = Math.Min(first.Z, second.Z);
        var maxZ = Math.Max(first.Z, second.Z);

        return IsInBounds(minX, maxX, minY, maxY, minZ, maxZ);
    }

    public static Coordinate3D operator +(Coordinate3D first, Coordinate3D second) =>
        first.Sum(second);

    public Coordinate3D Sum(Coordinate3D second) =>
        new(X + second.X, Y + second.Y, Z + second.Z);

    public static Coordinate3D operator -(Coordinate3D first, Coordinate3D second) =>
        first.Difference(second);

    public static bool operator ==(Coordinate3D first, Coordinate3D second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (first is null) return false;
        return first.Equals(second);
    }

    public static bool operator !=(Coordinate3D first, Coordinate3D second) =>
        !(first == second);

    public override string ToString() => $"({X}, {Y}, {Z})";

    public override int GetHashCode()
    {
        return X.GetHashCode() + 5441 * Y.GetHashCode() + 6257 * Z.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is not Coordinate3D other) return false;
        return other.X == X && other.Y == Y && other.Z == Z;
    }
}