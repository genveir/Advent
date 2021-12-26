using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class InputSet : Expression, ISet
    {
        public Constant input;
        public Expression[] Elements { get; }

        public override bool IsSet => true;

        public InputSet(Constant input, Expression[] members) : base(null, null)
        {
            if (members.Length != 9) throw new ArgumentException("input group must have 9 expressions as input");

            this.input = input;
            this.Elements = members;
        }

        public override string PrintToDepth(int depth)
        {
            StringBuilder builder = new StringBuilder($"INPUT_{input.Value}_SET ");
            for (int n = 0; n < 9; n++) builder.Append($"[{n + 1}: {Elements[n].PrintToDepth(depth + 1)}]");

            return builder.ToString();
        }

        public override bool IsEquivalentTo(Expression other)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is InputSet set)
            {
                for (int n = 0; n < 9; n++) if (!Elements[n].IsEquivalentTo(set.Elements[n])) return false;
            }

            return true;
        }

        public Expression ApplyLeft(Expression baseExpression)
        {
            var newExpressions = new Expression[9];
            for (int n = 0; n < Elements.Length; n++)
            {
                baseExpression.Left = Elements[n];
                newExpressions[n] = baseExpression.Simplify();
            }
            return new InputSet(input, newExpressions).Simplify();
        }

        public Expression ApplyRight(Expression baseExpression)
        {
            var newExpressions = new Expression[Elements.Length];
            for (int n = 0; n < Elements.Length; n++)
            {
                baseExpression.Right = Elements[n];
                newExpressions[n] = baseExpression.Simplify();
            }
            return new InputSet(input, newExpressions).Simplify();
        }

        public bool IsDisjunct(ISet otherSet)
        {
            for (int my = 0; my < Elements.Length; my++)
            {
                for (int their = 0; their < otherSet.Elements.Length; their++)
                {
                    if (Elements[my].IsEquivalentTo(otherSet.Elements[their])) return false;
                }
            }

            return true;
        }

        public override Expression Simplify()
        {
            // equivalences

            // most basic equivalence => rewrite to expression if all are the same
            if (Elements.All(m => m.IsEquivalentTo(Elements[0]))) return Elements[0];

            return this;
        }
    }
}
