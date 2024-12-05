using Advent2024.Shared;
using Advent2024.Shared.InputParsing;

namespace Advent2024.Day5;

public class Solution : ISolution
{
    public List<OrderRule> rules;
    public List<Update> updates;

    public Solution(string input)
    {
        var lines = Input.GetBlockLines(input).ToArray();

        var inputParser = new InputParser<OrderRule>("line");
        rules = inputParser.Parse(lines[0]);

        var inputParser2 = new InputParser<Update>("array");
        updates = inputParser2.Parse(lines[1]);
    }

    public Solution() : this("Input.txt")
    {
    }

    public class OrderRule
    {
        public int before;
        public int after;

        [ComplexParserTarget("a|b")]
        public OrderRule(int before, int after)
        {
            this.before = before;
            this.after = after;
        }

        public override string ToString()
        {
            return $"{before}|{after}";
        }
    }

    public class Update
    {
        public int[] pages;

        [ComplexParserTarget("array")]
        public Update(int[] pages)
        {
            this.pages = pages;
        }

        public bool MatchesRules(List<OrderRule> rules)
        {
            foreach (var rule in rules)
            {
                var beforeIndex = Array.IndexOf(pages, rule.before);
                var afterIndex = Array.IndexOf(pages, rule.after);

                if (afterIndex >= 0 && (afterIndex < beforeIndex))
                    return false;
            }
            return true;
        }

        public long GetMiddleNumber()
        {
            return pages[pages.Length / 2];
        }

        public long GetMiddleNumber2(List<int> inOrder)
        {
            var ordered = pages.OrderBy(inOrder.IndexOf).ToArray();

            return ordered[pages.Length / 2];
        }
    }

    public long GetMiddleInOrder(Update update)
    {
        List<int> InOrder = [];

        List<int> numbers = [];

        var filteredRules = rules.Where(r => update.pages.Contains(r.before) && update.pages.Contains(r.after)).ToList();

        foreach (var rule in filteredRules)
        {
            if (!numbers.Contains(rule.before))
            {
                numbers.Add(rule.before);
            }
            if (!numbers.Contains(rule.after))
            {
                numbers.Add(rule.after);
            }
        }

        foreach (var number in numbers)
        {
            if (InOrder.Count == 0)
            {
                InOrder.Add(number);
                continue;
            }

            for (int n = 0; n < InOrder.Count + 1; n++)
            {
                InOrder.Insert(n, number);
                if (MatchesRules(InOrder, rules))
                {
                    break;
                }
                InOrder.RemoveAt(n);
            }

            if (!InOrder.Contains(number))
            {
                throw new Exception("Could not find order");
            }
        }

        if (InOrder.Count != numbers.Count)
        {
            throw new Exception("Could not find order");
        }

        return InOrder[InOrder.Count / 2];
    }

    public static bool MatchesRules(List<int> numbers, List<OrderRule> rules)
    {
        foreach (var rule in rules)
        {
            var beforeIndex = numbers.IndexOf(rule.before);
            var afterIndex = numbers.IndexOf(rule.after);

            if (afterIndex >= 0 && (afterIndex < beforeIndex))
                return false;
        }
        return true;
    }

    public object GetResult1()
    {
        long sum = 0;
        foreach (var update in updates)
        {
            if (update.MatchesRules(rules))
            {
                var middle = update.GetMiddleNumber();
                sum += middle;
            }
        }

        return sum;
    }

    // not 5137
    public object GetResult2()
    {
        List<Update> outofOrder = [];

        foreach (var update in updates)
        {
            if (!update.MatchesRules(rules))
            {
                outofOrder.Add(update);
            }
        }

        long sum = 0;
        foreach (var update in outofOrder)
        {
            sum += GetMiddleInOrder(update);
        }
        return sum;
    }
}