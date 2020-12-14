﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.AdventBase
{
    class Tests
    {
        [TestCase("", "")]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "")]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
