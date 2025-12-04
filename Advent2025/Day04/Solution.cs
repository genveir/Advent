namespace Advent2025.Day04;

public class Solution
{
    public Grid<char> grid;

    public Solution(string input)
    {
        grid = new Grid<char>(Input.GetLetterGrid(input));
    }
    public Solution() : this("Input.txt") { }

    public List<Coordinate2D> GetRollsThatCanBeRemoved()
    {
        var rolls = new List<Coordinate2D>();
        foreach (var coordinate in grid.Coordinates)
        {
            if (grid[coordinate] != '@') continue;

            var neighbours = grid.GetNeighbours(orthogonalOnly: false, coord: coordinate);
            if (neighbours.Count(c => c == '@') < 4)
            {
                rolls.Add(coordinate);
            }
        }
        return rolls;
    }

    public object GetResult1()
    {
        return GetRollsThatCanBeRemoved().Count;
    }

    public object GetResult2()
    {
        long count = 0;
        List<Coordinate2D> toRemove;

        do
        {
            toRemove = GetRollsThatCanBeRemoved();

            count += toRemove.Count;

            foreach (var coord in toRemove)
            {
                grid[coord] = '.';
            }
        } while (toRemove.Count > 0);

        return count;
    }
}
