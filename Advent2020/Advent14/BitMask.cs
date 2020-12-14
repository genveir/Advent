using Advent2020.Advent14.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14
{
    public class BitMask
    {
        public bool?[] bitMask;

        public BitMask(bool?[] bitMask)
        {
            this.bitMask = bitMask;
        }

        public static BitMask FromString(string mask)
        {
            return new BitMask(mask.Select(c =>
            {
                bool? b = null;
                switch (c)
                {
                    case '0': b = false; break;
                    case '1': b = true; break;
                }
                return b;
            }).ToArray());
        }

        public bool? GetBit(int pointer)
        {
            return bitMask[pointer];
        }

        public MemoryValue Apply(MemoryValue value)
        {
            var result = new bool[Memory.REGISTER_SIZE];

            for (int n = 0; n < Memory.REGISTER_SIZE; n++)
            {
                if (bitMask[n].HasValue) result[n] = bitMask[n].Value;
                else result[n] = value.Value[n];
            }

            return new MemoryValue(result);
        }

        public MemoryPosition Apply(MemoryPosition value)
        {
            var result = new bool?[Memory.REGISTER_SIZE];

            for (int n = 0; n < Memory.REGISTER_SIZE; n++)
            {
                if (bitMask[n].HasValue)
                {
                    if (bitMask[n].Value) result[n] = true;
                    else result[n] = value.GetAction(n).Value;
                }
                else result[n] = null;
            }

            return new MemoryPosition(result);
        }

        public override string ToString()
        {
            return new string(bitMask.Select(b =>
            {
                char c;
                if (b.HasValue)
                {
                    c = b.Value ? '1' : '0';
                }
                else
                {
                    c = 'X';
                }
                return c;
            }).ToArray());
        }
    }
}
