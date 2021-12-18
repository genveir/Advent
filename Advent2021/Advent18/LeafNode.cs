using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18
{
    public class LeafNode : Pair
    {
        public long value;

        public override long Magnitude => value;

        public LeafNode(long value)
        {
            this.value = value;
        }

        public LeafNode(long value, TreeNode parent) : this(value)
        {
            this.Parent = parent;
        }

        public override bool DoExplosion(int depth) => false;

        public override string ToString() => value.ToString();

        public override void AddToLeftmost(long value) => this.value += value;


        public override void AddToRightmost(long value) => this.value += value;

        public override bool DoSplit()
        {
            if (value > 9)
            {
                Parent.SplitMe(this);
                return true;
            }
            else return false;

        }

        //public override long[] InOrderTraversal() => new long[] { Value };
    }
}
