using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent19
{
    public class RuleParser
    {
        private List<ProtoRule> protoRules;
        private List<char> characters = new List<char>() { (char)0 };

        public IRule[] Parse(string[] input)
        {
            protoRules = new List<ProtoRule>();

            var inputParser = new InputParser<int, string[]>("index: values") { ArrayDelimiters = new char[] { '|' } };

            foreach (var line in input)
            {
                var (index, products) = inputParser.Parse(line);

                var parsedProducts = new int[products.Length][];
                for (int n = 0; n < products.Length; n++) parsedProducts[n] = ParseProducts(products[n]);

                protoRules.Add(new ProtoRule(index, parsedProducts));
            }

            IRule[] rules = new IRule[protoRules.Count];
            for (int n = 0; n < rules.Length; n++)
            {
                var protoRule = protoRules[n];
                if (protoRule.IsTerminal())
                {
                    var production = protoRule.Products[0].Select(p => characters[-p]).Single();

                    rules[protoRule.Input] = new TerminalRule(protoRule.Input, production);
                }
                else rules[protoRule.Input] = new Rule(protoRule.Input);
            }

            foreach (var rule in protoRules) rule.LinkRule(rules);

            return rules;
        }

        private int[] ParseProducts(string input)
        {
            var inputs = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int[] result = new int[inputs.Length];
            for (int n = 0; n < inputs.Length; n++)
            {
                var current = inputs[n];
                if (current.StartsWith('"'))
                {
                    var c = current.Replace("\"", "")[0];
                    var index = characters.IndexOf(c);
                    if (index == -1)
                    {
                        characters.Add(c);
                        index = characters.Count - 1;
                    }
                    result[n] = -index;
                }
                else
                {
                    result[n] = int.Parse(current);
                }
            }
            return result;
        }
    }
}
