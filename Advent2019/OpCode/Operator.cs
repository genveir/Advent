using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.OpCode
{
    public enum OpMode { Pos, Int, Rel }

    public abstract class Operator
    {
        public Program program;
        public bool Break { get; set; }

        public Operator(Program program)
        {
            this.program = program;
        }

        public static Operator Get(Program program, long position)
        {
            bool breakOp = false;

            var instruction = program.GetAt(position).Trim();

            if (instruction.Contains("B"))
            {
                instruction = instruction.Replace("B", "");
                breakOp = true;
            }

            OpMode[] opMode = new OpMode[3] { OpMode.Pos, OpMode.Pos, OpMode.Pos };

            var broken = instruction.ToCharArray().Select(c => c - 48).ToArray(); ;
            int counter = 0;

            if (broken.Length < 6)
            {
                for (int n = broken.Length - 3; n >= 0; n--)
                {
                    switch(broken[n])
                    {
                        case 0: opMode[counter++] = OpMode.Pos;break;
                        case 1: opMode[counter++] = OpMode.Int; break;
                        case 2: opMode[counter++] = OpMode.Rel; break;
                    }
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
                case "9": op = new AdjustRelativeBase(program); break;
                case "99": op = new Stop(program); break;                
                    
                default: op = new NotAnOp(program); break;
            }
            op.OpModes = opMode;

            op.Break = breakOp;
            return op;
        }

        public static Operator GetCurrent(Program program)
        {
            return Get(program, program.instructionPointer);
        }

        public OpMode[] OpModes;

        protected virtual long GetParam(int paramIndex, bool IndexParam = false)
        {
            if (IndexParam)
            {
                switch (OpModes[paramIndex])
                {
                    case OpMode.Pos: return program.IAtOffset(paramIndex + 1);
                    case OpMode.Int: throw new NotImplementedException();
                    case OpMode.Rel:
                        long relBase = program.relativeBase;
                        long add = program.IAtOffset(paramIndex + 1);
                        return relBase + add;
                    default: throw new Exception("user your enum Geerten");
                }
            }
            else
            {
                switch (OpModes[paramIndex])
                {
                    case OpMode.Pos: return program.IGetAt(program.IAtOffset(paramIndex + 1));
                    case OpMode.Int: return program.IAtOffset(paramIndex + 1);
                    case OpMode.Rel:
                        long relBase = program.relativeBase;
                        long add = program.IAtOffset(paramIndex + 1);
                        return program.IGetAt(relBase + add);
                    default: throw new Exception("user your enum Geerten");
                }
            }
        }

        protected long param1 { get { return GetParam(0); } }
        protected long param2 { get { return GetParam(1); } }
        protected long param3 { get { return GetParam(2); } }

        protected long relParam1 { get { return GetParam(0, true); } }
        protected long relParam2 { get { return GetParam(1, true); } }
        protected long relParam3 { get { return GetParam(2, true); } }
    
        protected string FormatParams(params OpMode[] opModes)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var opMode in opModes)
            {
                switch(opMode)
                {
                    case OpMode.Pos: sb.Append("P"); break;
                    case OpMode.Int: sb.Append("I"); break;
                    case OpMode.Rel: sb.Append("R"); break;
                }
            }
            return sb.ToString();
        }

        public abstract void Execute();
        public abstract int OpLength { get; }
        public abstract string OpName { get; }
    }

    public abstract class BaseTwoPlaceOperator : Operator
    {
        public BaseTwoPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 2;

        protected long input { get { return param1; } }
        protected long relInput { get { return relParam1; } }

        protected string Format()
        {
            return OpName + ":" +
                (OpModes[0].ToString() + "(" + program.IAtOffset(1) + ")");
        }
    }

    public abstract class BaseThreePlaceOperator : Operator
    {
        public BaseThreePlaceOperator(Program program) : base(program) { }
        public override int OpLength => 3;

        protected long input { get { return param1; } }
        protected long relInput { get { return relParam1; } }

        protected long output { get { return param2; } }
        protected long relOutput { get { return relParam2; } }

        protected string Format()
        {
            return OpName + ":" +
                OpModes[0].ToString() + "(" + program.IAtOffset(1) + ")" +
                " = " +
                param1.ToString() +
                " => ref(" + output + ")";
        }
    }

    public abstract class BaseFourPlaceOperator : Operator
    {
        public BaseFourPlaceOperator(Program program) : base(program) { }
        public override int OpLength => 4;

        protected long input1 { get { return param1; } }
        protected long relInput1 { get { return relParam1; } }

        protected long input2 { get { return param2; } }
        protected long relInput2 { get { return relParam2; } }

        protected long output { get { return param3; } }
        protected long relOutput { get { return relParam3; } }

        protected string Format(string op)
        {
            return OpName + ":" +
                OpModes[0].ToString() + "(" + program.IAtOffset(1) + ")" +
                " " + op + " " +
                OpModes[1].ToString() + "(" + program.IAtOffset(2) + ")" +
                "  = " +
                param1.ToString() +
                " " + op + " " +
                param2.ToString() +
                " => ref(" + output + ")";
        }
    }

    public class In : BaseTwoPlaceOperator
    {
        public In(Program program) : base(program) { }
        public override string OpName => "In";

        public override void Execute()
        {
            string val;
            if (program.inputs.Count == 0)
            {
                program.Blocked = true;
                program.instructionPointer -= OpLength;
            }
            else
            {
                val = program.inputs.Dequeue();
                program.SetAt(relInput, val);
            }
        }

        public override string ToString()
        {
            return "Input " +
                ((program.inputs.Count != 0) ? program.inputs.Peek() : "none") +
                " => " + program.IAtOffset(1);
        }
    }

    public class Out : BaseTwoPlaceOperator
    {
        public Out(Program program) : base(program) { }
        public override string OpName => "Out";

        public override void Execute()
        {
            if (program.Verbose) Console.WriteLine("Output: " + param1);
            program.output.Enqueue(param1.ToString());
        }

        public override string ToString()
        {
            return "Output " + param1;
        }
    }

    public class AdjustRelativeBase : BaseTwoPlaceOperator
    {
        public AdjustRelativeBase(Program program) : base(program) { }
        public override string OpName => "ARB" + FormatParams(OpModes[0]);

        public override void Execute()
        {
            program.relativeBase += param1;
        }

        public override string ToString()
        {
            return Format();
        }
    }

    public class Add : BaseFourPlaceOperator
    {
        public Add(Program program) : base(program) { }
        public override string OpName => "Add" + FormatParams(OpModes[0], OpModes[1]);

        public override void Execute()
        {
            program.ISetAt(relOutput, input1 + input2);
        }

        public override string ToString()
        {
            return Format("+");
        }
    }
    

    public class Multiply : BaseFourPlaceOperator
    {
        public Multiply(Program program) : base(program) { }
        public override string OpName => "Mul" + FormatParams(OpModes[0], OpModes[1]);

        public override void Execute()
        {
            program.ISetAt(relOutput, input1 * input2);
        }

        public override string ToString()
        {
            return Format("*");
        }
    }

    public class JumpIfTrue : BaseThreePlaceOperator
    {
        public JumpIfTrue(Program program) : base(program) { }
        public override string OpName => "JIT" + FormatParams(OpModes[0]);

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
        public override string OpName => "JIF" + FormatParams(OpModes[0]);

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
        public override string OpName => "Les" + FormatParams(OpModes[0], OpModes[1]);

        public override void Execute()
        {
            if (input1 < input2) program.ISetAt(relOutput, 1);
            else program.ISetAt(relOutput, 0);
        }

        public override string ToString()
        {
            return Format("<");
        }
    }

    public class Equal : BaseFourPlaceOperator
    {
        public Equal(Program program) : base(program) { }
        public override string OpName => "Eq" + FormatParams(OpModes[0], OpModes[1]);

        public override void Execute()
        {
            if (input1 == input2) program.ISetAt(relOutput, 1);
            else program.ISetAt(relOutput, 0);
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

    public class NotAnOp : Operator
    {
        public NotAnOp(Program program) : base(program) { }
        public override string OpName => "NOP(" + program.AtPointer() + ")";
        public override int OpLength => 1;

        public override string ToString()
        {
            return OpName;
        }

        public override void Execute()
        {
            throw new NotImplementedException("Cannot execute NotOps");
        }
    }
}
