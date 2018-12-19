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

        private void _ParseInput(string input)
        {
            program = new List<ProgramLine>();

            var lines = input.Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("#ip"))
                {
                    var register = int.Parse(line.Split(' ')[1]);
                    boundRegister = register;
                }
                else program.Add(ParseProgramLine(line));
            }
        }

        private ProgramLine ParseProgramLine(string line)
        {
            var splitLine = line
                .Split(' ');

            var op = GetOpByName(splitLine[0]);
            var lineVals = splitLine
                .TakeLast(3)
                .Select(s => int.Parse(s))
                .ToArray();

            var programLine = new ProgramLine
            {
                op = op,
                a = lineVals[0],
                b = lineVals[1],
                c = lineVals[2]
            };

            return programLine;
        }

        private op GetOpByName(string name)
        {
            switch (name)
            {
                case "addr": return addr;
                case "addi": return addi;
                case "mulr": return mulr;
                case "muli": return muli;
                case "banr": return banr;
                case "bani": return bani;
                case "borr": return borr;
                case "bori": return bori;
                case "setr": return setr;
                case "seti": return seti;
                case "gtir": return gtir;
                case "gtri": return gtri;
                case "gtrr": return gtrr;
                case "eqir": return eqir;
                case "eqri": return eqri;
                case "eqrr": return eqrr;
                default: throw new Exception("magnie");
            }
        }

        public delegate void op(ref int[] r, int i1, int i2, int o);
        public static void addr(ref int[] r, int a, int b, int c) { r[c] = r[a] + r[b]; }
        public static void addi(ref int[] r, int a, int b, int c) { r[c] = r[a] + b; }
        public static void mulr(ref int[] r, int a, int b, int c) { r[c] = r[a] * r[b]; }
        public static void muli(ref int[] r, int a, int b, int c) { r[c] = r[a] * b; }
        public static void banr(ref int[] r, int a, int b, int c) { r[c] = r[a] & r[b]; }
        public static void bani(ref int[] r, int a, int b, int c) { r[c] = r[a] & b; }
        public static void borr(ref int[] r, int a, int b, int c) { r[c] = r[a] | r[b]; }
        public static void bori(ref int[] r, int a, int b, int c) { r[c] = r[a] | b; }
        public static void setr(ref int[] r, int a, int b, int c) { r[c] = r[a]; }
        public static void seti(ref int[] r, int a, int b, int c) { r[c] = a; }
        public static void gtir(ref int[] r, int a, int b, int c) { r[c] = a > r[b] ? 1 : 0; }
        public static void gtri(ref int[] r, int a, int b, int c) { r[c] = r[a] > b ? 1 : 0; }
        public static void gtrr(ref int[] r, int a, int b, int c) { r[c] = r[a] > r[b] ? 1 : 0; }
        public static void eqir(ref int[] r, int a, int b, int c) { r[c] = a == r[b] ? 1 : 0; }
        public static void eqri(ref int[] r, int a, int b, int c) { r[c] = r[a] == b ? 1 : 0; }
        public static void eqrr(ref int[] r, int a, int b, int c) { r[c] = r[a] == r[b] ? 1 : 0; }

        private class ProgramLine
        {
            public op op;
            public int a, b, c;

            public void Execute(ref int[] register)
            {
                op(ref register, a, b, c);
            }
        }

        int InstructionPointer { get { return register[boundRegister]; } }
        int boundRegister;
        List<ProgramLine> program;
        int[] register;

        public bool ExecuteStep()
        {
            var ip = InstructionPointer;

            if (ip < 0 || ip >= program.Count) return false;
            program[ip].Execute(ref register);

            register[boundRegister]++;
            return true;
        }

        public void WriteResult()
        {
            register = new int[6];
            for (int n = 0; n < 11; n++) ExecuteStep();
            HetProgramma();
            Console.WriteLine("part 1: " + register[0]);

            register = new int[6];
            register[0] = 1;
            for (int n = 0; n < 20; n++) ExecuteStep();
            HetProgramma();
            Console.WriteLine("part 2: " + register[0]);
        }

        private void HetProgramma()
        {
            int target = register[2];
            int output = register[0];

            for (int outerloopCounter = 1; outerloopCounter <= target; outerloopCounter++)
            {
                if (target % outerloopCounter != 0) continue;
                else output = outerloopCounter + output;
            }

            register[0] = output;
        }

        public override string ToString()
        {
            return "ip=" + InstructionPointer + " [" + string.Join(", ", register) + "]";
            
        }
    }
}
