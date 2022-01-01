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
                for (int i = 0; i < 9; i++)
                {
                    AllowedValues[n][i] = setAllValuesTo;
                }
            }
        }

        public AndConstraint(int input, int value) : this()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != value - 1) AllowedValues[input][i] = false;
            }
        }

        private bool? _isSimple;
        public override bool IsSimple()
        {
            if (_isSimple == null)
            {
                _isSimple = _IsSimple();
            }
            return _isSimple.Value;
        }

        private bool _IsSimple()
        {
            var byNumTrue = AllowedValues
                .GroupBy(av => av.Count(av => av));

            if (byNumTrue.Count() != 2) return false;

            if (byNumTrue.SingleOrDefault(bnt => bnt.Key == 9)?.Count() != 13) return false;
            if (byNumTrue.SingleOrDefault(bnt => bnt.Key == 1)?.Count() != 1) return false;

            return true;
        }

        private (int input, int value)? _simpleValue;
        public override (int input, int value) SimpleValue()
        {
            if (_simpleValue == null)
            {
                if (!IsSimple()) throw new Exception("tried to get simple value of a not-simple constraint");

                _simpleValue = _SimpleValue();
            }

            return _simpleValue.Value;
        }

        private (int input, int value) _SimpleValue()
        {
            for (int n = 0; n < 14; n++)
            {
                if (AllowedValues[n].All(av => av)) continue;

                for (int i = 0; i < 9; i++)
                {
                    if (AllowedValues[n][i]) return (n, i);
                }
            }
            throw new Exception("an unconstrained constraint is not simple");
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

            if (IsEquivalentTo(newConstraint)) return this; // don't redo caching if all values are the same

            return newConstraint;
        }

        public override bool IsEquivalentTo(Constraint constraint)
        {
            if (constraint is AndConstraint ac) return IsEquivalentTo(ac);
            else return false;
        }

        private bool? _isUnconstrained;
        public override bool IsUnconstrained()
        {
            if (_isUnconstrained == null)
            {
                _isUnconstrained = true;

                for (int n = 0; n < 14; n++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (!AllowedValues[n][i]) _isUnconstrained = false;
                    }
                }
            }
            return _isUnconstrained.Value;
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
