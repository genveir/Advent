namespace Advent2024.Day01;

public class Solution
{
    public List<long> left = [];
    public List<long> right = [];

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        foreach (var line in lines)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            left.Add(long.Parse(parts[0]));
            right.Add(long.Parse(parts[1]));
        }

        left.Sort();
        right.Sort();
    }

    public Solution() : this("Input.txt")
    {
    }

    public object GetResult1()
    {
        long num = 0;
        for (int n = 0; n < left.Count; n++)
        {
            num += Math.Abs(left[n] - right[n]);
        }

        return num;
    }

    public object GetResult2()
    {
        long num = 0;
        for (int n = 0; n < left.Count; n++)
        {
            num += right.Where(r => left[n] == r).Count() * left[n];
        }

        return num;
    }
}