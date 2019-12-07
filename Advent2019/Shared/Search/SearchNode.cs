using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Shared.Search
{
    class SearchNode<T> : IComparable<SearchNode<T>>
    {
        private ConcurrentDictionary<SearchNode<T>, int> expanded;

        public int time;
        public T item;

        // todo
        public int HeuristicDistance { get; set; }

        public SearchNode(ConcurrentDictionary<SearchNode<T>, int> expanded, T item, int time)
        {
            this.expanded = expanded;
            this.item = item;
            this.time = time;
        }

        public int Cost
        {
            get
            {
                return time + HeuristicDistance;
            }
        }

        public int CompareTo(SearchNode<T> other)
        {
            if (this.Cost != other.Cost) return Cost.CompareTo(other.Cost);
            else return HeuristicDistance.CompareTo(other.HeuristicDistance);
        }

        public void Expand()
        {
            (SearchNode<T> neighbour, int cost)[] neighbours = Neighbours;

            foreach (var neighbour in neighbours)
            {
                AddToResult(neighbour.cost);
            }
        }

        public List<SearchNode<T>> ExpandResult = new List<SearchNode<T>>();

        private void AddToResult(int time)
        {
            var node = new SearchNode<T>(expanded, time);

            int previousCost;
            var alreadyFound = expanded.TryGetValue(node, out previousCost);

            if ((previousCost == 0 || previousCost > node.Cost))
            {
                expanded.AddOrUpdate(node, node.Cost, (sn, cost) => node.Cost);
                ExpandResult.Add(node);
            }
        }

        public override int GetHashCode()
        {
            var tileHash = tile.GetHashCode() * 179;
            var toolHash = (int)(tool + 1) * 997300;
            return tileHash + toolHash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SearchNode<T>;
            var areEqual = tile.Equals(other.tile) && tool == other.tool;

            return areEqual;
        }

        public override string ToString()
        {
            return string.Format("Node ", time, tile, tool.ToString(), Cost);
        }
    }
}
