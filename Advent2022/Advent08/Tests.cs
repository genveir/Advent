using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent08
{
    class Tests
    {
        [TestCase(example, 21)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 8)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [TestCase(example, 0, 0, 2)]
        [TestCase(example, 4, 0, 0)]
        [TestCase(example, 2, 3, 2)]
        [TestCase(example, 3, 2, 1)]
        [TestCase(toTheRight, 0, 0, 1)]
        public void TestRight(string input, int x, int y, long expected)
        {
            var sol = new Solution(input);

            var grid = new Solution.Grid();
            var result = grid.GetScenicRight(sol.trees, y, x);

            Assert.AreEqual(expected, result);
        }

        [TestCase(example, 0, 0, 0)]
        [TestCase(example, 4, 0, 1)]
        [TestCase(example, 2, 3, 2)]
        [TestCase(example, 1, 1, 1)]
        [TestCase(toTheRight, 9, 0, 9)]
        public void TestLeft(string input, int x, int y, long expected)
        {
            var sol = new Solution(input);

            var grid = new Solution.Grid();
            var result = grid.GetScenicLeft(sol.trees, y, x);

            Assert.AreEqual(expected, result);
        }

        [TestCase(example, 0, 0, 0)]
        [TestCase(example, 0, 4, 1)]
        [TestCase(example, 2, 3, 2)]
        [TestCase(example, 1, 1, 1)]
        public void TestTop(string input, int x, int y, long expected)
        {
            var sol = new Solution(input);

            var grid = new Solution.Grid();
            var result = grid.GetScenicTop(sol.trees, y, x);

            Assert.AreEqual(expected, result);
        }

        [TestCase(example, 0, 0, 2)]
        [TestCase(example, 0, 4, 0)]
        [TestCase(example, 2, 3, 1)]
        [TestCase(example, 4, 0, 3)]
        public void TestBottom(string input, int x, int y, long expected)
        {
            var sol = new Solution(input);

            var grid = new Solution.Grid();
            var result = grid.GetScenicBottom(sol.trees, y, x);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestAll()
        {
            var sol = new Solution(example);

            var grid = new Solution.Grid();
            StringBuilder builder = new("");
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    var scenic = grid.GetScenic(sol.trees, y, x);
                    builder.Append(scenic);
                }

                builder.AppendLine();
            }

            Assert.AreEqual(scenics, builder.ToString());
        }

        public const string toTheRight = "0123456789";

        public const string example = @"30373
25512
65332
33549
35390";

        public const string scenics = @"00000
01410
06120
01830
00000
";
    }
}
