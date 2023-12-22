using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023.Advent19;
internal class Part2
{
    public List<AcceptNode> AcceptNodes { get; set; }

    public class Values
    {
        public static Values Full
        {
            get
            {
                var values = new Values();
                for (int n = 1; n <= 4000; n++)
                {
                    values.X.Add(n);
                    values.M.Add(n);
                    values.A.Add(n);
                    values.S.Add(n);
                }
                return values;
            }
        }

        public List<long> X { get; set; } = new();
        public List<long> M { get; set; } = new();
        public List<long> A { get; set; } = new();
        public List<long> S { get; set; } = new();

        public long Product => X.LongCount() * M.LongCount() * A.LongCount() * S.LongCount();
    }

    public abstract class Node
    {
        public string Name { get; set; }

        public Node Parent { get; set; }

        public Node(Node parent)
        {
            Parent = parent;
        }

        public abstract List<ComparisonNode> GetComparisons();

        public override string ToString() => Name ?? GetType().Name;
    }

    public class ComparisonNode : Node
    {
        public ComparisonNode(Node parent, string comparison) : base(parent)
        {
            Comparison = comparison;
        }

        public string Comparison { get; set; }

        public override List<ComparisonNode> GetComparisons()
        {
            var values = Parent == null ? new List<ComparisonNode>() : Parent.GetComparisons();
            values.Add(this);

            return values;
        }

        public void Apply(Values values)
        {
            var toFilterName = Comparison[0];
            var comparisonType = Comparison.Substring(1, 2);
            var value = long.Parse(Comparison.Substring(3));

            Func<long, bool> whereFunc = comparisonType switch
            {
                ">>" => (l) => l > value,
                "<<" => (l) => l < value,
                ">=" => (l) => l >= value,
                "<=" => (l) => l <= value,
                _ => throw new InvalidOperationException($"unknown comparison {comparisonType}")
            };

            switch (toFilterName)
            {
                case 'x': values.X = values.X.Where(whereFunc).ToList(); break;
                case 'm': values.M = values.M.Where(whereFunc).ToList(); break;
                case 'a': values.A = values.A.Where(whereFunc).ToList(); break;
                case 's': values.S = values.S.Where(whereFunc).ToList(); break;
                default: throw new InvalidOperationException($"unknown collection {toFilterName}");
            }
        }
    }

    public class AcceptNode : Node
    {
        public AcceptNode(Node parent) : base(parent) { }

        public override List<ComparisonNode> GetComparisons() => Parent.GetComparisons();
    }

    public static List<AcceptNode> ParsePart2(Dictionary<string, string> workflowDefinitions)
    {
        return ParseWorkFlowRule(null, "in", workflowDefinitions);
    }

    public static List<AcceptNode> ParseWorkFlowRule(Node parent, string name, Dictionary<string, string> workflowDefinitions)
    {
        if (name == "A")
        {
            var acceptNode = new AcceptNode(parent);

            return new List<AcceptNode>() { acceptNode };
        }
        if (name == "R") return new();

        var acceptNodes = new List<AcceptNode>();

        var definition = workflowDefinitions[name];

        var split = definition.Split(new[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);

        var conditionalRules = split.Skip(1).Take(split.Length - 2).ToArray();

        Node currentParent = parent;
        for (int n = 0; n < conditionalRules.Length; n++)
        {
            var conditionSplit = conditionalRules[n].Split(':');
            var condition = conditionSplit[0].Replace(">", ">>").Replace("<", "<<");

            var successCondition = new ComparisonNode(currentParent, condition) { Name = $"{name} {condition}" };
            acceptNodes.AddRange(ParseWorkFlowRule(successCondition, conditionSplit[1], workflowDefinitions));

            var invertedCondition = condition.Contains('>') ?
                condition.Replace(">>", "<=") :
                condition.Replace("<<", ">=");

            var failureCondition = new ComparisonNode(currentParent, invertedCondition) { Name = $"{name} {invertedCondition}" };
            currentParent = failureCondition;
        }
        acceptNodes.AddRange(ParseWorkFlowRule(currentParent, split.Last(), workflowDefinitions));

        return acceptNodes;
    }
}
