using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent5
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            modules = lines.Select(line =>
            {
                var pi = new ParsedInput(line);
                
                return pi;
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public string data;
            public long row;
            public long column;

            public ParsedInput(string data)
            {
                row = 0;
                int rowStep = 64;
                column = 0;
                int columnStep = 4;

                for (int n = 0; n < data.Length; n++)
                {
                    switch (data[n])
                    {
                        case 'F': rowStep /= 2; break;
                        case 'B': row = row + rowStep; rowStep /= 2; break;
                        case 'L': columnStep /= 2; break;
                        case 'R': column = column + columnStep; columnStep /= 2; break;
                    }
                }
            }

            public long seatId()
            {
                return row * 8 + column;
            }
        }

        public object GetResult1()
        {
            return modules.Select(m => m.seatId()).Max().ToString();
        }

        public object GetResult2()
        {
            var sorted = modules.OrderBy(m => m.seatId()).ToList();

            long last = -1000;
            for (int n = 0; n < sorted.Count(); n++)
            {
                var val = sorted[n].seatId();

                if (last == (val - 2)) return (val - 1).ToString();

                last = val;
            }

            return "no solution";
        }
    }
}
