using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.OpCode
{
    public class Executor
    {
        public Program startProgram;
        public Program program;

        public Executor(string[] ops) : this(new Program(ops)) { }
        public Executor(Program program)
        {
            this.startProgram = program;
            Reset();
        }

        public void Reset()
        {
            program = startProgram.Copy();
        }

        public int lastBreakPosition;
        public bool Break { get; set; }
        public void DoBreak()
        {
            Console.Clear();
            Print();
            Console.WriteLine("[C]ontinue operation, [I]gnore breakpoint this run, [D]elete breakpoint, or [Enter] for step");
            var action = Console.ReadLine();

            switch(action)
            {
                case "c":
                case "C":
                    Break = false;
                    break;
                case "i":
                case "I":
                    program.RemoveBreakpoint(lastBreakPosition);
                    Break = false;
                    break;
                case "d":
                case "D":
                    program.RemoveBreakpoint(lastBreakPosition);
                    startProgram.RemoveBreakpoint(lastBreakPosition);
                    Break = false;
                    break;
            }
        }

        public void Step()
        {
            if (!program.Stop)
            {
                var currentOp = program.CurrentOperator;
                if (currentOp.Break) { this.Break = true; lastBreakPosition = program.instructionPointer; }
                if (this.Break) DoBreak();
                currentOp.Execute();
                program.instructionPointer += currentOp.OpLength;
            }
        }

        public void Execute()
        {
            while (!program.Stop)
            {
                Step();
            }
        }


        public void Print()
        {
            // beter maken
            var copy = program.Copy();
            copy.instructionPointer = 0;
            while (copy.instructionPointer < copy.program.Length)
            {
                bool atPointer = copy.instructionPointer == program.instructionPointer;

                var op = Operator.GetCurrent(copy);
                
                Console.ForegroundColor = atPointer ? ConsoleColor.Red : ConsoleColor.White;
                Console.Write(copy.instructionPointer.ToString().PadRight(4));
                Console.Write(atPointer ? "[" : " ");
                Console.Write(op);
                Console.Write(atPointer ? "]" : " ");
                Console.WriteLine();

                copy.instructionPointer += op.OpLength;
            }
        }
    }
}
