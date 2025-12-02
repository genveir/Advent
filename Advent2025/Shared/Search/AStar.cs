namespace Advent2025.Shared.Search;

public class AStar<TNode> where TNode : IEquatable<TNode>
{
    public PriorityQueue<NodeData, long> Queue = new();

    public Func<TNode, bool> EndStates { get; }
    public List<TNode> StartNodes { get; }
    public Func<TNode, TNode, long> TransitionCostFunction { get; }
    public Func<TNode, long> HeuristicCostFunction { get; }
    public Func<TNode, IEnumerable<TNode>> FindNeighbourFunction { get; }

    private static readonly Func<TNode, TNode, long> DefaultTransitionCost = (TNode a, TNode b) => 1L;
    private static readonly Func<TNode, long> DefaultHeuristicCost = (TNode a) => 0L;

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNodes">The Node from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endNodes">The Nodes at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(TNode startNode, IEnumerable<TNode> endNodes,
        Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null) :
            this([startNode], endNodes, findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
    { }

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNodes">The Nodes from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endNodes">The Node at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(IEnumerable<TNode> startNodes, TNode endNode, Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null) :
        this(startNodes, [endNode], findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
    { }

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNodes">The Node from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endNodes">The Node at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(TNode startNode, TNode endNode, Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null) :
        this([startNode], [endNode], findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
    { }

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNodes">The Nodes from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endNodes">The Nodes at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(IEnumerable<TNode> startNodes, IEnumerable<TNode> endNodes, Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null)
    {
        StartNodes = startNodes.ToList();

        var _endNodes = new HashSet<TNode>(endNodes);
        EndStates = _endNodes.Contains;

        FindNeighbourFunction = findNeighbourFunction;
        TransitionCostFunction = transitionCostFunction ?? DefaultTransitionCost;
        HeuristicCostFunction = heuristicCostFunction ?? DefaultHeuristicCost;
    }

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNode">The Node from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endStates">A function to determine if a node is an endstate at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(TNode startNode, Func<TNode, bool> endStates, Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null) :
        this([startNode], endStates, findNeighbourFunction, transitionCostFunction, heuristicCostFunction)
    {
    }

    /// <summary>
    /// Create AStar Setup
    /// </summary>
    /// <param name="startNodes">The Nodes from which the algorithm will start it's search (at cost 0)</param>
    /// <param name="endStates">A function to determine if a node is an endstate at which the algorithm will stop and return</param>
    /// <param name="findNeighbourFunction">A function to find the reachable neighbours of a node</param>
    /// <param name="transitionCostFunction">A function to calculate the transition cost between two nodes. Leave null for (_, _) => 1</param>
    /// <param name="heuristicCostFunction">A function to calculate the heuristic distance to the target. Leave null for _ => 0</param>
    public AStar(IEnumerable<TNode> startNodes, Func<TNode, bool> endStates, Func<TNode, IEnumerable<TNode>> findNeighbourFunction,
        Func<TNode, TNode, long> transitionCostFunction = null,
        Func<TNode, long> heuristicCostFunction = null)
    {
        StartNodes = startNodes.ToList();
        EndStates = endStates;
        FindNeighbourFunction = findNeighbourFunction;
        TransitionCostFunction = transitionCostFunction ?? DefaultTransitionCost;
        HeuristicCostFunction = heuristicCostFunction ?? DefaultHeuristicCost;
    }

    /// <summary>
    /// If a node was exploited, it will be in here, with NodeData (i.e. cost at which it was exploited, and node it was discovered from with
    /// the lowest cost)
    /// </summary>
    public Dictionary<TNode, NodeData> ExploitationData;

    public delegate void NodeDataEventHandler(AStar<TNode> AStar, NodeData nodeData);

    public event NodeDataEventHandler OnDequeue;

    public event NodeDataEventHandler BeforeEnqueue;

    public delegate void NeighboursEventHandler(AStar<TNode> AStar, NodeData nodeData, IEnumerable<TNode> neighbours);

    public event NeighboursEventHandler BeforeHandlingNeighbours;

    /// <summary>
    /// Returns the cost of the shortest path
    /// </summary>
    /// <returns></returns>
    public NodeData FindShortest()
    {
        Reset();

        while (Queue.Count > 0)
        {
            var nodeData = Queue.Dequeue();
            OnDequeue?.Invoke(this, nodeData);

            var node = nodeData.Node;
            var cost = nodeData.Cost;

            if (ExploitationData.TryGetValue(node, out NodeData previousExploitation))
            {
                if (previousExploitation.Cost <= cost) continue;
            }
            ExploitationData[node] = nodeData;

            if (EndStates(node))
            {
                return nodeData;
            }

            var neighbours = FindNeighbourFunction(node).ToList();

            BeforeHandlingNeighbours?.Invoke(this, nodeData, neighbours);
            foreach (var neighbour in neighbours)
            {
                var transitionCost = TransitionCostFunction(node, neighbour);
                var heuristicCost = HeuristicCostFunction(neighbour);

                var costToReach = cost + transitionCost;
                var priority = costToReach + heuristicCost;

                var toEnqueue = new NodeData(neighbour, costToReach, nodeData);

                BeforeEnqueue?.Invoke(this, toEnqueue);
                Queue.Enqueue(toEnqueue, priority);
            }
        }

        return new(default, long.MaxValue, null);
    }

    private void Reset()
    {
        ExploitationData = [];
        Queue.Clear();
        SetupStartNodes();
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

        public List<TNode> PathNodes() => Path()
            .Select(nd => nd.Node)
            .ToList();

        public List<NodeData> Path()
        {
            var aggregate = new List<NodeData>();

            Path(aggregate);

            return aggregate;
        }

        public void Path(List<NodeData> aggregate)
        {
            DiscoveredBy?.Path(aggregate);

            aggregate.Add(this);
        }

        public override string ToString()
        {
            return $"NodeData for {Node}, Cost {Cost}";
        }
    }
}