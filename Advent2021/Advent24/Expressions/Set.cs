using Advent2021.Advent24.Constraints;
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
            for (int n = 0; n < Elements.Length; n++) builder.Append($"[{Elements[n].Constraint}: {Elements[n].PrintToDepth(depth + 1)}]");

            return builder.ToString();
        }

        public override bool IsEquivalentTo(Expression other, bool checkConstraint)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is InputSet set)
            {
                for (int n = 0; n < Elements.Length; n++) if (!Elements[n].IsEquivalentTo(set.Elements[n], checkConstraint)) return false;
            }

            return true;
        }

        public Expression ApplyLeft(Expression baseExpression)
        {
            var newExpressions = new Expression[Elements.Length];
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
                    if (Elements[my].IsEquivalentTo(otherSet.Elements[their], false)) return false;
                }
            }

            return true;
        }

        public override Expression Simplify()
        {
            // equivalences

            // if there's only one unconstrainted element this expression is equal to that element
            if (Elements.Length == 1 
                && 
                Elements[0].Constraint.IsUnconstrained()) return Elements[0];

            // rewrite to expression if all are the same
            if (Elements.All(m => m.IsEquivalentTo(Elements[0], true))) return Elements[0];

            var equivalentWithoutConstraints = new List<List<Expression>>();
            var matched = new bool[Elements.Length];

            for (int n = 0; n < Elements.Length; n++)
            {
                if (matched[n]) continue;
                List<Expression> equivalent = new List<Expression>() { Elements[n] };
                for (int i = n + 1; i < Elements.Length; i++)
                {
                    if (Elements[i].IsEquivalentTo(Elements[n], false))
                    {
                        equivalent.Add(Elements[i]);
                        matched[i] = true;
                    }
                }
                equivalentWithoutConstraints.Add(equivalent);
            }
            if (equivalentWithoutConstraints.Count < Elements.Length)
            {
                Expression[] newElements = new Expression[equivalentWithoutConstraints.Count];
                for (int n = 0; n < newElements.Length; n++)
                {
                    if (equivalentWithoutConstraints[n].Count == 1) newElements[n] = equivalentWithoutConstraints[n][0];
                    else
                    {
                        var newConstraint = new OrConstraint(equivalentWithoutConstraints[n].Select(ex => ex.Constraint));
                        newElements[n] = equivalentWithoutConstraints[n][0].CopyAndSetConstraint(newConstraint);
                    }
                }
                return new Set(newElements);
            }

            return this;
        }

        public override Expression CopyAndAddConstraint(Constraint constraint)
        {
            var newElements = Elements.Select(e => e.CopyAndAddConstraint(constraint)).ToArray();

            return new Set(newElements);
        }

        public override Expression CopyAndSetConstraint(Constraint constraint)
        {
            var newElements = Elements.Select(e => e.CopyAndSetConstraint(constraint)).ToArray();

            return new Set(newElements);
        }
    }
}
