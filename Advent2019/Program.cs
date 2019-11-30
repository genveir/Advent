using Advent2019.Shared;
using System;

namespace Advent2019
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolution solution = new Advent1.Solution();

            solution.WriteResult1();
            solution.WriteResult2();

            Console.ReadLine();
        }
    }
}
