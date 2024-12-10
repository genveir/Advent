namespace Advent2024.Day10;

public class Solution
{
    public long[][] grid;
    public List<Coordinate2D> starts = [];

    public Solution(string input)
    {
        grid = Input.GetDigitGrid(input).ToArray();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == 0)
                {
                    starts.Add(new Coordinate2D(x, y));
                }
            }
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public (long pt1, long pt2) GetTrailheadScores()
    {
        var BFSQueue = new Queue<(int startIndex, Coordinate2D current)>();
        for (int n = 0; n < starts.Count; n++)
        {
            BFSQueue.Enqueue((n, starts[n]));
        }

        HashSet<Coordinate2D>[] perStart = new HashSet<Coordinate2D>[starts.Count];
        for (int n = 0; n < perStart.Length; n++)
        {
            perStart[n] = [];
        }

        int numNines = 0;
        while (BFSQueue.Count > 0)
        {
            var currentNode = BFSQueue.Dequeue();
            var current = currentNode.current;

            var value = grid[current.Y][current.X];

            if (value == 9)
            {
                numNines++;
                perStart[currentNode.startIndex].Add(current);
                continue;
            }

            var neighbours = current.GetNeighbours(orthogonalOnly: true);

            foreach (var neighbour in neighbours)
            {
                if (!IsInBounds(neighbour)) continue;

                if (grid[neighbour.Y][neighbour.X] == value + 1)
                    BFSQueue.Enqueue((currentNode.startIndex, neighbour));
            }
        }

        return (perStart.Select(x => x.Count).Sum(), numNines);
    }

    public bool IsInBounds(Coordinate2D coord)
    {
        return coord.X >= 0 && coord.X < grid[0].Length && coord.Y >= 0 && coord.Y < grid.Length;
    }

    public object GetResult1()
    {
        return GetTrailheadScores().pt1;
    }

    public object GetResult2()
    {
        return GetTrailheadScores().pt2;
    }
}