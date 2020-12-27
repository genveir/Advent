using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.OpCode
{
    public abstract class Condition
    {
        public Func<long>[] parameters;

        public abstract bool Evaluate();
    }

    public class GreaterThan : Condition
    {
        public override bool Evaluate() => parameters[0]() > parameters[1]();
    }

    public class LessThan : Condition
    {
        public override bool Evaluate() => parameters[0]() < parameters[1]();
    }

    public class GreaterEqual : Condition
    {
        public override bool Evaluate() => parameters[0]() >= parameters[1]();
    }

    public class LessEqual : Condition
    {
        public override bool Evaluate() => parameters[0]() <= parameters[1]();
    }

    public class Equal : Condition
    {
        public override bool Evaluate() => parameters[0]() == parameters[1]();
    }

    public class NotEqual : Condition
    {
        public override bool Evaluate() => parameters[0]() != parameters[1]();
    }
}
