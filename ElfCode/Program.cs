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
                inputMode: ElfCode.ElfCodeRunner.InputMode.Resource,
                program: "Advent1.ElfCode",
                programMode: ElfCode.ElfCodeRunner.InputMode.Resource,
                numRegisters: 6);
            runner.Run();
            
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}