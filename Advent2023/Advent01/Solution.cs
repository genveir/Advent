using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent01;

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
        public int[] FirstNumber { get; private set; } = new int[] { -1, -1 };
        public int[] LastNumber { get; private set; } = new int[2];

        public int Value => 10 * FirstNumber[0] + LastNumber[0];
        public int Value2 => 10 * FirstNumber[1] + LastNumber[1];

        [ComplexParserConstructor]
        public ParsedInput(string line)
        {
            var padded = line + "     ";

            var numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int n = 0; n < line.Length; n++)
            {
                if (numbers.Contains(line[n]))
                {
                    Set(line[n] - '0', false);
                }
                else
                {
                    CheckNum(padded, n, "one", 1);
                    CheckNum(padded, n, "two", 2);
                    CheckNum(padded, n, "three", 3);
                    CheckNum(padded, n, "four", 4);
                    CheckNum(padded, n, "five", 5);
                    CheckNum(padded, n, "six", 6);
                    CheckNum(padded, n, "seven", 7);
                    CheckNum(padded, n, "eight", 8);
                    CheckNum(padded, n, "nine", 9);
                }
            }
        }

        private void Set(int number, bool p2)
        {
            if (FirstNumber[1] == -1)
                FirstNumber[1] = number;
            LastNumber[1] = number;

            if (!p2)
            {
                if (FirstNumber[0] == -1)
                    FirstNumber[0] = number;
                LastNumber[0] = number;
            }
        }

        private void CheckNum(string line, int index, string number, int value) 
        { 
            if (line.Substring(index, number.Length) == number)
            {
                Set(value, true);
            }
        }
    }

    public object GetResult1()
    {
        return modules.Sum(m => m.Value);
    }

    public object GetResult2()
    {
        return modules.Sum(m => m.Value2);
    }
}
