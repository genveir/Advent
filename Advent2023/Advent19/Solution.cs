using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;
using FluentAssertions;

namespace Advent2023.Advent19;

public class Solution : ISolution
{
    public WorkFlow In;
    public List<Part> parts;

    List<Part2.AcceptNode> acceptNodes;

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        Func<Part, bool> acceptFlow = (p) => true;
        Func<Part, bool> rejectFlow = (p) => false;

        Dictionary<string, WorkFlow> workFlowsByName = new()
        {
            { "A", new("A") {Applies = acceptFlow } },
            { "R", new("B") {Applies = rejectFlow } }
        };

        Dictionary<string, string> workFlowDefinitionsByName = new();
        for (int n = 0; n < blocks[0].Length; n++)
        {
            var name = blocks[0][n].Split('{').First();
            workFlowDefinitionsByName.Add(name, blocks[0][n]);
        }

        acceptNodes = Part2.ParsePart2(workFlowDefinitionsByName);

        ParseWorkFlow("in", workFlowsByName, workFlowDefinitionsByName);
        In = workFlowsByName["in"];

        var partParser = new InputParser<Part>("line");
        parts = partParser.Parse(blocks[1]);
    }
    public Solution() : this("Input.txt") { }

    private WorkFlow GetWorkFlow(string name, Dictionary<string, WorkFlow> workFlowsByName, Dictionary<string, string> workFlowDefinitionsByName)
    {
        if (!workFlowsByName.ContainsKey(name))
            ParseWorkFlow(name, workFlowsByName, workFlowDefinitionsByName);

        return workFlowsByName[name];
    }

    private void ParseWorkFlow(string name, Dictionary<string, WorkFlow> workFlowsByName, Dictionary<string, string> workFlowDefinitionsByName)
    {
        var newWorkFlow = new WorkFlow(name);
        workFlowsByName.Add(name, newWorkFlow);

        var definition = workFlowDefinitionsByName[name];

        var split = definition.Split(new[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);

        var conditionalRules = split.Skip(1).Reverse().Skip(1).ToArray();
        var defaultWorkFlow = split.Last();
        var defaultRuleWorkflow = GetWorkFlow(defaultWorkFlow, workFlowsByName, workFlowDefinitionsByName);

        var defaultRule = (Part p) => defaultRuleWorkflow.Applies(p);
        for (int i = 0; i < conditionalRules.Count(); i++)
        {
            defaultRule = ParseRule(conditionalRules[i], defaultRule, workFlowsByName, workFlowDefinitionsByName);
        }

        newWorkFlow.Applies = defaultRule;
    }

    public class WorkFlow
    {
        public string Name { get; }
        public Func<Part, bool> Applies;

        public WorkFlow(string name)
        {
            Name = name;
        }
    }

    public Func<Part, bool> ParseRule(string rule, Func<Part, bool> defaultRule,
        Dictionary<string, WorkFlow> workFlowsByName, Dictionary<string, string> workFlowDefinitionsByName)
    {
        var split = rule.Split(':');
        var workFlowName = split[1];

        var workFlow = GetWorkFlow(workFlowName, workFlowsByName, workFlowDefinitionsByName);

        var condition = split[0];

        Func<Part, long> toCheck = condition[0] switch
        {
            'x' => (p) => p.X,
            'm' => (p) => p.M,
            'a' => (p) => p.A,
            's' => (p) => p.S,
            _ => throw new InvalidOperationException($"invalid part rating {condition[0]}")
        };

        var operation = condition[1];
        var value = long.Parse(condition.Substring(2));

        return operation == '<' ?
            ((Part p) => toCheck(p) < value ? workFlow.Applies(p) : defaultRule(p)) :
            ((Part p) => toCheck(p) > value ? workFlow.Applies(p) : defaultRule(p));
    }

    // pt2



    public object GetResult1()
    {
        return parts.Where(In.Applies).Sum(p => p.Value);
    }

    public object GetResult2()
    {
        var comparisons = acceptNodes.Select(an => an.GetComparisons()).ToArray();

        long sum = 0;
        foreach (var comparison in comparisons)
        {
            var values = Part2.Values.Full;
            foreach (var node in comparison)
                node.Apply(values);

            sum += values.Product;
        }

        return sum;
    }
}
