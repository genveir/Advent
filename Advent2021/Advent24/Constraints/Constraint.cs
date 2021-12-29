using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Constraints
{
    public abstract class Constraint
    {
        public static AndConstraint None() => AndConstraint.None();

        public abstract Constraint And(Constraint constraint);

        public abstract OrConstraint Or(Constraint constraint);

        public abstract bool IsEquivalentTo(Constraint constraint);

        public abstract bool IsUnconstrained();

        public abstract bool CannotBeSatisfied();

        public abstract Constraint Simplify();
    }
}
