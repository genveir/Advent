using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.ElfCode
{
    class ElfCodeRunner
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    WriteUsage();
                    return;
                }

                var registers = int.Parse(args[0]);
                var programFile = args[1];
                var inputFile = args[2];

                var runner = new ElfCodeRunner(programFile, InputMode.File, inputFile, InputMode.File, registers);
                runner.Run();

                Console.WriteLine("done");
            }
            catch (Exception)
            {
                WriteUsage();
                
                throw;
            }
        }

        private static void WriteUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("The ElfCode Interpreter takes 3 arguments:");
            Console.WriteLine("first argument: the number of registers the program will have access to");
            Console.WriteLine("second argument: the path of the program file");
            Console.WriteLine("third argument: the path of the input file");
            Console.WriteLine();
            Console.WriteLine("Code (in C#) can be found at https://github.com/genveir/Advent/tree/ElfCode");
            Console.WriteLine("The language is ElfCode from https://adventofcode.com/2018/day/19");
            Console.WriteLine("Expanded with I/O commands as shown in ");
            Console.WriteLine("  https://old.reddit.com/r/adventofcode/comments/a7o5ww/2018_day_1_part_1_in_elfcode/");
            Console.WriteLine();
            Console.WriteLine("In addition to that, you can add breakpoints by putting a B behind a");
            Console.WriteLine("line in the program (so: eqri 0 0 0 B). Lines starting with // are also");
            Console.WriteLine("ignored. Anything on an input line (except a B) behind the 4th argument");
            Console.WriteLine("is also ignored, so you can just write comments behind the lines.");
            Console.WriteLine();
            Console.WriteLine("The interpreter is not very friendly, and will crash on any mistakes in your");
            Console.WriteLine("program or input.");
        }

        public enum InputMode { String, File, Resource }

        public ElfCodeInterpreter interpreter;

        public ElfCodeRunner(int numRegisters) : this("Advent1.ElfCode", InputMode.File, "Advent1.Input", InputMode.File, numRegisters) { }
        public ElfCodeRunner(string program, InputMode programMode, string input, InputMode inputMode, int numRegisters)
        {
            if (inputMode == InputMode.File) input = ReadInputFile(input);
            if (inputMode == InputMode.Resource) input = ReadInput(input);
            if (programMode == InputMode.File) program = ReadInputFile(program);
            if (programMode == InputMode.Resource) program = ReadInput(program);

            interpreter = new ElfCodeInterpreter(program, numRegisters);

            var inputAsInts = input.Select(c => (int)c).ToArray();
            interpreter.SetInput(inputAsInts);
        }

        private string ReadInputFile(string fileName)
        {
            string input;
            using (var txt = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                input = txt.ReadToEnd();
            }

            return input;
        }

        private string ReadInput(string resource)
        {
            var resourceName = this.GetType().Assembly.GetManifestResourceNames()
                .Where(rn => rn.Contains(resource))
                .Single();

            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            return input;
        }

        public void Step()
        {
            interpreter.ExecuteStep();
        }

        public void Run()
        {
            while(interpreter.ExecuteStep()) { }
        }
    }
}
