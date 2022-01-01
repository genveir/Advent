using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public abstract class Expression : IPrintable
    {
        public bool Mutable = false;   

        public static long _idCursor = 0;
        public long Id { get; }

        public Expression() 
        { 
            Id = _idCursor++;
        }

        public abstract string PrintToDepth(int depth);

        public override string ToString()
        {
            return PrintToDepth(1);
        }
    }
}
