namespace Advent2024.Day06;

public class Mover
{
    public Mover(Coordinate2D startPosition, Coordinate2D position, int direction)
    {
        StartPosition = startPosition;

        this.position = position;
        this.direction = direction;

        Data = new(position, direction);
    }

    public Mover(Coordinate2D position, int direction)
    {
        StartPosition = position;

        this.position = position;
        this.direction = direction;

        Data = new(position, direction);
    }

    public Coordinate2D StartPosition { get; }

    private Coordinate2D position;
    public Coordinate2D Position
    {
        get => position;
        set
        {
            position = value;
            Data = new(position, direction);
        }
    }

    private int direction;
    public int Direction
    {
        get => direction;
        set
        {
            direction = value;
            Data = new(position, direction);
        }
    }

    public MoverData Data { get; private set; }

    public Coordinate2D Forward()
    {
        return direction switch
        {
            0 => position.ShiftY(-1),
            1 => position.ShiftX(1),
            2 => position.ShiftY(1),
            3 => position.ShiftX(-1),
            _ => throw new Exception("Invalid direction")
        };
    }

    public void JumpForwardToWall(Dictionary<long, List<long>> wallsByX, Dictionary<long, List<long>> wallsByY, Coordinate2D blocked)
    {
        IEnumerable<long> xWalls = wallsByX[position.X];
        if (blocked.X == position.X)
        {
            xWalls = xWalls.Append(blocked.Y);
        }

        IEnumerable<long> yWalls = wallsByY[position.Y];
        if (blocked.Y == position.Y)
        {
            yWalls = yWalls.Append(blocked.X);
        }

        position = direction switch
        {
            0 => new(position.X, xWalls.Where(p => p < position.Y).Max()),
            1 => new(yWalls.Where(p => p > position.X).Min(), position.Y),
            2 => new(position.X, xWalls.Where(p => p > position.Y).Min()),
            3 => new(yWalls.Where(p => p < position.X).Max(), position.Y),
            _ => throw new Exception("Invalid direction")
        };
        position = Backward();

        Direction = (direction + 1) % 4;
    }

    public Coordinate2D Backward()
    {
        return direction switch
        {
            0 => position.ShiftY(1),
            1 => position.ShiftX(-1),
            2 => position.ShiftY(-1),
            3 => position.ShiftX(1),
            _ => throw new Exception("Invalid direction")
        };
    }

    public Coordinate2D ToTheLeft()
    {
        return direction switch
        {
            0 => position.ShiftX(-1),
            1 => position.ShiftY(-1),
            2 => position.ShiftX(1),
            3 => position.ShiftY(1),
            _ => throw new Exception("Invalid direction")
        };
    }

    public override string ToString()
    {
        return $"{position} {direction}";
    }
}