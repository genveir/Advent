namespace Advent2024.Day11;

public class Solution
{
    public long[] nums;
    public long[] rocks;

    public Dictionary<long, long[]> cachedSplits = [];

    public int NumBlinksPt1 { get; set; } = 25;

    public Solution(string input)
    {
        nums = Input.GetNumbers(input, [' ']);

        Reset();
    }

    public void Reset()
    {
        rocks = Helper.DeepCopy(nums);
    }

    public Solution() : this("Input.txt")
    {
    }

    public long[] Split(long value, Dictionary<long, long[]> cachedSplits)
    {
        if (!cachedSplits.TryGetValue(value, out var splits))
        {
            if (value == 0)
            {
                splits = [1];
            }
            else
            {
                var asString = value.ToString();

                if (asString.Length % 2 == 0)
                {
                    var half = asString.Length / 2;
                    var left = asString[..half];
                    var right = asString[half..];
                    splits = [long.Parse(left), long.Parse(right)];
                }
                else
                {
                    splits = [value * 2024];
                }
            }

            cachedSplits[value] = splits;
        }
        return splits;
    }

    public Dictionary<(long rock, long blinks), long> cachedCounts = [];

    public long GetCountForSingleRock(long rock, long blinks)
    {
        if (blinks == 0) return 1;

        if (!cachedCounts.TryGetValue((rock, blinks), out var count))
        {
            var split = Split(rock, cachedSplits);

            count = split.Sum(r => GetCountForSingleRock(r, blinks - 1));

            cachedCounts[(rock, blinks)] = count;
        }

        return count;
    }

    public object GetResult1()
    {
        Reset();
        long sum = 0;

        foreach (var rock in rocks)
        {
            sum += GetCountForSingleRock(rock, NumBlinksPt1);
        }
        return sum;
    }

    public object GetResult2()
    {
        Reset();

        long sum = 0;

        foreach (var rock in rocks)
        {
            sum += GetCountForSingleRock(rock, 75);
        }
        return sum;
    }
}