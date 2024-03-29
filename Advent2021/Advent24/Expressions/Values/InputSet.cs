﻿using Advent2021.Advent24.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Expressions.Values
{
    public class InputSet : Set
    {
        public InputSet(int input) : base(null)
        {
            Mutable = true;

            Elements = new Constant[9];
            for (int n = 0; n < 9; n++)
            {
                Elements[n] = new Constant(n + 1, constraint: new AndConstraint(input, n + 1));
            }

            Mutable = false;
        }
    }
}
