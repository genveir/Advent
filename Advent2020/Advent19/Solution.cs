using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent19
{
    public class Solution : ISolution
    {
        IRule[] rules;

        string[] messages;

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input);

            rules = new RuleParser().Parse(blocks[0]);

            messages = blocks[1].OrderBy(m => m).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            int counter = 0;
            foreach(var message in messages)
            {
                foreach (var match in rules[0].Matches(message, 0))
                {
                    if (match == message.Length)
                    {
                        counter++;
                        break;
                    }
                }
            }
            return counter;
        }

        public object GetResult2()
        {
            var rules8 = new IRule[2][];
            rules8[0] = new IRule[] { rules[42] };
            rules8[1] = new IRule[] { rules[42], rules[8] };

            rules[8].Link(rules8);

            var rules11 = new IRule[2][];
            rules11[0] = new IRule[] { rules[42], rules[31] };
            rules11[1] = new IRule[] { rules[42], rules[11], rules[31] };

            rules[11].Link(rules11);

            int counter = 0;
            foreach (var message in messages)
            {
                foreach (var match in rules[0].Matches(message, 0))
                {
                    if (match == message.Length)
                    {
                        counter++;
                        break;
                    }
                }
            }
            return counter;
        }
    }
}
