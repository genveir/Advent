namespace Advent2024.Day14;

public class Solution
{
    public List<ParsedInput> modules;

    public long Height { get; set; } = 103;
    public long Width { get; set; } = 101;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        modules = inputParser.Parse(lines);
    }

    public Solution() : this("Input.txt")
    {
    }

    public class ParsedInput
    {
        public Coordinate2D position;
        public Coordinate2D velocity;

        [ComplexParserTarget("\\p=coords \\v=coords")]
        public ParsedInput(Coordinate2D position, Coordinate2D velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public Coordinate2D SimulateSteps(long steps, long width, long height)
        {
            for (int i = 0; i < steps; i++)
            {
                SimulateStep(width, height);
            }

            return position;
        }

        public void SimulateStep(long width, long height)
        {
            position = position.Shift(velocity.X, velocity.Y);

            position = new Coordinate2D(new ModNum(position.X, width).number, new ModNum(position.Y, height).number);
        }

        public Coordinate2D PositionAt(long tick, long width, long height)
        {
            var xShift = velocity.X * tick;
            var yShift = velocity.Y * tick;

            var newPos = position.Shift(xShift, yShift);

            var modPos = new Coordinate2D(new ModNum(newPos.X, width).number, new ModNum(newPos.Y, height).number);

            return modPos;
        }
    }

    public class Quadrant
    {
        public Quadrant(Coordinate2D topLeft, Coordinate2D bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public Coordinate2D TopLeft { get; set; }
        public Coordinate2D BottomRight { get; set; }
    }

    public bool IsInQuadrant(Coordinate2D position, Quadrant quadrant)
    {
        return position.X >= quadrant.TopLeft.X && position.X <= quadrant.BottomRight.X &&
               position.Y >= quadrant.TopLeft.Y && position.Y <= quadrant.BottomRight.Y;
    }

    public List<Quadrant> MakeQuadrants()
    {
        var list = new List<Quadrant>();

        var topLeftQuadrant = new Quadrant(new Coordinate2D(0, 0), new Coordinate2D(Width / 2 - 1, Height / 2 - 1));
        var topRightQuadrant = new Quadrant(new Coordinate2D(Width / 2 + 1, 0), new Coordinate2D(Width - 1, Height / 2 - 1));
        var bottomLeftQuadrant = new Quadrant(new Coordinate2D(0, Height / 2 + 1), new Coordinate2D(Width / 2 - 1, Height - 1));
        var bottomRightQuadrant = new Quadrant(new Coordinate2D(Width / 2 + 1, Height / 2 + 1), new Coordinate2D(Width - 1, Height - 1));

        list.Add(topLeftQuadrant);
        list.Add(topRightQuadrant);
        list.Add(bottomLeftQuadrant);
        list.Add(bottomRightQuadrant);

        return list;
    }

    // not 208926000 <-- this is what you get when you swap Height and Width :/
    public object GetResult1()
    {
        var positions = new List<Coordinate2D>();

        foreach (var module in modules)
        {
            positions.Add(module.PositionAt(100, Width, Height));
        }

        var quadrants = MakeQuadrants();

        var topLeftQuadrant = quadrants[0];
        var topRightQuadrant = quadrants[1];
        var bottomLeftQuadrant = quadrants[2];
        var bottomRightQuadrant = quadrants[3];

        var topLeftCount = positions.Count(p => IsInQuadrant(p, topLeftQuadrant));
        var topRightCount = positions.Count(p => IsInQuadrant(p, topRightQuadrant));
        var bottomLeftCount = positions.Count(p => IsInQuadrant(p, bottomLeftQuadrant));
        var bottomRightCount = positions.Count(p => IsInQuadrant(p, bottomRightQuadrant));

        return topLeftCount * topRightCount * bottomLeftCount * bottomRightCount;
    }

    public object GetResult2()
    {
        int counter = 0;
        while (true)
        {
            counter++;

            foreach (var module in modules)
            {
                module.SimulateStep(Width, Height);
            }

            if (modules.Select(m => m.position).Distinct().Count() == modules.Count)
            {
                Console.Clear();
                Console.WriteLine(counter);
                Print();
                var input = Console.ReadLine();

                if (input == "y")
                    return counter;
            }
        }
    }

    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (modules.Any(m => m.position.X == x && m.position.Y == y))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
}