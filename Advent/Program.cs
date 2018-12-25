using System;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            var sol = new Advent25.Solution();
            //sol = new Advent25.Solution("Test", Advent25.Solution.InputMode.File);
            sol.WriteResult();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}