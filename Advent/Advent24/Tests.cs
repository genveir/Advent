using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent24
{
    class Tests
    {
        [Test]
        public void ExamplePart1()
        {
            var sol = new Solution("Test", Solution.InputMode.File);

            Assert.AreEqual(5216, sol.GetPart1());
        }

        [Test]
        public void BoostExample()
        {
            var sol = new Solution("Test", Solution.InputMode.File);
            foreach (var group in sol.fight.immuneSystemArmy) group.Boost(1570);

            Assert.AreEqual(51, sol.GetPart1());
            Assert.AreEqual(51, sol.GetPart2());
        }

        [Test]
        public void Part1IsStillCorrect()
        {
            var sol = new Solution();

            Assert.AreEqual(22996, sol.GetPart1());
        }

        [Test]
        public void BoostDrawTimeTest()
        {
            var sol = new Solution();
            foreach (var group in sol.fight.immuneSystemArmy) group.Boost(50);

            var result = sol.GetPart1();
        }

        [Test]
        public void Part2IsNotWrong()
        {
            var sol = new Solution();
            var result = sol.GetPart2();

            Assert.AreNotEqual(51, result);
        }
    }
}
