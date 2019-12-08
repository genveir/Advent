using Advent2019.LeaderBoard;
using Advent2019.Shared;
using System;
using System.Diagnostics;

namespace Advent2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            int numRuns = 1;
            string result1 = "";
            string result2 = "";
            for (int n = 0; n < numRuns; n++)
            {
                ISolution solution = new Advent8.Solution();

                result1 = solution.GetResult1();
                result2 = solution.GetResult2();
            }

            stopWatch.Stop();

            Console.WriteLine(string.Format("Result for part 1: {0}", result1));
            Console.WriteLine(string.Format("Result for part 2: {0}", result2));
            Console.WriteLine();
            Console.WriteLine("Total runtime: " + (stopWatch.ElapsedMilliseconds / numRuns) + "ms");

            Console.ReadLine();
        }
    }
}
