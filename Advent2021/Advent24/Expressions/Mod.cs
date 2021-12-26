using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Mod : Expression
    {
        public Mod(Expression left, Expression right, bool mutable = false, Constraint constraint = null)
            : base(left, right, mutable: mutable, constraint: constraint) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Right is Constant) return new Constant(Left.Value % Right.Value, Left.Constraint.And(Right.Constraint));

            // can't mod by 0, so this will always be / 1
            if (Right is Eql) return Left.CopyAndAddConstraint(Right.Constraint);

            if (Left is Set && Right is Set) return this;
            if (Left is Set) return ((Set)Left).ApplyLeft(new Mod(null, Right, true));
            if (Right is Set) return ((Set)Right).ApplyRight(new Mod(Left, null, true));

            return this;
        }

        public override Expression CopyAndAddConstraint(Constraint constraint) => new Mod(Left, Right, false, Constraint.And(constraint));

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is Mod && Constraint.IsEquivalentTo(other.Constraint))
            {
                return Left.IsEquivalentTo(other.Left) && Right.IsEquivalentTo(other.Right);
            }

            return false;
        }

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] % [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) % ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
