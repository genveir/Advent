using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public class Memory
    {
        public static int REGISTER_SIZE = 36;

        public int chipVersion;
        public BitMask bitMask;
        public IMemNode memRoot;


        public Memory(int chipVersion)
        {
            this.chipVersion = chipVersion;
            this.memRoot = new UnsplitNode();
            this.bitMask = BitMask.FromString("".PadRight(Memory.REGISTER_SIZE, 'X'));
        }

        public void SetMask(BitMask mask)
        {
            this.bitMask = mask;
        }

        public void SetValue(MemoryPosition memPosition, MemoryValue value)
        {
            if (chipVersion == 1) value = bitMask.Apply(value);
            if (chipVersion == 2) memPosition = bitMask.Apply(memPosition);

            if (memRoot.ShouldSplit(memPosition, 0)) memRoot = memRoot.Split();
            memRoot.SetValue(memPosition, 0, value);
        }

        public long GetSummedValue()
        {
            return memRoot.GetSummedValue(0);
        }
    }
}
