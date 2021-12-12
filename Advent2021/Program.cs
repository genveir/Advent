using Advent2021.Shared;
using System;
using System.Diagnostics;
using TextCopy;

namespace Advent2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();

            int numRuns = 10000;
            string result1 = "";
            string result2 = "";

            stopWatch.Start();
            for (int n = 0; n < numRuns; n++)
            {
                var rogier = new Advent12.Day12DFS();

                result2 = rogier.Lezgo().ToString();
            }
            stopWatch.Stop();

            if (!string.IsNullOrEmpty(result1)) ClipboardService.SetText(result1);
            if (!string.IsNullOrEmpty(result2)) ClipboardService.SetText(result2);

            Console.WriteLine(string.Format("Result for part 1: {0}", result1));
            Console.WriteLine(string.Format("Result for part 2: {0}", result2));
            Console.WriteLine();
            Console.WriteLine("Number of runs: " + numRuns);
            Console.WriteLine("Total runtime: " + stopWatch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Average runtime: " + (stopWatch.ElapsedMilliseconds / numRuns) + "ms");

            Console.ReadLine();
        }
    }
}
