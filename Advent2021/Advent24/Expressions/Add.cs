using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Add : Expression
    {
        public Add(Expression left, Expression right, bool mutable = false, Constraint constraint = null) 
            : base(left, right, mutable: mutable, constraint: constraint) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Left.Value == 0) return Right.CopyAndAddConstraint(Left.Constraint);
            if (Right is Constant && Right.Value == 0) return Left.CopyAndAddConstraint(Right.Constraint);

            if (Left is Constant && Right is Constant) return new Constant(Left.Value + Right.Value, Left.Constraint.And(Right.Constraint));

            if (Left is Set && Right is Set) return this;
            if (Left is Set) return ((Set)Left).ApplyLeft(new Add(null, Right, true));
            if (Right is Set) return ((Set)Right).ApplyRight(new Add(Left, null, true));

            return this;
        }

        public override Expression CopyAndAddConstraint(Constraint constraint) => new Add(Left, Right, false, Constraint.And(constraint));
        public override Expression CopyAndSetConstraint(Constraint constraint) => new Add(Left, Right, false, constraint);

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] + [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) + ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
