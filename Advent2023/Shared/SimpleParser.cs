using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Advent2023.Shared;

public class SimpleParser
{
    private bool startsWithValue;
    private int numberOfValues;
    public int NumberOfValues
    {
        get { return numberOfValues; }
        set
        {
            if (value <= 0) throw new NotImplementedException("can't build a parser for 0 or fewer values");
            if (value > 7) throw new NotImplementedException("can't build a parser for more than 7 values yet");
            numberOfValues = value;
        }
    }
    public string[] delimiters;

    public bool ShouldTrimBeforeParsing { get; set; } = true;

    public SimpleParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
    {
        this.startsWithValue = startsWithValue;
        this.NumberOfValues = numberOfValues;
        this.delimiters = delimiters.ToArray();
    }

    public SimpleParser(string pattern)
    {
        var numberOfValues = 0;
        var delimiters = new List<string>();

        var hasCurrentValue = false;
        var currentDelimiter = new List<char>();
        bool startSet = false;
        foreach (var c in pattern)
        {
            bool isText = false;
            if (c >= 97 && c <= 122) isText = true;
            if (c >= 65 && c <= 90) isText = true;

            if (isText)
            {
                if (currentDelimiter.Count > 0)
                {
                    delimiters.Add(new string(currentDelimiter.ToArray()));
                    currentDelimiter = new List<char>();
                }
                hasCurrentValue = true;
                if (!startSet) startsWithValue = true;
            }
            else
            {
                if (hasCurrentValue)
                {
                    numberOfValues++;
                    hasCurrentValue = false;
                }
                currentDelimiter.Add(c);
                if (!startSet) startsWithValue = false;
            }

            startSet = true;
        }

        if (currentDelimiter.Count > 0) delimiters.Add(new string(currentDelimiter.ToArray()));
        if (hasCurrentValue) numberOfValues++;

        this.NumberOfValues = numberOfValues;
        this.delimiters = delimiters.ToArray();
    }

    public dynamic Parse(string input)
    {
        if (ShouldTrimBeforeParsing) input = input.Trim();

        Type vtType = GetVTType();

        var instance = Activator.CreateInstance(vtType);

        if (!startsWithValue) input = input.Substring(this.delimiters[0].Length);
        for (int valueIndex = 0; valueIndex < NumberOfValues; valueIndex++)
        {
            int delimiterIndex = valueIndex + (startsWithValue ? 0 : 1);

            string val;
            if (this.delimiters.Length <= delimiterIndex) val = input;
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
        ValueTuple<T1, T2> output = new ValueTuple<T1, T2>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);

        return output;
    }

    public ValueTuple<T1, T2, T3> Parse<T1, T2, T3>(string input)
    {
        ValueTuple<string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3> output = new ValueTuple<T1, T2, T3>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);
        output.Item3 = (T3)Convert<T3>(halfway.Item3);

        return output;
    }

    public ValueTuple<T1, T2, T3, T4> Parse<T1, T2, T3, T4>(string input)
    {
        ValueTuple<string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4> output = new ValueTuple<T1, T2, T3, T4>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);
        output.Item3 = (T3)Convert<T3>(halfway.Item3);
        output.Item4 = (T4)Convert<T4>(halfway.Item4);

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5> Parse<T1, T2, T3, T4, T5>(string input)
    {
        ValueTuple<string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5> output = new ValueTuple<T1, T2, T3, T4, T5>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);
        output.Item3 = (T3)Convert<T3>(halfway.Item3);
        output.Item4 = (T4)Convert<T4>(halfway.Item4);
        output.Item5 = (T5)Convert<T5>(halfway.Item5);

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5, T6> Parse<T1, T2, T3, T4, T5, T6>(string input)
    {
        ValueTuple<string, string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5, T6> output = new ValueTuple<T1, T2, T3, T4, T5, T6>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);
        output.Item3 = (T3)Convert<T3>(halfway.Item3);
        output.Item4 = (T4)Convert<T4>(halfway.Item4);
        output.Item5 = (T5)Convert<T5>(halfway.Item5);
        output.Item6 = (T6)Convert<T6>(halfway.Item6);

        return output;
    }

    public ValueTuple<T1, T2, T3, T4, T5, T6, T7> Parse<T1, T2, T3, T4, T5, T6, T7>(string input)
    {
        ValueTuple<string, string, string, string, string, string, string> halfway = Parse(input);
        ValueTuple<T1, T2, T3, T4, T5, T6, T7> output = new ValueTuple<T1, T2, T3, T4, T5, T6, T7>();
        output.Item1 = (T1)Convert<T1>(halfway.Item1);
        output.Item2 = (T2)Convert<T2>(halfway.Item2);
        output.Item3 = (T3)Convert<T3>(halfway.Item3);
        output.Item4 = (T4)Convert<T4>(halfway.Item4);
        output.Item5 = (T5)Convert<T5>(halfway.Item5);
        output.Item6 = (T6)Convert<T6>(halfway.Item6);
        output.Item7 = (T7)Convert<T7>(halfway.Item7);

        return output;
    }

    public bool CanConvert(Type t)
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
            .Where(m => m.GetCustomAttribute(typeof(ConvertArrayTargetAttribute)) != null)
            .Single()
            .MakeGenericMethod(elementType);

        return method.Invoke(this, new object[] { input });
    }

    [ConvertArrayTarget]
    private TElement[] ConvertElements<TElement>(string input)
    {
        var elementType = typeof(TElement);
        
        var elements = SplitElements(input);
        return elements
            .Select(el => (TElement)Convert<TElement>(el))
            .ToArray();
    }

    public char[] ArrayDelimiters { get; set; } = new char[] { ',' };
    public bool EmptyArrayDelimiter = false;
    private string[] SplitElements(string input)
    {
        var elements = input.Split(ArrayDelimiters, StringSplitOptions.RemoveEmptyEntries);
        if (EmptyArrayDelimiter) elements = elements.SelectMany(el => el.ToCharArray().Select(c => c.ToString())).ToArray();

        return elements;
    }

    private Type GetVTType()
    {
        switch (NumberOfValues)
        {
            case 1: return typeof(ValueTuple<string>);
            case 2: return typeof(ValueTuple<string, string>);
            case 3: return typeof(ValueTuple<string, string, string>);
            case 4: return typeof(ValueTuple<string, string, string, string>);
            case 5: return typeof(ValueTuple<string, string, string, string, string>);
            case 6: return typeof(ValueTuple<string, string, string, string, string, string>);
            case 7: return typeof(ValueTuple<string, string, string, string, string, string, string>);
            default: throw new NotImplementedException("can't go over 7 fields yet");
        }
    }

    private class ConvertArrayTargetAttribute : Attribute
    {
    }
}

