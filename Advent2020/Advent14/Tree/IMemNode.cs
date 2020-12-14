using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public interface IMemNode
    {
        bool ShouldSplit(MemoryPosition memPosition, int memoryPointer);

        SplitNode Split();

        void SetValue(MemoryPosition memPosition, int memoryPointer, MemoryValue value);

        long GetSummedValue(int depth);

        long GetTreeSize();

        IMemNode Copy();
    }
}
