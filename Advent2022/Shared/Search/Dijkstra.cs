using NetTopologySuite.GeometriesGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Search
{
    public class Dijkstra<TNode> where TNode : IEquatable<TNode>
    {
        public PriorityQueue<NodeData, long> Queue = new();

        public List<TNode> EndNodes { get; }
        public List<TNode> StartNodes { get; }
        public TransitionCost TransitionCostFunction { get; }
        public HeuristicCost HeuristicCostFunction { get; }
        public FindNeighbours FindNeighbourFunction { get; }

        public delegate long TransitionCost(TNode start, TNode target);
        public delegate long HeuristicCost(TNode node);
        public delegate IEnumerable<TNode> FindNeighbours(TNode node);

        private static TransitionCost DefaultTransitionCost = (TNode a, TNode b) => 1L;
        private static HeuristicCost DefaultHeuristicCost = (TNode a) => 0L;

        /// <summary>
        /// Create Dijkstra Setup
        /// </summary>
        /// <param name="startNodes">The Node from which the algorithm will start it's search (at cost 0)</param>
        /// <param name="endNodes">The Nodes at which the algorithm will stop and return</param>
        /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
        /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
        /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
        public Dijkstra(TNode startNode, IEnumerable<TNode> endNodes, FindNeighbours findNeighbourFunction,
            TransitionCost transitionCostFunction = null,
            HeuristicCost heuristicCostFunction = null) :
                this(new[] { startNode }, endNodes, findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
        { }

        /// <summary>
        /// Create Dijkstra Setup
        /// </summary>
        /// <param name="startNodes">The Nodes from which the algorithm will start it's search (at cost 0)</param>
        /// <param name="endNodes">The Node at which the algorithm will stop and return</param>
        /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
        /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
        /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
        public Dijkstra(IEnumerable<TNode> startNodes, TNode endNode, FindNeighbours findNeighbourFunction,
            TransitionCost transitionCostFunction = null,
            HeuristicCost heuristicCostFunction = null) :
            this(startNodes, new[] { endNode }, findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
        { }

        /// <summary>
        /// Create Dijkstra Setup
        /// </summary>
        /// <param name="startNodes">The Node from which the algorithm will start it's search (at cost 0)</param>
        /// <param name="endNodes">The Node at which the algorithm will stop and return</param>
        /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
        /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
        /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
        public Dijkstra(TNode startNode, TNode endNode, FindNeighbours findNeighbourFunction, 
            TransitionCost transitionCostFunction = null, 
            HeuristicCost heuristicCostFunction = null) :
            this(new[] { startNode }, new[] { endNode }, findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
        { }

        /// <summary>
        /// Create Dijkstra Setup
        /// </summary>
        /// <param name="startNodes">The Nodes from which the algorithm will start it's search (at cost 0)</param>
        /// <param name="endNodes">The Nodes at which the algorithm will stop and return</param>
        /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
        /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
        /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
        public Dijkstra(IEnumerable<TNode> startNodes, IEnumerable<TNode> endNodes, FindNeighbours findNeighbourFunction, 
            TransitionCost transitionCostFunction = null, 
            HeuristicCost heuristicCostFunction = null)
        {
            StartNodes = startNodes.ToList();
            EndNodes = endNodes.ToList();
            FindNeighbourFunction = findNeighbourFunction;
            TransitionCostFunction = transitionCostFunction ?? DefaultTransitionCost;
            HeuristicCostFunction = heuristicCostFunction ?? DefaultHeuristicCost;
        }

        public HashSet<TNode> HasBeenExplored;
        public Dictionary<TNode, NodeData> ExplorationData;

        public List<TNode> PathToNode(TNode node)
        {
            if (ExplorationData.ContainsKey(node))
            {
                return PathToNode(ExplorationData[node])
                    .Select(nd => nd.Node)
                    .ToList();
            }

            return null;
        }
        public List<NodeData> PathToNode(NodeData nodeData)
        {
            List<NodeData> aggregate = new();

            PathToNode(nodeData, aggregate);

            return aggregate;
        }

        public void PathToNode(NodeData nodeData, List<NodeData> aggregate)
        {
            if (nodeData.DiscoveredBy != null) 
                PathToNode(nodeData.DiscoveredBy, aggregate);

            aggregate.Add(nodeData);
        }

        /// <summary>
        /// Returns the cost of the shortest path
        /// </summary>
        /// <returns></returns>
        public NodeData FindShortest()
        {
            HasBeenExplored = new();
            ExplorationData = new();

            Queue.Clear();
            SetupStartNodes();

            while(Queue.Count > 0)
            {
                var nodeData = Queue.Dequeue();
                var node = nodeData.Node;
                var cost = nodeData.Cost;
                
                if (HasBeenExplored.Contains(node)) continue;
                HasBeenExplored.Add(node);
                ExplorationData.Add(node, nodeData);

                if (EndNodes.Contains(node))
                {
                    return nodeData;
                }

                var neighbours = FindNeighbourFunction(node).ToArray();

                foreach(var neighbour in neighbours)
                {
                    var transitionCost = TransitionCostFunction(node, neighbour);
                    var heuristicCost = HeuristicCostFunction(neighbour);

                    var costToReach = cost + transitionCost;
                    var priority = costToReach + heuristicCost;

                    Queue.Enqueue(new(neighbour, costToReach, nodeData), priority);
                }
            }

            return new(default, long.MaxValue, null);
        }

        private void SetupStartNodes()
        {
            foreach (var node in StartNodes)
            {
                var enqueable = new NodeData(node, 0, null);
                var priority = HeuristicCostFunction(node);

                Queue.Enqueue(enqueable, priority);
            }
        }

        public class NodeData
        {
            public TNode Node;
            public long Cost;
            public NodeData DiscoveredBy;

            public NodeData(TNode node, long cost, NodeData discoveredBy)
            {
                Node = node;
                Cost = cost;
                DiscoveredBy = discoveredBy;
            }
        }
    }
}
