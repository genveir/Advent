using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18
{
    public abstract class Pair
    {
        public TreeNode Parent;

        //public abstract long[] InOrderTraversal();

        public abstract bool DoExplosion(int depth);
        public abstract bool DoSplit();

        public abstract void AddToLeftmost(long value);
        public abstract void AddToRightmost(long value);

        public abstract long Magnitude { get; }
    }
}
