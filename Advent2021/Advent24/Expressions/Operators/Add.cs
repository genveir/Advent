using Advent2021.Advent24.Constraints;
using Advent2021.Advent24.Expressions.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Operators
{
    public class Add : Operator
    {
        public Add(Set left, Set right) : base(left, right) { }

        public override Constant ApplyToElement(Constant left, Constant right) =>
            new Constant(left.Value + right.Value, left.Constraint.And(right.Constraint));

        public override string PrintToDepth(int depth)
        {
            if (depth == 0) return "[...] + [...]";
            else return $"({Left.PrintToDepth(depth - 1)}) + ({Right.PrintToDepth(depth - 1)})";
        }
    }
}
