using Advent2017.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent3
{
    class Tests
    {
        [TestCase("1", 0)]
        [TestCase("12", 3)]
        [TestCase("23", 2)]
        [TestCase("1024", 31)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.FindNum(long.Parse(input)).ManhattanDistance(new Coordinate(0,0)));
        }
    }
}
