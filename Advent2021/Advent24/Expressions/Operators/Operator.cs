using Advent2021.Advent24.Expressions.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Operators
{
    public abstract class Operator : Expression
    {
        public Operator(Set left, Set right) : base()
        {
            Mutable = true;

            Left = left;
            Right = right;

            Mutable = false;
        }

        public Set Apply()
        {
            var newExpressions = new List<Constant>();

            foreach (var left in Left.Elements)
            {
                foreach (var right in Right.Elements)
                {
                    newExpressions.Add(ApplyToElement(left, right));
                }
            }

            return new Set(newExpressions.ToArray()).Simplify();
        }

        public abstract Constant ApplyToElement(Constant left, Constant right);

        private Set _left;
        public Set Left
        {
            get => _left;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Left on immutable operators");
                _left = value;
            }
        }

        private Set _right;
        public Set Right
        {
            get => _right;
            set
            {
                if (!Mutable) throw new ImmutableObjectException("can't set Right on immutable operators");
                _right = value;
            }
        }
    }
}
