using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.AdventActive;

public class Solution : ISolution
{
    public string[] Lines;
    public List<Node> Nodes;
    public List<Edge> Edges;

    public Solution(string input)
    {
        Lines = Input.GetInputLines(input).ToArray();

        Reset();
    }
    public Solution() : this("Input.txt") { }

    public void Reset()
    {
        var inputParser = new InputParser<string, string[]>("node: nodes") { ArrayDelimiters = new[] { ' ' } };

        List<(string left, string[] right)> parsed = inputParser.Parse(Lines);

        var allNames = parsed.Select(p => p.left).Union(parsed.SelectMany(p => p.right));

        Dictionary<string, Node> nodes = new Dictionary<string, Node>();
        foreach (var name in allNames)
            nodes.Add(name, new(name));

        Nodes = nodes.Values.ToList();

        Edges = new();
        foreach (var p in parsed)
        {
            for (int n = 0; n < p.right.Length; n++)
            {
                var edge = nodes[p.left].Link(nodes[p.right[n]]);
                Edges.Add(edge);
            }
        }
    }

    public class Node
    {
        public string Name { get; set; }

        public List<Node> FoldedNodes = new();

        public List<Edge> Edges = new();

        public Node(string name) { Name = name; }

        public Edge Link(Node node)
        {
            var edge = new Edge(this, node);
            Edges.Add(edge);
            node.Edges.Add(edge);

            return edge;
        }

        public List<Edge> Fold(Node other)
        {
            FoldedNodes.Add(other);
            foreach (var foldedNode in other.FoldedNodes)
                FoldedNodes.Add(foldedNode);

            foreach (var edge in other.Edges)
                Edges.Add(edge);

            List<Edge> selfEdges = new();
            foreach(var edge in Edges)
            {
                if (edge.Left == other)
                    edge.Left = this;
                if (edge.Right == other)
                    edge.Right = this;

                if (edge.Left == this && edge.Right == this)
                    selfEdges.Add(edge);
            }

            foreach (var edge in selfEdges)
                Edges.Remove(edge);

            return selfEdges;
        }

        public override string ToString()
        {
            var fNodes = FoldedNodes.Select(fn => fn.Name);
            var folded = string.Join(',', fNodes);

            return $"{Name} ({folded})";
        }
    }

    public class Edge
    {
        public Edge(Node left, Node right)
        {
            Left = left;
            Right = right;
        }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public override string ToString()
        {
            return $"[{Left}] - [{Right}]";
        }
    }

    public Random Random = new();

    public long numRuns = 0;
    public void RunKargers()
    {
        while(Nodes.Count > 2)
        {
            var randomEdge = Edges[Random.Next(Edges.Count)];
            Nodes.Remove(randomEdge.Right);

            var selfEdges = randomEdge.Left.Fold(randomEdge.Right);

            foreach (var edge in selfEdges)
                Edges.Remove(edge);
        }
    }

    public long RunKargersUntilCutFound()
    {
        while (Edges.Count != 3)
        {
            Reset();

            RunKargers();
        }

        return (1 + Nodes[0].FoldedNodes.Count) * (1 + Nodes[1].FoldedNodes.Count);
    }

    public object GetResult1()
    {
        return RunKargersUntilCutFound();
    }

    public object GetResult2()
    {
        return "";
    }
}
