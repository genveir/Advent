using Advent2019.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static Advent2019.Advent22.Solution;

namespace Advent2019.Advent22
{
    class Tests
    {
        [Test]
        public void TestCut()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var res1 = new Cut("3", 10).Apply(cards);
            var expected = new int[] { 3, 4, 5, 6, 7, 8, 9, 0, 1, 2 };

            for (int n = 0; n < cards.Length; n++)
            {
                Assert.AreEqual(res1[n], expected[n]);
            }

            var res2 = new Cut("-4", 10).Apply(cards);
            expected = new int[] { 6, 7, 8, 9, 0, 1, 2, 3, 4, 5 };

            for (int n = 0; n < cards.Length; n++)
            {
                Assert.AreEqual(res2[n], expected[n]);
            }
        }

        [Test]
        public void TestDeal()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var res1 = new Deal("3", 10).Apply(cards);
            var expected = new int[] { 0, 7, 4, 1, 8, 5, 2, 9, 6, 3 };

            for (int n = 0; n < cards.Length; n++)
            {
                Assert.AreEqual(res1[n], expected[n]);
            }
        }

        public const string example1 = @"deal with increment 7
deal into new stack
deal into new stack";

        [Test]
        public void Example1()
        {
            var sol = new Solution(Shared.Input.InputMode.String, example1);
            sol.Setup(10);
            sol.ResetDeck();
            sol.ApplyTechniques();

            var expected = new int[] { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 };
            var cards = sol.cards;

            for (int n = 0; n < cards.Length; n++)
            {
                Assert.AreEqual(cards[n], expected[n]);
            }
        }

        //[Test]
        public void DoingThisRight()
        {
            long numCards = 10337; // iets grotere prime, willekeurig gekozen

            var sol = new Solution();
            sol.Setup(10337);
            sol.ResetDeck();

            int[] at0 = null;
            int[] at5 = null;
            int[] at5000 = null;

            for (int n = 0; n < 30000; n++)
            {
                sol.ApplyTechniques();

                if (n == 0) at0 = sol.cards;
                if (n == 5) at5 = sol.cards;
                if (n == 5000) at5000 = sol.cards;

                if (n % (numCards - 1) == 0) Assert.That(AreEqual(at0, sol.cards));
                if (n % (numCards - 1) == 5) Assert.That(AreEqual(at5, sol.cards));
                if (n % (numCards - 1) == 5000) Assert.That(AreEqual(at5000, sol.cards));
            }
        }

        [Test]
        public void DoingThisRightPt2()
        {
            var sol = new Solution();
            sol.Setup(10007);

            var res = sol.BackTrack(4775, 1);

            Assert.AreEqual(2019, res);
        }

        [Test] // time test
        public void DoingThisRightPt3()
        {
            var sol = new Solution();
            sol.Setup(10007);

            var res = sol.BackTrack(1, 100060000000);
            var res2 = sol.BackTrack(4775, 100060000001);

            Assert.AreEqual(1, res);
            Assert.AreEqual(2019, res2);
        }

        [TestCase(10007)]
        [TestCase(22307)]
        [TestCase(127363)]
        [TestCase(1573603)]
        [TestCase(18700529)]
        [TestCase(216329021)]
        [TestCase(2453972743)]
        [TestCase(27426994103)]
        [TestCase(302990358137)]
        [TestCase(3315877503149)]
        [TestCase(119315717514047)]
        public void DoingThisRightPt4(long testCase)
        {
            for (int n = 0; n < 10; n++)
            {
                var sol = new Solution();
                sol.Setup(testCase);

                var res = sol.BackTrack(1, testCase - 1);

                Assert.AreEqual(1, res);
            }
        }

        [TestCase(10007)]
        [TestCase(22307)]
        [TestCase(127363)]
        [TestCase(1573603)]
        [TestCase(18700529)]
        [TestCase(216329021)]
        [TestCase(2453972743)]
        [TestCase(27426994103)]
        [TestCase(302990358137)]
        [TestCase(3315877503149)]
        public void IsPrime(long testCase)
        {
            var sqrt = Math.Sqrt(testCase);

            for (int n = 1; n < sqrt; n++)
            {
                var GCD = Helper.GCD(n, testCase);
                Assert.AreEqual(1, GCD);
            }
        }

        public bool AreEqual(int[] first, int[] second)
        {
            for (int n = 0; n < first.Length; n++)
            {
                if (first[n] != second[n]) return false;
            }

            return true;
        }

        [Test]
        public void TestBackTrackRevert()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var reverted = new Revert().Apply(cards);

            for(int n = 0; n < cards.Length; n++)
            {
                var indexInReverted = n;
                var indexInCards = new Revert().BackTrack(indexInReverted, 10);

                Assert.AreEqual(cards[indexInCards], reverted[indexInReverted]);
            }
        }

        [Test]
        public void TestBackTrackCutPos()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var cutPos = new Cut("3", 10).Apply(cards);

            for (int n = 0; n < cards.Length; n++)
            {
                var indexInCut = n;
                var indexInCards = new Cut("3", 10).BackTrack(indexInCut, 10);

                Assert.AreEqual(cards[indexInCards], cutPos[indexInCut]);
            }
        }

        [Test]
        public void TestBackTrackCutNeg()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var cutPos = new Cut("-4", 10).Apply(cards);

            for (int n = 0; n < cards.Length; n++)
            {
                var indexInCut = n;
                var indexInCards = new Cut("-4", 10).BackTrack(indexInCut, 10);

                Assert.AreEqual(cards[indexInCards], cutPos[indexInCut]);
            }
        }

        [Test]
        public void TestBackTrackDeal()
        {
            var cards = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            for (int i = 1; i < 11; i++)
            {
                var dealed = new Deal(i.ToString(), 11).Apply(cards);

                for (int n = 0; n < cards.Length; n++)
                {
                    var indexInDealt = n;
                    var indexInCards = new Deal(i.ToString(), 11).BackTrack(indexInDealt, 11);

                    Assert.AreEqual(indexInCards, dealed[indexInDealt]);
                }
            }
        }
    }
}
