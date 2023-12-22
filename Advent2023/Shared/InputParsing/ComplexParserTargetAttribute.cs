using System;
using System.Collections.Generic;

namespace Advent2023.Shared.InputParsing;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class ComplexParserTargetAttribute : Attribute
{
    public char[] ArrayDelimiters { get => InputParser.ArrayDelimiters; set => InputParser.ArrayDelimiters = value; }
    public int NumberOfValues { get => InputParser.NumberOfValues; set => InputParser.NumberOfValues = value; }
    public bool EmptyArrayDelimiter { get => InputParser.EmptyArrayDelimiter; set => InputParser.EmptyArrayDelimiter = value; }
    public bool ShouldTrimBeforeParsing { get => InputParser.ShouldTrimBeforeParsing; set => InputParser.ShouldTrimBeforeParsing = value; }

    public InputParser InputParser { get; set; }

    public ComplexParserTargetAttribute(string pattern)
    {
        InputParser = new InputParser(pattern);
    }

    public ComplexParserTargetAttribute(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
    {
        InputParser = new InputParser(startsWithValue, numberOfValues, delimiters);
    }
}
