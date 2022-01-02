using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Constraints
{
    public class OrConstraint : Constraint
    {
        private Constraint[] Constraints { get; }

        public OrConstraint(IEnumerable<Constraint> constraints)
        {
            Constraints = constraints.ToArray();
        }

        public override Constraint And(Constraint constraint)
        {
            return new OrConstraint(Constraints.Select(c => c.And(constraint))).Simplify();
        }

        public override bool IsEquivalentTo(Constraint constraint)
        {
            if (constraint is OrConstraint oc)
            {
                foreach (var myConstraint in Constraints)
                {
                    if (!oc.Constraints.Any(c => c.IsEquivalentTo(myConstraint))) return false;
                }
                return true;
            }
            return false;
        }

        public override Constraint Or(Constraint constraint)
        {
            return new OrConstraint(Constraints.Append(constraint)).Simplify();
        }

        private Constraint _simplified;
        public override Constraint Simplify()
        {
            if (_simplified == null)
            {
                _simplified = _Simplify();
            }
            return _simplified;
        }

        private Constraint _Simplify()
        {
            if (Constraints.Count() == 0) return Impossible();

            // merge all equivalent constraints
            var mergedUnderEquivalence = MergeUnderEquivalence(Constraints);
            if (mergedUnderEquivalence.Length != Constraints.Length)
            {
                return new OrConstraint(mergedUnderEquivalence).Simplify();
            }

            // a single or is not an or
            if (Constraints.Count() == 1) return Constraints.Single();

            // remove all unconstrained elements
            if (Constraints.Any(c => c.IsUnconstrained()))
                return new OrConstraint(Constraints.Where(c => !c.IsUnconstrained())).Simplify();

            // remove all unsatisfiable elements
            if (Constraints.Any(c => c.CannotBeSatisfied()))
                return new OrConstraint(Constraints.Where(c => !c.CannotBeSatisfied())).Simplify();

            // an or constraint with all simple values for an input is not constrained over that input

            var shouldBeRemoved = Constraints
                    .Where(c => c.IsSimple())
                    .GroupBy(c => c.SimpleValue().input)
                    .Where(g => g.Count() == 9)
                    .Select(g => g.Key);

            if (shouldBeRemoved.Count() > 0)
            {
                IEnumerable<Constraint> newConstraints = Constraints.ToList();
                foreach (var removal in shouldBeRemoved)
                {
                    var toBeRewritten = Constraints
                            .Where(c => c.IsSimple())
                            .Where(c => c.SimpleValue().input == removal);

                    // rewrite to the unconstrained constraint (because if we just remove it, we're claiming
                    // it's impossible to satisfy these constraints instead of saying it's always true
                    newConstraints = newConstraints.Except(toBeRewritten).Append(Constraint.None());
                }

                return new OrConstraint(newConstraints).Simplify();
            }

            return this;
        }

        private Constraint[] _mergedUnderEquivalence;
        private Constraint[] MergeUnderEquivalence(IEnumerable<Constraint> constraints)
        {
            if (_mergedUnderEquivalence == null)
            {
                var constraintArray = constraints.ToArray();
                var newConstraints = new List<Constraint>();

                bool[] matched = new bool[constraintArray.Length];
                for (int n = 0; n < constraintArray.Length; n++)
                {
                    if (matched[n]) continue;
                    else newConstraints.Add(constraintArray[n]);

                    for (int i = n; i < constraintArray.Length; i++)
                    {
                        if (constraintArray[n].IsEquivalentTo(constraintArray[i]))
                        {
                            matched[i] = true;
                        }
                    }
                }

                _mergedUnderEquivalence = newConstraints.ToArray();
            }
            return _mergedUnderEquivalence;
        }

        public override bool IsSimple()
        {
            return Constraints.Length == 1 && Constraints.Single().IsSimple();
        }

        public override (int input, int value) SimpleValue()
        {
            return Constraints.Single().SimpleValue();
        }

        public override bool IsUnconstrained()
        {
            // not complete, but at least correct
            return Constraints.Any(c => c.IsUnconstrained());
        }

        public override bool CannotBeSatisfied()
        {
            return Constraints.All(c => c.CannotBeSatisfied()) && Constraints.Count() > 0;
        }

        public override string ToString()
        {
            return "[" + string.Join<Constraint>(" or ", Constraints) + "]";
        }
    }
}
