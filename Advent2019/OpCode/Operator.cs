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

        public Operator(Program program)
        {
            this.program = program;
        }

        public static Dictionary<long, Func<Program, Operator>> _operators = new Dictionary<long, Func<Program, Operator>>();
        public static Operator Get(Program program, long position)
        {
            var instruction = program.GetAt(position);

            if (!_operators.ContainsKey(instruction))
            {
                OpMode[] opMode = new OpMode[3] { OpMode.Pos, OpMode.Pos, OpMode.Pos };

                int counter = 0;
                for (int spot = 100; spot < 100000; spot *= 10)
                {
                    var digit = (instruction / spot) % 10;
                    switch (digit)
                    {
                        case 0: opMode[counter++] = OpMode.Pos; break;
                        case 1: opMode[counter++] = OpMode.Int; break;
                        case 2: opMode[counter++] = OpMode.Rel; break;
                    }
                }

                switch (instruction % 100)
                {
                    case 1: _operators[instruction] = (p) => { var op = new Add(p); op.OpModes = opMode; return op; }; break;
                    case 2: _operators[instruction] = (p) => { var op = new Multiply(p); op.OpModes = opMode; return op; }; break;
                    case 3: _operators[instruction] = (p) => { var op = new In(p); op.OpModes = opMode; return op; }; break;
                    case 4: _operators[instruction] = (p) => { var op = new Out(p); op.OpModes = opMode; return op; }; break;
                    case 5: _operators[instruction] = (p) => { var op = new JumpIfTrue(p); op.OpModes = opMode; return op; }; break;
                    case 6: _operators[instruction] = (p) => { var op = new JumpIfFalse(p); op.OpModes = opMode; return op; }; break;
                    case 7: _operators[instruction] = (p) => { var op = new Less(p); op.OpModes = opMode; return op; }; break;
                    case 8: _operators[instruction] = (p) => { var op = new Equal(p); op.OpModes = opMode; return op; }; break;
                    case 9: _operators[instruction] = (p) => { var op = new AdjustRelativeBase(p); op.OpModes = opMode; return op; }; break;
                    case 99: _operators[instruction] = (p) => { var op = new Stop(p); op.OpModes = opMode; return op; }; break;

                    default: _operators[instruction] = (p) => { var op = new NotAnOp(p); op.OpModes = opMode; return op; }; break;
                }
            }

            return _operators[instruction](program);
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
                    case OpMode.Pos: return program.AtOffset(paramIndex + 1);
                    case OpMode.Int: throw new NotImplementedException();
                    case OpMode.Rel:
                        long relBase = program.relativeBase;
                        long add = program.AtOffset(paramIndex + 1);
                        return relBase + add;
                    default: throw new Exception("user your enum Geerten");
                }
            }
            else
            {
                switch (OpModes[paramIndex])
                {
                    case OpMode.Pos: return program.GetAt(program.AtOffset(paramIndex + 1));
                    case OpMode.Int: return program.AtOffset(paramIndex + 1);
                    case OpMode.Rel:
                        long relBase = program.relativeBase;
                        long add = program.AtOffset(paramIndex + 1);
                        return program.GetAt(relBase + add);
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
                (OpModes[0].ToString() + "(" + program.AtOffset(1) + ")");
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
                OpModes[0].ToString() + "(" + program.AtOffset(1) + ")" +
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
                OpModes[0].ToString() + "(" + program.AtOffset(1) + ")" +
                " " + op + " " +
                OpModes[1].ToString() + "(" + program.AtOffset(2) + ")" +
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
            long val;
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
                ((program.inputs.Count != 0) ? program.inputs.Peek().ToString() : "none") +
                " => " + program.AtOffset(1);
        }
    }

    public class Out : BaseTwoPlaceOperator
    {
        public Out(Program program) : base(program) { }
        public override string OpName => "Out";

        public override void Execute()
        {
            if (program.Verbose) Console.WriteLine("Output: " + param1);
            program.output.Enqueue(param1);
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
            program.SetAt(relOutput, input1 + input2);
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
            program.SetAt(relOutput, input1 * input2);
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
            if (input1 < input2) program.SetAt(relOutput, 1);
            else program.SetAt(relOutput, 0);
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
            if (input1 == input2) program.SetAt(relOutput, 1);
            else program.SetAt(relOutput, 0);
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
