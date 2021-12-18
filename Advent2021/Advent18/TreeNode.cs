using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18
{
    public class TreeNode : Pair
    {
        public Pair left;
        public Pair right;

        public override long Magnitude => 3 * left.Magnitude + 2 * right.Magnitude;

        public TreeNode(Pair left, Pair right)
        {
            this.left = left;
            this.right = right;

            left.Parent = this;
            right.Parent = this;
        }

        public TreeNode Clone()
        {
            Pair newLeft, newRight;

            if (left is TreeNode) newLeft = ((TreeNode)left).Clone();
            else newLeft = new LeafNode(((LeafNode)left).value);

            if (right is TreeNode) newRight = ((TreeNode)right).Clone();
            else newRight = new LeafNode(((LeafNode)right).value);

            return new TreeNode(newLeft, newRight);
        }

        public override bool DoExplosion(int depth)
        {
            if (left.DoExplosion(depth + 1)) return true;
            if (right.DoExplosion(depth + 1)) return true;

            if (left is LeafNode && right is LeafNode && depth >= 4)
            {
                var lLeaf = (LeafNode)left;
                var rLeaf = (LeafNode)right;

                Parent.AddNumberToTheLeft(this, lLeaf.value);
                Parent.AddNumberToTheRight(this, rLeaf.value);

                Parent.SetMeToZero(this);

                return true;
            }

            return false;
        }

        public override bool DoSplit()
        {
            if (left.DoSplit()) return true;
            if (right.DoSplit()) return true;

            return false;
        }

        //long[] leftTraverse;
        //long[] rightTraverse;
        //public override long[] InOrderTraversal()
        //{
        //    leftTraverse = left.InOrderTraversal();
        //    rightTraverse = right.InOrderTraversal();

        //    return leftTraverse.Concat(rightTraverse).ToArray();
        //}

        public void AddNumberToTheLeft(Pair asker, long value)
        {
            if (left == asker)
            {
                if (Parent != null) Parent.AddNumberToTheLeft(this, value);
            }
            else left.AddToRightmost(value);
        }

        public void AddNumberToTheRight(Pair asker, long value)
        {
            if (right == asker)
            {
                if (Parent != null) Parent.AddNumberToTheRight(this, value);
            }
            else right.AddToLeftmost(value);
        }

        public override void AddToLeftmost(long value) => left.AddToLeftmost(value);
        public override void AddToRightmost(long value) => right.AddToRightmost(value);

        public void SetMeToZero(Pair pair)
        {
            if (pair == left) left = new LeafNode(0, this);
            else if (pair == right) right = new LeafNode(0, this);
            else throw new InvalidOperationException("SetMeToZero called by non-child");
        }

        public void SplitMe(LeafNode pair)
        {
            var value = pair.value;

            var oneIfOdd = value & 1;

            var half = value / 2;
            var leftVal = half;
            var rightVal = half + oneIfOdd;

            var newNode = new TreeNode(
                new LeafNode(leftVal), new LeafNode(rightVal));
            newNode.Parent = this;

            if (pair == left) left = newNode;
            else if (pair == right) right = newNode;
            else throw new InvalidOperationException("SplitMe called by non-child");
        }

        public override string ToString()
        {
            return $"[{left},{right}]";
        }
    }
}
