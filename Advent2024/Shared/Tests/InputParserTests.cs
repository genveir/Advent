using Advent2024.Shared.InputParsing;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Advent2024.Shared.Tests;

internal class InputParserTests
{
    [Test]
    public void InputParserCanDetectStartWithSeparator()
    {
        var parser = new InputParser("<min, max>");
        parser.simpleParser.ParsePattern();

        Assert.That(parser.simpleParser.delimiters.SequenceEqual(["<", ", ", ">"]));
        parser.NumberOfValues.Should().Be(2);
    }

    [Test]
    public void InputParserParsesPatternWhenNumberOfValuesIsRead()
    {
        var parser = new InputParser("pattern pattern");

        parser.simpleParser.delimiters.Should().BeNull();

        parser.NumberOfValues.Should().Be(2);

        parser.simpleParser.delimiters.Should().BeEquivalentTo([" "]);
    }

    [Test]
    public void InputParserParsesPatternWhenArrayDelimitersIsRead()
    {
        var parser = new InputParser("pattern pattern");

        parser.simpleParser.delimiters.Should().BeNull();

        parser.ArrayDelimiters.Should().BeEquivalentTo([',']);

        parser.simpleParser.delimiters.Should().BeEquivalentTo([" "]);
    }

    [Test]
    public void InputParserParsesPatternWhenTrimBeforeParsingIsRead()
    {
        var parser = new InputParser("pattern pattern");

        parser.simpleParser.delimiters.Should().BeNull();

        parser.ShouldTrimBeforeParsing.Should().BeTrue();

        parser.simpleParser.delimiters.Should().BeEquivalentTo([" "]);
    }

    [Test]
    public void InputParserParsesPatternOnFirstRun()
    {
        var parser = new InputParser("pattern pattern");

        parser.simpleParser.delimiters.Should().BeNull();

        parser.Parse<int, int>("1 2");

        parser.simpleParser.delimiters.Should().BeEquivalentTo([" "]);
    }

    [Test]
    public void InputParserCanParseDay2Pattern()
    {
        var parser = new InputParser("min-max letter: password");
        parser.simpleParser.ParsePattern();

        parser.NumberOfValues.Should().Be(4);
        Assert.That(parser.simpleParser.delimiters.SequenceEqual(["-", " ", ": "]));
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
        parser.simpleParser.ParsePattern();
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
        parser.simpleParser.ParsePattern();
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
        parser.simpleParser.ParsePattern();
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
        parser.simpleParser.ParsePattern();

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
        parser.simpleParser.ParsePattern();

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
    public void CanSetCustomArrayDelimiters()
    {
        var parser = new InputParser<int[], char[], string[]>("array: array array") { ArrayDelimiters = ['.', '-'] };
        parser.simpleParser.ParsePattern();
        var (ints, chars, strings) = parser.Parse("1-8: a.b a,b-c,d");

        ints[0].Should().Be(1);
        ints[1].Should().Be(8);
        chars[0].Should().Be('a');
        chars[1].Should().Be('b');
        strings[0].Should().Be("a,b");
        strings[1].Should().Be("c,d");
    }

    [Test]
    public void CanSetZeroArrayDelimiter()
    {
        var parser = new InputParser<int, int, int, int[]>("min-max num: pw") { EmptyArrayDelimiter = true };
        parser.simpleParser.ParsePattern();
        var (min, max, num, pw) = parser.Parse("1-2 3: 45");

        min.Should().Be(1);
        max.Should().Be(2);
        num.Should().Be(3);
        pw[0].Should().Be(4);
        pw[1].Should().Be(5);
    }

    [Test]
    public void CanParseComplexObjectWithSingleParameter()
    {
        var parser = new InputParser<SingleParameterTestClass>("coord");
        parser.simpleParser.ParsePattern();
        var testClass = parser.Parse("1,2");

        testClass.Coordinate.X.Should().Be(1);
        testClass.Coordinate.Y.Should().Be(2);
    }

    [Test]
    public void CanParseComplexObjects()
    {
        var parser = new InputParser("line");
        parser.simpleParser.ParsePattern();
        var testClass = parser.Parse<ParsingTestClass>("1,2 -> 3,4 -> 10,11");

        testClass.Should()
            .BeEquivalentTo(new ParsingTestClass([1, 2], new Coordinate2D(3, 4), 10, 11));
    }

    [Test]
    public void CanParseNestedObjects()
    {
        var parser = new InputParser("num |- testclass");
        parser.simpleParser.ParsePattern();
        var (num, nestedClass) = parser.Parse<int, NestingTestClass>("5 |- 1: 1,2 -> 3,4 -> 10,11; 2,3 -> 4,5 -> 12,13");

        num.Should().Be(5);

        nestedClass.Number.Should().Be(1);
        nestedClass.TestClasses.Should().HaveCount(2);

        nestedClass.TestClasses.First().Should()
            .BeEquivalentTo(new ParsingTestClass([1, 2], new Coordinate2D(3, 4), 10, 11));
        nestedClass.TestClasses.Last().Should()
            .BeEquivalentTo(new ParsingTestClass([2, 3], new Coordinate2D(4, 5), 12, 13));
    }

    [Test]
    public void CanUseFactoryMethod()
    {
        var parser = new InputParser("line");
        var factoryTestClass = parser.Parse<FactoryMethodClass>("0: ab,bc,cd,de");

        factoryTestClass.Should().NotBeNull();
        factoryTestClass.Values[0].Should().Be("ba");
        factoryTestClass.Values[1].Should().Be("cb");
        factoryTestClass.Values[2].Should().Be("dc");
        factoryTestClass.Values[3].Should().Be("ed");
    }

    [Test]
    public void CanUseWordsAsDelimiters()
    {
        var parser = new InputParser(false, 2, "Game ", ": ");

        var (num, values) = parser.Parse<string, int[]>("Game 1: 1, 2, 3, 4");
        num.Should().Be("1");
        values.Should().BeEquivalentTo([1, 2, 3, 4]);
    }

    [Test]
    public void CanEscapeWord()
    {
        var parser = new InputParser("\\Game num: values");
        parser.simpleParser.ParsePattern();

        parser.simpleParser.delimiters.Should().BeEquivalentTo(["Game ", ": "]);

        var (num, values) = parser.Parse<string, int[]>("Game 1: 1, 2, 3, 4");
        num.Should().Be("1");
        values.Should().BeEquivalentTo([1, 2, 3, 4]);
    }

    [Test]
    public void CanOverrideEscapeChar()
    {
        var parser = new InputParser("/Game \\num: values")
        {
            PatternEscapeChar = '/'
        };
        parser.simpleParser.ParsePattern();

        parser.simpleParser.delimiters.Should().BeEquivalentTo(["Game \\", ": "]);

        var (num, values) = parser.Parse<string, int[]>("Game \\1: 1, 2, 3, 4");
        num.Should().Be("1");
        values.Should().BeEquivalentTo([1, 2, 3, 4]);
    }

    [Test]
    public void EscapedDelimiterDoesNotEndOnNonText()
    {
        var parser = new InputParser("\\Game: num: values");
        parser.simpleParser.ParsePattern();

        parser.simpleParser.delimiters.Should().BeEquivalentTo(["Game: ", ": "]);

        var (num, values) = parser.Parse<string, int[]>("Game: 1: 1, 2, 3, 4");
        num.Should().Be("1");
        values.Should().BeEquivalentTo([1, 2, 3, 4]);
    }

    private class SingleParameterTestClass
    {
        public readonly Coordinate2D Coordinate;

        [ComplexParserTarget("coord")]
        public SingleParameterTestClass(long[] coords)
        {
            Coordinate = new Coordinate2D(coords[0], coords[1]);
        }
    }

    private class ParsingTestClass
    {
        public readonly Coordinate2D Begin;
        public readonly Coordinate2D Middle;
        public readonly Coordinate2D End;

        [ComplexParserTarget("begin -> middle -> endx,endy")]
        public ParsingTestClass(long[] begin, Coordinate2D middle, long endX, long endY)
        {
            Begin = new Coordinate2D(begin);
            Middle = middle;
            End = new Coordinate2D(endX, endY);
        }
    }

    private class NestingTestClass
    {
        public int Number;
        public ParsingTestClass[] TestClasses;

        [ComplexParserTarget("num: testClasses", ArrayDelimiters = [';'])]
        public NestingTestClass(int number, ParsingTestClass[] testClasses)
        {
            Number = number;
            TestClasses = testClasses;
        }
    }

    private class FactoryMethodClass
    {
        public Dictionary<int, string> Values { get; set; }

        [ComplexParserTarget("num: values")]
        public static FactoryMethodClass FactoryMethod(int number, string[] values)
        {
            var dict = new Dictionary<int, string>();
            foreach (var value in values)
            {
                dict.Add(number++, new string(value.Reverse().ToArray()));
            }

            return new FactoryMethodClass(dict);
        }

        public FactoryMethodClass(Dictionary<int, string> values)
        {
            Values = values;
        }
    }
}