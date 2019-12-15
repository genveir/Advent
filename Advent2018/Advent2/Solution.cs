using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent2
{
    class Solution : ISolution
    {
        private List<char[]> GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent2018.Advent2.Input.txt");

            var vals = new List<char[]>();

            using (var txt = new StreamReader(input))
            {
                while (!txt.EndOfStream)
                    vals.Add(txt.ReadLine().ToCharArray());
            }

            return vals;
        }

        public long GetChecksum()
        {
            var vals = GetInput();

            long twos = 0;
            long threes = 0;

            for(int n = 0; n < vals.Count; n++)
            {
                var val = vals[n];

                bool foundTwo = false;
                bool foundThree = false;
                for (int i = 0; i < 26; i++)
                {
                    var c = ((int)'a') + i;
                    var count = val.Where(p => p == c).Count();

                    if (count == 2 && !foundTwo) { twos++; foundTwo = true; }
                    if (count == 3 && !foundThree) { threes++; foundThree = true; }
                }
            }

            return twos * threes;
        }

        public string GetMatchingChars()
        {
            var vals = GetInput();

            for (int n = 0; n < vals.Count; n++)
            {
                var testCase = vals[n];

                for (int i = n+1; i < vals.Count; i++)
                {
                    int differences = 0;
                    int diffIndex = 0;

                    for (int ch = 0; ch < testCase.Length; ch++)
                    {
                        if (testCase[ch] != vals[i][ch])
                        {
                            differences++;
                            diffIndex = ch;
                        }
                    }

                    if (differences == 1)
                    {
                        Console.WriteLine("line " + (n + 1) + " diff at " + diffIndex);
                        string output = "";
                        for (int ch = 0; ch < testCase.Length; ch++ )
                        {
                            if (ch == diffIndex) continue;
                            output += testCase[ch];
                        }
                        return output;
                    }
                }
            }

            return null;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetChecksum());
            Console.WriteLine("part2: " + GetMatchingChars());
        }
    }
}
