using Advent2022.Shared.Search;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tests
{
    internal class AStarTests
    {
        [Test]
        public void CanDoSimpleMathsWithAStar()
        {
            var aStar = new AStar<int>(
                startNode: 1,
                endNode: 10,
                findNeighbourFunction: (int n) => new[] { n - 1, n + 1 }
                );

            var result = aStar.FindShortest();

            Assert.AreEqual(19, aStar.ExploitationData.Count); // up and down

            Assert.AreEqual(9, result.Cost);
        }

        [Test]
        public void CanDoCleverMathsWithAStar()
        {
            var aStar = new AStar<int>(
                startNode: 1,
                endNode: 10,
                findNeighbourFunction: (int n) => new[] { n - 1, n + 1 },
                heuristicCostFunction: (int n) => 10 - n
                );

            var result = aStar.FindShortest();

            Assert.AreEqual(10, aStar.ExploitationData.Count); // only up

            Assert.AreEqual(9, result.Cost);
        }

        [Test]
        public void CanFindShortestPath()
        {
            var node1 = new TestNode() { index = 1, costToGoToEnd = 10, heuristic = 1 };
            var node2 = new TestNode() { index = 2, costToGoToEnd = 9, heuristic = 9 };

            var endNode = new TestNode() { index = 3, costToGoToEnd = 0, heuristic = 0 };

            var aStar = new AStar<TestNode>(
                startNodes: new[] { node1, node2 },
                endNode: endNode,
                findNeighbourFunction: tn => new[] { endNode },
                heuristicCostFunction: tn => tn.heuristic,
                transitionCostFunction: (origin, _) => origin.costToGoToEnd
                );

            var result = aStar.FindShortest();

            Assert.AreEqual(9, result.Cost);
        }

        private class TestNode : IEquatable<TestNode>
        {
            public int index;
            public int costToGoToEnd;
            public int heuristic;

            public bool Equals(TestNode other)
            {
                return index == other.index;
            }
        }
    }
}
