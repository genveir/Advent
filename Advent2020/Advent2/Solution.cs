using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent2
{
    public class Solution : ISolution
    {
        List<ParsedInput> passwords;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<int, int, char, string>("min-max letter: password");

            passwords = lines.Select(line =>
            {
                var pi = new ParsedInput();
                (pi.minimum, pi.maximum, pi.letter, pi.password) = inputParser.Parse(line);
                return pi;
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public int minimum;
            public int maximum;
            public char letter;
            public string password;

            public bool Validate()
            {
                int charCount = password.Where(c => c == letter).Count();

                if (charCount < minimum) return false;
                if (charCount > maximum) return false;
                return true;
            }

            public bool ValidateP2()
            {
                var minVal = password[minimum - 1] == letter ? 1 : 0;
                var maxVal = password[maximum - 1] == letter ? 1 : 0;

                return minVal + maxVal == 1;
            }
        }


        public object GetResult1()
        {
            var validPws = passwords.Where(pw => pw.Validate());

            return validPws.Count().ToString();
        }

        public object GetResult2()
        {
            var validPws = passwords.Where(pw => pw.ValidateP2());

            return validPws.Count().ToString();
        }
    }
}
