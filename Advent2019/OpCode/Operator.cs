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

        public static Operator Get(Program program, int position)
        {
            bool breakOp = false;

            var instruction = program.GetAt(position);
            if (instruction.Contains("B"))
            {
                instruction = instruction.Replace("B", "");
                breakOp = true;
            }

            Operator op;
            switch (instruction)
            {
                case "1": op = new AddRR(program); break;
                case "2": op = new MultiplyRR(program); break;
                case "99": op = new Stop(program); break;
                default: op = new NotAnOp(program); break;
            }

            op.Break = breakOp;
            return op;
        }

        public static Operator GetCurrent(Program program)
        {
            return Get(program, program.instructionPointer);
        }

        public abstract void Execute();
        public abstract int OpLength { get; }
        public abstract string OpName { get; }
    }

    public abstract class BaseTwoPlaceOperator : Operator
    {
        public BaseTwoPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 2;

        protected int input { get { return program.IAtOffset(1); } }

        protected string Format(bool inputIsRef)
        {
            return OpName + ":" +
                (inputIsRef ? "ref(" : "") + input + (inputIsRef ? ")" : "");
        }
    }

    public abstract class BaseThreePlaceOperator : Operator
    {
        public BaseThreePlaceOperator(Program program) : base(program) { }
        public override int OpLength => 3;

        protected int input { get { return program.IAtOffset(1); } }
        protected int outputRef { get { return program.IAtOffset(2); } }

        protected string Format(bool inputIsRef)
        {
            return OpName + ":" +
                (inputIsRef ? "ref(" : "") + input + (inputIsRef ? ")" : "") +
                " = " +
                (inputIsRef ? program.IGetAt(input).ToString() : input.ToString()) +
                " => ref(" + outputRef + ")";
        }
    }

    public abstract class BaseFourPlaceOperator : Operator
    {
        public BaseFourPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 4;

        protected int input1 { get { return program.IAtOffset(1); } }
        protected int input2 { get { return program.IAtOffset(2); } }
        protected int outputRef { get { return program.IAtOffset(3); } }

        protected string Format(bool input1IsRef, bool input2IsRef, string op)
        {
            return OpName + ":" +
                (input1IsRef ? "ref(" : "") + input1 + (input1IsRef ? ")" : "") +
                " " + op + " " +
                (input2IsRef ? "ref(" : "") + input2 + (input1IsRef ? ")" : "") +
                "  = " + 
                (input1IsRef ? program.IGetAt(input1).ToString() : input1.ToString()) + 
                " " + op + " " +
                (input2IsRef ? program.IGetAt(input2).ToString() : input2.ToString()) +
                " => ref(" + outputRef + ")";
        }
    }

    public class AddRR : BaseFourPlaceOperator
    {
        public AddRR(Program program) : base(program) { }
        public override string OpName => "AddRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) + program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "+");
        }
    }

    public class AddRI : BaseFourPlaceOperator
    {
        public AddRI(Program program) : base(program) { }
        public override string OpName => "AddRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) + input2);
        }

        public override string ToString()
        {
            return Format(true, false, "+");
        }
    }

    public class AddIR : BaseFourPlaceOperator
    {
        public AddIR(Program program) : base(program) { }
        public override string OpName => "AddIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 + program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "+");
        }
    }

    public class SubtractRR : BaseFourPlaceOperator
    {
        public SubtractRR(Program program) : base(program) { }
        public override string OpName => "SubRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) - program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "-");
        }
    }

    public class SubtractRI : BaseFourPlaceOperator
    {
        public SubtractRI(Program program) : base(program) { }
        public override string OpName => "SubRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) - input2);
        }

        public override string ToString()
        {
            return Format(true, false, "-");
        }
    }

    public class SubtractIR : BaseFourPlaceOperator
    {
        public SubtractIR(Program program) : base(program) { }
        public override string OpName => "SubIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 - program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "-");
        }
    }

    public class MultiplyRR : BaseFourPlaceOperator
    {
        public MultiplyRR(Program program) : base(program) { }
        public override string OpName => "MulRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) * program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "*");
        }
    }

    public class MultiplyRI : BaseFourPlaceOperator
    {
        public MultiplyRI(Program program) : base(program) { }
        public override string OpName => "MulRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) * input2);
        }

        public override string ToString()
        {
            return Format(true, false, "*");
        }
    }

    public class MultiplyIR : BaseFourPlaceOperator
    {
        public MultiplyIR(Program program) : base(program) { }
        public override string OpName => "MulIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 * program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "*");
        }
    }

    public class DivideRR : BaseFourPlaceOperator
    {
        public DivideRR(Program program) : base(program) { }
        public override string OpName => "DivRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) / program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "/");
        }
    }

    public class DivideRI : BaseFourPlaceOperator
    {
        public DivideRI(Program program) : base(program) { }
        public override string OpName => "DivRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) / input2);
        }

        public override string ToString()
        {
            return Format(true, false, "/");
        }
    }

    public class DivideIR : BaseFourPlaceOperator
    {
        public DivideIR(Program program) : base(program) { }
        public override string OpName => "DivIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 / program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "/");
        }
    }

    public class ModRR : BaseFourPlaceOperator
    {
        public ModRR(Program program) : base(program) { }
        public override string OpName => "ModRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) % program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "%");
        }
    }

    public class ModRI : BaseFourPlaceOperator
    {
        public ModRI(Program program) : base(program) { }
        public override string OpName => "ModRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) % input2);
        }

        public override string ToString()
        {
            return Format(true, false, "%");
        }
    }

    public class ModIR : BaseFourPlaceOperator
    {
        public ModIR(Program program) : base(program) { }
        public override string OpName => "ModIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 % program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "%");
        }
    }

    public class BANDRR : BaseFourPlaceOperator
    {
        public BANDRR(Program program) : base(program) { }
        public override string OpName => "BANDRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) & program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "&");
        }
    }

    public class BANDRI : BaseFourPlaceOperator
    {
        public BANDRI(Program program) : base(program) { }
        public override string OpName => "BANDRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) & input2);
        }

        public override string ToString()
        {
            return Format(true, false, "&");
        }
    }

    public class BANDIR : BaseFourPlaceOperator
    {
        public BANDIR(Program program) : base(program) { }
        public override string OpName => "BANDIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 & program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "&");
        }
    }

    public class BORRR : BaseFourPlaceOperator
    {
        public BORRR(Program program) : base(program) { }
        public override string OpName => "BORRR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) | program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(true, true, "|");
        }
    }

    public class BORRI : BaseFourPlaceOperator
    {
        public BORRI(Program program) : base(program) { }
        public override string OpName => "BORRI";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input1) | input2);
        }

        public override string ToString()
        {
            return Format(true, false, "|");
        }
    }

    public class BORIR : BaseFourPlaceOperator
    {
        public BORIR(Program program) : base(program) { }
        public override string OpName => "BORIR";

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 | program.IGetAt(input2));
        }

        public override string ToString()
        {
            return Format(false, true, "|");
        }
    }

    public class JumpR : BaseTwoPlaceOperator
    {
        public JumpR(Program program) : base(program) { }
        public override string OpName => "JumpR";

        public override void Execute()
        {
            program.instructionPointer += program.IGetAt(input) - 2; // assumption: jump ahead N places, no shift after
        }

        public override string ToString()
        {
            return Format(true);
        }
    }

    public class JumpI : BaseTwoPlaceOperator
    {
        public JumpI(Program program) : base(program) { }
        public override string OpName => "JumpI";

        public override void Execute()
        {
            program.instructionPointer += input - 2; // assumption: jump ahead N places, no shift after
        }

        public override string ToString()
        {
            return Format(false);
        }
    }

    public class GotoR : BaseTwoPlaceOperator
    {
        public GotoR(Program program) : base(program) { }
        public override string OpName => "GotoR";

        public override void Execute()
        {
            program.instructionPointer = program.IGetAt(input) - 2; // assumption: jump ahead N places, no shift after
        }
        public override string ToString()
        {
            return Format(true);
        }

    }

    public class GotoI : BaseTwoPlaceOperator
    {
        public GotoI(Program program) : base(program) { }
        public override string OpName => "GotoI";

        public override void Execute()
        {
            program.instructionPointer = input - 2; // assumption: jump ahead N places, no shift after
        }

        public override string ToString()
        {
            return Format(false);
        }
    }

    public class SetR : BaseThreePlaceOperator
    {
        public SetR(Program program) : base(program) { }
        public override string OpName => "SetR";

        public override void Execute()
        {
            program.ISetAt(outputRef, program.IGetAt(input));
        }

        public override string ToString()
        {
            return Format(true);
        }
    }

    public class SetI : BaseThreePlaceOperator
    {
        public SetI(Program program) : base(program) { }
        public override string OpName => "SetI";

        public override void Execute()
        {
            program.ISetAt(outputRef, input);
        }

        public override string ToString()
        {
            return Format(false);
        }
    }

    public class Stop : Operator
    {
        public Stop(Program program) : base(program) { }
        public override int OpLength => 1;
        public override string OpName => "Stop";

        public override void Execute()
        {
            program.Stop = true;
        }

        public override string ToString()
        {
            return OpName;
        }
    }

    public class NotAnOp : Stop
    {
        public NotAnOp(Program program) : base(program) { }
        public override string OpName => "NOP(" + program.AtPointer() + ")";

        public override string ToString()
        {
            return OpName;
        }
    }
}
