﻿using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent12
{
    public class Solution : ISolution
    {
        List<Cave> caves;
        Dictionary<string, Cave> allCaves = new Dictionary<string, Cave>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<ParsedInput>("a-b");

            var links = inputParser.Parse(lines);

            foreach (var link in links) link.SetCaves(allCaves); 

            caves = allCaves.Values.ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public string from;
            public string to;

            public Cave fromCave;
            public Cave toCave;

            [ComplexParserConstructor]
            public ParsedInput(string from, string to)
            {
                this.from = from;
                this.to = to;
            }

            public void SetCaves(Dictionary<string, Cave> allCaves)
            {
                if (!allCaves.TryGetValue(from, out fromCave))
                {
                    fromCave = new Cave(from);
                    allCaves.Add(from, fromCave);
                }
                if (!allCaves.TryGetValue(to, out toCave))
                {
                    toCave = new Cave(to);
                    allCaves.Add(to, toCave);
                }

                fromCave.LinkNeighbour(toCave);
                if (fromCave.name != "start") toCave.LinkNeighbour(fromCave);
            }
        }

        public class Cave
        {
            public string name;

            public bool isBig;

            public List<Cave> neighbours = new List<Cave>();

            public Cave(string name)
            {
                this.name = name;
                isBig = name.ToUpper() == name;
            }

            public void LinkNeighbour(Cave neighbour)
            {
                neighbours.Add(neighbour);
            }

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, this)) return true;
                var other = obj as Cave;
                return other.name == this.name;
            }

            public override string ToString()
            {
                return name;
            }
        }

        public class SearchNode1
        {
            public HashSet<Cave> cantVisit = new HashSet<Cave>();
            public Cave Current;

            public SearchNode1(Cave start)
            {
                this.Current = start;
            }

            public SearchNode1(SearchNode1 parent, Cave newCurrent)
            {
                this.cantVisit = new HashSet<Cave>(parent.cantVisit);
                this.Current = newCurrent;

                if (!parent.Current.isBig) this.cantVisit.Add(parent.Current);
            }

            public IEnumerable<SearchNode1> Explore()
            {
                foreach (var neighbour in Current.neighbours)
                {
                    if (cantVisit.Contains(neighbour)) continue;
                    else yield return new SearchNode1(this, neighbour);
                }
            }

            public bool isWin => Current.name == "end";
        }

        public class SearchNode2
        {
            public List<Cave> inOrder = new List<Cave>();
            public HashSet<Cave> smallCavesVisited = new HashSet<Cave>();
            public bool cantVisitSmall = false;
            public Cave Current;

            public SearchNode2(Cave start) 
            {
                this.Current = start;
                this.inOrder = new List<Cave>() { start };
            }

            public SearchNode2(SearchNode2 parent, Cave newCurrent)
            {
                this.inOrder = new List<Cave>(parent.inOrder);
                this.inOrder.Add(newCurrent);

                this.smallCavesVisited = new HashSet<Cave>(parent.smallCavesVisited); // small visited once
                this.cantVisitSmall = parent.cantVisitSmall;
                this.Current = newCurrent;

                if (smallCavesVisited.Contains(Current)) this.cantVisitSmall = true;

                if (!Current.isBig) smallCavesVisited.Add(Current);
            }

            public IEnumerable<SearchNode2> Explore()
            {
                foreach(var neighbour in Current.neighbours)
                {
                    if (cantVisitSmall && smallCavesVisited.Contains(neighbour)) continue;
                    else yield return new SearchNode2(this, neighbour);
                }
            }

            public bool isWin => Current.name == "end";

            public override string ToString()
            {
                //return $"node at {Current}, visited {string.Join(",", this.smallCavesVisited)}, can't visit: {string.Join(",", this.cantVisit)}";
                return string.Join(",", inOrder);
            }
        }

        public IEnumerable<SearchNode1> EnumeratePaths1()
        {
            var start = allCaves["start"];
            var node = new SearchNode1(start);

            var queue = new Queue<SearchNode1>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var popped = queue.Dequeue();

                if (popped.isWin) yield return popped;
                else
                {
                    var newNodes = popped.Explore();
                    foreach (var newNode in newNodes) queue.Enqueue(newNode);
                }
            }
        }
    

        public IEnumerable<SearchNode2> EnumeratePaths2()
        {
            var start = allCaves["start"];
            var node = new SearchNode2(start);

            var queue = new Queue<SearchNode2>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var popped = queue.Dequeue();

                if (popped.isWin) yield return popped;
                else
                {
                    var newNodes = popped.Explore();
                    foreach (var newNode in newNodes) queue.Enqueue(newNode);
                }
            }
        }

        public object GetResult1()
        {
            return EnumeratePaths1().Count();
        }

        public object GetResult2()
        {
            var allResults = EnumeratePaths2().ToList();
            return allResults.Count;
        }
    }
}
