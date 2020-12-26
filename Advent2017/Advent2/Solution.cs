using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent2
{
    public class Solution : ISolution
    {
        long[][] rows;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            rows = lines.Select(line => line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)).Select(longs => longs.Select(l => long.Parse(l)).ToArray()).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            long sum = 0;
            foreach (var row in rows) sum += (row.Max() - row.Min());

            return sum;
        }

        public object GetResult2()
        {
            long sum = 0;
            foreach (var row in rows)
            {
                for (int first = 0; first < row.Length; first++)
                {
                    for (int second = first + 1; second < row.Length; second++)
                    {
                        long larger, smaller;
                        if (row[first] >= row[second]) { larger = row[first]; smaller = row[second]; }
                        else { larger = row[second]; smaller = row[first]; }

                        var div = larger / smaller;
                        if (smaller * div == larger) sum += div;
                    }
                }
            }
            return sum;
        }
    }
}
