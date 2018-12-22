﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent22
{
    public enum Tool { None = 0, Torch = 1, Gear = 2 };

    class SearchNode : IComparable<SearchNode>
    {
        private ConcurrentDictionary<SearchNode, byte> explored;

        public int time;
        public Tile tile;
        public Tool tool;

        public SearchNode(ConcurrentDictionary<SearchNode, byte> explored, int time, Tile tile, Tool tool)
        {
            this.explored = explored;
            this.time = time;
            this.tile = tile;
            this.tool = tool;
        }

        public int Cost
        {
            get
            {
                return time + tile.HeuristicDistance;
            }
        }

        public int CompareTo(SearchNode other)
        {
            if (this.Cost != other.Cost) return Cost.CompareTo(other.Cost);
            else return tile.HeuristicDistance.CompareTo(other.tile.HeuristicDistance);
        }

        public void Explore()
        {
            var neighbours = tile.Neighbours;

            foreach (var neighbour in neighbours)
            {
                if (IsValidTool(neighbour, tool)) AddToResult(time + 1, neighbour, tool);
            }

            for (int n = 0; n < 3; n++)
            {
                if (IsValidTool(tile, (Tool)n)) AddToResult(time + 7, tile, (Tool)n);
            }
        }

        public List<SearchNode> ExploreResult = new List<SearchNode>();

        private bool IsValidTool(Tile tile, Tool tool)
        {
            var type = tile.Type;
            switch (type)
            {
                case 0: return tool != Tool.None;
                case 1: return tool != Tool.Torch;
                case 2: return tool != Tool.Gear;
                default: return false;
            }
        }

        private void AddToResult(int time, Tile tile, Tool tool)
        {
            var node = new SearchNode(explored, time, tile, tool);
            if (node.Cost < Cost) return;

            if (explored.TryAdd(node, 0))
            {
                ExploreResult.Add(node);
            }
        }

        public override int GetHashCode()
        {
            var tileHash = tile.GetHashCode() * 179;
            var toolHash = (int)(tool + 1) * 997300;
            var timeHash = time * 46370000;
            return tileHash + toolHash + timeHash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SearchNode;
            var areEqual = tile.Equals(other.tile) && tool == other.tool && time.Equals(other.time);

            return areEqual;
        }

        public override string ToString()
        {
            return string.Format("{0} / {3}: {1}, {2}", time, tile, tool.ToString(), Cost);
        }
    }
}
