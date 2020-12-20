using Advent2020.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent19
{
    class Tests
    {
        const string sample = @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: 'a'
5: 'b'

ababbb
bababa
abbbab
aaabbb
aaaabbb";

        [TestCase(sample, 2)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input.Replace("'", "\""));

            Assert.AreEqual(output, sol.GetResult1());
        }

        private IRule CreateSimpleRule(int input, IRule output)
        {
            var rule = new Rule(input);
            rule.Products = new IRule[1][];
            rule.Products[0] = new IRule[] { output };

            return rule;
        }

        private IRule CreateSplittingRule(int input, params IRule[] simpleOutputs)
        {
            var rule = new Rule(input);
            rule.Products = new IRule[simpleOutputs.Length][];
            for (int n = 0; n < simpleOutputs.Length; n++) rule.Products[n] = new IRule[] { simpleOutputs[n] };

            return rule;
        }

        private IRule CreateCombinedRule(int input, params IRule[] combinedOutput)
        {
            var rule = new Rule(input);
            rule.Products = new IRule[1][];
            rule.Products[0] = combinedOutput;

            return rule;
        }

        [Test]
        public void TerminalRuleReturns1WhenMatched()
        {
            var rule = new TerminalRule(0, 'a');

            var matches = rule.Matches("a", 0).ToList();

            Assert.AreEqual(1, matches.Single());
        }

        [Test]
        public void TerminalRuleReturnsNothingWhenNotMatched()
        {
            var rule = new TerminalRule(0, 'a');

            var matches = rule.Matches("b", 0).ToList();

            Assert.AreEqual(0, matches.Count);
        }

        [Test]
        public void TerminalRuleRespectsOffset()
        {
            var rule = new TerminalRule(0, 'a');

            var matches = rule.Matches("bbbabbb", 3).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(1, matches.Single());
        }

        [Test]
        public void SimpleRuleWorks()
        {
            var terminal = new TerminalRule(1, 'a');
            var rule = CreateSimpleRule(0, terminal);

            var matches = rule.Matches("a", 0).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(1, matches.Single());
        }

        [Test]
        public void CanChainSimpleRules()
        {
            var terminal = new TerminalRule(2, 'a');
            var rule1 = CreateSimpleRule(1, terminal);
            var rule2 = CreateSimpleRule(2, rule1);

            var matches = rule2.Matches("a", 0).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(1, matches.Single());
        }

        [TestCase("a")]
        [TestCase("b")]
        public void SplittingRuleWorks(string toMatch)
        {
            var terminalA = new TerminalRule(2, 'a');
            var terminalB = new TerminalRule(1, 'b');
            var rule = CreateSplittingRule(0, terminalA, terminalB);

            var matches = rule.Matches(toMatch, 0).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(1, matches.Single());
        }

        [TestCase]
        public void CombinedRuleWorks()
        {
            var terminalA = new TerminalRule(1, 'a');
            var terminalB = new TerminalRule(2, 'b');
            var rule = CreateCombinedRule(0, terminalA, terminalB);

            var matches = rule.Matches("ab", 0).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreEqual(2, matches.Single());
        }

        [TestCase]
        public void CanHandleRecursingRule()
        {
            var terminalA = new TerminalRule(1, 'a');
            var rule = new Rule(0);
            rule.Products = new IRule[2][];
            rule.Products[0] = new IRule[] { terminalA };
            rule.Products[1] = new IRule[] { terminalA, rule };

            var matches = rule.Matches("aaaaaaaa", 0).ToList();

            Assert.AreEqual(8, matches.Count);
            Assert.That(matches.Where(m => m == 8).Any());
        }
    }
}
