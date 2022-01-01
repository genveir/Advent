using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Values
{
    public class Set : ExpressionValue
    {
        private Constant[] _elements;
        public Constant[] Elements
        {
            get => _elements;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Elements on immutable sets");
                _elements = value;
            }
        }

        public Set(Constant[] elements) :base()
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

        public bool IsEquivalentTo(Expression other, bool checkConstraint)
        {
            if (ReferenceEquals(other, this)) return true;

            if (other is InputSet set)
            {
                for (int n = 0; n < Elements.Length; n++) if (!Elements[n].IsEquivalentTo(set.Elements[n], checkConstraint)) return false;
            }

            return true;
        }

        public Set Simplify()
        {
            // equivalences

            // if any constraints are impossible their expressions should be removed from the set
            if (Elements.Any(element => element.Constraint.CannotBeSatisfied()))
            {
                return new Set(Elements.Where(el => !el.Constraint.CannotBeSatisfied()).ToArray()).Simplify();
            }

            // if two or more elements are the same, merge them
            var completelyEquivalent = GetEquivalencesWithConstraints();
            if (completelyEquivalent.Count < Elements.Length)
            {
                Constant[] newElements = new Constant[completelyEquivalent.Count];
                for (int n = 0; n < newElements.Length; n++) newElements[n] = completelyEquivalent[n][0];

                return new Set(newElements).Simplify();
            }

            // merge equivalent elements with different constraints into or-constrained expressions
            var equivalentWithoutConstraints = GetEquivalencesWithoutConstraints();

            if (equivalentWithoutConstraints.Count < Elements.Length)
            {
                Constant[] newElements = new Constant[equivalentWithoutConstraints.Count];
                for (int n = 0; n < newElements.Length; n++)
                {
                    if (equivalentWithoutConstraints[n].Count == 1) newElements[n] = equivalentWithoutConstraints[n][0];
                    else
                    {
                        var newConstraint = new OrConstraint(equivalentWithoutConstraints[n].Select(ex => ex.Constraint)).Simplify();
                        newElements[n] = equivalentWithoutConstraints[n][0].CopyAndSetConstraint(newConstraint);
                    }
                }
                return new Set(newElements).Simplify();
            }

            // we are fully simplified
            return this;
        }

        private List<List<Constant>> _equivalencesWithoutConstraints;
        private List<List<Constant>> GetEquivalencesWithoutConstraints()
        {
            if (_equivalencesWithoutConstraints == null)
            {
                _equivalencesWithoutConstraints = GetEquivalences(false);
            }
            return _equivalencesWithoutConstraints;
        }

        private List<List<Constant>> _equivalencesWithConstraints;
        private List<List<Constant>> GetEquivalencesWithConstraints()
        {
            if (_equivalencesWithConstraints == null)
            {
                _equivalencesWithConstraints = GetEquivalences(true);
            }
            return _equivalencesWithConstraints;
        }

        private List<List<Constant>> GetEquivalences(bool withConstraints)
        {
            var equivalentWithConstraints = new List<List<Constant>>();
            var matched = new bool[Elements.Length];

            for (int n = 0; n < Elements.Length; n++)
            {
                if (matched[n]) continue;
                List<Constant> equivalent = new List<Constant>() { Elements[n] };
                for (int i = n + 1; i < Elements.Length; i++)
                {
                    if (Elements[i].IsEquivalentTo(Elements[n], withConstraints))
                    {
                        equivalent.Add(Elements[i]);
                        matched[i] = true;
                    }
                }
                equivalentWithConstraints.Add(equivalent);
            }

            return equivalentWithConstraints;
        }
    }
}
