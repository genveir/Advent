using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent20
{
    class Tests
    {
        [TestCase("^WNE$", 3)]
        [TestCase("^WNE|WNE$", 3)]
        [TestCase("^ENWWW(NEEE|SSE(EE|N))$", 10)]
        [TestCase("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
        [TestCase("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23)]
        [TestCase("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31)]
        public void Test1(string input, int output)
        {
            var solution = new Solution(input, Solution.InputMode.String);

            var result = solution.GetResult1();

            Assert.AreEqual(output, result);
        }
    }
}
