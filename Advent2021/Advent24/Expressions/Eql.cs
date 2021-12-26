using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Eql : Expression
    {
        public Eql(Expression left, Expression right, bool mutable = false) : base(left, right, mutable: mutable) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Right is Constant) return new Constant(Left.Value == Right.Value ? 1 : 0);

            if (Left is ISet setL && Right is ISet setR)
            {
                if (setL.IsDisjunct(setR)) return new Constant(0);
                else return this;
            }

            if (Left is ISet) return ((ISet)Left).ApplyLeft(new Eql(null, Right, true));
            if (Right is ISet) return ((ISet)Right).ApplyRight(new Eql(Left, null, true));

            return this;
        }

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is Eql)
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
