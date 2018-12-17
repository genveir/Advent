using System;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            new Advent17.Solution("Test", Advent17.Solution.InputMode.File).WriteResult();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}