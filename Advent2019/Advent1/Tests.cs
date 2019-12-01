using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent1
{
    public class Tests
    {        
        [TestCase("12", "2")]
        [TestCase("14", "2")]
        [TestCase("1969", "654")]
        [TestCase("100756", "33583")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("14", "2")]
        [TestCase("1969", "966")]
        [TestCase("100756", "50346")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
