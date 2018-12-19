using System;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Advent1.Solution().WriteResult();

            var runner = new ElfCode.ElfCodeRunner(
                input: "Advent1.Input",
                inputMode: ElfCode.ElfCodeRunner.InputMode.File,
                program: "Advent1.ElfCode",
                programMode: ElfCode.ElfCodeRunner.InputMode.File,
                numRegisters: 6);
            runner.Run();
            

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}