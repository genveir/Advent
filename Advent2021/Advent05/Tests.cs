using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent05
{
    class Tests
    {
        [TestCase(example, 5)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            var result = sol.GetResult1();

            var drawn = DrawGrid(sol.grid);

            Assert.AreEqual(output, result);
        }

        [TestCase(example, 12)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            sol.GetResult1();
            var result = sol.GetResult2();

            var drawn = DrawGrid(sol.grid);

            Assert.AreEqual(output, result);
        }

        [Test]
        public void LineOfLength1Draws1()
        {
            var sol = new Solution("1,1 -> 1,1");

            sol.GetResult1();

            Assert.AreEqual(1, sol.grid[1][1]);
        }

        public string DrawGrid(long[][] grid)
        {
            StringBuilder gridString = new StringBuilder(); 
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    gridString.Append(grid[x][y] == 0 ? "." : grid[x][y].ToString());
                }
                gridString.AppendLine();
            }

            return gridString.ToString();
        }

        public const string example = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
    }
}
