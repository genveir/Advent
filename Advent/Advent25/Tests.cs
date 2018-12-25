using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent25
{
    class Tests
    {
        [Test]
        public void Example()
        {
            var sol = new Solution("Test", Solution.InputMode.File);
            var p1 = sol.GetPart1();

            Assert.AreEqual(2, p1);
        }

        [SetUp]
        public void setup()
        {
            inputs = new string[3];

            inputs[0] = @"-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0";

            inputs[1] = @"1,-1,0,1
2,0,-1,0
3,2,-1,0
0,0,3,1
0,0,-1,-1
2,3,-2,0
-2,2,0,0
2,-2,0,-1
1,-1,0,-1
3,2,0,2";

            inputs[2] = @"1,-1,-1,-2
-2,-2,0,1
0,2,1,3
-2,3,-2,1
0,2,3,-2
-1,-1,1,-2
0,-2,-1,0
-2,2,3,-1
1,2,2,0
-1,-2,0,-2";
        }
        string[] inputs;

        [TestCase(0, 4)]
        [TestCase(1, 3)]
        [TestCase(2, 8)]
        public void Examples(int input, int expected)
        {
            var sol = new Solution(inputs[input], Solution.InputMode.String);
            var p1 = sol.GetPart1();

            Assert.AreEqual(expected, p1);
        }


        [Test]
        public void ExamplePart2()
        {
            var sol = new Solution("Test", Solution.InputMode.File);
            var p2 = sol.GetPart2();

            Assert.AreEqual(0, p2);
        }
    }
}
