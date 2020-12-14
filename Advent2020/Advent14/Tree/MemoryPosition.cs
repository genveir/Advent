using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public class MemoryPosition
    {
        public bool?[] Value;

        public MemoryPosition(bool?[] value) { this.Value = value; }

        public static MemoryPosition FromLong(long value)
        {
            var stringRep = Convert.ToString(value, 2);

            return FromString(stringRep);
        }

        public static MemoryPosition FromString(string input)
        {
            var result = input.PadLeft(Memory.REGISTER_SIZE, '0').Select(c =>
            {
                bool? b = null;
                switch (c)
                {
                    case '0': b = false; break;
                    case '1': b = true; break;
                    case 'X': b = null; break;
                }
                return b;
            }).ToArray();

            return new MemoryPosition(result);
        }

        public bool? GetAction(int pointer)
        {
            return Value[pointer];
        }

        public override string ToString()
        {
            return new string(Value.Select(b =>
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
