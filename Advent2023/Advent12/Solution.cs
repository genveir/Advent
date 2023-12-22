using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent12;

public class Solution : ISolution
{
    public List<ParsedInput> modules;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        modules = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class ParsedInput
    {
        public string Pattern { get; set; }
        public int[] Values { get; set; }

        [ComplexParserConstructor("pattern values")]
        public ParsedInput(string pattern, int[] values)
        {
            Pattern = pattern;
            Values = values;
        }

        public string BigPattern => $"{Pattern}?{Pattern}?{Pattern}?{Pattern}?{Pattern}";
        public int[] BigValues { 
            get
            {
                var newValues = new int[Values.Length * 5];
                for (int n = 0; n < 5; n++)
                    Array.Copy(Values, 0, newValues, n * Values.Length, Values.Length);

                return newValues;
            } 
        }
    }

    Dictionary<string, long> options = new();
    public long FindOptions(string pattern, int[] values)
    {
        if (values.Length == 0)
        {
            return pattern.Contains('#') ? 0 : 1;
        }
        if (pattern.Length == 0)
        {
            return values.Length == 0 ? 1 : 0;
        }
        if (values.Sum() + values.Count() - 1 > pattern.Length)
            return 0;

        var key = $"{pattern}_{string.Join(",", values)}";
        if (!options.ContainsKey(key)) 
        {
            long numOptions;
            if (pattern.StartsWith('#'))
            {
                if (CanPlace(pattern, values[0]))
                    numOptions = FindOptions(CutPattern(pattern, values[0] + 1), values.Skip(1).ToArray());
                else return 0;
            }
            else if (pattern.StartsWith('?'))
            {
                long dontPlaceOptions = FindOptions(CutPattern(pattern, 1), values);

                long placeOptions = 0;
                if (CanPlace(pattern, values[0]))
                    placeOptions = FindOptions(CutPattern(pattern, values[0] + 1), values.Skip(1).ToArray());

                numOptions = dontPlaceOptions + placeOptions;
            }
            else
            {
                numOptions = FindOptions(pattern.Substring(1), values);
            }

            options.Add(key, numOptions);
        }

        return options[key];
    }

    public static bool CanPlace(string pattern, int length) =>
        pattern.Length >= length &&
        pattern.Substring(0, length).All(c => c is '#' or '?') &&
        NextIsNotRequired(pattern, length);

    public static bool NextIsNotRequired(string pattern, int length) =>
        pattern.Length == length ||
        pattern[length] != '#';

    public static string CutPattern(string pattern, int length) =>
        pattern.Length <= length ? "" : pattern.Substring(length).TrimStart('.');

    public object GetResult1()
    {
        return modules.Sum(m => FindOptions(m.Pattern, m.Values));
    }

    public object GetResult2()
    {
        return modules.Sum(m => FindOptions(m.BigPattern, m.BigValues));
    }
}
