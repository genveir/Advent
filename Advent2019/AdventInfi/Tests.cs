using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.AdventInfi
{ 
    class Tests
    {
        [TestCase("{\"flats\": [[1,4],[3,8],[4,3],[5,7],[7,4],[10,3]],\"sprongen\": [[2,0],[0,4],[1,0],[0,0]]}", "4")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
