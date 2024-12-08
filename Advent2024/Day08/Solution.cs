using System.Text;

namespace Advent2024.Day08;

public class Solution
{
    public List<char> frequencies = [];
    public Dictionary<char, List<Coordinate2D>> antennas = [];
    public Dictionary<Coordinate2D, char> antennasByLoc = [];

    public HashSet<Coordinate2D> antiNodes = [];

    public char[][] grid;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] != '.')
                {
                    if (!frequencies.Contains(grid[y][x]))
                    {
                        frequencies.Add(grid[y][x]);
                        antennas[grid[y][x]] = [];
                    }
                    antennas[grid[y][x]].Add(new Coordinate2D(x, y));
                    antennasByLoc[new Coordinate2D(x, y)] = grid[y][x];
                }
            }
        }
    }

    public void CalculateAntiNodes()
    {
        foreach (var freq in frequencies)
        {
            foreach (var ant in antennas[freq])
            {
                foreach (var ant2 in antennas[freq])
                {
                    if (ant != ant2)
                    {
                        var diffX = ant2.X - ant.X;
                        var diffY = ant2.Y - ant.Y;

                        var node1 = new Coordinate2D(ant.X - diffX, ant.Y - diffY);
                        var node2 = new Coordinate2D(ant2.X + diffX, ant2.Y + diffY);

                        if (IsInGrid(node1)) antiNodes.Add(node1);
                        if (IsInGrid(node2)) antiNodes.Add(node2);
                    }
                }
            }
        }
    }

    public void CalculateAntiNodes2()
    {
        foreach (var freq in frequencies)
        {
            foreach (var ant in antennas[freq])
            {
                foreach (var ant2 in antennas[freq])
                {
                    if (ant != ant2)
                    {
                        var diffX = ant2.X - ant.X;
                        var diffY = ant2.Y - ant.Y;

                        var posNode = new Coordinate2D(ant.X + diffX, ant.Y + diffY);

                        while (IsInGrid(posNode))
                        {
                            antiNodes.Add(posNode);
                            posNode = new Coordinate2D(posNode.X + diffX, posNode.Y + diffY);
                        }

                        var negNode = new Coordinate2D(ant.X - diffX, ant.Y - diffY);

                        while (IsInGrid(negNode))
                        {
                            antiNodes.Add(negNode);
                            negNode = new Coordinate2D(negNode.X - diffX, negNode.Y - diffY);
                        }
                    }
                }
            }
        }
    }

    private bool IsInGrid(Coordinate2D coord)
    {
        return coord.X >= 0 && coord.X < grid[0].Length && coord.Y >= 0 && coord.Y < grid.Length;
    }

    public string PrintGrid()
    {
        StringBuilder builder = new();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (antennasByLoc.ContainsKey(new Coordinate2D(x, y)))
                {
                    builder.Append(antennasByLoc[new Coordinate2D(x, y)]);
                }
                else if (antiNodes.Contains(new Coordinate2D(x, y)))
                {
                    builder.Append('#');
                }
                else
                {
                    builder.Append('.');
                }
            }
            builder.AppendLine();
        }
        return builder.ToString().Trim();
    }

    public Solution() : this("Input.txt")
    {
    }



    public object GetResult1()
    {
        CalculateAntiNodes();

        return antiNodes.Count;
    }

    public object GetResult2()
    {
        CalculateAntiNodes2();

        return antiNodes.Count;
    }
}