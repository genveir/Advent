using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent08
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<ParsedInput>("words | words");
            inputParser.ArrayDelimiters = new char[] { ' ' };

            modules = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public string[] wires;
            public string[] segments;

            [ComplexParserConstructor]
            public ParsedInput(string[] wires, string[] segments)
            {
                this.wires = wires.Select(w =>
                {
                    var orderedArray = w.OrderBy(c => c).ToArray();
                    return new string(orderedArray);
                }).ToArray();
                this.segments = segments.Select(w =>
                {
                    var orderedArray = w.OrderBy(c => c).ToArray();
                    return new string(orderedArray);
                }).ToArray();
            }

            public long Count()
            {
                return segments.Where(s => s.Length == 2 || s.Length == 3 || s.Length == 7 || s.Length == 4).Count();
            }

            public long GetValue()
            {
                var values = GetValues();

                var digits = string.Join("", segments.Select(s => values[s]));

                return long.Parse(digits);
            }

            public Dictionary<string, int> GetValues()
            {
                // 0 yes
                // 1 yes
                // 2 yes
                // 3 yes
                // 4 yes
                // 5 yes
                // 6 yes
                // 7 yes
                // 8 yes
                // 9 yes

                var allSegments = wires.Union(segments);
                Console.WriteLine(allSegments.Count());

                var one = allSegments.Single(s => s.Length == 2);
                var seven = allSegments.Single(s => s.Length == 3);
                var four = allSegments.Single(s => s.Length == 4);
                var eight = allSegments.Single(s => s.Length == 7);

                var twoThreeAndFive = allSegments.Where(s => s.Length == 5);
                var zeroSixAndNine = allSegments.Where(s => s.Length == 6);

                var cf = one;
                var acf = seven;
                var bcdf = four;
                var a = acf.Except(cf);
                var bd = bcdf.Except(cf);

                // 3
                var acdfg = twoThreeAndFive.Where(n => n.Contains(cf[0]) && n.Contains(cf[1])).Single();
                var three = acdfg;

                var twoAndFive = twoThreeAndFive.Except(new List<string>() { three });

                var adg = acdfg.Except(cf);
                var dg = adg.Except(a);

                var d = dg.Intersect(bd);
                var b = bd.Except(d);
                var g = dg.Except(d);

                // 0
                var abcefg = eight.Except(d);
                var zero = new string(abcefg.ToArray());

                var sixAndNine = zeroSixAndNine.Except(new List<string>() { zero });

                var five = twoAndFive.Where(n => n.Contains(b.Single())).Single();
                var two = twoAndFive.Except( new List<string>() { five });

                var acdeg = two.Single().ToArray();

                var e = acdeg.Except(acdfg).Except(cf);

                var six = sixAndNine.Where(n => n.Contains(e.Single())).Single();
                var nine = sixAndNine.Except(new List<string>() { six });

                var dict = new Dictionary<string, int>();
                dict.Add(zero, 0);
                dict.Add(one, 1);
                dict.Add(two.Single(), 2);
                dict.Add(three, 3);
                dict.Add(four, 4);
                dict.Add(five, 5);
                dict.Add(six, 6);
                dict.Add(seven, 7);
                dict.Add(eight, 8);
                dict.Add(nine.Single(), 9);

                return dict;
            }
        }        

        public object GetResult1()
        {
            return modules.Sum(m => m.Count());
        }

        public object GetResult2()
        {
            return modules.Sum(m => m.GetValue());
        }
    }
}
