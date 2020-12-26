using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.Shared
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

        [Test]
        public void TypedInputParserCanParseDay2Input()
        {
            var parser = new InputParser<int, int, char, string>("min-max letter: password");
            var (min, max, letter, password) = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            Assert.AreEqual(9, min);
            Assert.AreEqual(10, max);
            Assert.AreEqual('d', letter);
            Assert.AreEqual("dddddddddwdldmdddddd", password);
        }

        [Test]
        public void CanParseCommaDelimitedArrays()
        {
            var parser = new InputParser<int[], long[], bool[], char[], string[]>("array array array array array");
            var (ints, longs, bools, chars, strings) = parser.Parse("1,2,3,4,5 6,7,8,9,10 true,false y,o hallo,hoi,hee");

            for (int n = 0; n < 5; n++) Assert.AreEqual(ints[n], n + 1);

            for (int n = 0; n < 5; n++) Assert.AreEqual(longs[n], n + 6);

            Assert.IsTrue(bools[0]);
            Assert.IsFalse(bools[1]);

            Assert.AreEqual('y', chars[0]);
            Assert.AreEqual('o', chars[1]);

            Assert.AreEqual("hallo", strings[0]);
            Assert.AreEqual("hoi", strings[1]);
            Assert.AreEqual("hee", strings[2]);
        }

        [Test]
        public void CanSetCustomDelimiters()
        {
            var parser = new InputParser<int[], char[], string[]>("array: array array") { ArrayDelimiters = new char[] { '.', '-' } };
            var (ints, chars, strings) = parser.Parse("1-8: a.b a,b-c,d");

            Assert.AreEqual(1, ints[0]);
            Assert.AreEqual(8, ints[1]);
            Assert.AreEqual('a', chars[0]);
            Assert.AreEqual('b', chars[1]);
            Assert.AreEqual("a,b", strings[0]);
            Assert.AreEqual("c,d", strings[1]);
        }

        [Test]
        public void CanSetZeroDelimiter()
        {
            var parser = new InputParser<int, int, int, int[]>("min-max num: pw") { EmptyArrayDelimiter = true };
            var (min, max, num, pw) = parser.Parse("1-2 3: 45");

            Assert.AreEqual(1, min);
            Assert.AreEqual(2, max);
            Assert.AreEqual(3, num);
            Assert.AreEqual(4, pw[0]);
            Assert.AreEqual(5, pw[1]);
        }
    }
}
