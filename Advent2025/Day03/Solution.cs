namespace Advent2025.Day03;

public class Solution
{
    public long[][] grid;

    public Solution(string input)
    {
        grid = Input.GetDigitGrid(input).ToArray();
    }
    public Solution() : this("Input.txt") { }

    private long getMaxNumber(int row, int digits)
    {
        var batteries = new long[digits];

        for (int col = 0; col < grid[row].Length; col++)
        {
            var value = grid[row][col];

            int startBattery = Math.Max(0, digits - (grid[row].Length - col));

            for (int battery = startBattery; battery < digits; battery++)
            {
                if (value > batteries[battery])
                {
                    batteries[battery] = value;
                    for (int reset = battery + 1; reset < digits; reset++)
                    {
                        batteries[reset] = 0;
                    }
                    break;
                }
            }
        }

        var stringValue = batteries.Select(n => n.ToString()).Aggregate((a, b) => a + b);
        return long.Parse(stringValue);
    }

    public object GetResult1()
    {
        List<long> maxNumbers = [];

        for (int n = 0; n < grid.Length; n++)
        {
            maxNumbers.Add(getMaxNumber(n, 2));
        }

        return maxNumbers.Sum();
    }

    public object GetResult2()
    {
        List<long> maxNumbers = [];

        for (int n = 0; n < grid.Length; n++)
        {
            maxNumbers.Add(getMaxNumber(n, 12));
        }

        return maxNumbers.Sum();
    }
}
