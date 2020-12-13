using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent13
{
    class Tests
    {
        [TestCase("", "")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        const string sample1 = @"0
7,13,x,x,59,x,31,19";

        const string sample2 = @"0
17,x,13,19";

        const string sample3 = @"0
67,7,59,61";

        const string sample4 = @"0
67,x,7,59,61";

        const string sample5 = @"0
67,7,x,59,61";

        const string sample6 = @"0
1789,37,47,1889";

        [TestCase(sample1, 1068781)]
        [TestCase(sample2, 3417)]
        [TestCase(sample3, 754018)]
        [TestCase(sample4, 779210)]
        [TestCase(sample5, 1261476)]
        [TestCase(sample6, 1202161486)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
