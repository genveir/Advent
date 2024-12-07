using Advent2024.Shared;
using Advent2024.Shared.InputParsing;

namespace Advent2024.Day7;

public class Solution : ISolution
{
    public List<ParsedInput> modules;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        modules = inputParser.Parse(lines);
    }

    public Solution() : this("Input.txt")
    {
    }

    public class ParsedInput
    {
        [ComplexParserTarget("testValue: values", ArrayDelimiters = [' '])]
        public ParsedInput(long testValue, long[] values)
        {
            TestValue = testValue;
            Values = values.Reverse().ToArray();
        }

        public long TestValue { get; set; }
        public long[] Values { get; set; }

        public long[] GetPossibleValues(int index, bool withConcat)
        {
            if (index == Values.Length - 1)
            {
                return [Values[index]];
            }

            var value = Values[index];

            var fromHere = GetPossibleValues(index + 1, withConcat);

            var concatenated = fromHere.Select(
                fh => long.Parse(fh.ToString() + value.ToString()));

            var result = fromHere
                .Select(v => v + value)
                .Concat(fromHere.Select(v => v * value))
                .ToArray();

            if (withConcat)
                result = result
                    .Concat(concatenated)
                    .ToArray();

            return result;
        }

        public bool TestValueIsPossible(bool withConcat)
        {
            var possibleValues = GetPossibleValues(0, withConcat);

            return possibleValues.Any(v => v == TestValue);
        }

        public override string ToString()
        {
            return $"{TestValue}: {string.Join(' ', Values)}";
        }
    }

    public object GetResult1()
    {
        return modules
            .Where(m => m.TestValueIsPossible(false))
            .Sum(m => m.TestValue);
    }

    // not 456567254157484
    public object GetResult2()
    {
        return modules
            .Where(m => m.TestValueIsPossible(true))
            .Sum(m => m.TestValue);
    }
}