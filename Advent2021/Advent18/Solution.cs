using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent18
{
    public class Solution : ISolution
    {
        public List<WholeValue> wholeValues;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            wholeValues = lines.Select(l => Parser.ParseLine(l)).ToList();
        }
        public Solution() : this("Input.txt") { }

        public WholeValue AggregateAdd() => wholeValues.Aggregate((a, b) => WholeValue.Add(a, b));

        public object GetResult1()
        {
            return AggregateAdd().Magnitude;
        }

        public object GetResult2()
        {
            long highest = 0;
            for (int n = 0; n < wholeValues.Count; n++)
            {
                var wv1 = wholeValues[n];
                for (int i = 0; i < wholeValues.Count; i++)
                {                    
                    if (n == i) continue;

                    var wv2 = wholeValues[i];
                    var mag = WholeValue.Add(wv1, wv2).Magnitude;

                    if (mag > highest) highest = mag;
                }
            }

            return highest;
        }
    }
}
