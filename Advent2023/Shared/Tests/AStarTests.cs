using System;
using Advent2023.Shared.Search;
using FluentAssertions;
using NUnit.Framework;

namespace Advent2023.Shared.Tests;

internal class AStarTests
{
    [Test]
    public void CanDoSimpleMathsWithAStar()
    {
        var aStar = new AStar<int>(
            startNode: 1,
            endNode: 10,
            findNeighbourFunction: n => new[] { n - 1, n + 1 }
            );

        var result = aStar.FindShortest();

        aStar.ExploitationData.Count.Should().Be(19); // up and down

        result.Cost.Should().Be(9);
    }

    [Test]
    public void CanDoCleverMathsWithAStar()
    {
        var aStar = new AStar<int>(
            startNode: 1,
            endNode: 10,
            findNeighbourFunction: n => new[] { n - 1, n + 1 },
            heuristicCostFunction: n => 10 - n
            );

        var result = aStar.FindShortest();

        aStar.ExploitationData.Count.Should().Be(10); // only up

        result.Cost.Should().Be(9);
    }

    [Test]
    public void CanFindShortestPath()
    {
        var node1 = new TestNode { Index = 1, CostToGoToEnd = 10, Heuristic = 1 };
        var node2 = new TestNode { Index = 2, CostToGoToEnd = 9, Heuristic = 9 };

        var endNode = new TestNode { Index = 3, CostToGoToEnd = 0, Heuristic = 0 };

        var aStar = new AStar<TestNode>(
            startNodes: new[] { node1, node2 },
            endNode: endNode,
            findNeighbourFunction: _ => new[] { endNode },
            heuristicCostFunction: tn => tn.Heuristic,
            transitionCostFunction: (origin, _) => origin.CostToGoToEnd
            );

        var result = aStar.FindShortest();

        result.Cost.Should().Be(9);
    }

    [Test]
    public void CanFindShortestPathWithEndState()
    {
        var node1 = new TestNode { Index = 1, CostToGoToEnd = 10, Heuristic = 1 };
        var node2 = new TestNode { Index = 2, CostToGoToEnd = 9, Heuristic = 9 };

        var endNode = new TestNode { Index = 3, CostToGoToEnd = 0, Heuristic = 0 };

        var aStar = new AStar<TestNode>(
            startNodes: new[] { node1, node2 },
            endStates: n => n.Index == 3,
            findNeighbourFunction: _ => new[] { endNode },
            heuristicCostFunction: tn => tn.Heuristic,
            transitionCostFunction: (origin, _) => origin.CostToGoToEnd
            );

        var result = aStar.FindShortest();

        result.Cost.Should().Be(9);
    }

    private class TestNode : IEquatable<TestNode>
    {
        public int Index;
        public int CostToGoToEnd;
        public int Heuristic;

        public bool Equals(TestNode other)
        {
            return Index == other?.Index;
        }
    }
}
