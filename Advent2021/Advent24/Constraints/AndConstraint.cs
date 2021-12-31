using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Constraints
{
    public class AndConstraint : Constraint
    {
        private AndConstraint(bool setAllValuesTo = true)
        {
            AllowedValues = new bool[14][];
            for (int n = 0; n < 14; n++)
            {
                AllowedValues[n] = new bool[9];
                for (int i = 0; i < 9; i++) AllowedValues[n][i] = true;
            }
        }

        public AndConstraint(int input, int value) : this()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != value - 1) AllowedValues[input][i] = false;
            }
        }

        public new static AndConstraint None() => new AndConstraint(true);
        public new static AndConstraint Impossible() => new AndConstraint(false);


        public bool[][] AllowedValues { get; }

        public override Constraint And(Constraint constraint)
        {
            if (constraint is AndConstraint ac) return And(ac).Simplify();
            else return constraint.And(this).Simplify();
        }

        public override Constraint Or(Constraint constraint)
        {
            return new OrConstraint(new[] { constraint, this }).Simplify();
        }

        public AndConstraint And(AndConstraint other)
        {
            var newConstraint = new AndConstraint();
            for (int n = 0; n < 14; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    newConstraint.AllowedValues[n][i] = AllowedValues[n][i] && other.AllowedValues[n][i];
                }
            }
            return newConstraint;
        }

        public override bool IsEquivalentTo(Constraint constraint)
        {
            if (constraint is AndConstraint ac) return IsEquivalentTo(ac);
            else return false;
        }

        public override bool IsUnconstrained()
        {
            for (int n = 0; n < 14; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (!AllowedValues[n][i]) return false;
                }
            }

            return true;
        }

        public override bool CannotBeSatisfied()
        {
            for (int n = 0; n < 14; n++)
            {
                if (AllowedValues[n].All(a => !a)) return true;
            }
            return false;
        }

        public bool IsEquivalentTo(AndConstraint constraint)
        {
            if (ReferenceEquals(this, constraint)) return true;

            for (int n = 0; n < 14; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (AllowedValues[n][i] != constraint.AllowedValues[n][i]) return false;
                }
            }

            return true;
        }

        public override Constraint Simplify()
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int n = 0; n < 14; n++)
            {
                if (AllowedValues[n].All(a => a)) continue;
                for (int i = 0; i < 9; i++)
                {
                    if (AllowedValues[n][i]) result.Append($"[{n}.{i + 1}]");
                }
            }
            return result.ToString();
        }
    }
}
