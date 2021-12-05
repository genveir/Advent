using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent03
{
    class Tests
    {
        [TestCase("Input.txt", 2595824)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("Input.txt", 2135254)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void GetGammaIsCorrectForTwoItems()
        {
            int[][] input = new int[4][];
            input[0] = new int[2] { 1, 0 };
            input[1] = new int[2] { 0, 0 };
            input[2] = new int[2] { 1, 1 };
            input[3] = new int[2] { 0, 1 };

            var gamma = Solution.GetMatch(input, 1);

            Assert.AreEqual(1, gamma[0]);
            Assert.AreEqual(0, gamma[1]);
            Assert.AreEqual(1, gamma[2]);
            Assert.AreEqual(1, gamma[3]);
        }

        [Test]
        public void GetEpsilonIsCorrectForTwoItems()
        {
            int[][] input = new int[4][];
            input[0] = new int[2] { 1, 0 };
            input[1] = new int[2] { 0, 0 };
            input[2] = new int[2] { 1, 1 };
            input[3] = new int[2] { 0, 1 };

            var epsilon = Solution.GetMatch(input, 0);

            Assert.AreEqual(0, epsilon[0]);
            Assert.AreEqual(1, epsilon[1]);
            Assert.AreEqual(0, epsilon[2]);
            Assert.AreEqual(0, epsilon[3]);
        }

        [Test]
        public void OxyIsCorrect()
        {
            var solution = new Solution(example);

            var numbers = solution.numbers;

            var oxy = Solution.GetGasMatch(numbers, 1, 0);
            var oxyString = string.Join("", oxy);

            var number = Convert.ToInt64(oxyString, 2);

            Assert.AreEqual(23, number);
        }

        private const string example = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";
    }
}
