using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Add : Expression
    {
        public Add(Expression left, Expression right, bool mutable = false) : base(left, right, mutable: mutable) { }

        public override Expression Simplify()
        {
            if (Left is Constant && Left.Value == 0) return Right;
            if (Right is Constant && Right.Value == 0) return Left;

            if (Left is Constant && Right is Constant) return new Constant(Left.Value + Right.Value);

            if (Left is ISet && Right is ISet) return this;
            if (Left is ISet) return ((ISet)Left).ApplyLeft(new Add(null, Right, true));
            if (Right is ISet) return ((ISet)Right).ApplyRight(new Add(Left, null, true));

            return this;
        }

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;
            
            if (other is Add)
            {
                return Left.IsEquivalentTo(other.Left) && Right.IsEquivalentTo(other.Right);
            }

            return false;
        }

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] + [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) + ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
