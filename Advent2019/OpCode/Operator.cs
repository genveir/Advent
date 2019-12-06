using System;
using System.Collections.Generic;
using System.Linq;
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

            bool[] isRef = new bool[3] { true, true, true };

            var broken = instruction.ToCharArray().Select(c => c - 48).ToArray(); ;
            int counter = 0;
            if (broken.Length < 6)
            {
                for (int n = broken.Length - 3; n >= 0; n--)
                {
                    isRef[counter++] = broken[n] == 0;
                }
            }


            var instructionlength2 = broken.Length > 1;
            int startindex = broken.Length - (instructionlength2 ? 2 : 1);
            instruction = instruction.Substring(startindex);
            instruction = instruction.TrimStart('0');

            Operator op;
            switch (instruction)
            {
                case "1": op = new Add(program); break;
                case "2": op = new Multiply(program); break;
                case "3": op = new In(program);break;
                case "4": op = new Out(program);break;
                case "5": op = new JumpIfTrue(program); break;
                case "6": op = new JumpIfFalse(program); break;
                case "7": op = new Less(program); break;
                case "8": op = new Equal(program); break;
                case "99": op = new Stop(program); break;                
                    
                default: op = new NotAnOp(program); break;
            }
            op.Ref = isRef;

            op.Break = breakOp;
            return op;
        }

        public static Operator GetCurrent(Program program)
        {
            return Get(program, program.instructionPointer);
        }

        public bool[] Ref;
        protected int param1 { get { return Ref[0] ? program.IGetAt(program.IAtOffset(1)) : program.IAtOffset(1); } }
        protected int param2 { get { return Ref[1] ? program.IGetAt(program.IAtOffset(2)) : program.IAtOffset(2); } }
        protected int param3 { get { return Ref[2] ? program.IGetAt(program.IAtOffset(3)) : program.IAtOffset(3); } }

        public abstract void Execute();
        public abstract int OpLength { get; }
        public abstract string OpName { get; }
    }

    public abstract class BaseTwoPlaceOperator : Operator
    {
        public BaseTwoPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 2;

        protected int input { get { return param1; } }

        protected string Format()
        {
            bool inputIsRef = Ref[0];

            return OpName + ":" +
                (inputIsRef ? "ref(" : "") + input + (inputIsRef ? ")" : "");
        }
    }

    public abstract class BaseThreePlaceOperator : Operator
    {
        public BaseThreePlaceOperator(Program program) : base(program) { }
        public override int OpLength => 3;

        protected int input { get { return param1; } }
        protected int output { get { return param2; } }

        protected string Format()
        {
            bool inputIsRef = Ref[0];

            return OpName + ":" +
                (inputIsRef ? "ref(" : "") + input + (inputIsRef ? ")" : "") +
                " = " +
                (inputIsRef ? program.IGetAt(input).ToString() : input.ToString()) +
                " => ref(" + output + ")";
        }
    }

    public abstract class BaseFourPlaceOperator : Operator
    {
        public BaseFourPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 4;

        protected int input1 { get { return param1; } }
        protected int input2 { get { return param2; } }
        protected int outputRef { get { return program.IAtOffset(3); } }

        protected string Format(string op)
        {
            bool input1IsRef = Ref[0];
            bool input2IsRef = Ref[1];

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

    public class In : BaseTwoPlaceOperator
    {
        public In(Program program) : base(program) { }
        public override string OpName => "In";

        public override void Execute()
        {
            Ref[0] = false;
            Console.Write("inp: ");
            var val = Console.ReadLine();

            program.SetAt(input, val);
        }

        public override string ToString()
        {
            return Format();
        }
    }

    public class Out : BaseTwoPlaceOperator
    {
        public Out(Program program) : base(program) { }
        public override string OpName => "Out";

        public override void Execute()
        {
            Console.WriteLine("Output: " + param1);
        }

        public override string ToString()
        {
            return Format();
        }
    }

    public class Add : BaseFourPlaceOperator
    {
        public Add(Program program) : base(program) { }
        public override string OpName => "Add" + (Ref[0] ? "R" : "I") + (Ref[1] ? "R" : "I");

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 + input2);
        }

        public override string ToString()
        {
            return Format("+");
        }
    }
    

    public class Multiply : BaseFourPlaceOperator
    {
        public Multiply(Program program) : base(program) { }
        public override string OpName => "Mul" + (Ref[0] ? "R" : "I") + (Ref[1] ? "R" : "I");

        public override void Execute()
        {
            program.ISetAt(outputRef, input1 * input2);
        }

        public override string ToString()
        {
            return Format("*");
        }
    }

    public class JumpIfTrue : BaseThreePlaceOperator
    {
        public JumpIfTrue(Program program) : base(program) { }
        public override string OpName => "JIT" + (Ref[0] ? "R" : "I");

        public override void Execute()
        {
            if (input != 0) program.instructionPointer = output - 3;
        }

        public override string ToString()
        {
            return Format();
        }
    }

    public class JumpIfFalse : BaseThreePlaceOperator
    {
        public JumpIfFalse(Program program) : base(program) { }
        public override string OpName => "JIF" + (Ref[0] ? "R" : "I");

        public override void Execute()
        {
            if (input == 0) program.instructionPointer = output - 3;
        }

        public override string ToString()
        {
            return Format();
        }
    }

    public class Less : BaseFourPlaceOperator
    {
        public Less(Program program): base(program) { }
        public override string OpName => "Les" + (Ref[0] ? "R" : "I") + (Ref[1] ? "R" : "I");

        public override void Execute()
        {
            if (input1 < input2) program.ISetAt(outputRef, 1);
            else program.ISetAt(outputRef, 0);
        }

        public override string ToString()
        {
            return Format("<");
        }
    }

    public class Equal : BaseFourPlaceOperator
    {
        public Equal(Program program) : base(program) { }
        public override string OpName => "Eq" + (Ref[0] ? "R" : "I") + (Ref[1] ? "R" : "I");

        public override void Execute()
        {
            if (input1 == input2) program.ISetAt(outputRef, 1);
            else program.ISetAt(outputRef, 0);
        }

        public override string ToString()
        {
            return Format("=");
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
