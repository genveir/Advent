using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Values
{
    public class Constant : IPrintable
    {
        public long Value { get; }
        public Constraint Constraint { get; }

        public Constant(long value, Constraint constraint)
        {
            this.Value = value;
            this.Constraint = constraint;
        }

        public Constant CopyAndAddConstraint(Constraint constraint) => new Constant(Value, Constraint.And(constraint));
        public Constant CopyAndSetConstraint(Constraint constraint) => new Constant(Value, constraint);

        public bool IsEquivalentTo(Constant other, bool checkConstraint)
        {
            if (ReferenceEquals(other, this)) return true;

            if (!checkConstraint || Constraint.IsEquivalentTo(other.Constraint))
            {
                return Value == other.Value;
            }

            return false;
        }

        public string PrintToDepth(int depth) => Value.ToString();
    }
}
