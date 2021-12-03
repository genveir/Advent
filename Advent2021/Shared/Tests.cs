using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Shared
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

        [Test]
        public void ModListIsCircular()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 10; n++) modList.Add(n);

            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(1, modList[11]);
            Assert.AreEqual(1, modList[-9]);
        }

        [Test]
        public void ModListIndexCanBeHigherThanMaxInt()
        {
            var modList = new ModList<int>();
            modList.Add(1);

            Assert.AreEqual(1, modList[500_000_000_000_000]);
        }

        [Test]
        public void CanInsertIntoModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithNegativeStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(-1, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithHighStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(500_000_000_000_001, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertIntoModListWithVeryHighStartIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            modList.Insert(21, new List<int>() { 3, 4 });

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(4, modList[2]);
            Assert.AreEqual(1, modList[3]);
        }

        [Test]
        public void CanInsertLargeDataIntoModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 2; n++) modList.Add(n);

            var newList = new List<int>();
            for (int n = 0; n < 100000; n++) newList.Add(n);

            modList.Insert(1, newList);

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(0, modList[1]);
            Assert.AreEqual(1, modList[-1]);
            Assert.AreEqual(99999, modList[-2]);
        }

        [Test]
        public void CanReversePartOfModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(1, 3);

            Assert.AreEqual(0, modList[0]);
            Assert.AreEqual(3, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(1, modList[3]);
            Assert.AreEqual(4, modList[4]);
        }

        [Test]
        public void CanReversePastEndOfModList()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(3, 3);

            Assert.AreEqual(3, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(0, modList[3]);
            Assert.AreEqual(4, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithNegativeIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(-1, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithHighIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(9, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

        [Test]
        public void CanStartReverseOfModListWithVeryHighIndex()
        {
            var modList = new ModList<int>();
            for (int n = 0; n < 5; n++) modList.Add(n);

            modList.ReverseRange(500_000_000_000_004, 2);

            Assert.AreEqual(4, modList[0]);
            Assert.AreEqual(1, modList[1]);
            Assert.AreEqual(2, modList[2]);
            Assert.AreEqual(3, modList[3]);
            Assert.AreEqual(0, modList[4]);
        }

        [Test]
        public void CanPivotSquareArray()
        {
            var input = new int[2][];
            input[0] = new int[] { 0, 1 };
            input[1] = new int[] { 2, 3 };

            // 0 1            0 2
            // 2 3  pivots to 1 3

            var pivotted = input.Pivot();

            Assert.AreEqual(0, pivotted[0][0]);
            Assert.AreEqual(2, pivotted[0][1]);
            Assert.AreEqual(1, pivotted[1][0]);
            Assert.AreEqual(3, pivotted[1][1]);
        }

        [Test]
        public void CanPivotRectangularArray()
        {
            var input = new int[2][];
            input[0] = new int[] { 0, 1, 2 };
            input[1] = new int[] { 3, 4, 5 };

            // 0 1 2            0 3
            // 3 4 5  pivots to 1 4
            //                  2 5

            var pivotted = input.Pivot();

            Assert.AreEqual(0, pivotted[0][0]);
            Assert.AreEqual(3, pivotted[0][1]);
            Assert.AreEqual(1, pivotted[1][0]);
            Assert.AreEqual(4, pivotted[1][1]);
            Assert.AreEqual(2, pivotted[2][0]);
            Assert.AreEqual(5, pivotted[2][1]);
        }
    }
}
