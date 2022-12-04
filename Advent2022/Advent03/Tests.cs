using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent03
{
    class Tests
    {
        [TestCase(example, 157)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 70)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [TestCase(example, 70)]
        [TestCase("Input.txt", 2342)]
        [TestCase(niels, 2)]
        [TestCase(nielsExtended, 3)]
        public void Test2Fast(string input, object output)
        {
            var sol = new FastSolution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string example = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw";

        public const string niels = @"aaab
aabb
bbbb";

        public const string nielsExtended = @"aaaa
aabb
aaab
aaab
aabb
bbbb";
    }
}
