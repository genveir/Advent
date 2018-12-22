using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Advent.Advent22
{
    class Advent22PriorityQueue
    {
        private ConcurrentStack<SearchNode>[] stacks;

        int count = 0;
        int prioStart;
        public Advent22PriorityQueue(int prioStart)
        {
            stacks = new ConcurrentStack<SearchNode>[1000];
            for (int n = 0; n < stacks.Length; n++)
            {
                stacks[n] = new ConcurrentStack<SearchNode>();
            }
            this.prioStart = prioStart;
        }

        public int LowestPrio
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

        public (SearchNode[] nodes, int num) DequeueLowestPrio(int max)
        {
            var returnArray = new SearchNode[max];
            var num = stacks[LowestPrio].TryPopRange(returnArray, 0, max);
            count -= num;
            return (returnArray, num);
        }

        public int Count { get { return count; } }

        public void Enqueue(SearchNode node)
        {
            stacks[node.Priority - prioStart].Push(node);
            Interlocked.Increment(ref count);
        }

        public void Enqueue(IEnumerable<SearchNode> nodes)
        {
            foreach (var node in nodes) Enqueue(node);
        }
    }
}
