using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Shared.Search
{
    public class Test
    {
        [Test]
        public void SingleStep()
        {
            var baseNode = new GridNode(new ConcurrentDictionary<SearchNode, int>(), 0, 0, 0, 2);

            var result = new Search().Execute(baseNode) as GridNode;

            Assert.AreEqual(2, result.x);
            Assert.AreEqual(1, result.Cost);
        }

        [Test]
        public void Longer()
        {
            var baseNode = new GridNode(new ConcurrentDictionary<SearchNode, int>(), 0, 0, 0, 1000000);

        }
    }

    public class GridNode : SearchNode
    {
        public int x;
        public int y;
        public int target;

        public GridNode(ConcurrentDictionary<SearchNode, int> expanded, int time, int x, int y, int target) : base(expanded, time)
        {
            this.x = x;
            this.y = y;
            this.target = target;
        }

        public override int GetHeuristicDistance()
        {
            return (target - x) / 2;
        }

        public override (int cost, SearchNode neighbour)[] GetNeighbours()
        {
            var neighbours = new (int cost, SearchNode neighbour)[4];
            for (int xShift = 0; xShift < 2; xShift++)
            {
                for (int yShift = 0; yShift < 2; yShift++)
                {
                    neighbours[xShift * 2 + yShift] = (1, new GridNode(expanded, Cost + 1, x + xShift, y + yShift, target));
                }
            }

            return neighbours;
        }

        public override bool IsAtTarget()
        {
            return x == target;
        }

        public override int GetHashCode()
        {
            return x * 12345 + y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as GridNode;
            if (other == null) return false;
            return other.x == this.x && other.y == this.y && other.target == this.target;
        }
    }
}
