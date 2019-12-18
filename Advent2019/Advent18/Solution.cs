using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2019.Shared.Search;
using System.Collections.Concurrent;

namespace Advent2019.Advent18
{
    public class Solution : ISolution
    {
        public List<Tile> tiles;
        public Tile start;
        public List<Tile> keyTiles;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            Parse(lines);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Parse(string[] lines)
        {
            tiles = new List<Tile>();
            keyTiles = new List<Tile>();
            var allTiles = new Dictionary<(int x, int y), Tile>();

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                
                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];

                    string lockedBy = null;
                    string hasKey = null;

                    if (c == '#') continue;
                    if (c - 'A' < 26 && c - 'A' >= 0) lockedBy = c.ToString().ToLower();
                    if (c - 'a' < 26 && c - 'a' >= 0)
                    {
                        hasKey = c.ToString();
                    }

                    var coord = new Coordinate(x, y);

                    var tile = new Tile(coord);
                    tile.HasKey = hasKey;
                    tile.LockedBy = lockedBy;

                    if (c == '@') start = tile;

                    tiles.Add(tile);
                    allTiles[(x, y)] = tile;
                    if (tile.HasKey != null) keyTiles.Add(tile);

                    if (allTiles.ContainsKey((x - 1, y))) tile.Link(allTiles[(x - 1, y)]);
                    if (allTiles.ContainsKey((x, y - 1))) tile.Link(allTiles[(x , y - 1)]);
                }
            }
        }

        public class Tile
        {
            public List<Tile> linked;
            public Coordinate coord;
            public string HasKey;
            public string LockedBy;

            public Tile(Coordinate coord)
            {
                this.coord = coord;
                this.linked = new List<Tile>();
            }

            public void Link(Tile toLink)
            {
                linked.Add(toLink);
                toLink.linked.Add(this);
            }

            public override string ToString()
            {
                return "Tile " + coord + " " + (HasKey == null ? "" : HasKey) + (LockedBy == null ? "" : LockedBy.ToUpper());
            }
        }

        public class TileNode : SearchNode
        {
            public Tile currentTile;
            public List<Tile> keyTiles;

            public TileNode(ConcurrentDictionary<SearchNode, int> expanded, int time) : base(expanded, time)
            {

            }

            int _heuristicDistance = -1;
            public override int GetHeuristicDistance()
            {
                if (_heuristicDistance == -1)
                {
                    if (keyTiles.Count == 0) return 0;

                    var distances = keyTiles.Select(kt => kt.coord.IntegerDistance(currentTile.coord));

                    _heuristicDistance = (int)distances.Sum();
                }
                return _heuristicDistance;
            }

            public override (int cost, SearchNode neighbour)[] GetNeighbours()
            {
                var result = new List<(int cost, SearchNode neighbour)>();

                var linked = currentTile.linked;
                var keys = keyTiles.Select(kt => kt.HasKey);
                for (int n = 0; n < linked.Count; n++)
                {
                    if (keys.Contains(linked[n].LockedBy)) continue;

                    var newNode = new TileNode(expanded, time + 1);
                    newNode.currentTile = currentTile.linked[n];
                    newNode.keyTiles = new List<Tile>();

                    foreach (var kt in keyTiles)
                    {
                        if (kt != newNode.currentTile) newNode.keyTiles.Add(kt);
                    }

                    result.Add((1, newNode));
                }

                return result.ToArray();
            }

            public override bool IsAtTarget()
            {
                return keyTiles.Count == 0;
            }

            public override int GetHashCode()
            {
                var keys = keyTiles.Sum(kt => kt.GetHashCode());
                return keys.GetHashCode() + currentTile.coord.GetHashCode() + time;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, this)) return true;
                var other = obj as TileNode;
                if (other == null) return false;
                if (!other.currentTile.coord.Equals(this.currentTile.coord)) return false;
                if (!other.keyTiles.SequenceEqual(this.keyTiles)) return false;

                return true;
            }

            public override string ToString()
            {
                return "TileNode " + "(" + Cost + ")" + currentTile.ToString();
            }
        }

        public string GetResult1()
        {
            var search = new Search();

            var expanded = new ConcurrentDictionary<SearchNode, int>();
            var baseNode = new TileNode(expanded, 0);
            baseNode.currentTile = start;
            baseNode.keyTiles = keyTiles;

            var result = new Search().Execute(baseNode);

            ;

            return result.Cost.ToString();
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
