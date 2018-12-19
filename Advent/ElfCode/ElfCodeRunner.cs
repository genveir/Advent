using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.ElfCode
{
    class ElfCodeRunner
    {
        public enum InputMode { String, File }

        public ElfCodeInterpreter interpreter;

        public ElfCodeRunner(int numRegisters) : this("Advent1.ElfCode", InputMode.File, "Advent1.Input", InputMode.File, numRegisters) { }
        public ElfCodeRunner(string program, InputMode programMode, string input, InputMode inputMode, int numRegisters)
        {
            if (inputMode == InputMode.File) input = ReadInput(input);
            if (programMode == InputMode.File) program = ReadInput(program);

            interpreter = new ElfCodeInterpreter(program, numRegisters);

            var inputAsInts = input.Select(c => (int)c).ToArray();
            interpreter.SetInput(inputAsInts);
        }

        private string ReadInput(string fileName)
        {
            var resourceName = this.GetType().Assembly.GetManifestResourceNames()
                .Where(rn => rn.Contains(fileName))
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
