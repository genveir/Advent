using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared
{
    public class InputParser
    {
        internal SimpleParser simpleParser;

        public char[] ArrayDelimiters { get => simpleParser.ArrayDelimiters; set => simpleParser.ArrayDelimiters = value; }
        public int NumberOfValues { get => simpleParser.NumberOfValues; set => simpleParser.NumberOfValues = value; }
        public bool EmptyArrayDelimiter { get => simpleParser.EmptyArrayDelimiter; set => simpleParser.EmptyArrayDelimiter = value; }

        public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        {
            simpleParser = new SimpleParser(startsWithValue, numberOfValues, delimiters);
        }

        public InputParser(string pattern)
        {
            simpleParser = new SimpleParser(pattern);
        }

        public dynamic Parse(string input) => simpleParser.Parse(input);

        public List<T1> Parse<T1>(IEnumerable<string> inputs) => inputs.Select(Parse<T1>).ToList();
        public T1 Parse<T1>(string input)
        {
            if (simpleParser.CanConvert(typeof(T1))) return simpleParser.Parse<T1>(input);
            else
            {
                return new ComplexParser(simpleParser).Parse<T1>(input);
            }
        }

        public List<(T1, T2)> Parse<T1, T2>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2>).ToList();
        public (T1, T2) Parse<T1, T2>(string input) => simpleParser.Parse<T1, T2>(input);

        public List<(T1, T2, T3)> Parse<T1, T2, T3>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3>).ToList();
        public (T1, T2, T3) Parse<T1, T2, T3>(string input) => simpleParser.Parse<T1, T2, T3>(input);

        public List<(T1, T2, T3, T4)> Parse<T1, T2, T3, T4>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4>).ToList();
        public (T1, T2, T3, T4) Parse<T1, T2, T3, T4>(string input) => simpleParser.Parse<T1, T2, T3, T4>(input);

        public List<(T1, T2, T3, T4, T5)> Parse<T1, T2, T3, T4, T5>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5>).ToList();
        public (T1, T2, T3, T4, T5) Parse<T1, T2, T3, T4, T5>(string input) => simpleParser.Parse<T1, T2, T3, T4, T5>(input);

        public List<(T1, T2, T3, T4, T5, T6)> Parse<T1, T2, T3, T4, T5, T6>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5, T6>).ToList();
        public (T1, T2, T3, T4, T5, T6) Parse<T1, T2, T3, T4, T5, T6>(string input) => simpleParser.Parse<T1, T2, T3, T4, T5, T6>(input);

        public List<(T1, T2, T3, T4, T5, T6, T7)> Parse<T1, T2, T3, T4, T5, T6, T7>(IEnumerable<string> inputs) => inputs.Select(Parse<T1, T2, T3, T4, T5, T6, T7>).ToList();
        public (T1, T2, T3, T4, T5, T6, T7) Parse<T1, T2, T3, T4, T5, T6, T7>(string input) => simpleParser.Parse<T1, T2, T3, T4, T5, T6, T7>(input);
    }

    public class InputParser<T1> : InputParser
    {
        public InputParser(bool startsWithValue, int numberOfValues, params string[] delimiters)
            : base(startsWithValue, numberOfValues, delimiters)
        {
            if (simpleParser.CanConvert(typeof(T1)))
            {
                if (NumberOfValues != 1) throw new NotImplementedException("number of values does not match number of type arguments");
            }
        }

        public InputParser(string pattern) : base(pattern)
        {
            if (simpleParser.CanConvert(typeof(T1)))
            {
                if (NumberOfValues != 1) throw new NotImplementedException("number of values does not match number of type arguments");
            }
        }

        public List<T1> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();
        new public T1 Parse(string input)
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
        new public ValueTuple<T1, T2> Parse(string input)
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
        new public ValueTuple<T1, T2, T3> Parse(string input)
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
        new public ValueTuple<T1, T2, T3, T4> Parse(string input)
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
        new public ValueTuple<T1, T2, T3, T4, T5> Parse(string input)
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
        new public ValueTuple<T1, T2, T3, T4, T5, T6> Parse(string input)
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
        new public ValueTuple<T1, T2, T3, T4, T5, T6, T7> Parse(string input)
        {
            return Parse<T1, T2, T3, T4, T5, T6, T7>(input);
        }
    }
}
