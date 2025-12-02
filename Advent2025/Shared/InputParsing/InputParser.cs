namespace Advent2025.Shared.InputParsing;

public class InputParser
{
    internal SimpleParser simpleParser;

    public char[] ArrayDelimiters { get => simpleParser.ArrayDelimiters; set => simpleParser.ArrayDelimiters = value; }
    public int NumberOfValues { get => simpleParser.NumberOfValues; set => simpleParser.NumberOfValues = value; }
    public bool EmptyArrayDelimiter { get => simpleParser.EmptyArrayDelimiter; set => simpleParser.EmptyArrayDelimiter = value; }
    public bool ShouldTrimBeforeParsing { get => simpleParser.ShouldTrimBeforeParsing; set => simpleParser.ShouldTrimBeforeParsing = value; }
    public char PatternEscapeChar { get => simpleParser.PatternEscapeChar; set => simpleParser.PatternEscapeChar = value; }

    public InputParser(bool startsWithValue, int numberOfValues, params string[] delimiters) :
        this(startsWithValue, numberOfValues, (IEnumerable<string>)delimiters)
    { }

    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
    {
        simpleParser = new SimpleParser(startsWithValue, numberOfValues, delimiters);
    }

    public InputParser(string pattern)
    {
        simpleParser = new SimpleParser(pattern);
    }

    private static bool CheckValues(params Type[] types)
    {
        // hacky but simple
        foreach (var type in types)
        {
            if (!SimpleParser.CanConvert(type))
                throw new InvalidOperationException($"Parser cannot convert type {type.Name}");
        }

        return true;
    }

    public dynamic Parse(string input) => simpleParser.Parse(input);

    public List<T1> Parse<T1>(IEnumerable<string> inputs) => inputs.Select(Parse<T1>).ToList();

    public T1 Parse<T1>(string input) =>
        CheckValues(typeof(T1)) ?
        simpleParser.Parse<T1>(input) : default;

    public List<(T1, T2)> Parse<T1, T2>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2>).ToList();

    public (T1, T2) Parse<T1, T2>(string input) =>
        CheckValues(typeof(T1), typeof(T2)) ?
        simpleParser.Parse<T1, T2>(input) : default;

    public List<(T1, T2, T3)> Parse<T1, T2, T3>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3>).ToList();

    public (T1, T2, T3) Parse<T1, T2, T3>(string input) =>
        CheckValues(typeof(T1), typeof(T2), typeof(T3)) ?
        simpleParser.Parse<T1, T2, T3>(input) : default;

    public List<(T1, T2, T3, T4)> Parse<T1, T2, T3, T4>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4>).ToList();

    public (T1, T2, T3, T4) Parse<T1, T2, T3, T4>(string input) =>
        CheckValues(typeof(T1), typeof(T2), typeof(T3), typeof(T4)) ?
        simpleParser.Parse<T1, T2, T3, T4>(input) : default;

    public List<(T1, T2, T3, T4, T5)> Parse<T1, T2, T3, T4, T5>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5>).ToList();

    public (T1, T2, T3, T4, T5) Parse<T1, T2, T3, T4, T5>(string input) =>
        CheckValues(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)) ?
        simpleParser.Parse<T1, T2, T3, T4, T5>(input) : default;

    public List<(T1, T2, T3, T4, T5, T6)> Parse<T1, T2, T3, T4, T5, T6>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5, T6>).ToList();

    public (T1, T2, T3, T4, T5, T6) Parse<T1, T2, T3, T4, T5, T6>(string input) =>
        CheckValues(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)) ?
        simpleParser.Parse<T1, T2, T3, T4, T5, T6>(input) : default;

    public List<(T1, T2, T3, T4, T5, T6, T7)> Parse<T1, T2, T3, T4, T5, T6, T7>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5, T6, T7>).ToList();

    public (T1, T2, T3, T4, T5, T6, T7) Parse<T1, T2, T3, T4, T5, T6, T7>(string input) =>
        CheckValues(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)) ?
        simpleParser.Parse<T1, T2, T3, T4, T5, T6, T7>(input) : default;
}

public class InputParser<T1> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, params string[] delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 1) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 1) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<T1> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new T1 Parse(string input)
    {
        return Parse<T1>(input);
    }
}

public class InputParser<T1, T2> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 2) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 2) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2> Parse(string input)
    {
        return Parse<T1, T2>(input);
    }
}

public class InputParser<T1, T2, T3> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 3) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 3) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2, T3)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2, T3> Parse(string input)
    {
        return Parse<T1, T2, T3>(input);
    }
}

public class InputParser<T1, T2, T3, T4> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 4) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 4) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2, T3, T4)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2, T3, T4> Parse(string input)
    {
        return Parse<T1, T2, T3, T4>(input);
    }
}

public class InputParser<T1, T2, T3, T4, T5> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 5) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 5) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2, T3, T4, T5)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2, T3, T4, T5> Parse(string input)
    {
        return Parse<T1, T2, T3, T4, T5>(input);
    }
}

public class InputParser<T1, T2, T3, T4, T5, T6> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 6) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 6) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2, T3, T4, T5, T6)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2, T3, T4, T5, T6> Parse(string input)
    {
        return Parse<T1, T2, T3, T4, T5, T6>(input);
    }
}

public class InputParser<T1, T2, T3, T4, T5, T6, T7> : InputParser
{
    public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        : base(startsWithValue, numberOfValues, delimiters)
    {
        if (NumberOfValues != 7) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public InputParser(string pattern) : base(pattern)
    {
        if (NumberOfValues != 7) throw new NotImplementedException("number of values does not match number of type arguments");
    }

    public List<(T1, T2, T3, T4, T5, T6, T7)> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();

    public new ValueTuple<T1, T2, T3, T4, T5, T6, T7> Parse(string input)
    {
        return Parse<T1, T2, T3, T4, T5, T6, T7>(input);
    }
}