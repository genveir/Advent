using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent25
{
    class Constellation
    {
        public List<Star> Stars;

        public Constellation()
        {
            Stars = new List<Star>();
        }

        public Constellation Join(Constellation other)
        {
            other.Stars.AddRange(Stars);

            return other;
        }
    }
}
