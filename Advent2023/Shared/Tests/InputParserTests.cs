using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Advent2023.Shared.InputParsing;
using FluentAssertions;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace Advent2023.Shared.Tests;

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
        var parser = new InputParser("line");
        var testClass = parser.Parse<ParsingTestClass>("1,2 -> 3,4 -> 10,11");

        testClass.Should()
            .BeEquivalentTo(new ParsingTestClass(new long[] { 1, 2 }, new Coordinate2D(3, 4), 10, 11));
    }

    [Test]
    public void CanParseNestedObjects()
    {
        var parser = new InputParser("num |- testclass");
        var (num, nestedClass) = parser.Parse<int, NestingTestClass>("5 |- 1: 1,2 -> 3,4 -> 10,11; 2,3 -> 4,5 -> 12,13");

        num.Should().Be(5);

        nestedClass.Number.Should().Be(1);
        nestedClass.TestClasses.Should().HaveCount(2);

        nestedClass.TestClasses.First().Should()
            .BeEquivalentTo(new ParsingTestClass(new long[] { 1, 2 }, new Coordinate2D(3, 4), 10, 11));
        nestedClass.TestClasses.Last().Should()
            .BeEquivalentTo(new ParsingTestClass(new long[] { 2, 3 }, new Coordinate2D(4, 5), 12, 13));
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

    // ReSharper disable ClassNeverInstantiated.Local
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

        [ComplexParserTarget("num: testClasses", ArrayDelimiters = new[] { ';' })]
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
