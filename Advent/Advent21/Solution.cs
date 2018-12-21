using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent21
{
    class Solution : ISolution
    {
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
        private int accumulationRegister;

        private void _ParseInput(string input)
        {
            interpreter = new ElfCode.ElfCodeInterpreter(input, 6);
            accumulationRegister = interpreter.Program[28].a;
            // performance rewrite
            var toDivi = interpreter.Program[17];
            var loopCheckRegister = interpreter.Program[20].b;
            interpreter.Program[17] = interpreter.ParseProgramLine("divi " + loopCheckRegister + " 256 " + toDivi.c);
        }

        public void WriteResult()
        {
            HashSet<int> results = new HashSet<int>();
            bool isFirst = true;
            int lastAdded = 0;
            while (interpreter.ExecuteStep() == 0)
            {
                if (interpreter.InstructionPointer == 28)
                {
                    if (results.Contains(interpreter.GetRegister(accumulationRegister))) break;

                    if (isFirst)
                    {
                        isFirst = false;
                        Console.WriteLine("part1: " + interpreter.GetRegister(accumulationRegister));
                    }
                    lastAdded = interpreter.GetRegister(accumulationRegister);
                    results.Add(interpreter.GetRegister(accumulationRegister));
                }
            };
            Console.WriteLine("part2: " + lastAdded);
        }
    }
}
