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
            var paths = new List<List<int>>();
            for (int i = 0; i < SmallCaves.Count; i++)
            {
                var list = new List<int>();
                for (int j = 0; j < SmallCaves.Count; j++)
                {
                    list.Add(CountPaths(SmallCaves[i], SmallCaves[j]));
                }
                paths.Add(list);
            }

            NumberOfPaths = paths.Select(p => p.ToArray()).ToArray();
            Visted = new int[Caves.Count];
            return DFSWithMem(startIndex, 0);
            ;
        }

        public long Convert(int[] Visted, int caveFrom, int twice)
        {
            long acc = caveFrom;
            int i = 0;
            for (; i < Visted.Length; i++)
            {
                acc += Visted[i] << (i + 4);
            }
            acc += twice << (i + 4);
            return acc;
        }

        int[][] NumberOfPaths;
        int[] Visted;
        private unsafe int DFSWithMem(int caveFrom, int twice)
        {
            fixed (int* memPtr = Memory)
            fixed (int* visPtr = Visted)
            {
                long key = Convert(Visted, caveFrom, twice);
                if (memPtr[key] == 0)
                {
                    int count = 0;
                    for (int caveTo = 0; caveTo < NumberOfPaths.Length; caveTo++)
                    {
                        if (NumberOfPaths[caveFrom][caveTo] == 0 || caveTo == startIndex) continue;

                        var visited = visPtr[caveTo];

                        if (caveTo == endIndex)
                        {
                            count += NumberOfPaths[caveFrom][caveTo];
                        }
                        else if (visited == 0 && twice == 0)
                        {
                            visPtr[caveTo] = 1 - visPtr[caveTo];
                            count += (NumberOfPaths[caveFrom][caveTo]) * DFSWithMem(caveTo, visited + twice);
                            visPtr[caveTo] = 1 - visPtr[caveTo];
                        }
                        else if (visited == 0 && twice == 1)
                        {
                            visPtr[caveTo] = 1 - visPtr[caveTo];
                            count += (NumberOfPaths[caveFrom][caveTo]) * DFSWithMem(caveTo, visited + twice);
                            visPtr[caveTo] = 1 - visPtr[caveTo];
                        }
                        else if (visited == 1 && twice == 0)
                        {
                            count += (NumberOfPaths[caveFrom][caveTo]) * DFSWithMem(caveTo, 1);
                        }
                        else if (visited == 1 && twice == 1)
                        {
                            count += 0;
                        }
                    }
                    memPtr[key] = count;
                }
                return memPtr[key];
            }
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