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

            passwords = ParsedInput.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public int minimum;
            public int maximum;
            public char letter;
            public string password;

            public static List<ParsedInput> Parse(IEnumerable<string> lines)
            {
                var parsedInputs = new List<ParsedInput>();

                foreach(var line in lines)
                {
                    var splitline = line.Split(new char[] { ' ', '-', ':' }, StringSplitOptions.RemoveEmptyEntries);

                    var pi = new ParsedInput()
                    {
                        minimum = int.Parse(splitline[0]),
                        maximum = int.Parse(splitline[1]),
                        letter = splitline[2].ToCharArray().Single(),
                        password = splitline[3].PadRight(30, '*')
                    };

                    parsedInputs.Add(pi);
                }

                return parsedInputs;
            }

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


        public string GetResult1()
        {
            var validPws = passwords.Where(pw => pw.Validate());

            return validPws.Count().ToString();
        }

        public string GetResult2()
        {
            var validPws = passwords.Where(pw => pw.ValidateP2());

            return validPws.Count().ToString();
        }
    }
}
