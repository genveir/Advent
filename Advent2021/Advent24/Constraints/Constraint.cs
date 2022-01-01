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

        public static AndConstraint Impossible() => AndConstraint.Impossible();

        public abstract Constraint And(Constraint constraint);

        public abstract Constraint Or(Constraint constraint);

        public abstract bool IsEquivalentTo(Constraint constraint);

        public abstract bool IsUnconstrained();

        public abstract bool CannotBeSatisfied();

        /// <summary>
        /// does this constrain one input to one value
        /// </summary>
        /// <returns></returns>
        public abstract bool IsSimple();
        public abstract (int input, int value) SimpleValue();

        public abstract Constraint Simplify();
    }
}
