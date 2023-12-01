using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Shared.Tests
{
    class InputParserTests
    {
        [Test]
        public void InputParserCanDetectStartWithSeparator()
        {
            var parser = new InputParser("<min, max>");

            Assert.That(parser.simpleParser.delimiters.SequenceEqual(new[] { "<", ", ", ">" }));
            parser.NumberOfValues.Should().Be(2);
        }

        [Test]
        public void InputParserCanParseDay2Pattern()
        {
            var parser = new InputParser("min-max letter: password");

            parser.NumberOfValues.Should().Be(4);
            Assert.That(parser.simpleParser.delimiters.SequenceEqual(new[] { "-", " ", ": " }));
        }

        [Test]
        public void InputParserCanParseDay2Input()
        {
            var parser = new InputParser(true, 4, "-", " ", ": ");
            (string min, string max, string letter, string password) output = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            output.min.Should().Be("9");
            output.max.Should().Be("10");
            output.letter.Should().Be("d");
            output.password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void InputParserCanParseDay2InputFromPattern()
        {
            var parser = new InputParser("min-max letter: password");
            (string min, string max, string letter, string password) output = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            output.min.Should().Be("9");
            output.max.Should().Be("10");
            output.letter.Should().Be("d");
            output.password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void InputParserCanParseDay2InputWithStartingDelimiter()
        {
            var parser = new InputParser(false, 4, ".", "-", " ", ": ");
            (string min, string max, string letter, string password) output = parser.Parse(".9-10 d: dddddddddwdldmdddddd");

            output.min.Should().Be("9");
            output.max.Should().Be("10");
            output.letter.Should().Be("d");
            output.password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void InputParserCanParseDay2InputWithStartingDelimiterFromPattern()
        {
            var parser = new InputParser(".min-max letter: password");
            (string min, string max, string letter, string password) output = parser.Parse(".9-10 d: dddddddddwdldmdddddd");

            output.min.Should().Be("9");
            output.max.Should().Be("10");
            output.letter.Should().Be("d");
            output.password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void InputParserCanParseTypedDay2Input()
        {
            var parser = new InputParser("min-max letter: password");
            var (min, max, letter, password) = parser.Parse<int, int, char, string>("9-10 d: dddddddddwdldmdddddd");

            min.Should().Be(9);
            max.Should().Be(10);
            letter.Should().Be('d');
            password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void TypedInputParserCanParseDay2Input()
        {
            var parser = new InputParser<int, int, char, string>("min-max letter: password");
            var (min, max, letter, password) = parser.Parse("9-10 d: dddddddddwdldmdddddd");

            min.Should().Be(9);
            max.Should().Be(10);
            letter.Should().Be('d');
            password.Should().Be("dddddddddwdldmdddddd");
        }

        [Test]
        public void CanParseCommaDelimitedArrays()
        {
            var parser = new InputParser<int[], long[], bool[], char[], string[]>("array array array array array");
            var (ints, longs, bools, chars, strings) = parser.Parse("1,2,3,4,5 6,7,8,9,10 true,false y,o hallo,hoi,hee");

            for (int n = 0; n < 5; n++) ints[n].Should().Be(n + 1);

            for (int n = 0; n < 5; n++) longs[n].Should().Be(n + 6);

            bools[0].Should().BeTrue();
            bools[1].Should().BeFalse();

            chars[0].Should().Be('y');
            chars[1].Should().Be('o');

            strings[0].Should().Be("hallo");
            strings[1].Should().Be("hoi");
            strings[2].Should().Be("hee");
        }

        [Test]
        public void CanSetCustomDelimiters()
        {
            var parser = new InputParser<int[], char[], string[]>("array: array array") { ArrayDelimiters = new[] { '.', '-' } };
            var (ints, chars, strings) = parser.Parse("1-8: a.b a,b-c,d");

            ints[0].Should().Be(1);
            ints[1].Should().Be(8);
            chars[0].Should().Be('a');
            chars[1].Should().Be('b');
            strings[0].Should().Be("a,b");
            strings[1].Should().Be("c,d");
        }

        [Test]
        public void CanSetZeroDelimiter()
        {
            var parser = new InputParser<int, int, int, int[]>("min-max num: pw") { EmptyArrayDelimiter = true };
            var (min, max, num, pw) = parser.Parse("1-2 3: 45");

            min.Should().Be(1);
            max.Should().Be(2);
            num.Should().Be(3);
            pw[0].Should().Be(4);
            pw[1].Should().Be(5);
        }

        [Test]
        public void CanParseComplexObjectsWithNoParameters()
        {
            var parser = new InputParser("begin -> end");
            var testClass = parser.Parse<EmptyConstructorClass>("1,1 -> 10,10");

            testClass.Should().NotBeNull();
        }

        [Test]
        public void CanParseComplexObjectWithSingleParameter()
        {
            var parser = new InputParser<SingleParameterTestClass>("coord");
            var testClass = parser.Parse("1,2");

            testClass.Coordinate.X.Should().Be(1);
            testClass.Coordinate.Y.Should().Be(2);
        }

        [Test]
        public void CanParseComplexObjects()
        {
            var parser = new InputParser("begin -> endx,endy");
            var testClass = parser.Parse<ParsingTestClass>("1,2 -> 10,11");

            testClass.Begin.X.Should().Be(1);
            testClass.Begin.Y.Should().Be(2);
            testClass.End.X.Should().Be(10);
            testClass.End.Y.Should().Be(11);
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private class EmptyConstructorClass { }

        private class SingleParameterTestClass
        {
            public readonly Coordinate Coordinate;

            public SingleParameterTestClass(long[] coords)
            {
                Coordinate = new Coordinate(coords[0], coords[1]);
            }
        }

        private class ParsingTestClass 
        {
            public readonly Coordinate Begin;
            public readonly Coordinate End;

            [ComplexParserConstructor]
            public ParsingTestClass(long[] begin, long endX, long endY)
            {
                Begin = new Coordinate(begin);
                End = new Coordinate(endX, endY);
            }
        }
    }
}
