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
        Dictionary<char, Dictionary<char, char>> rules = new Dictionary<char, Dictionary<char, char>>();

        LinkedListNode firstNode;

        Dictionary<string, long> pairCounts = new Dictionary<string, long>();
        Dictionary<string, string[]> transitions = new Dictionary<string, string[]>();

        public Solution(string input)
        {
            var lines = Input.GetBlockLines(input).ToArray();

            var inputParser = new InputParser<string, char>("AB -> C");

            foreach(var rule in lines[1])
            {
                var (inputs, output) = inputParser.Parse(rule);

                if (!rules.ContainsKey(inputs[0])) rules[inputs[0]] = new Dictionary<char, char>();
                rules[inputs[0]][inputs[1]] = output;
            }

            LinkedListNode previous = null;
            foreach(var c in lines[0][0])
            {
                previous = new LinkedListNode(c, previous, null);
                if (firstNode == null) firstNode = previous;
            }
        }
        public Solution() : this("Input.txt") { }

        private class LinkedListNode
        {
            public char polymer;
            public LinkedListNode previous;
            public LinkedListNode next;

            public static Dictionary<char, long> counts = new Dictionary<char, long>();

            public LinkedListNode(char polymer, LinkedListNode previous, LinkedListNode next)
            {
                this.polymer = polymer;
                this.previous = previous;
                this.next = next;

                if (previous != null) previous.next = this;
                if (next != null) next.previous = this;

                if (!counts.ContainsKey(polymer)) counts[polymer] = 0;

                counts[polymer]++;
            }

            public void DoRun(Dictionary<char, Dictionary<char, char>> rules)
            {
                if (previous != null) Multiply(rules);

                if (next != null) next.DoRun(rules);
            }

            public void Multiply(Dictionary<char, Dictionary<char, char>> rules)
            {
                if (rules.TryGetValue(previous.polymer, out Dictionary<char, char> inner))
                {
                    if (inner.TryGetValue(polymer, out char output) )
                    {
                        new LinkedListNode(output, previous, this);
                    }
                }
            }

            public override string ToString()
            {
                return polymer.ToString();
            }

            public string WholeString()
            {
                var sb = new StringBuilder();

                WholeString(sb);

                return sb.ToString();
            }

            public void WholeString(StringBuilder b)
            {
                b.Append(polymer);

                if (next != null) next.WholeString(b);
            }
        }

        public object GetResult1()
        {
            for (int n = 0; n < 20; n++)
            {
                firstNode.DoRun(rules);
            }

            var counts = LinkedListNode.counts;

            var inORder = counts.Values.OrderBy(v => v);

            return inORder.Last() - inORder.First();
        }

        public object GetResult2()
        {
            


            for (int n = 0; n < 40; n++)
            {
                //firstNode.DoRun(rules);
            }

            var counts = LinkedListNode.counts;

            var inORder = counts.Values.OrderBy(v => v);

            return inORder.Last() - inORder.First();
        }
    }
}
