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
        public bool Blocked { get; set; } = false;

        public long instructionPointer;
        public long relativeBase;

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
            newProgram.Blocked = this.Blocked;
            newProgram.instructionPointer = this.instructionPointer;
            newProgram.relativeBase = this.relativeBase;
            newProgram.inputs = new Queue<string>(inputs);
            newProgram.output = new Queue<string>(output);

            return newProgram;
        }

        public string AtPointer()
        {
            return SafeGet(instructionPointer);
        }
        public long IAtPointer()
        {
            return IntGet(instructionPointer);
        }

        public string AtOffset(long offset)
        {
            return SafeGet(instructionPointer + offset);
        }
        public long IAtOffset(long offset)
        {
            return IntGet(instructionPointer + offset);
        }

        public string GetAt(long position)
        {
            return SafeGet(position);
        }
        public long IGetAt(long position)
        {
            return IntGet(position);
        }

        public void SetAt(long position, string value)
        {
            var hasBreak = SafeGet(position)?.Contains("B") ?? false;
            if (position >= program.Length)
            {
                var newLength = Math.Min(int.MaxValue, position * 2);
                var buffer = new string[newLength];
                program.CopyTo(buffer, 0);
                program = buffer;
            }
            program[position] = value + (hasBreak ? "B" : "");
        }
        public void ISetAt(long position, long value)
        {
            SetAt(position, value.ToString());
        }


        public long IntGet(long position) { return long.Parse(SafeGet(position).Replace("B", "")); }
        public string SafeGet(long position)
        {
            if (position < 0 || position >= program.Length) return "0";
            else
            {
                if (program[position] == null) program[position] = "0";

                var trimmed = program[position].TrimStart('0');
                if (trimmed.Length == 0) return "0";
                else return trimmed;
            }
        }

        public void RemoveBreakpoint(long position)
        {
            string noBreak = SafeGet(position).Replace("B", "");
            program[position] = noBreak;
        }
    }
}
