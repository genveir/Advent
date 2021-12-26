using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Eql : Expression
    {
        public Eql(Expression left, Expression right, bool mutable = false, Constraint constraint = null)
            : base(left, right, mutable: mutable, constraint: constraint) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Right is Constant) return new Constant(Left.Value == Right.Value ? 1 : 0, Left.Constraint.And(Right.Constraint));

            if (Left is Set setL && Right is Set setR) return this;

            if (Left is Set) return ((Set)Left).ApplyLeft(new Eql(null, Right, true));
            if (Right is Set) return ((Set)Right).ApplyRight(new Eql(Left, null, true));

            return this;
        }

        public override Expression CopyAndAddConstraint(Constraint constraint) => new Eql(Left, Right, false, Constraint.And(constraint));

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is Eql && Constraint.IsEquivalentTo(other.Constraint))
            {
                return Left.IsEquivalentTo(other.Left) && Right.IsEquivalentTo(other.Right);
            }

            return false;
        }

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] == [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) == ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
