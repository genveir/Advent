using Advent2023.Shared;
using System.Collections.Generic;

namespace Advent2023.Advent18;
public class Point
{
    public Point(Coordinate location, Direction directionOfTrench)
    {
        Location = location;
        DirectionOfTrench = directionOfTrench;
    }

    public Point(Coordinate location)
    {
        Location = location;
        IsCreated = true;
    }

    public long X => Location.X;
    public long Y => Location.Y;

    public Coordinate Location { get; set; }

    public Direction DirectionOfTrench { get; set; }

    public bool IsInside { get; set; }

    public bool IsCreated { get; set; }

    public Dictionary<Direction, Line> Lines { get; set; } = new();

    public char LetterCode => IsInside ? (IsCreated ? 'X' : 'I') : 'O';

    public override string ToString()
    {
        return $"{Location} {LetterCode}";
    }
}