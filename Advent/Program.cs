using System;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            var sol = new Advent24.Solution();
            sol.fight.Boost(36);
            sol.fight.Print();
            sol.GetPart1();
            sol.fight.Print();


            //sol.WriteResult();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}