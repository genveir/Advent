namespace Advent2025.Day06;

public class Solution
{
    public long[][] p1Numbers;
    public long[][] p2Numbers;
    public string[] operators;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        for (int n = 0; n < lines.Length - 1; n++)
        {
            var parts = lines[n].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            p1Numbers = p1Numbers ?? new long[lines.Length - 1][];
            p1Numbers[n] = parts.Select(p => long.Parse(p)).ToArray();
        }
        operators = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        p2Numbers = ParseP2Numbers(lines);
    }
    public Solution() : this("Input.txt") { }

    public static long[][] ParseP2Numbers(string[] lines)
    {
        var blocks = GetP2Blocks(lines);

        List<long[]> p2Numbers = [];

        for (int n = 0; n < blocks.Length; n++)
        {
            p2Numbers.Add(GetNumbersFromP2Block(blocks[n]));
        }

        return p2Numbers.ToArray();
    }

    public static long[] GetNumbersFromP2Block(string[] block)
    {
        List<long> numbers = [];

        for (int n = 0; n < block[0].Length; n++)
        {
            numbers.Add(GetNumberFromP2Block(block, n));
        }

        return numbers.ToArray();
    }

    public static long GetNumberFromP2Block(string[] block, int index)
    {
        List<long> digits = [];

        for (int n = 0; n < block.Length; n++)
        {
            if (block[n][index] != ' ')
            {
                digits.Add(block[n][index].AsDigit());
            }
        }

        return long.Parse(string.Concat(digits));
    }

    public static string[][] GetP2Blocks(string[] lines)
    {
        List<string[]> p2Blocks = [];
        int blockStart = 0;
        for (int n = 0; n < lines[0].Length; n++)
        {
            bool allBlank = true;
            for (int l = 0; l < lines.Length - 1; l++)
            {
                if (lines[l][n] != ' ')
                {
                    allBlank = false;
                    break;
                }
            }

            if (allBlank)
            {
                var block = new string[lines.Length - 1];
                for (int l = 0; l < lines.Length - 1; l++)
                {
                    block[l] = lines[l][blockStart..n];
                }
                p2Blocks.Add(block);
                blockStart = n + 1;
            }
        }
        var lastBlock = new string[lines.Length - 1];
        int highestLength = 0;
        for (int l = 0; l < lines.Length - 1; l++)
        {
            lastBlock[l] = lines[l][blockStart..];
            if (lastBlock[l].Length > highestLength) highestLength = lastBlock[l].Length;
        }
        for (int l = 0; l < lines.Length - 1; l++)
        {
            lastBlock[l] = lastBlock[l].PadRight(highestLength, ' ');
        }

        p2Blocks.Add(lastBlock);

        return p2Blocks.ToArray();
    }

    public object GetResult1()
    {
        var pivot = Helper.PivotToArray(p1Numbers);

        long sum = 0;
        for (int n = 0; n < pivot.Length; n++)
        {
            switch (operators[n])
            {
                case "+":
                    sum += pivot[n].Sum();
                    break;
                case "*":
                    sum += pivot[n].Aggregate(1L, (a, b) => a * b);
                    break;
            }
        }

        return sum;
    }

    public object GetResult2()
    {
        long sum = 0;
        for (int n = 0; n < p2Numbers.Length; n++)
        {
            switch (operators[n])
            {
                case "+":
                    sum += p2Numbers[n].Sum();
                    break;
                case "*":
                    sum += p2Numbers[n].Aggregate(1L, (a, b) => a * b);
                    break;
            }
        }

        return sum;
    }
}
