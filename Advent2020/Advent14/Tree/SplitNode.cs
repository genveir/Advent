using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public class SplitNode : IMemNode
    {
        public IMemNode Zero;
        public IMemNode One;

        public SplitNode()
        {
            Zero = new UnsplitNode();
            One = new UnsplitNode();
        }

        public bool ShouldSplit(MemoryPosition memoryPosition, int memoryPointer) => false;

        public SplitNode Split() => this;

        public void SetValue(MemoryPosition memoryPosition, int memoryPointer, MemoryValue value)
        {
            // splitnode should never be a leaf

            bool? memoryAction = memoryPosition.GetAction(memoryPointer);

            bool toZero, toOne;
            if (memoryAction.HasValue)
            {
                toOne = memoryAction.Value;
                toZero = !toOne;
            }
            else
            {
                toOne = true;
                toZero = true;
            }

            if (toZero)
            {
                if (Zero.ShouldSplit(memoryPosition, memoryPointer + 1)) Zero = Zero.Split();
                Zero.SetValue(memoryPosition, memoryPointer + 1, value);
            }
            if (toOne)
            {
                if (One.ShouldSplit(memoryPosition, memoryPointer + 1)) One = One.Split();
                One.SetValue(memoryPosition, memoryPointer + 1, value);
            }
        }

        public long GetSummedValue(int depth)
        {
            return Zero.GetSummedValue(depth + 1) + One.GetSummedValue(depth + 1);
        }

        public long GetTreeSize()
        {
            return 1 + Zero.GetTreeSize() + One.GetTreeSize();
        }

        public IMemNode Copy()
        {
            return new SplitNode()
            {
                Zero = Zero.Copy(),
                One = One.Copy()
            };
        }

        public override string ToString()
        {
            return $"({Zero}, {One}";
        }
    }
}
