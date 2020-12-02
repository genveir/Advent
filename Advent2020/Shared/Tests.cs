using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Shared
{
    class Tests
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

        [Test]
        public void InputParserCanDetectStartWithSeparator()
        {
            var parser = new InputParser("<min, max>");

            Assert.That(parser.delimiters.SequenceEqual(new string[] { "<", ", ", ">" }));
            Assert.AreEqual(2, parser.NumberOfValues);
        }

        [Test]
        public void InputParserCanParseDay2Pattern()
        {
            var parser = new InputParser("min-max letter: password");

            Assert.AreEqual(4, parser.NumberOfValues);
            Assert.That(parser.delimiters.SequenceEqual(new string[] { "-", " ", ": " }));
        }

        [Test]
        public void InputParserCanParseDay2Input()
        {
            var parser = new InputParser(true, 4, new string[] { "-", " ", ": " });
            (string min, string max, string letter, string password) output = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual("9", output.min);
            Assert.AreEqual("10", output.max);
            Assert.AreEqual("d", output.letter);
            Assert.AreEqual("dddddddddwdldmdddddd", output.password);
        }

        [Test]
        public void InputParserCanParseDay2InputFromPattern()
        {
            var parser = new InputParser("min-max letter: password");
            (string min, string max, string letter, string password) output = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual("9", output.min);
            Assert.AreEqual("10", output.max);
            Assert.AreEqual("d", output.letter);
            Assert.AreEqual("dddddddddwdldmdddddd", output.password);
        }

        [Test]
        public void InputParserCanParseDay2InputWithStartingDelimiter()
        {
            var parser = new InputParser(false, 4, new string[] { ".", "-", " ", ": " });
            (string min, string max, string letter, string password) output = parser.Parse(".9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual("9", output.min);
            Assert.AreEqual("10", output.max);
            Assert.AreEqual("d", output.letter);
            Assert.AreEqual("dddddddddwdldmdddddd", output.password);
        }

        [Test]
        public void InputParserCanParseDay2InputWithStartingDelimiterFromPattern()
        {
            var parser = new InputParser(".min-max letter: password");
            (string min, string max, string letter, string password) output = parser.Parse(".9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual("9", output.min);
            Assert.AreEqual("10", output.max);
            Assert.AreEqual("d", output.letter);
            Assert.AreEqual("dddddddddwdldmdddddd", output.password);
        }

        [Test]
        public void InputParserCanParseTypedDay2Input()
        {
            var parser = new InputParser("min-max letter: password");
            var (min, max, letter, password) = parser.Parse<int, int, char, string>("9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual(9, min);
            Assert.AreEqual(10, max);
            Assert.AreEqual('d', letter);
            Assert.AreEqual("dddddddddwdldmdddddd", password);
        }
    }
}
