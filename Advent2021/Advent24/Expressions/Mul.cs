using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Mul : Expression
    {
        public Mul(Expression left, Expression right, bool mutable = false, Constraint constraint = null)
            : base(left, right, mutable: mutable, constraint: constraint) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Left.Value == 0) return Left.CopyAndAddConstraint(Right.Constraint);
            if (Right is Constant && Right.Value == 0) return Right.CopyAndAddConstraint(Left.Constraint);

            if (Left is Constant && Left.Value == 1) return Right.CopyAndAddConstraint(Left.Constraint);
            if (Right is Constant && Right.Value == 1) return Left.CopyAndAddConstraint(Right.Constraint);

            if (Left is Constant && Right is Constant) return new Constant(Left.Value * Right.Value, Left.Constraint.And(Right.Constraint));

            if (Left is Set && Right is Set) return this;
            if (Left is Set) return ((Set)Left).ApplyLeft(new Mul(null, Right, true));
            if (Right is Set) return ((Set)Right).ApplyRight(new Mul(Left, null, true));

            return this;
        }

        public override Expression CopyAndAddConstraint(Constraint constraint) => new Mul(Left, Right, false, Constraint.And(constraint));

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is Mul && Constraint.IsEquivalentTo(other.Constraint))
            {
                return Left.IsEquivalentTo(other.Left) && Right.IsEquivalentTo(other.Right);
            }

            return false;
        }

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] * [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) * ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
