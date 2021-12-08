using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent08
{
    public class Solution : ISolution
    {
        List<SegmentArray> display;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<SegmentArray>("words | words");
            inputParser.ArrayDelimiters = new char[] { ' ' };

            display = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class SegmentArray
        {
            public string[] signalPattern;
            public string[] outputs;

            [ComplexParserConstructor]
            public SegmentArray(string[] signalPattern, string[] outputs)
            {
                this.signalPattern = sortedStrings(signalPattern);
                this.outputs = sortedStrings(outputs);

                
            }

            private string[] sortedStrings(string[] input) =>
                input
                    .Select(sequence => sequence.OrderBy(c => c))
                    .Select(chars => new string(chars.ToArray()))
                    .ToArray();


            public long Count()
            {
                return outputs.Where(s => s.Length == 2 || s.Length == 3 || s.Length == 7 || s.Length == 4).Count();
            }

            public long GetValue()
            {
                var values = MapDigits();

                var digits = string.Join("", this.outputs.Select(s => values[s]));

                return long.Parse(digits);
            }

            public Dictionary<string, int> MapDigits()
            {
                var allDigits = signalPattern.Union(outputs).ToArray();

                var one = allDigits.Single(s => s.Length == 2);
                var seven = allDigits.Single(s => s.Length == 3);
                var four = allDigits.Single(s => s.Length == 4);
                var eight = allDigits.Single(s => s.Length == 7);

                // intersects with 1, 4, 7 and 8 in a format that works as a key
                Dictionary<string, int> intersectMapping = new Dictionary<string, int>();
                intersectMapping.Add("2336", 0);
                intersectMapping.Add("2222", 1);
                intersectMapping.Add("1225", 2);
                intersectMapping.Add("2335", 3);
                intersectMapping.Add("2424", 4);
                intersectMapping.Add("1325", 5);
                intersectMapping.Add("1326", 6);
                intersectMapping.Add("2233", 7);
                intersectMapping.Add("2437", 8);
                intersectMapping.Add("2436", 9);

                var intersectValues = allDigits.Select(digit => string.Join("", new int[]
                {
                    digit.Intersect(one).Count(),
                    digit.Intersect(four).Count(),
                    digit.Intersect(seven).Count(),
                    digit.Intersect(eight).Count()
                })).ToArray();

                var result = new Dictionary<string, int>();
                for (int n = 0; n < 10; n++) result.Add(allDigits[n], intersectMapping[intersectValues[n]]);
                return result;
            }
        }

        public object GetResult1()
        {
            return display.Sum(m => m.Count());
        }

        public object GetResult2()
        {
            return display.Sum(m => m.GetValue());
        }
    }
}
