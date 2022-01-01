using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2021.Advent24.Constraints;

namespace Advent2021.Advent24.Expressions.Values
{
    public class SetWithConstant : Set
    {
        public SetWithConstant(int value) : base(new[] { new Constant(value, Constraint.None()) }) { }
    }
}
