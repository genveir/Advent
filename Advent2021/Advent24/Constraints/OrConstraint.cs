using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Constraints
{
    public class OrConstraint : Constraint
    {
        private IEnumerable<Constraint> Constraints { get; }

        public OrConstraint(IEnumerable<Constraint> constraints)
        {
            Constraints = constraints;
        }

        public override Constraint And(Constraint constraint)
        {
            return new OrConstraint(Constraints.Select(c => c.And(constraint))).Simplify();
        }

        public override bool IsEquivalentTo(Constraint constraint)
        {
            if (constraint is OrConstraint oc)
            {
                foreach(var myConstraint in Constraints)
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

        public override Constraint Simplify()
        {
            if (Constraints.Count() == 0) return Constraint.Impossible();

            // remove all unconstrained elements
            if (Constraints.Any(c => c.IsUnconstrained())) 
                return new OrConstraint(Constraints.Where(c => !c.IsUnconstrained())).Simplify();

            // remove all unsatisfiable elements
            if (Constraints.Any(c => c.CannotBeSatisfied())) 
                return new OrConstraint(Constraints.Where(c => !c.CannotBeSatisfied())).Simplify();

            // a single or is not an or
            if (Constraints.Count() == 1) return Constraints.Single();

            return this;
        }

        public override bool IsUnconstrained()
        {
            return Constraints.Aggregate((a, b) => a.And(b)).IsUnconstrained();
        }

        public override bool CannotBeSatisfied()
        {
            return Constraints.All(c => c.CannotBeSatisfied()) && Constraints.Count() > 0;
        }

        public override string ToString()
        {
            return "[" + string.Join(" or ", Constraints) + "]";
        }
    }
}
