﻿using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.OpCode
{
    public class Executor
    {
        public List<string> input;

        public List<Operator> operators = new List<Operator>();
        public int instructionIndex;

        public long accumulator;

        public Executor(List<string> input) 
        {
            this.input = input;

            Reset();
        }
        public Executor(string[] input) : this(input.ToList()) { }

        public bool ExecuteStep()
        {
            if (instructionIndex == operators.Count)
            {
                return false;
            }

            operators[instructionIndex].Execute(this);
            
            return true;
        }

        public void Reset()
        {
            operators = new List<Operator>();
            for (int n = 0; n < input.Count(); n++)
            {
                operators.Add(Operator.Parse(n, input[n]));
            };

            instructionIndex = 0;
            accumulator = 0;
        }
    }

    public abstract class Operator
    {
        public int Position { get; set; }

        public int Argument { get; set; }

        public Operator(int position, int argument)
        {
            Position = position;
            Argument = argument;
        }

        public static Operator Parse(int position, string opLine)
        {
            var op = opLine.Replace("+", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var argument = int.Parse(op[1]);
            switch (op[0])
            {
                case "nop": return new Nop(position, argument);
                case "acc": return new Acc(position, argument);
                case "jmp": return new Jmp(position, argument);
                default: throw new ParseException("could not parse line " + opLine);
            }
        }

        public abstract void Execute(Executor executor);

        public virtual int NextPointer()
        {
            return this.Position + 1;
        }
    }

    public class Acc : Operator
    {
        public Acc(int position, int argument) : base(position, argument) { }

        public override void Execute(Executor executor)
        {
            executor.accumulator += Argument;
            executor.instructionIndex++;
        }
    }

    public class Jmp : Operator
    {
        public Jmp(int position, int argument) : base(position, argument) { }

        public override void Execute(Executor executor)
        {
            executor.instructionIndex += Argument;
        }

        public override int NextPointer()
        {
            return this.Position + Argument;
        }
    }

    public class Nop : Operator
    {
        public Nop(int position, int argument) : base(position, argument) { }

        public override void Execute(Executor executor)
        {
            executor.instructionIndex++;
        }
    }
}
