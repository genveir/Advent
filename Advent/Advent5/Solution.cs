using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent5
{
    class Solution : ISolution
    {
        private string GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent.Advent5.Input.txt");

            using (var txt = new StreamReader(input))
            {
                return txt.ReadToEnd();
            }
        }

        public string GetResult(string input)
        {
            input = input ?? GetInput();

            long replaces = 0;
            do
            {
                replaces = 0;
                for (int n = 0; n < 26; n++)
                {
                    char c = (char)('a' + n);
                    char C = (char)('A' + n);

                    if (input.Contains("" + c + C) || input.Contains("" + C + c)) replaces = 1;
                    input = input.Replace("" + c + C, "");
                    input = input.Replace("" + C + c, "");
                }

            } while (replaces != 0);

            return input;
        }

        public int GetResult2()
        {
            var input = GetInput();

            var lengths = new List<int>();
            for (int n = 0; n < 26; n++)
            {
                char c = (char)('a' + n);

                var i2 = input.Replace("" + c, "", StringComparison.InvariantCultureIgnoreCase);

                lengths.Add(GetResult(i2).Length);
            }

            lengths.Sort();
            return lengths.First();
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetResult(null).Length);
            Console.WriteLine("part2: " + GetResult2());
        }
    }
}
