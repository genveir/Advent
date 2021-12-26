using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public interface ISet
    {
        public Expression[] Elements { get; }

        bool IsDisjunct(ISet otherSet);

        Expression ApplyRight(Expression baseExpression);

        Expression ApplyLeft(Expression baseExpression);
    }
}
