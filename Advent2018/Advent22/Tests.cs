﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent22
{
    class Tests
    {
        [Test]
        public void Example()
        {
            var sol = new Solution(510, (10, 10));

            Assert.AreEqual(114, sol.GetTotalRisk());
            Assert.AreEqual(45, sol.FindFastest());
        }

        [Test]
        public void StartGoesWell()
        {
            var sol = new Solution(510, (4, 1));

            Assert.AreEqual(19, sol.FindFastest());
        }

        [Test]
        public void Part1StillCorrect()
        {
            var sol = new Solution(10647, (7, 770));

            Assert.AreEqual(6208, sol.GetTotalRisk());
        }

        [Test]
        public void Part2StillCorrect()
        {
            var sol = new Solution(10647, (7, 770));

            Assert.AreEqual(1039, sol.FindFastest());
        }
    }
}
