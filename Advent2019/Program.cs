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

            ISolution solution = new Advent6.Solution();

            var result1 = solution.GetResult1();
            var result2 = solution.GetResult2();

            stopWatch.Stop();

            Console.WriteLine(string.Format("Result for part 1: {0}", result1));
            Console.WriteLine(string.Format("Result for part 2: {0}", result2));
            Console.WriteLine();
            Console.WriteLine("Total runtime: " + stopWatch.ElapsedMilliseconds + "ms");

            Console.ReadLine();
        }
    }
}
