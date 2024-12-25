namespace Advent2024.Day25;

public class Solution
{
    public List<int[]> Locks = [];
    public List<int[]> Keys = [];

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input).ToArray();

        foreach (var block in blocks)
        {
            var pivoted = block.Pivot();
            if (pivoted[0][0] == '#') Locks.Add(pivoted.Select(p => p.Count(c => c == '#') - 1).ToArray());
            else Keys.Add(pivoted.Select(p => p.Count(c => c == '#') - 1).ToArray());
        }
    }
    public Solution() : this("Input.txt") { }


    public object GetResult1()
    {
        long sum = 0;
        foreach (var key in Keys)
        {
            foreach (var _lock in Locks)
            {
                if (_lock[0] + key[0] <= 5 &&
                    _lock[1] + key[1] <= 5 &&
                    _lock[2] + key[2] <= 5 &&
                    _lock[3] + key[3] <= 5 &&
                    _lock[4] + key[4] <= 5)
                {
                    sum++;
                }
            }
        }

        return sum;
    }



    public object GetResult2()
    {
        return "";
    }
}
