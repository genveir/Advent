﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2019.Shared.Search
{
    public class Search
    {
        public SearchNode Execute(SearchNode baseNode)
        {
            var expanded = new ConcurrentDictionary<SearchNode, int>();

            var dequeueSize = 5;
            var pQueue = new ConcurrentPriorityQueue(baseNode.Cost, dequeueSize);
            pQueue.Enqueue(baseNode);
            while (pQueue.Count > 0)
            {
                List<SearchNode> atCost = new List<SearchNode>();

                (var nodes, var num) = pQueue.DequeueLowestCost();

                for (int n = 0; n < num; n++)
                {
                    if (nodes[n].IsAtTarget())
                    {
                        return nodes[n];
                    }

                    atCost.Add(nodes[n]);
                }

                if (atCost.Count > 0)
                {
                    Parallel.ForEach(atCost, node => node.Expand());

                    pQueue.Enqueue(atCost.SelectMany(ap => ap.ExpandResult));
                }
            }

            return null;
        }
    }
}
