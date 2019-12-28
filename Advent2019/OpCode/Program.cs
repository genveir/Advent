using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.OpCode
{
    public class Program
    {
        public string Name { get; set; }

        public long[] program;

        public bool Stop { get; set; } = false;
        public bool Blocked { get; set; } = false;

        public long instructionPointer;
        public long relativeBase;

        public Queue<long> inputs;

        public bool Verbose { get; set; } = false;
        public Queue<long> output;

        private Program() { inputs = new Queue<long>(); output = new Queue<long>(); }
        public Program(long[] ops) : this()
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
            newProgram.inputs = new Queue<long>(inputs);
            newProgram.output = new Queue<long>(output);

            return newProgram;
        }

        public long AtPointer()
        {
            return SafeGet(instructionPointer);
        }

        public long AtOffset(long offset)
        {
            return SafeGet(instructionPointer + offset);
        }

        public long GetAt(long position)
        {
            return SafeGet(position);
        }

        public void SetAt(long position, long value)
        {
            if (position >= program.Length)
            {
                var newLength = Math.Min(int.MaxValue, position * 2);
                var buffer = new long[newLength];
                program.CopyTo(buffer, 0);
                program = buffer;
            }
            program[position] = value;
        }

        public long SafeGet(long position)
        {
            if (position < 0 || position >= program.Length) return 0;
            else
            {
                return program[position];
            }
        }
    }
}
