using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.ElfCode
{
    class ElfCodeInterpreter
    {
        List<ProgramLine> program;
        int[] register;
        int boundRegister = -1;

        bool bound;
        int _ip;
        int InstructionPointer
        {
            get
            {
                if (bound) _ip = register[boundRegister];
                return _ip;
            }
            set
            {
                if (bound) register[boundRegister] = value;
                _ip = value;
            }
        }

        bool hasNextInput { get { return inputCursor != input.Length; } }
        int nextInput { get { return hasNextInput ? input[inputCursor++] : 0; } }
        int[] input;
        int inputCursor = 0;

        public ElfCodeInterpreter(string input, int numRegisters)
        {
            program = new List<ProgramLine>();
            register = new int[numRegisters];

            var lines = input.Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("//")) continue;

                if (line.StartsWith("#ip"))
                {
                    var register = int.Parse(line.Split(' ')[1]);
                    boundRegister = register;
                    bound = true;
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
                .Take(4)
                .TakeLast(3)
                .Select(s => int.Parse(s))
                .ToArray();
            var breakHere = splitLine.Take(5).Last().Trim() == "B";

            var programLine = new ProgramLine
            {
                op = op,
                a = lineVals[0],
                b = lineVals[1],
                c = lineVals[2],
                breakHere = breakHere
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
                case "peek": return peek;
                case "geti": return geti;
                case "outi": return outi;
                case "outr": return outr;
                default: throw new Exception("unknown instruction " + name);
            }
        }

        private string GetOpName(op op)
        {
            return op.Method.Name;
        }

        public delegate void op(ref int[] r, int i1, int i2, int o);
        public void addr(ref int[] r, int a, int b, int c) { r[c] = r[a] + r[b]; }
        public void addi(ref int[] r, int a, int b, int c) { r[c] = r[a] + b; }
        public void mulr(ref int[] r, int a, int b, int c) { r[c] = r[a] * r[b]; }
        public void muli(ref int[] r, int a, int b, int c) { r[c] = r[a] * b; }
        public void banr(ref int[] r, int a, int b, int c) { r[c] = r[a] & r[b]; }
        public void bani(ref int[] r, int a, int b, int c) { r[c] = r[a] & b; }
        public void borr(ref int[] r, int a, int b, int c) { r[c] = r[a] | r[b]; }
        public void bori(ref int[] r, int a, int b, int c) { r[c] = r[a] | b; }
        public void setr(ref int[] r, int a, int b, int c) { r[c] = r[a]; }
        public void seti(ref int[] r, int a, int b, int c) { r[c] = a; }
        public void gtir(ref int[] r, int a, int b, int c) { r[c] = a > r[b] ? 1 : 0; }
        public void gtri(ref int[] r, int a, int b, int c) { r[c] = r[a] > b ? 1 : 0; }
        public void gtrr(ref int[] r, int a, int b, int c) { r[c] = r[a] > r[b] ? 1 : 0; }
        public void eqir(ref int[] r, int a, int b, int c) { r[c] = a == r[b] ? 1 : 0; }
        public void eqri(ref int[] r, int a, int b, int c) { r[c] = r[a] == b ? 1 : 0; }
        public void eqrr(ref int[] r, int a, int b, int c) { r[c] = r[a] == r[b] ? 1 : 0; }

        public void peek(ref int[] r, int a, int b, int c) { r[c] = hasNextInput ? 1 : 0; }
        public void geti(ref int[] r, int a, int b, int c) { r[c] = nextInput; }

        public void outi(ref int[]r, int a, int b, int c) { Console.WriteLine(c); }
        public void outr(ref int[]r, int a, int b, int c) { Console.WriteLine(r[c]); }

        private class ProgramLine
        {
            public op op;
            public int a, b, c;
            public bool breakHere;

            public void Execute(ref int[] register)
            {
                op(ref register, a, b, c);
            }

            public override string ToString()
            {
                return op.Method.Name + " " + a + " " + b + " " + c;
            }
        }

        public void Reset()
        {
            for (int n = 0; n < register.Length; n++) register[n] = 0;
        }

        public void SetRegister(int index, int value)
        {
            register[index] = value;
        }

        public int GetRegister(int index)
        {
            return register[index];
        }

        public void SetInput(int[] input)
        {
            inputCursor = 0;
            this.input = input;
        }

        bool breakNext;
        public bool ExecuteStep()
        {
            var ip = InstructionPointer;

            if (ip < 0 || ip >= program.Count) return false;
            var line = program[ip];
            if (line.breakHere || breakNext)
            {
                Console.WriteLine(this + " " + line);
                Console.WriteLine("(s)tep, [c]ontinue" + (line.breakHere ? "or (d)elete breakpoint and continue?" : ""));

                breakNext = false;
                var i = Console.ReadLine();
                if (i == "s") breakNext = true;
                else if (i == "d") line.breakHere = false;
            }
            line.Execute(ref register);

            InstructionPointer++;
            return true;
        }

        public override string ToString()
        {
            return "ip: " + InstructionPointer + " ["+ string.Join(", ", register) + "]";
        }
    }
}
