using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Values
{
    public abstract class ExpressionValue : Expression
    {
        private Constraint _constraint;
        public Constraint Constraint
        {
            get => _constraint;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Constraint on immutable expressions");
                _constraint = value;
            }
        }

        private long? _value;
        public long? Value
        {
            get => _value;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Value on immutable expressions");
                _value = value;
            }
        }
    }
}
