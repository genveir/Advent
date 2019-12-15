using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent16
{
    class Solution : ISolution
    {
        List<RegTest> TestCases;
        List<ProgramLine> RegProgram;
        private class RegTest
        {
            public int[] before;
            public ProgramLine programLine;
            public int[] after;
        }

        private class ProgramLine
        {
            public int opcode, a, b, c;

            public int Test(Operators operators, int[] before, int[] after)
            {
                int numMatching = operators.Test(opcode, before, a, b, c, after);
                return numMatching;
            }

            public void Execute(Operators operators, ref int[] register)
            {
                operators.Execute(opcode, ref register, a, b, c);
            }
        }

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
            string resourceName = "Advent.Advent16." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        private void _ParseInput(string input)
        {
            TestCases = new List<RegTest>();
            RegProgram = new List<ProgramLine>();

            var lines = input.Split('\n');

            int lineIndex = 0;

            string line = lines[lineIndex];
            do
            {
                var newTest = new RegTest();
                newTest.before = line
                    .Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries)
                    .Take(5).TakeLast(4)
                    .Select(i => int.Parse(i))
                    .ToArray();
                newTest.programLine = ParseProgramLine(lines[++lineIndex]);
                newTest.after = lines[++lineIndex]
                    .Split(new char[] { '[', ',', ']' }, StringSplitOptions.RemoveEmptyEntries)
                    .Take(5).TakeLast(4)
                    .Select(i => int.Parse(i))
                    .ToArray();

                ++lineIndex;
                line = lines[++lineIndex];

                TestCases.Add(newTest);
            } while (line.StartsWith("Before"));

            lineIndex++;
            lineIndex++;

            while(lineIndex < lines.Length)
            {
                RegProgram.Add(ParseProgramLine(lines[lineIndex]));

                lineIndex++;
            }
        }

        private ProgramLine ParseProgramLine(string line)
        {
            var lineVals = line
                .Split(' ')
                .Select(i => int.Parse(i))
                .ToArray();

            var programLine = new ProgramLine
            {
                opcode = lineVals[0],
                a = lineVals[1],
                b = lineVals[2],
                c = lineVals[3]
            };

            return programLine;
        }

        public void WriteResult()
        {
            int num3OrMore = 0;
            var operators = new Operators();
            foreach(var regtest in TestCases)
            {
                var numMatches = regtest.programLine.Test(operators, regtest.before, regtest.after);
                if (numMatches >= 3) num3OrMore++;
            }
            Console.WriteLine("part1: " + num3OrMore);

            operators.Simplify();

            var register = new int[] { 0, 0, 0, 0 };
            foreach(var line in RegProgram)
            {
                line.Execute(operators, ref register);
            }
            Console.WriteLine("part2: " + register[0]);
        }
    }
}
