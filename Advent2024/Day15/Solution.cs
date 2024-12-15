namespace Advent2024.Day15;

public class Solution
{
    public char[][] grid;

    public State smallState;
    public State bigState;

    public List<int> moves = [];

    public Solution(string input)
    {
        var lines = Input.GetBlocks(input);

        grid = Input.GetLetterGrid(lines[0]);

        smallState = ParseSmall();
        bigState = ParseBig();

        var moveLines = Input.GetInputLines(lines[1]);

        foreach (var line in moveLines)
        {
            foreach (var c in line)
            {
                moves.Add(c switch
                {
                    '^' => 0,
                    '>' => 1,
                    'v' => 2,
                    '<' => 3,
                    _ => throw new NotImplementedException()
                });
            }
        }
    }

    public State ParseSmall()
    {
        var boxes = new List<Movable>();
        Robot smallrobot = null;

        var smallgrid = new char[grid.Length][];
        for (int y = 0; y < grid.Length; y++)
        {
            smallgrid[y] = new char[grid[y].Length];
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '#')
                {
                    smallgrid[y][x] = '#';
                }
                else
                {
                    smallgrid[y][x] = '.';


                    if (grid[y][x] == '@')
                    {
                        smallrobot = new Robot(new Coordinate2D(x, y));
                    }
                    else if (grid[y][x] == 'O')
                    {
                        boxes.Add(new Box(new Coordinate2D(x, y)));
                    }
                }
            }
        }

        return new State
        {
            Grid = smallgrid,
            Boxes = boxes,
            Robot = smallrobot
        };
    }

    public State ParseBig()
    {
        var bigboxes = new List<Movable>();
        Robot bigrobot = null;
        var biggergrid = new char[grid.Length][];
        for (int y = 0; y < grid.Length; y++)
        {
            biggergrid[y] = new char[grid[y].Length * 2];
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '#')
                {
                    biggergrid[y][x * 2] = '#';
                    biggergrid[y][x * 2 + 1] = '#';
                }
                else
                {
                    biggergrid[y][x * 2] = '.';
                    biggergrid[y][x * 2 + 1] = '.';
                    if (grid[y][x] == '@')
                    {
                        bigrobot = new Robot(new Coordinate2D(x * 2, y));
                    }
                    else if (grid[y][x] == 'O')
                    {
                        bigboxes.Add(new BigBox(new Coordinate2D(x * 2, y), new Coordinate2D(x * 2 + 1, y)));
                    }
                }
            }
        }

        return new State
        {
            Grid = biggergrid,
            Boxes = bigboxes,
            Robot = bigrobot
        };
    }

    public class State
    {
        public char[][] Grid;
        public List<Movable> Boxes;
        public Robot Robot;

        public void Print()
        {
            for (int y = 0; y < Grid.Length; y++)
            {
                for (int x = 0; x < Grid[y].Length; x++)
                {
                    if (Robot.Position[0].X == x && Robot.Position[0].Y == y)
                    {
                        Robot.PrintPos(new(x, y));
                    }
                    else
                    {
                        var box = Boxes.FirstOrDefault(b => b.Position.Any(p => p.X == x && p.Y == y));
                        if (box != null)
                        {
                            box.PrintPos(new(x, y));
                        }
                        else
                        {
                            Console.Write(Grid[y][x]);
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public class MoveResult
    {
        public bool CanMove;
        public IEnumerable<Movable> Movables;

        public MoveResult(bool canMove, IEnumerable<Movable> movables)
        {
            CanMove = canMove;
            Movables = movables;
        }

        public static MoveResult Cant() => new(false, Array.Empty<Movable>());

        public static MoveResult MoveIntoSelf() => new(true, Array.Empty<Movable>());

        public static MoveResult Can(IEnumerable<Movable> movables) => new(true, movables);
    }

    public abstract class Movable
    {
        public Coordinate2D[] Position { get; set; }

        public abstract void PrintPos(Coordinate2D pos);

        public int lastMove = -1;

        public MoveResult CanMove(int direction, State state)
        {
            List<Movable> movablesThatCanMove = [];
            foreach (var pos in Position)
            {
                var tryMove = CanMoveOne(direction, pos, state);

                if (!tryMove.CanMove)
                {
                    return MoveResult.Cant();
                }
                else
                {
                    movablesThatCanMove.AddRange(tryMove.Movables);
                }
            }

            return MoveResult.Can(movablesThatCanMove.Append(this));
        }

        public void DoMove(int direction, State state, int counter)
        {
            if (counter == lastMove)
            {
                return;
            }
            lastMove = counter;

            Position = Position.Select(pos =>
                direction switch
                {
                    0 => pos.ShiftY(-1),
                    1 => pos.ShiftX(1),
                    2 => pos.ShiftY(1),
                    3 => pos.ShiftX(-1),
                    _ => throw new NotImplementedException()
                }).ToArray();
        }

        public MoveResult CanMoveOne(int direction, Coordinate2D posToMove, State state)
        {
            var newPos = direction switch
            {
                0 => posToMove.ShiftY(-1),
                1 => posToMove.ShiftX(1),
                2 => posToMove.ShiftY(1),
                3 => posToMove.ShiftX(-1),
                _ => throw new NotImplementedException()
            };

            if (state.Grid[newPos.Y][newPos.X] == '#')
            {
                return MoveResult.Cant();
            }

            var boxInNewPos = state.Boxes.FirstOrDefault(b => b.Position.Contains(newPos));
            if (boxInNewPos == this)
            {
                return MoveResult.MoveIntoSelf();
            }

            if (boxInNewPos == null)
            {
                return MoveResult.Can([this]);
            }
            else
            {
                var tryMove = boxInNewPos.CanMove(direction, state);

                if (tryMove.CanMove)
                {
                    return MoveResult.Can(tryMove.Movables.Append(this));
                }
                else
                {
                    return MoveResult.Cant();
                }
            }
        }
    }

    public class Robot : Movable
    {
        public Robot(Coordinate2D position)
        {
            Position = [position];
        }

        public override void PrintPos(Coordinate2D pos)
        {
            Console.Write('@');
        }
    }

    public class Box : Movable
    {
        public Box(Coordinate2D position)
        {
            Position = [position];
        }

        public override void PrintPos(Coordinate2D pos)
        {
            Console.Write('O');
        }
    }

    public class BigBox : Movable
    {
        public BigBox(Coordinate2D position1, Coordinate2D position2)
        {
            Position = [position1, position2];
        }

        public override void PrintPos(Coordinate2D pos)
        {
            if (Position[0] == pos)
            {
                Console.Write('[');
            }
            else
            {
                Console.Write(']');
            }
        }
    }

    public object GetResult1()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            var toMove = smallState.Robot.CanMove(moves[i], smallState);
            foreach (var movable in toMove.Movables)
            {
                movable.DoMove(moves[i], smallState, i);
            }
        }

        long sum = 0;
        foreach (var box in smallState.Boxes)
        {
            sum += box.Position[0].Y * 100 + box.Position[0].X;
        }
        return sum;
    }

    public bool printSteps = false;

    // not 1465725, 1485072
    public object GetResult2()
    {
        for (int i = 0; i < moves.Count; i++)
        {
            var toMove = bigState.Robot.CanMove(moves[i], bigState);
            foreach (var movable in toMove.Movables)
            {
                movable.DoMove(moves[i], bigState, i);
            }

            if (printSteps)
            {
                bigState.Print();
                Console.WriteLine("moved " + moves[i] switch
                {
                    0 => "up",
                    1 => "right",
                    2 => "down",
                    3 => "left",
                    _ => throw new NotImplementedException()
                });
                Console.ReadLine();
            }
        }

        long sum = 0;
        foreach (var box in bigState.Boxes)
        {
            sum += box.Position[0].Y * 100 + box.Position[0].X;
        }
        return sum;
    }
}