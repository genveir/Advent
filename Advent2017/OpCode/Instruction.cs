using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.OpCode
{
    public abstract class Instruction
    {
        public Register register;
        public Func<long>[] parameters;
        public Condition condition;

        public abstract void Execute();
    }

    public class Inc : Instruction
    {
        public override void Execute()
        {
            if (condition.Evaluate()) register.Value += parameters[0]();
        }
    }

    public class Dec : Instruction
    {
        public override void Execute()
        {
            if (condition.Evaluate()) register.Value -= parameters[0]();
        }
    }
}
