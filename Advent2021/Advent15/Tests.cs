using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent15
{
    class Tests
    {
        [TestCase(example, 40)]
        [TestCase("11", 1)]
        [TestCase("11111", 4)]
        [TestCase(test, 6)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 315)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void RewriteTest()
        {
            var ints = Input.GetInputLines(rewrite).Select(l => l.AsDigits()).ToArray();

            var ex = Input.GetInputLines(expected).Select(l => l.AsDigits()).ToArray();

            var result = Solution.RewriteMap(ints);

            for (int y = 0; y < ex.Length; y++)
            {
                for (int x = 0; x < ex[0].Length; x++)
                {
                    Assert.AreEqual(ex[y][x], result[y][x]);
                }
            }
        }

        public const string rewrite = @"123";

        public const string expected = @"
123234345456567
234345456567678
345456567678789
456567678789891
567678789891912";

        public const string test = @"12222
11111
22221";

        public const string example = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";
    }
}
