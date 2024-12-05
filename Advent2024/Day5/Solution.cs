using Advent2024.Shared;
using Advent2024.Shared.InputParsing;

namespace Advent2024.Day5;

public class Solution : ISolution
{
    public List<OrderRule> rules;
    public List<Update> updates;

    public RuleComparer comparer;

    public Solution(string input)
    {
        var lines = Input.GetBlockLines(input).ToArray();

        var inputParser = new InputParser<OrderRule>("line");
        rules = inputParser.Parse(lines[0]);

        var inputParser2 = new InputParser<Update>("array");
        updates = inputParser2.Parse(lines[1]);

        comparer = new RuleComparer(rules);
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

        public bool IsInOrder(Comparer<int> ruleComparer)
        {
            var sorted = SortedPages(ruleComparer);

            return pages.SequenceEqual(sorted);
        }

        public long GetMiddleNumber()
        {
            return pages[pages.Length / 2];
        }

        public List<int> SortedPages(Comparer<int> ruleComparer)
        {
            var asList = pages.ToList();
            asList.Sort(ruleComparer);
            return asList;
        }

        public long GetMiddleNumber2(Comparer<int> ruleComparer)
        {
            return SortedPages(ruleComparer)[pages.Length / 2];
        }
    }

    public class RuleComparer : Comparer<int>
    {
        private readonly List<OrderRule> rules;

        public RuleComparer(List<OrderRule> rules)
        {
            this.rules = rules;
        }

        public override int Compare(int x, int y)
        {
            foreach (var rule in rules)
            {
                if (rule.before == x && rule.after == y)
                    return -1;
                if (rule.before == y && rule.after == x)
                    return 1;
            }
            return 0;
        }
    }

    public object GetResult1()
    {
        long sum = 0;
        foreach (var update in updates)
        {
            if (update.IsInOrder(comparer))
            {
                sum += update.GetMiddleNumber();
            }
        }

        return sum;
    }

    // not 5137
    public object GetResult2()
    {
        long sum = 0;
        foreach (var update in updates)
        {
            if (!update.IsInOrder(comparer))
            {
                sum += update.GetMiddleNumber2(comparer);
            }
        }

        return sum;
    }
}