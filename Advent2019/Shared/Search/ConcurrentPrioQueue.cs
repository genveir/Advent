using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Advent2019.Shared.Search
{
    public class ConcurrentPriorityQueue
    {
        private ConcurrentStack<SearchNode>[] stacks;

        int count = 0;
        int prioStart;
        int dequeueSize;

        SearchNode[] returnArray;
        public ConcurrentPriorityQueue(int prioStart, int dequeueSize)
        {
            stacks = new ConcurrentStack<SearchNode>[1000];
            for (int n = 0; n < stacks.Length; n++)
            {
                stacks[n] = new ConcurrentStack<SearchNode>();
            }
            this.dequeueSize = dequeueSize;
            this.prioStart = prioStart;
            returnArray = new SearchNode[dequeueSize];
        }

        public int LowestCost
        {
            get
            {
                for (int n = 0; n < 1000; n++)
                {
                    if (stacks[n].Count > 0)
                    {
                        return n;
                    }
                }
                return -1;
            }
        }

        public (SearchNode[] nodes, int num) DequeueLowestCost()
        {
            var num = stacks[LowestCost].TryPopRange(returnArray, 0, dequeueSize);
            count -= num;
            return (returnArray, num);
        }

        public int Count { get { return count; } }

        public void Enqueue(SearchNode node)
        {
            stacks[node.Cost - prioStart].Push(node);
            Interlocked.Increment(ref count);
        }

        public void Enqueue(IEnumerable<SearchNode> nodes)
        {
            foreach (var node in nodes) Enqueue(node);
        }
    }
}
