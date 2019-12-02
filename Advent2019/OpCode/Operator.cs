using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.OpCode
{
    public abstract class Operator
    {
        public Program program;
        public bool Break { get; set; }

        public Operator(Program program)
        {
            this.program = program;
        }

        public static Operator GetCurrent(Program program)
        {
            bool breakOp = false;

            var instruction = program.AtPointer();
            if (instruction.Contains("B"))
            {
                instruction = instruction.Replace("B", "");
                breakOp = true;
            }

            Operator op;
            switch (instruction)
            {
                case "1": op = new AddRef(program); break;
                case "2": op = new MultiplyRef(program); break;
                case "99": op = new Stop(program); break;
                default: op = new NotAnOp(program); break;
            }

            op.Break = breakOp;
            return op;
        }

        public abstract void Execute();
        public abstract int OpLength { get; }
    }

    public abstract class BaseFourPlaceOperator : Operator
    {
        public BaseFourPlaceOperator(Program program) : base(program) { }
        public abstract string OpName { get; }
        public override int OpLength => 4;

        protected int input1Ref { get { return program.IAtOffset(1); } }
        protected int input2Ref { get { return program.IAtOffset(2); } }
        protected int outputRef { get { return program.IAtOffset(3); } }

        public override string ToString()
        {
            return string.Format("{0} {1} [{2}] + {3} [{4}] => {5} {6}",
                OpName,
                input1Ref, program.GetAt(input1Ref),
                input2Ref, program.GetAt(input2Ref),
                outputRef,
                Break ? "BREAK" : "");
        }
    }

    public class AddRef : BaseFourPlaceOperator
    {
        public AddRef(Program program) : base(program) { }
        public override string OpName => "AddRef";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1Ref) + program.IGetAt(input2Ref));
        }
    }

    public class SubtractRef : BaseFourPlaceOperator
    {
        public SubtractRef(Program program) : base(program) { }
        public override string OpName => "SubRef";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1Ref) - program.IGetAt(input2Ref));
        }
    }

    public class MultiplyRef : BaseFourPlaceOperator
    {
        public MultiplyRef(Program program) : base(program) { }
        public override string OpName => "MulRef";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1Ref) * program.IGetAt(input2Ref));
        }
    }

    public class DivideRef : BaseFourPlaceOperator
    {
        public DivideRef(Program program) : base(program) { }
        public override string OpName => "DivRef";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1Ref) / program.IGetAt(input2Ref));
        }
    }

    public class Stop : Operator
    {
        public Stop(Program program) : base(program) { }
        public override int OpLength => 1;

        public override void Execute()
        {
            program.Stop = true;
        }

        public override string ToString()
        {
            return "Stop " + (Break ? "BREAK" : "");
        }
    }

    public class NotAnOp : Stop
    {
        public NotAnOp(Program program) : base(program) { }

        public override string ToString()
        {
            return program.GetAt(program.instructionPointer).ToString();
        }
    }
}
