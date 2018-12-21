using Advent.ElfCode;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent19
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
            string resourceName = "Advent.Advent19." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        private ElfCodeInterpreter interpreter;

        private void _ParseInput(string input)
        {
            interpreter = new ElfCodeInterpreter(input, 6);
        }

        public void WriteResult()
        {
            interpreter.Reset();
            while(interpreter.ExecuteStep() == 0) { }
            Console.WriteLine("part 1: " + interpreter.GetRegister(0));

            interpreter.Reset();
            interpreter.SetRegister(0, 1);
            for (int n = 0; n < 20; n++) interpreter.ExecuteStep();
            var output = HetProgramma();
            Console.WriteLine("part 2: " + output);
        }

        private int HetProgramma()
        {
            int target = interpreter.GetRegister(2);
            int output = interpreter.GetRegister(0);

            for (int outerloopCounter = 1; outerloopCounter <= target; outerloopCounter++)
            {
                if (target % outerloopCounter != 0) continue;
                else output = outerloopCounter + output;
            }

            return output;
        }
    }
}
