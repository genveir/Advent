using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2023.Shared;
using Advent2023.Shared.Search;

namespace Advent2023.Advent17;

public class Solution : ISolution
{
    Dictionary<Coordinate, long> TileValues = new();
    Coordinate bottomRight;

    public Solution(string input)
    {
        var grid = Input.GetLetterGrid(input);

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                TileValues[new(x, y)] = grid[y][x] - '0';
            }
        }

        bottomRight = new(grid[0].Length - 1, grid.Length - 1);
    }
    public Solution() : this("Input.txt") { }

    public class Node : IEquatable<Node>
    {
        public Coordinate Position { get; set; }
        public long Value { get; set; }

        public Coordinate LastDirection { get; set; }
        public long NumOfLast { get; set; }

        public Node(Coordinate position, long value, Coordinate lastMove, long numOfLast)
        {
            Position = position;
            Value = value;
            LastDirection = lastMove;
            NumOfLast = numOfLast;
        }

        public IEnumerable<Node> FindNeighbours(Dictionary<Coordinate, long> tileValues)
        {
            var prospects = Position.GetNeighbours(orthogonalOnly: true);

            List<Node> neighbours = new();
            foreach (var prospect in prospects)
            {
                if (!tileValues.TryGetValue(prospect, out long value))
                    continue;

                var intendedMove = prospect - Position;

                if (new Coordinate(0, 0) - intendedMove == LastDirection)
                    continue;

                if (intendedMove == LastDirection)
                {
                    if (NumOfLast == 3) continue;
                    neighbours.Add(new Node(prospect, value, intendedMove, NumOfLast + 1));
                }
                else
                {
                    neighbours.Add(new Node(prospect, value, intendedMove, 1));
                }
            }
            return neighbours;
        }

        public IEnumerable<Node> FindUltraNeighbours(Dictionary<Coordinate, long> tileValues)
        {
            List<Coordinate> prospects = new();
            for (int n = 4; n < 11; n++)
            {
                prospects.Add(Position.ShiftX(n));
                prospects.Add(Position.ShiftY(n));
                prospects.Add(Position.ShiftX(-n));
                prospects.Add(Position.ShiftY(-n));
            }

            List<Node> neighbours = new();
            foreach(var prospect in prospects) 
            { 
                if (!tileValues.ContainsKey(prospect)) 
                    continue;

                var intendedMove = prospect - Position;
                var intendedDirection = new Coordinate(Normalize(intendedMove.X), Normalize(intendedMove.Y));
                var intendedDistance = intendedMove.AbsX + intendedMove.AbsY;

                var pos = Position;
                long value = 0;
                for (int n = 0; n < intendedDistance; n++)
                {
                    pos += intendedDirection;
                    value += tileValues[pos];
                }

                if (new Coordinate(0, 0) - intendedDirection == LastDirection)
                    continue;
                else if (intendedDirection == LastDirection)
                    continue;
                else if (LastDirection == new Coordinate(0,0))
                {
                    neighbours.Add(new Node(prospect, value, intendedDirection, intendedDistance));
                }
                else
                {
                    neighbours.Add(new Node(prospect, value, intendedDirection, intendedDistance));
                }
            }
            return neighbours;
        }

        public static long Normalize(long value) => value switch
        {
            0 => 0,
            < 0 => -1,
            > 0 => 1
        };

        public override int GetHashCode()
        {
            return Position.GetHashCode() + LastDirection.GetHashCode() + (int)NumOfLast;
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            return other.Position == Position &&
                other.LastDirection == LastDirection &&
                other.NumOfLast == NumOfLast;
        }

        public override string ToString()
        {
            return $"{Position}, Walked {LastDirection} x {NumOfLast}";
        }
    }

    public object GetResult1()
    {
        var aStar = new AStar<Node>(
            startNode: new Node(new(0, 0), 0, new(0, 0), 1),
            endStates: n => n.Position == bottomRight,
            findNeighbourFunction: n => n.FindNeighbours(TileValues),
            transitionCostFunction: (from, to) => to.Value,
            heuristicCostFunction: n => n.Position.ManhattanDistance(bottomRight));

        var nodeData = aStar.FindShortest();

        return nodeData.Cost;
    }

    // 891 too low
    public object GetResult2()
    {
        var aStar = new AStar<Node>(
            startNode: new Node(new(0, 0), 0, new(0, 0), 1),
            endStates: n => n.Position == bottomRight,
            findNeighbourFunction: n => n.FindUltraNeighbours(TileValues),
            transitionCostFunction: (from, to) => to.Value,
            heuristicCostFunction: n => n.Position.ManhattanDistance(bottomRight));

        var nodeData = aStar.FindShortest();

        var visual = Print(nodeData.PathNodes());

        return nodeData.Cost;
    }

    public string Print(IEnumerable<Node> nodes)
    {
        var nodeMap = nodes.ToDictionary(n => n.Position, n => n);

        var sb = new StringBuilder();
        for (int y = 0; y <= bottomRight.Y; y++)
        {
            for (int x = 0; x <= bottomRight.X; x++)
            {
                if (nodeMap.ContainsKey(new(x, y)))
                    sb.Append('#');
                else
                    sb.Append(TileValues[new(x, y)]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
