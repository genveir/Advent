using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.OpCode
{
    public class Program
    {
        public Dictionary<int, string> program;

        public bool Stop { get; set; } = false;

        public int instructionPointer;

        private Program() { program = new Dictionary<int, string>(); }
        public Program(string[] ops) : this()
        {
            for (int n = 0; n < ops.Length; n++)
            {
                program[n] = ops[n];
            }
            Stop = false;

            instructionPointer = 0;
        }

        public Operator CurrentOperator { get { return Operator.GetCurrent(this); } }

        public Program Copy()
        {
            var newProgram = new Program();
            foreach(var kvp in program) { newProgram.SetAt(kvp.Key, kvp.Value); }
            newProgram.Stop = this.Stop;
            newProgram.instructionPointer = this.instructionPointer;

            return newProgram;
        }

        public string AtPointer()
        {
            return SafeGet(instructionPointer);
        }
        public int IAtPointer()
        {
            return IntGet(instructionPointer);
        }

        public string AtOffset(int offset)
        {
            return SafeGet(instructionPointer + offset);
        }
        public int IAtOffset(int offset)
        {
            return IntGet(instructionPointer + offset);
        }

        public string GetAt(int position)
        {
            return SafeGet(position);
        }
        public int IGetAt(int position)
        {
            return IntGet(position);
        }

        public void SetAt(int position, string value)
        {
            var hasBreak = SafeGet(position)?.Contains("B") ?? false;
            program[position] = value + (hasBreak ? "B" : "");
        }
        public void ISetAt(int position, int value)
        {
            SetAt(position, value.ToString());
        }


        public int IntGet(int position) { return int.Parse(SafeGet(position).Replace("B", "")); }
        public string SafeGet(int position)
        {
            string output;
            program.TryGetValue(position, out output);
            return output;
        }

        public void RemoveBreakpoint(int position)
        {
            string noBreak = SafeGet(position).Replace("B", "");
            program[position] = noBreak;
        }
    }
}
