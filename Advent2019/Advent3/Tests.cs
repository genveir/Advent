using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent3
{
    class Tests
    {
        [TestCase("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62, R66, U55, R34, D71, R55, D58, R83", "159")]
        [TestCase("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98, R91, D20, R16, D67, R40, U7, R15, U6, R7", "135")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62, R66, U55, R34, D71, R55, D58, R83", "610")]
        [TestCase("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98, R91, D20, R16, D67, R40, U7, R15, U6, R7", "410")]
        [TestCase("U5,L5,R5\nR5,U5,L5", "20")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
