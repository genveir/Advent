using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.OpCode
{
    public class Program
    {
        public string Name { get; set; }

        public string[] program;

        public bool Stop { get; set; } = false;

        public int instructionPointer;

        public Queue<string> inputs;

        public bool Verbose { get; set; } = false;
        public Queue<string> output;

        private Program() { inputs = new Queue<string>(); output = new Queue<string>(); }
        public Program(string[] ops) : this()
        {
            program = ops.DeepCopy();
            Stop = false;

            instructionPointer = 0;
        }

        public Operator CurrentOperator { get { return Operator.GetCurrent(this); } }

        public Program Copy()
        {
            var newProgram = new Program(program);
            newProgram.Stop = this.Stop;
            newProgram.instructionPointer = this.instructionPointer;
            newProgram.inputs = new Queue<string>(inputs);
            newProgram.output = new Queue<string>(output);

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
            if (position >= program.Length)
            {
                var newLength = Math.Min(int.MaxValue, Math.Max(position, program.LongLength * 2L));
                var buffer = new string[newLength];
                program.CopyTo(buffer, 0);
                program = buffer;
            }
            program[position] = value + (hasBreak ? "B" : "");
        }
        public void ISetAt(int position, int value)
        {
            SetAt(position, value.ToString());
        }


        public int IntGet(int position) { return int.Parse(SafeGet(position).Replace("B", "")); }
        public string SafeGet(int position)
        {
            if (position < 0 || position >= program.Length) return "0";
            else return program[position];
        }

        public void RemoveBreakpoint(int position)
        {
            string noBreak = SafeGet(position).Replace("B", "");
            program[position] = noBreak;
        }
    }
}
