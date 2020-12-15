using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent15
{
    class Tests
    {
        [TestCase("0,3,6", 436)]
        [TestCase("1,3,2", 1)]
        [TestCase("2,1,3", 10)]
        [TestCase("1,2,3", 27)]
        [TestCase("2,3,1", 78)]
        [TestCase("3,2,1", 438)]
        [TestCase("3,1,2", 1836)]
        [TestCase("0,8,15,2,12,1,4", 289)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("0,3,6", 175594)]
        [TestCase("1,3,2", 2578)]
        [TestCase("2,1,3", 3544142)]
        [TestCase("1,2,3", 261214)]
        [TestCase("2,3,1", 6895259)]
        [TestCase("3,2,1", 18)]
        [TestCase("3,1,2", 362)]
        [TestCase("0,8,15,2,12,1,4", 1505722)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
