namespace Advent2024.Day06;

public readonly struct MoverData
{
    public Coordinate2D Position { get; }
    public int Direction { get; }

    public MoverData(Coordinate2D position, int direction)
    {
        Position = position;
        Direction = direction;
    }

    public override int GetHashCode() =>
        Position.GetHashCode() ^ Direction.GetHashCode();

    public override bool Equals(object obj) =>
        obj is MoverData other && other.Position == Position && other.Direction == Direction;

    public override string ToString() => $"{Position} {Direction}";

    public static bool operator ==(MoverData left, MoverData right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MoverData left, MoverData right)
    {
        return !(left == right);
    }
}