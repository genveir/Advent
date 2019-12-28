using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.OpCode
{
    public class Executor
    {
        public Program startProgram;
        public Program program;

        public Executor(string[] ops)
        {
            startProgram = new Program(ops.Select(op => long.Parse(op)).ToArray());
            Reset();
        }
        public Executor(Program program)
        {
            this.startProgram = program;
            Reset();
        }

        public void Reset()
        {
            program = startProgram.Copy();
        }

        public void AddInput(long input)
        {
            program.inputs.Enqueue(input);

            if (program.Blocked)
            {
                program.Blocked = false;
                Execute();
            }
        }

        public void AddAscInput(string input)
        {
            input = input.Replace("\r", "").TrimStart('\n');

            foreach (var ch in input) AddInput(ch);
            AddInput(10);
        }

        public string GetAscOutput()
        {
            var sb = new StringBuilder();
            while (program.output.Count > 0)
            {
                var output = program.output.Dequeue();

                if (output > 256) sb.Append(output);
                else sb.Append((char)output);
            }
            return sb.ToString();
        }

        public long lastBreakPosition;
        public bool Break { get; set; }
        public long PrintIndex { get; set; } = -1;
        public bool SkipZeroNops { get; set; }
        

        public void Step()
        {
            if (!program.Stop)
            {
                var currentOp = program.CurrentOperator;
                currentOp.Execute();
                program.instructionPointer += currentOp.OpLength;
            }
        }

        public void Execute()
        {
            while (!program.Stop && !program.Blocked)
            {
                Step();
            }
        }

        public void ExecuteToOutput()
        {
            while (!program.Stop && !program.Blocked && program.output.Count == 0)
            {
                Step();
            }
        }
    }
}
