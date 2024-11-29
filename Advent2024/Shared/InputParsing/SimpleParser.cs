using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Advent2024.Shared.InputParsing;

public class SimpleParser
{
    private readonly string pattern;

    private bool parseDone;

    private bool startsWithValue;
    private int numberOfValues;

    public int NumberOfValues
    {
        get
        {
            if (!parseDone) ParsePattern();
            return numberOfValues;
        }
        set
        {
            if (value <= 0) throw new NotImplementedException("can't build a parser for 0 or fewer values");
            if (value > 7) throw new NotImplementedException("can't build a parser for more than 7 values yet");
            numberOfValues = value;
        }
    }

    public string[] delimiters;

    private bool shouldTrimBeforeParsing = true;

    public bool ShouldTrimBeforeParsing
    {
        get
        {
            if (!parseDone) ParsePattern();
            return shouldTrimBeforeParsing;
        }
        set => shouldTrimBeforeParsing = true;
    }

    public SimpleParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
    {
        this.startsWithValue = startsWithValue;
        NumberOfValues = numberOfValues;
        this.delimiters = delimiters.ToArray();

        parseDone = true;
    }

    private char patternEscapeChar = '\\';

    public char PatternEscapeChar
    {
        get
        {
            if (!parseDone) ParsePattern();
            return patternEscapeChar;
        }
        set => patternEscapeChar = value;
    }

    public SimpleParser(string pattern)
    {
        this.pattern = pattern ??
            throw new ArgumentNullException(nameof(pattern));
        parseDone = false;
    }

    public void ParsePattern()
    {
        parseDone = true;

        var numberOfValues = 0;
        var delimiters = new List<string>();

        var hasCurrentValue = false;
        var currentDelimiter = new List<char>();
        bool startSet = false;
        bool escaped = false;

        void AddToDelimiter(char c)
        {
            if (hasCurrentValue)
            {
                numberOfValues++;
                hasCurrentValue = false;
            }
            currentDelimiter.Add(c);
        }

        foreach (var c in pattern)
        {
            bool isText = false;
            if (c == PatternEscapeChar)
            {
                escaped = true;
                continue;
            }
            if (c >= 97 && c <= 122) isText = true;
            if (c >= 65 && c <= 90) isText = true;

            if (isText && !escaped)
            {
                if (currentDelimiter.Count > 0)
                {
                    delimiters.Add(new string([.. currentDelimiter]));
                    currentDelimiter = [];
                }
                hasCurrentValue = true;
                if (!startSet) startsWithValue = true;
            }
            else if (isText && escaped)
            {
                AddToDelimiter(c);
                if (!startSet) startsWithValue = false;
            }
            else
            {
                escaped = false;

                AddToDelimiter(c);
                if (!startSet) startsWithValue = false;
            }

            startSet = true;
        }

        if (currentDelimiter.Count > 0) delimiters.Add(new string([.. currentDelimiter]));
        if (hasCurrentValue) numberOfValues++;

        NumberOfValues = numberOfValues;
        this.delimiters = [.. delimiters];
    }

    public dynamic Parse(string input)
    {
        if (!parseDone) ParsePattern();

        if (ShouldTrimBeforeParsing) input = input.Trim();

        Type vtType = GetVTType();

        var instance = Activator.CreateInstance(vtType);

        if (!startsWithValue) input = input[delimiters[0].Length..];
        for (int valueIndex = 0; valueIndex < NumberOfValues; valueIndex++)
        {
            int delimiterIndex = valueIndex + (startsWithValue ? 0 : 1);

            string val;
            if (delimiters.Length <= delimiterIndex) val = input;
            else
            {
                var split = input.Split(delimiters[delimiterIndex], 2);
                val = split[0];
                input = split[1];
            }

            vtType.GetField("Item" + (valueIndex + 1)).SetValue(instance, val);
        }

        return instance;
    }

    public T1 Parse<T1>(string input)
    {
        ValueTuple<string> halfway = Parse(input);
        T1 output = (T1)Convert<T1>(halfway.Item1);

        return output;
    }

    public ValueTuple<T1, T2> Parse<T1, T2>(string input)
    {
        ValueTuple<string, string> halfway = Parse(input);
        ValueTuple<T1, T2> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2)
        };

        return output;
    }

    public ValueTuple<T1, T2, T3> Parse<T1, T2, T3>(string input)
    {
        ValueTuple<string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2),
            Item3 = (T3)Convert<T3>(halfway.Item3)
        };

        return output;
    }

    public ValueTuple<T1, T2, T3, T4> Parse<T1, T2, T3, T4>(string input)
    {
        ValueTuple<string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2),
            Item3 = (T3)Convert<T3>(halfway.Item3),
            Item4 = (T4)Convert<T4>(halfway.Item4)
        };

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5> Parse<T1, T2, T3, T4, T5>(string input)
    {
        ValueTuple<string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2),
            Item3 = (T3)Convert<T3>(halfway.Item3),
            Item4 = (T4)Convert<T4>(halfway.Item4),
            Item5 = (T5)Convert<T5>(halfway.Item5)
        };

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5, T6> Parse<T1, T2, T3, T4, T5, T6>(string input)
    {
        ValueTuple<string, string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5, T6> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2),
            Item3 = (T3)Convert<T3>(halfway.Item3),
            Item4 = (T4)Convert<T4>(halfway.Item4),
            Item5 = (T5)Convert<T5>(halfway.Item5),
            Item6 = (T6)Convert<T6>(halfway.Item6)
        };

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5, T6, T7> Parse<T1, T2, T3, T4, T5, T6, T7>(string input)
    {
        ValueTuple<string, string, string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> output = new()
        {
            Item1 = (T1)Convert<T1>(halfway.Item1),
            Item2 = (T2)Convert<T2>(halfway.Item2),
            Item3 = (T3)Convert<T3>(halfway.Item3),
            Item4 = (T4)Convert<T4>(halfway.Item4),
            Item5 = (T5)Convert<T5>(halfway.Item5),
            Item6 = (T6)Convert<T6>(halfway.Item6),
            Item7 = (T7)Convert<T7>(halfway.Item7)
        };

        return output;
    }

    public static bool CanConvert(Type t)
    {
        if (t.IsArray) return CanConvert(t.GetElementType());

        if (t == typeof(int)) return true;
        if (t == typeof(long)) return true;
        if (t == typeof(char)) return true;
        if (t == typeof(bool)) return true;
        if (t == typeof(string)) return true;
        if (ComplexParser.CanParse(t)) return true;

        return false;
    }

    private dynamic Convert<T>(string input)
    {
        var t = typeof(T);

        if (t.IsArray) return ConvertArray<T>(input);

        if (t == typeof(int)) return int.Parse(input);
        if (t == typeof(long)) return long.Parse(input);
        if (t == typeof(char)) return char.Parse(input);
        if (t == typeof(bool)) return bool.Parse(input);
        if (t == typeof(string)) return input;
        if (ComplexParser.CanParse(t)) return new ComplexParser().Parse<T>(input);

        throw new NotImplementedException("cannot convert non-primitive types or types without marked complex parser constructors");
    }

    private dynamic ConvertArray<T>(string input)
    {
        var t = typeof(T);

        var elementType = t.GetElementType();

        MethodInfo method = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<ConvertArrayTargetAttribute>() != null)
            .Single()
            .MakeGenericMethod(elementType);

        return method.Invoke(this, [input]);
    }

#pragma warning disable IDE0051

    [ConvertArrayTarget]
    private TElement[] ConvertElements<TElement>(string input)
    {
        var elementType = typeof(TElement);

        var elements = SplitElements(input);
        return elements
            .Select(el => (TElement)Convert<TElement>(el))
            .ToArray();
    }

#pragma warning restore IDE0051

    private char[] arrayDelimiters = [','];

    public char[] ArrayDelimiters
    {
        get
        {
            if (!parseDone) ParsePattern();
            return arrayDelimiters;
        }
        set => arrayDelimiters = value;
    }

    public bool EmptyArrayDelimiter = false;

    private string[] SplitElements(string input)
    {
        var elements = input.Split(ArrayDelimiters, StringSplitOptions.RemoveEmptyEntries);
        if (EmptyArrayDelimiter) elements = elements.SelectMany(el => el.ToCharArray().Select(c => c.ToString())).ToArray();

        return elements;
    }

    private Type GetVTType() =>
        NumberOfValues switch
        {
            1 => typeof(ValueTuple<string>),
            2 => typeof(ValueTuple<string, string>),
            3 => typeof(ValueTuple<string, string, string>),
            4 => typeof(ValueTuple<string, string, string, string>),
            5 => typeof(ValueTuple<string, string, string, string, string>),
            6 => typeof(ValueTuple<string, string, string, string, string, string>),
            7 => typeof(ValueTuple<string, string, string, string, string, string, string>),
            _ => throw new NotImplementedException("can't go over 7 fields yet"),
        };

    [AttributeUsage(AttributeTargets.Method)]
    private class ConvertArrayTargetAttribute : Attribute
    {
    }
}