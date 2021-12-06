using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Advent2021.Advent06.Solution;

namespace Advent2021.Advent06
{
    class Tests
    {
        [TestCase(example, 18, 26)]
        [TestCase(example, 80, 5934)]
        public void Test1(string input, long days, object output)
        {
            var sol = new Solution(input);
            Solution.days = days;

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 18, 26)]
        [TestCase(example, 80, 5934)]
        [TestCase(example, 256, 26984457539)]
        public void Test2(string input, long days, object output)
        {
            var sol = new Solution(input);
            Solution.days2 = days;

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void WaarGaatHetFoutDan()
        {
            var expected = Input.GetInputLines(output).ToArray();

            var solution = new Solution(example);
            for (int n = 0; n < 18; n++)
            {
                var nextGen = new List<LanternFish>();

                foreach (var fish in solution.fishes)
                {
                    fish.Tick(nextGen);
                }
                solution.fishes.AddRange(nextGen);

                Assert.AreEqual(expected[n], string.Join(',', solution.fishes));
            }
        }

        public const string example = @"3,4,3,1,2";

        public const string output = @"2,3,2,0,1
1,2,1,6,0,8
0,1,0,5,6,7,8
6,0,6,4,5,6,7,8,8
5,6,5,3,4,5,6,7,7,8
4,5,4,2,3,4,5,6,6,7
3,4,3,1,2,3,4,5,5,6
2,3,2,0,1,2,3,4,4,5
1,2,1,6,0,1,2,3,3,4,8
0,1,0,5,6,0,1,2,2,3,7,8
6,0,6,4,5,6,0,1,1,2,6,7,8,8,8
5,6,5,3,4,5,6,0,0,1,5,6,7,7,7,8,8
4,5,4,2,3,4,5,6,6,0,4,5,6,6,6,7,7,8,8
3,4,3,1,2,3,4,5,5,6,3,4,5,5,5,6,6,7,7,8
2,3,2,0,1,2,3,4,4,5,2,3,4,4,4,5,5,6,6,7
1,2,1,6,0,1,2,3,3,4,1,2,3,3,3,4,4,5,5,6,8
0,1,0,5,6,0,1,2,2,3,0,1,2,2,2,3,3,4,4,5,7,8
6,0,6,4,5,6,0,1,1,2,6,0,1,1,1,2,2,3,3,4,6,7,8,8,8,8";
    }
}