using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions
{
    public class InputSet : Set
    {
        public override bool IsSet => true;

        public InputSet(int input) : base(null)
        {
            Mutable = true;

            Elements = new Expression[9];
            for (int n = 0; n < 9; n++)
            {
                Elements[n] = new Constant(n + 1, constraint: new Constraint(input, n + 1));
            }

            Mutable = false;
        }
    }
}
