﻿using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2019.Shared.Search;
using System.Collections.Concurrent;
using Collections.Generic;

namespace Advent2019.Advent18
{
    public class Solution : ISolution
    {
        public List<Tile> tiles;
        public Tile start;
        public List<Tile> keyTiles;

        public Solution(Input.InputMode inputMode, string input)
        {
            Parse(inputMode, input);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Parse(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            tiles = new List<Tile>();
            keyTiles = new List<Tile>();
            var allTiles = new Dictionary<(int x, int y), Tile>();

            var allKeys = new List<string>();
            var doorTiles = new List<Tile>();
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
                        allKeys.Add(hasKey);
                    }

                    var coord = new Coordinate(x, y);

                    var tile = new Tile(coord);
                    tile.HasKey = hasKey;
                    tile.LockedBy = lockedBy;
                    doorTiles.Add(tile);

                    if (c == '@') start = tile;

                    tiles.Add(tile);
                    allTiles[(x, y)] = tile;
                    if (tile.HasKey != null) keyTiles.Add(tile);

                    if (allTiles.ContainsKey((x - 1, y))) tile.Link(allTiles[(x - 1, y)]);
                    if (allTiles.ContainsKey((x, y - 1))) tile.Link(allTiles[(x, y - 1)]);
                }
            }

            foreach(var door in doorTiles)
            {
                if (!allKeys.Contains(door.LockedBy)) door.LockedBy = null;
            }

            SetKeyRoutes();
        }

        public class Tile
        {
            public List<Tile> linked;
            public Coordinate coord;
            public string HasKey;
            public string LockedBy;

            public long searchNum;
            public long searchDist;
            public HashSet<string> searchBlockers = new HashSet<string>();

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

            public bool CanGoHere(List<string> keys)
            {
                if (LockedBy == null) return true;
                else return keys.Contains(LockedBy);
            }

            public override string ToString()
            {
                return "Tile " + coord + " " + (HasKey == null ? "" : HasKey) + (LockedBy == null ? "" : LockedBy.ToUpper());
            }
        }

        public static Dictionary<string, Dictionary<string, KeyAndDist>> KeyRoutes;

        public void SetKeyRoutes()
        {
            KeyRoutes = new Dictionary<string, Dictionary<string, KeyAndDist>>();

            keyTiles = keyTiles.OrderBy(kt => kt.HasKey).ToList();
            for (int kr1 = 0; kr1 < keyTiles.Count; kr1++)
            {
                var kti = keyTiles[kr1];

                var state = new State(kti, keyTiles.Where(kt => kt != kti).ToList(), keyTiles.Select(kt => kt.HasKey).Where(kt => kt != kti.HasKey).ToList(), 0);

                var keyRoutes = state.GetAll();

                KeyRoutes.Add(kti.HasKey, new Dictionary<string, KeyAndDist>());
                foreach (var route in keyRoutes)
                {
                    KeyRoutes[kti.HasKey].Add (route.Key, route);
                }
            }
        }

        public class KeyAndDist
        {
            public KeyAndDist()
            {
                Blockers = new HashSet<string>();
            }

            public string Key;
            public long Distance;
            public HashSet<string> Blockers;

            public override string ToString()
            {
                return Key + " " + Distance + "[" + string.Join(",", Blockers) + "]";
            }
        }

        public class State
        {
            public Tile currentTile;
            public List<Tile> keyTiles;
            public List<string> keys;
            public long DistanceSoFar;

            public static long searchNum = 0;

            public State(Tile currentTile, List<Tile> keyTiles, List<string> keys, long totalDistance)
            {
                this.currentTile = currentTile;
                this.keyTiles = keyTiles;
                this.keys = keys;
                this.DistanceSoFar = totalDistance;
            }

            public List<KeyAndDist> GetAll()
            {
                searchNum = searchNum + 1;
                var tilesToSearch = new Queue<Tile>();

                var keyDistances = new List<KeyAndDist>();

                currentTile.searchNum = searchNum;
                currentTile.searchDist = 0;
                currentTile.searchBlockers = new HashSet<string>();
                tilesToSearch.Enqueue(currentTile);
                while (tilesToSearch.Count > 0)
                {
                    var tile = tilesToSearch.Dequeue();

                    var linked = tile.linked;

                    foreach (var link in linked)
                    {
                        if (link.searchNum == searchNum) continue;
                        link.searchNum = searchNum;

                        link.searchDist = tile.searchDist + 1;
                        link.searchBlockers = new HashSet<string>(tile.searchBlockers);

                        if (link.LockedBy != null) link.searchBlockers.Add(link.LockedBy);

                        if (link.HasKey != null)
                        {
                            keyDistances.Add(new KeyAndDist() { Key = link.HasKey, Distance = link.searchDist, Blockers = new HashSet<string>(link.searchBlockers) });
                            link.searchBlockers.Add(link.HasKey);
                        }

                        tilesToSearch.Enqueue(link);
                    }
                }

                return keyDistances;
            }

            public List<KeyAndDist> GetCachedReachable()
            {
                // return KeyDistances waar blockers.execpt(keys) leeg is
                var kds = KeyRoutes[currentTile.HasKey];

                var validKeys = keyTiles.Select(t => t.HasKey);

                return kds
                    .Values
                    .Where(kd => kd.Blockers.Except(keys).Count() == 0)
                    .Where(kd => validKeys.Contains(kd.Key))
                    .ToList();
            }

            public List<KeyAndDist> GetReachable()
            {
                if (currentTile.HasKey != null) return GetCachedReachable();

                searchNum = searchNum + 1;
                var tilesToSearch = new Queue<Tile>();

                var reachable = new List<KeyAndDist>();

                currentTile.searchNum = searchNum++;
                currentTile.searchDist = 0;
                tilesToSearch.Enqueue(currentTile);
                while (tilesToSearch.Count > 0)
                {
                    var tile = tilesToSearch.Dequeue();

                    var linked = tile.linked;

                    foreach(var link in linked)
                    {
                        if (link.searchNum == searchNum) continue;
                        link.searchNum = searchNum;
                        link.searchBlockers = new HashSet<string>();

                        if (!link.CanGoHere(keys)) continue;

                        link.searchDist = tile.searchDist + 1;

                        if (keyTiles.Contains(link)) reachable.Add(new KeyAndDist() { Key = link.HasKey, Distance = link.searchDist });

                        tilesToSearch.Enqueue(link);
                    }
                }

                return reachable;
            }

            public List<State> GetNextStates()
            {
                var reachable = GetReachable();

                var states = new List<State>();

                foreach(var reached in reachable)
                {
                    var newKeys = new List<string>(keys);
                    newKeys.Add(reached.Key);

                    var newTile = keyTiles.Single(kt => kt.HasKey == reached.Key);

                    var newKeyTiles = new List<Tile>(keyTiles);
                    newKeyTiles.Remove(newTile);

                    var newState = new State(newTile, newKeyTiles, newKeys, DistanceSoFar + reached.Distance);
                    states.Add(newState);
                }

                return states;
            }

            public override string ToString()
            {
                return (DistanceSoFar + " [").PadLeft(6) + string.Join(' ', keys) + "] at " + currentTile;
            }
        }

        public State GetRoute(State state)
        {
            var prioQueue = new BinaryHeap<long, State>();

            prioQueue.Enqueue(0, state);

            HashSet<string> handled = new HashSet<string>();

            while (!prioQueue.IsEmpty)
            {
                var currentState = prioQueue.Dequeue().Value;

                if (currentState.keyTiles.Count == 0) return currentState;

                var newStates = currentState.GetNextStates();

                foreach (var newState in newStates)
                {
                    newState.keys.Sort();
                    var dist = newState.DistanceSoFar;
                    var tile = newState.currentTile.HasKey;
                    var sumKey = string.Concat(newState.keys) + dist.ToString() + tile;

                    if (handled.Contains(sumKey)) continue;
                    handled.Add(sumKey);

                    prioQueue.Enqueue(newState.DistanceSoFar, newState);
                }
            }

            return null;
        }

        public long GetDistance()
        {
            var state = new State(start, keyTiles, new List<string>(), 0);

            var route = GetRoute(state);

            return route.DistanceSoFar;
        }
        
        public string GetResult1()
        {
            return GetDistance().ToString();
        }

        public string GetResult2()
        {
            Parse(Input.InputMode.Embedded, "Input21");
            var route1 = GetDistance();

            Parse(Input.InputMode.Embedded, "Input22");
            var route2 = GetDistance();

            Parse(Input.InputMode.Embedded, "Input23");
            var route3 = GetDistance();

            Parse(Input.InputMode.Embedded, "Input24");
            var route4 = GetDistance();

            return (route1 + route2 + route3 + route4).ToString();
        }
    }
}
