using Advent.ElfCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElfCode
{
    public class InterpreterEntry
    {
        public static void Main(string[] args)
        {
            TestOperators();
            Console.WriteLine();

            var runner = new ElfCodeRunner(
                program: "ElfcodeInterpreter.ec",
                programMode: ElfCodeRunner.InputMode.Resource,
                input: @"#ip 3
setr 123 321 21 
addi 456 654 123456",
                inputMode: ElfCodeRunner.InputMode.String,
                numRegisters: 20);
            var exit = runner.Run();

            Console.WriteLine("program exited with ip " + exit);
            Console.ReadLine();
        }

        private static int opResult(string op)
        {
            var runner = new ElfCodeRunner(
                program: "ElfcodeInterpreter.ec",
                programMode: ElfCodeRunner.InputMode.Resource,
                input: op + " 0 0 0",
                inputMode: ElfCodeRunner.InputMode.String,
                numRegisters: 20);
            runner.DontBreak = true;
            return runner.Run();
        }

        public static void TestOperators()
        {
            var ops = new List<string>()
            { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti",
              "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr", "peek", "geti", "outi", "outr" };

            ops.Sort();
            int expected = 5000;
            foreach (var op in ops)
            {
                Console.WriteLine(op + " should be " + expected++ + " and is " + opResult(op));
            }
        }
    }
}
