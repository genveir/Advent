using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent14.Tree
{
    public struct MemoryValue
    {
        public bool Initialized;
        public bool[] Value;

        public MemoryValue(bool[] value)
        {
            this.Initialized = true;
            this.Value = value;
        }

        public static MemoryValue FromLong(long value)
        {
            var bools = Convert.ToString(value, 2).PadLeft(Memory.REGISTER_SIZE, '0').Select(c => c == '1').ToArray();

            return new MemoryValue(bools);
        }

        public long ToLong()
        {
            if (!Initialized) return 0;
            else return Convert.ToInt64(new string(Value.Select(b => b ? '1' : '0').ToArray()), 2);
        }

        public override string ToString()
        {
            return ToLong().ToString();
        }
    }
}
