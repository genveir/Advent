using Advent2020.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.AdventInfi
{
    class Tests
    {
        [TestCase(1, 5)]
        [TestCase(2, 24)]
        [TestCase(3, 57)]
        [TestCase(4, 104)]
        [TestCase(10, 680)]
        [TestCase(25, 4325)]
        public void OctagonSurfaceIsCorrect(int size, int surface)
        {
            Assert.AreEqual(surface, new Octagon(size).Surface());
        }
    }
}
