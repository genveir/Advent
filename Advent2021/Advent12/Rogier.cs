using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advent2021.Advent12
{
    class Day12DFS
    {
        int startIndex;
        int endIndex;
        int[] Memory;
        List<string> Caves;
        List<string> SmallCaves;
        List<string> BigCaves;
        Dictionary<string, List<string>> Edges;

        public object Lezgo()
        {
            var lines = new string[]
            {
                "ln-nr",
                "ln-wy",
                "fl-XI",
                "qc-start",
                "qq-wy",
                "qc-ln",
                "ZD-nr",
                "qc-YN",
                "XI-wy",
                "ln-qq",
                "ln-XI",
                "YN-start",
                "qq-XI",
                "nr-XI",
                "start-qq",
                "qq-qc",
                "end-XI",
                "qq-YN",
                "ln-YN",
                "end-wy",
                "qc-nr",
                "end-nr"}.ToList();

            GetCaves(lines);
            startIndex = SmallCaves.IndexOf("start");
            endIndex = SmallCaves.IndexOf("end");
            Memory = new int[40000];
            var paths = new List<List<(int to, int num)>>();
            for (int i = 0; i < SmallCaves.Count; i++)
            {
                var list = new List<(int to, int num)>();
                for (int j = 0; j < SmallCaves.Count; j++)
                {
                    if (j == startIndex) continue;

                    var num = CountPaths(SmallCaves[i], SmallCaves[j]);
                    if (num > 0) list.Add((j, num));
                }
                paths.Add(list);
            }

            NumberOfPaths = paths.Select(p => p.ToArray()).ToArray();

            Visted = new int[Caves.Count + 2];
            twiceIndex = Visted.Length - 2;
            caveIndex = Visted.Length - 1; // not binary

            Visted[caveIndex] = startIndex;
            
            return DFSWithMem();
        }

        public long Convert(int[] Visted)
        {
            long acc = Visted[caveIndex]; // not binary, can't fold like the rest
            int i = 0;
            for (; i < Visted.Length - 1; i++)
            {
                acc += Visted[i] << (i + 4);
            }
            return acc;
        }

        (int to, int num)[][] NumberOfPaths;
        int[] Visted;
        int twiceIndex;
        int caveIndex;

        private int DFSWithMem()
        {
            var caveFrom = Visted[caveIndex];

            if (caveFrom == endIndex) return 1;

            long key = Convert(Visted);
            if (Memory[key] == 0)
            {
                int count = 0;
                for (int target = 0; target < NumberOfPaths[caveFrom].Length; target++)
                {
                    var caveTo = NumberOfPaths[caveFrom][target].to;
                    var num = NumberOfPaths[caveFrom][target].num;

                    var visited = Visted[caveTo];
                    var twice = Visted[twiceIndex];

                    Visted[caveTo] = 1;
                    Visted[twiceIndex] = visited | twice;
                    Visted[caveIndex] = caveTo;

                    count += ((visited & twice) == 1) ? 0 :  num * DFSWithMem();

                    Visted[caveTo] = visited & Visted[caveTo];
                    Visted[twiceIndex] = twice;
                    Visted[caveIndex] = caveFrom;
                }
                Memory[key] = count;
            }
            return Memory[key];   
        }

        private void GetCaves(List<string> Lines)
        {
            HashSet<string> CavesSet = new HashSet<string>();
            HashSet<string> SmallCavesSet = new HashSet<string>();
            HashSet<string> BigCavesSet = new HashSet<string>();
            Edges = new Dictionary<string, List<string>>();

            foreach (var trans in Lines.Select(x => x.Split('-')))
            {
                void Add(string from, string to)
                {
                    if (!Edges.ContainsKey(from)) Edges[from] = new List<string>();
                    Edges[from].Add(to);
                }
                CavesSet.Add(trans[0]);
                CavesSet.Add(trans[1]);
                Add(trans[0], trans[1]);
                Add(trans[1], trans[0]);
            }
            foreach (var cave in CavesSet)
            {
                if (char.IsUpper(cave.ToCharArray()[0]))
                {
                    BigCavesSet.Add(cave);
                }
                if (char.IsLower(cave.ToCharArray()[0]))
                {
                    SmallCavesSet.Add(cave);
                }
            }

            Caves = CavesSet.ToList();
            SmallCaves = SmallCavesSet.ToList();
            BigCaves = BigCavesSet.ToList();
        }

        private int CountPaths(string start, string end)
        {

            Queue<string> q = new Queue<string>();
            HashSet<string> visted = new HashSet<string>();
            q.Enqueue(start);
            int count = 0;
            while (q.Count > 0)
            {
                var current = q.Dequeue();
                foreach (var next in Caves.Where(next => Edges.ContainsKey(current) && Edges[current].Contains(next) && !visted.Contains(next)))
                {
                    if (next == end)
                    {
                        count++;
                    }
                    else if (BigCaves.Contains(next))
                    {
                        visted.Add(next);
                        q.Enqueue(next);
                    }
                }
            }
            return count;
        }
    }
}