using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent21
{
    class Solution : ISolution
    {
        private string input;

        public enum InputMode { String, File }

        public Solution() : this("Input", InputMode.File) { }
        public Solution(string input, InputMode mode)
        {
            if (mode == InputMode.File) ParseInput(input);
            else
            {
                _ParseInput(input);
            }
        }

        private void ParseInput(string fileName)
        {
            string resourceName = "Advent.Advent21." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        private ElfCode.ElfCodeInterpreter interpreter;

        private void _ParseInput(string input)
        {
            this.input = input;
        }

        public void WriteResult()
        {
            interpreter = new ElfCode.ElfCodeInterpreter(input, 6);
            HashSet<int> results = new HashSet<int>();
            bool isFirst = true;
            int lastAdded = 0;
            while (interpreter.ExecuteStep() == 0)
            {
                if (interpreter.GetRegister(2) == 28)
                {
                    if (results.Contains(interpreter.GetRegister(3))) break;

                    if (isFirst)
                    {
                        isFirst = false;
                        Console.WriteLine("part1: " + interpreter.GetRegister(3));
                    }
                    lastAdded = interpreter.GetRegister(3);
                    results.Add(interpreter.GetRegister(3));
                }
            };
            Console.WriteLine("part2: " + lastAdded);
        }
    }
}
