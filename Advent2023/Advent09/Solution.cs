using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent09;

public class Solution : ISolution
{
    public long[][] inputs;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<long[]>("line") { ArrayDelimiters = new[] { ' ' } };

        inputs = inputParser.Parse(lines).ToArray();
    }
    public Solution() : this("Input.txt") { }

    public long[][] Extrapolate(long[] input)
    {
        if (input.All(x => x == 0))
            return new long[1][] { new long[input.Length + 1] };

        var diffArray = new long[input.Length - 1];
        for (int n = 0; n < input.Length - 1; n++)
        {
            diffArray[n] = input[n + 1] - input[n];
        }

        var extrapolated = Extrapolate(diffArray);
        var newInput = input.Append(input.Last() + extrapolated.First().Last()).ToArray();

        var result = new long[extrapolated.Length + 1][];
        result[0] = newInput;
        for (int n = 0; n <  extrapolated.Length; n++)
            result[n + 1] = extrapolated[n];

        return result;
    }

    public long[][] ExtrapolateFront(long[] input)
    {
        if (input.All(x => x == 0))
            return new long[1][] { new long[input.Length + 1] };

        var diffArray = new long[input.Length - 1];
        for (int n = 0; n < input.Length - 1; n++)
        {
            diffArray[n] = input[n + 1] - input[n];
        }

        var extrapolated = ExtrapolateFront(diffArray);
        var newInput = new long[] { input.First() - extrapolated.First().First() }
            .Concat(input)
            .ToArray();

        var result = new long[extrapolated.Length + 1][];
        result[0] = newInput;
        for (int n = 0; n < extrapolated.Length; n++)
            result[n + 1] = extrapolated[n];

        return result;
    }

    public object GetResult1()
    {
        var extrapolated = inputs.Select(Extrapolate);

        return extrapolated.Sum(ex => ex.First().Last());
    }

    public object GetResult2()
    {
        var extrapolated = inputs.Select(ExtrapolateFront);

        return extrapolated.Sum(ex => ex.First().First());
    }
}
