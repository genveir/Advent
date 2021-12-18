using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18
{
    public class WholeValue
    {
        public TreeNode pair;

        public WholeValue(TreeNode pair, bool reduce = false)
        {
            this.pair = pair;

            if (reduce) while (Reduce()) { }
        }

        public static WholeValue Add(WholeValue left, WholeValue right)
        {
            var pair = new TreeNode(left.pair.Clone(), right.pair.Clone());

            return new WholeValue(pair, true);
        }

        public long Magnitude => pair.Magnitude;

        /// <summary>
        /// Reduces a tree
        /// </summary>
        /// <returns>true if it did a reduction step</returns>
        public bool Reduce()
        {
            var didExplosion = pair.DoExplosion(0);
            if (didExplosion) return true;

            var didSplit = pair.DoSplit();
            if (didSplit) return true;

            return false;
        }

        public override string ToString()
        {
            return pair.ToString();
        }
    }
}
