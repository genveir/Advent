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
            var registers = int.Parse(args[0]);
            var programFile = args[1];
            var inputFile = args[2];

            var runner = new ElfCodeRunner(programFile, InputMode.File, inputFile, InputMode.File, registers);
            runner.Run();

            Console.WriteLine("done");
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
