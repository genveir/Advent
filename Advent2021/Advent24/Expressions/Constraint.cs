using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class Constraint
    {
        private Constraint()
        {
            AllowedValues = new bool[14][];
            for (int n = 0; n < 14; n++)
            {
                AllowedValues[n] = new bool[9];
                for (int i = 0; i < 9; i++) AllowedValues[n][i] = true;
            }
        }

        public Constraint(int input, int value) : this()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i != value - 1) AllowedValues[input][i] = false;
            }
        }

        public static Constraint None() => new Constraint();

        public bool[][] AllowedValues { get; }

        public Constraint And(Constraint other)
        {
            var newConstraint = new Constraint();
            for (int n = 0; n < 14; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    newConstraint.AllowedValues[n][i] = AllowedValues[n][i] && other.AllowedValues[n][i];
                }
            }
            return newConstraint;
        }

        public bool IsEquivalentTo(Constraint constraint)
        {
            for (int n = 0; n < 14; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (AllowedValues[n][i] != constraint.AllowedValues[n][i]) return false;
                }
            }

            return true;
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
