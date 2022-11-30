using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tests
{
    class InputTests
    {
        [Test]
        public void GetNumbersWorksForAllNumbers()
        {
            var numbers = Input.GetNumbers("0123456789");

            for (int n = 0; n < 10; n++) Assert.AreEqual(n, numbers[n]);
        }

        [Test]
        public void GetNumbersCanSplit()
        {
            var numbers = Input.GetNumbers("0,1,2,3,4,5,6,7,8,9", new char[] { ',' });

            for (int n = 0; n < 10; n++) Assert.AreEqual(n, numbers[n]);
        }

        [Test]
        public void SplitGetNumbersCanBeLarger()
        {
            var numbers = Input.GetNumbers("10,11,12,13,14,15,16,17,18,19", new char[] { ',' });

            for (int n = 0; n < 10; n++) Assert.AreEqual(n + 10, numbers[n]);
        }

        [Test]
        public void SpitGetNumbersDontCareAboutSpaces()
        {
            var numbers = Input.GetNumbers("0, 1, 2, 3, 4, 5, 6, 7, 8, 9", new char[] { ',' });

            for (int n = 0; n < 10; n++) Assert.AreEqual(n, numbers[n]);
        }

        [Test]
        public void SplitGetNumbersWorksWithNegativeNumbers()
        {
            var numbers = Input.GetNumbers("0,-1,-2,-3,-4,-5,-6,-7,-8,-9", new char[] { ',' });

            for (int n = 0; n < 10; n++) Assert.AreEqual(-n, numbers[n]);
        }
    }
}
