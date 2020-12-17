using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent17
{
    class Tests
    {
        const string sample = @".#.
..#
###";

        [TestCase(sample, 112)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(0, 5)]
        [TestCase(1, 11)]
        [TestCase(2, 21)]
        public void TestSample(int numRuns, int numResult)
        {
            var sol = new Solution(sample);

            sol.SetGrid(3);
            for (int n = 0; n < numRuns; n++)
            {
                sol.RunUpdate();
            }

            Assert.AreEqual(numResult, sol.grid.Front.Get().Count());
        }

        const string test = @".......
.......
.......
...#...
....#..
..###..
.......";

        [TestCase(0, 5)]
        [TestCase(1, 5)]
        public void Test2D(int numRuns, int numResult)
        {
            var sol = new Solution(test);

            sol.SetGrid(2);
            for (int n = 0; n < numRuns; n++)
            {
                sol.RunUpdate();
            }

            Assert.AreEqual(numResult, sol.grid.Front.Get().Count());
        }

        [TestCase("input", 1908)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
