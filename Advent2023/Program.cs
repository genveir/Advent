using System;
using System.Diagnostics;
using Advent2023.Shared;
using TextCopy;

namespace Advent2023;

class Program
{
    static void Main(string[] args)
    {
        var stopWatch = new Stopwatch();

        int numRuns = 1;
        object result1 = "";
        object result2 = "";

        stopWatch.Start();
        for (int n = 0; n < numRuns; n++)
        {
            ISolution solution = new AdventActive.Solution();

            result1 = solution.GetResult1();
            result2 = solution.GetResult2();
        }
        stopWatch.Stop();

        var stringResult1 = result1.ToString();
        var stringResult2 = result2.ToString();

        if (!string.IsNullOrEmpty(stringResult1)) ClipboardService.SetText(stringResult1);
        if (!string.IsNullOrEmpty(stringResult2)) ClipboardService.SetText(stringResult2);

        Console.WriteLine($"Result for part 1: {stringResult1}");
        Console.WriteLine($"Result for part 2: {stringResult2}");
        Console.WriteLine();
        Console.WriteLine($"Number of runs: {numRuns}");
        Console.WriteLine($"Total runtime: {FormatTime(stopWatch)}");
        Console.WriteLine($"Average runtime: {FormatTime(stopWatch, numRuns)}");
    }

    private static string FormatTime(Stopwatch stopWatch, int? numRuns = 1)
    {
        TimeUnit timeUnit = TimeUnit.Milliseconds;
        if (stopWatch.ElapsedMilliseconds / numRuns == 0) timeUnit = TimeUnit.Microseconds;

        return timeUnit switch
        {
            TimeUnit.Microseconds => $"{stopWatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000) / numRuns} micro-seconds",
            TimeUnit.Milliseconds => $"{stopWatch.ElapsedMilliseconds / numRuns} ms",
            _ => throw new NotSupportedException("invalid TimeUnit")
        };
    }

    private enum TimeUnit { Milliseconds, Microseconds }
}
