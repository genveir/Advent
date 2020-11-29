using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Shared.Search
{
    public class Test
    {
        [Test]
        public void SingleStep()
        {
            var baseNode = new GridNode(new ConcurrentDictionary<SearchNode, int>(), 0, 0, 0, 1);

            var result = new Search().Execute(baseNode) as GridNode;

            Assert.AreEqual(1, result.x);
            Assert.AreEqual(1, result.Cost);
        }

        [Test]
        public void Longer()
        {
            var baseNode = new GridNode(new ConcurrentDictionary<SearchNode, int>(), 0, 0, 0, 1000000);

            var result = new Search().Execute(baseNode) as GridNode;

            Assert.AreEqual(1000000, result.x);
            Assert.AreEqual(1000000, result.Cost);
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
            return Math.Abs(target - x) + Math.Abs(y);
        }

        public override (int cost, SearchNode neighbour)[] GetNeighbours()
        {
            var neighbours = new (int cost, SearchNode neighbour)[9];
            for (int xShift = -1; xShift <= 1; xShift++)
            {
                for (int yShift = -1; yShift <= 1; yShift++)
                {
                    neighbours[(xShift + 1) * 3 + (yShift + 1)] = (1, new GridNode(expanded, Cost + 1, x + xShift, y + yShift, target));
                }
            }

            return neighbours;
        }

        public override bool IsAtTarget()
        {
            return x == target && y == 0;
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

        public override string ToString()
        {
            return "GridNode " + x + ", " + y;
        }
    }
}
