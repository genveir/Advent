using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Set : Expression
    {
        private Expression[] _elements;
        public Expression[] Elements
        {
            get => _elements;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Elements on immutable sets");
                _elements = value;
            }
        }

        public Set(Expression[] elements) : base(null, null) 
        {
            Mutable = true;

            Elements = elements;

            Mutable = false;
        }

        public override string PrintToDepth(int depth)
        {
            StringBuilder builder = new StringBuilder($"SET ");
            for (int n = 0; n < 9; n++) builder.Append($"[{Elements[n].Constraint}: {Elements[n].PrintToDepth(depth + 1)}]");

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
                newExpressions[n] = baseExpression.CopyAndAddConstraint(Elements[n].Constraint).Simplify();
            }
            return new Set(newExpressions).Simplify();
        }

        public Expression ApplyRight(Expression baseExpression)
        {
            var newExpressions = new Expression[Elements.Length];
            for (int n = 0; n < Elements.Length; n++)
            {
                baseExpression.Right = Elements[n];
                newExpressions[n] = baseExpression.CopyAndAddConstraint(Elements[n].Constraint).Simplify();
            }
            return new Set(newExpressions).Simplify();
        }

        public bool IsDisjunct(Set otherSet)
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

        public override Expression CopyAndAddConstraint(Constraint constraint)
        {
            var newElements = Elements.Select(e => e.CopyAndAddConstraint(constraint)).ToArray();

            return new Set(newElements);
        }
    }
}
