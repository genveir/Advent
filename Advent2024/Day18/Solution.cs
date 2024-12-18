namespace Advent2024.Day18;

public class Solution
{
    public List<Coordinate2D> bytes;
    public HashSet<Coordinate2D> corrupted = [];

    public Coordinate2D Start => new(0, 0);
    public Coordinate2D Target => new(GridMax, GridMax);

    public long GridMax { get; set; } = 70;
    public long FallAmount { get; set; } = 1024;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<Coordinate2D>("line");

        bytes = inputParser.Parse(lines);
    }

    public Solution() : this("Input.txt")
    {
    }

    public long GetCost()
    {
        var search = new AStar<Coordinate2D>(
            startNode: Start,
            endNode: Target,
            findNeighbourFunction: c => c.GetNeighbours(orthogonalOnly: true)
                .Where(c => c.IsInBounds(Start, Target))
                .Where(c => !corrupted.Contains(c)),
            transitionCostFunction: (_, _) => 1,
            heuristicCostFunction: c => c.ManhattanDistance(Target));

        return search.FindShortest().Cost;
    }

    public object GetResult1()
    {
        for (int n = 0; n < FallAmount; n++)
            corrupted.Add(bytes[n]);

        return GetCost();
    }

    // not 4,62 <-- was returning n-1 instead of n
    public object GetResult2()
    {
        for (int n = 0; n < FallAmount; n++)
            corrupted.Add(bytes[n]);

        for (int n = (int)FallAmount; n < bytes.Count; n++)
        {
            corrupted.Add(bytes[n]);

            var cost = GetCost();

            if (cost == long.MaxValue)
            {
                return $"{bytes[n].X},{bytes[n].Y}";
            }
        }

        return "no result";
    }
}