using Advent2019.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent7
{
    class Tests
    {
        [TestCase("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", "43210")]
        [TestCase("3,23,3,24,1002,24,10,24,1002,23,-1,23,101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0", "54321")]
        [TestCase("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0", "65210")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5", "139629729")]
        [TestCase("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4,53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10", "18216")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void TestPermutationBaseCase()
        {
            int[] input = new int[] { 0 };
            var permutations = input.GetPermutations();
            Assert.AreEqual(1, permutations.Length);
            Assert.AreEqual(1, permutations[0].Length);
            Assert.AreEqual(0, permutations[0][0]);
        }


        [Test]
        public void TestPermutation2()
        {
            int[] input = new int[] { 1, 0 };
            var permutations = input.GetPermutations();
            Assert.AreEqual(2, permutations.Length);
            Assert.AreEqual(2, permutations[0].Length);
            Assert.AreEqual(1, permutations[0][0]);
            Assert.AreEqual(0, permutations[0][1]);
            Assert.AreEqual(0, permutations[1][0]);
            Assert.AreEqual(1, permutations[1][1]);
        }

        [Test]
        public void TestPermutation3()
        {
            int[] input = new int[] { 2, 1, 0 };
            var permutations = input.GetPermutations();
            Assert.AreEqual(6, permutations.Length);
            Assert.AreEqual(3, permutations[0].Length);
            Assert.AreEqual(2, permutations[0][0]);
            Assert.AreEqual(1, permutations[0][1]);
            Assert.AreEqual(0, permutations[0][2]);
        }

        [Test]
        public void TestWithoutNth()
        {
            int[] input = new int[] { 0, 1, 2, 3, 4 };

            for (int n = 0; n < 5; n ++)
            {
                var result = input.WithoutNth(n);
                Assert.AreEqual(10 - n, result.Sum());
            }
        }
    }
}
