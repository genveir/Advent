using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Constant : Expression
    {

        public Constant(long? value, Constraint constraint = null) : base(null, null, value, constraint: constraint) { }

        public override Expression Simplify() => this;

        public override long UniqueSimplifyableExpressionCount() => 0;

        public override Expression CopyAndAddConstraint(Constraint constraint) => new Constant(Value, Constraint.And(constraint));
        public override Expression CopyAndSetConstraint(Constraint constraint) => new Constant(Value, constraint);

        public override bool IsEquivalentTo(Expression other, bool checkConstraint)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is Constant)
            {
                if (!checkConstraint || Constraint.IsEquivalentTo(other.Constraint))
                {
                    return Value == other.Value;
                }
            }

            return false;
        }

        public override string PrintToDepth(int depth) => Value.ToString();
    }
}
