namespace Advent2024.Day22;

public class Solution
{
    public long[][] lastFour = new long[4][];
    public long[] numbers;
    public long[] prices;

    public long bestPrice;

    public Solution(string input)
    {
        numbers = Input.GetNumbers(input);

        for (int n = 0; n < 4; n++)
        {
            lastFour[n] = new long[numbers.Length];
        }
        prices = new long[numbers.Length];

        for (int n = 0; n < numbers.Length; n++)
        {
            lastFour[3][n] = numbers[n] % 10;
        }
    }
    public Solution() : this("Input.txt") { }

    public long CalculateNext(long number)
    {
        var times64 = number * 64;
        number = Mix(times64, number);
        number = Prune(number);

        var div32 = number / 32;
        number = Mix(div32, number);
        number = Prune(number);

        var time2048 = number * 2048;
        number = Mix(time2048, number);
        number = Prune(number);

        return number;
    }

    public static long Mix(long a, long b) => a ^ b;
    public static long Prune(long a) => a % 16777216;

    public long[] Calculate(int steps)
    {
        var copy = numbers.ToArray();

        Dictionary<long, long[]> sequencesAndPrices = [];
        Dictionary<long, bool[]> sequencesSeen = [];
        for (int n = 0; n < steps; n++)
        {
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = CalculateNext(copy[i]);

                prices[i] = copy[i] % 10;
            }

            for (int i = 0; i < copy.Length; i++)
            {
                var sequence = (lastFour[0][i] - lastFour[1][i] + 10)
                    + 100 * (lastFour[1][i] - lastFour[2][i] + 10)
                    + 10000 * (lastFour[2][i] - lastFour[3][i] + 10)
                    + 1000000 * (lastFour[3][i] - prices[i] + 10);

                if (!sequencesAndPrices.ContainsKey(sequence))
                    sequencesAndPrices[sequence] = new long[numbers.Length];

                if (!sequencesSeen.ContainsKey(sequence))
                    sequencesSeen[sequence] = new bool[numbers.Length];

                if (sequencesSeen[sequence][i] == false)
                {
                    sequencesSeen[sequence][i] = true;
                    sequencesAndPrices[sequence][i] = prices[i];
                }
            }

            lastFour[0] = lastFour[1].ToArray();
            lastFour[1] = lastFour[2].ToArray();
            lastFour[2] = lastFour[3].ToArray();
            lastFour[3] = prices.ToArray();
        }

        var _bestPrice = sequencesAndPrices.Values.Select(v => v.Sum()).Max();
        if (_bestPrice > bestPrice)
        {
            bestPrice = _bestPrice;
        }

        return copy;
    }

    public object GetResult1()
    {
        return Calculate(2000).Sum();
    }

    public object GetResult2()
    {
        Calculate(2000);

        return bestPrice;
    }
}
