using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent16
{
    class Tests
    {
        [TestCase("12345678", "23845678")]
        [TestCase("80871224585914546619083218645595", "24176176")]
        [TestCase("19617804207202209144916044189917", "73745418")]
        [TestCase("69317163492948606335995924319873", "52432133")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("03036732577212944063491565474664", "84462026")]
        [TestCase("02935109699940807407585447034323", "78725270")]
        [TestCase("03081770884921959731165446850517", "53553731")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
