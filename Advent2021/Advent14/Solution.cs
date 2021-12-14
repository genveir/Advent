using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advent2021.Advent14
{
    public class Solution : ISolution
    {
        Dictionary<string, long> pairCounts = new Dictionary<string, long>();
        Dictionary<string, string[]> transitions = new Dictionary<string, string[]>();

        char firstPolymer;
        char lastPolymer;

        public Solution(string input)
        {
            var lines = Input.GetBlockLines(input).ToArray();

            var inputParser = new InputParser<string, string>("AB -> C");

            foreach (var rule in lines[1])
            {
                var (inputs, output) = inputParser.Parse(rule);

                var outputPairs = new string[] { inputs[0] + output , output + inputs[1] };

                transitions[inputs] = outputPairs;
            }

            for (int n = 0; n < lines[0][0].Length - 1; n++)
            {
                var pair = lines[0][0].Substring(n, 2);

                if (!pairCounts.ContainsKey(pair)) pairCounts[pair] = 0;

                pairCounts[pair]++;
            }

            firstPolymer = lines[0][0].First();
            lastPolymer = lines[0][0].Last();

            RunAll();
        }
        public Solution() : this("Input.txt") { }

        public void DoRun()
        {
            Dictionary<string, long> newPairCounts = new Dictionary<string, long>();

            foreach (var pairType in pairCounts.Keys)
            {
                if (transitions.ContainsKey(pairType))
                {
                    var targets = transitions[pairType];
                    foreach (var target in targets) newPairCounts.InitAndUpdate(target, l => l + pairCounts[pairType]);
                }
                else
                {
                    newPairCounts.InitAndUpdate(pairType, l => l + pairCounts[pairType]);
                }
            }

            pairCounts = newPairCounts;
        }

        Dictionary<char, long> CalculateCounts()
        {
            Dictionary<char, long> counts = new Dictionary<char, long>();
            foreach (var pairType in pairCounts)
            {
                var chars = pairType.Key.ToCharArray();

                foreach(var c in chars) counts.InitAndUpdate(c, l => l + pairType.Value);
            }
            counts[firstPolymer]++;
            counts[lastPolymer]++;

            var keys = counts.Keys;
            foreach(var key in keys)
            {
                counts[key] = counts[key] / 2;
            }

            return counts;
        }

        long p1, p2;
        private void RunAll()
        {
            for (int n = 0; n < 10; n++)
            {
                DoRun();
            }

            p1 = CalculateAnswer();

            for (int n = 0; n < 30; n++)
            {
                DoRun();
            }

            p2 = CalculateAnswer();
        }

        private long CalculateAnswer()
        {
            var counts = CalculateCounts();

            var vals = counts.Values;

            var inOrder = vals.OrderBy(v => v);

            return inOrder.Last() - inOrder.First();
        }

        public object GetResult1()
        {
            return p1;
        }

        public object GetResult2()
        {
            return p2;
        }
    }
}
