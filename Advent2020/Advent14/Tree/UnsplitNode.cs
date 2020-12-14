using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public class UnsplitNode : IMemNode
    {
        public MemoryValue Value;
        public IMemNode Child;

        public UnsplitNode()
        {
            
        }

        public bool ShouldSplit(MemoryPosition memoryPosition, int memoryPointer) 
        {
            if (memoryPointer == Memory.REGISTER_SIZE) return false;

            if (memoryPosition.GetAction(memoryPointer) != null) return true;
            return false;
        }

        public SplitNode Split()
        {
            return new SplitNode()
            {
                Zero = Child ?? new UnsplitNode(),
                One = Child?.Copy() ?? new UnsplitNode()
            };
        }

        public void SetValue(MemoryPosition memoryPosition, int memoryPointer, MemoryValue value)
        {
            if (memoryPointer == Memory.REGISTER_SIZE) Value = value;
            else
            {
                if (Child == null) Child = new UnsplitNode();

                if (Child.ShouldSplit(memoryPosition, memoryPointer + 1)) Child = Child.Split();
                Child.SetValue(memoryPosition, memoryPointer + 1, value);
            }
        }

        public long GetSummedValue(int depth)
        {
            var depthMultiplier = (long)Math.Pow(2, Memory.REGISTER_SIZE - depth);

            if (Child == null) return depthMultiplier * Value.ToLong();
            else return 2 * Child.GetSummedValue(depth + 1);
        }

        public long GetTreeSize()
        {
            return 1 + Child?.GetTreeSize() ?? 0;
        }

        public IMemNode Copy()
        {
            return new UnsplitNode()
            {
                Value = Value,
                Child = Child?.Copy()
            };
        }

        public override string ToString()
        {
            if (Child != null) return $"({Child})";
            else return $"[{Value.ToLong()}]";
        }
    }
}
