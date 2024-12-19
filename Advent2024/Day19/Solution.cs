namespace Advent2024.Day19;

public class Solution
{
    public string[] patterns;
    public string[] targets;

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input).ToArray();

        patterns = blocks[0].Single().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        targets = blocks[1];
    }

    public Solution() : this("Input.txt")
    {
    }

    public Dictionary<string, long> matches = [];
    public long Solve(string target)
    {
        if (matches.TryGetValue(target, out var result))
        {
            return result;
        }
        if (target.Length == 0)
        {
            return 1;
        }

        var paddedTarget = target + "           ";

        long solveCount = 0;
        foreach (var pattern in patterns)
        {
            if (paddedTarget.StartsWith(pattern))
            {
                var waysToSolveSubTarget = Solve(target[pattern.Length..]);

                solveCount += waysToSolveSubTarget;
            }
        }

        matches[target] = solveCount;
        return solveCount;
    }

    public object GetResult1()
    {
        int count = 0;
        foreach (var target in targets)
        {
            var result = Solve(target);
            if (result != 0)
            {
                count++;
            }
        }
        return count;
    }

    public object GetResult2()
    {
        long count = 0;
        foreach (var target in targets)
        {
            var result = Solve(target);

            count += result;
        }
        return count;
    }
}