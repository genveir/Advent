using Advent2019.Shared;
using System;

namespace Advent2019
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolution solution = new Advent4.Solution();

            Console.WriteLine(string.Format("Result for part 1: {0}", solution.GetResult1()));
            Console.WriteLine(string.Format("Result for part 2: {0}", solution.GetResult2()));

            Console.ReadLine();
        }
    }
}
