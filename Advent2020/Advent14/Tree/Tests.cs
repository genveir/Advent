using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent2020.Advent14.Solution;

namespace Advent2020.Advent14.Tree
{
    public class Tests
    {
        [Test]
        public void AllXesGives36Nodes()
        {
            var mask = BitMask.FromString("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            var memPos = MemoryPosition.FromString("111111111111111111111111111111111111");

            memPos = mask.Apply(memPos);

            var topNode = new UnsplitNode();

            topNode.SetValue(memPos, 0, MemoryValue.FromLong(1));

            Assert.AreEqual(37, topNode.GetTreeSize());
        }

        [Test]
        public void SmallerTest()
        {
            Memory.REGISTER_SIZE = 6;

            var mem = new Memory(2);

            var bitMask = BitMask.FromString("X1001X");
            var memPos = MemoryPosition.FromLong(42);
            var memVal = MemoryValue.FromLong(100);

            mem.SetMask(bitMask);
            mem.SetValue(memPos, memVal);

            Assert.AreEqual(400, mem.GetSummedValue());
        }
    }
}
