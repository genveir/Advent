using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent2021.Shared.Searching
{
    public abstract class SearchNode : IComparable<SearchNode>
    {
        protected ConcurrentDictionary<SearchNode, int> expanded;

        public int time;

        public abstract int GetHeuristicDistance();
        public abstract (int cost, SearchNode neighbour)[] GetNeighbours();
        public abstract bool IsAtTarget();

        public SearchNode(ConcurrentDictionary<SearchNode, int> expanded, int time)
        {
            this.expanded = expanded;
            this.time = time;
        }

        public int Cost
        {
            get
            {
                return time + GetHeuristicDistance();
            }
        }

        public int CompareTo(SearchNode other)
        {
            if (this.Cost != other.Cost) return Cost.CompareTo(other.Cost);
            else return GetHeuristicDistance().CompareTo(other.GetHeuristicDistance());
        }

        public void Expand()
        {
            (int cost, SearchNode neighbour)[] neighbours = GetNeighbours();

            foreach (var neighbour in neighbours)
            {
                AddToResult(neighbour.cost, neighbour.neighbour);
            }
        }

        public List<SearchNode> ExpandResult = new List<SearchNode>();

        private void AddToResult(int transitionCost, SearchNode neighbour)
        {
            neighbour.time = this.time + transitionCost;

            int previousCost;
            var alreadyFound = expanded.TryGetValue(neighbour, out previousCost);

            if ((previousCost == 0 || previousCost > neighbour.Cost))
            {
                expanded.AddOrUpdate(neighbour, neighbour.Cost, (sn, cost) => neighbour.Cost);
                ExpandResult.Add(neighbour);
            }
        }
    }
}
