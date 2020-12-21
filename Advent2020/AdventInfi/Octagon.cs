using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.AdventInfi
{
    public class Octagon
    {
        public long Size { get; }

        public Octagon(int size)
        {
            this.Size = size;
        }

        public long Surface()
        {
            var result = 7 * Size * Size - 2 * Size;
            
            return result;
        }

        public long Circumference()
        {
            return 8 * Size;
        }
    }
}