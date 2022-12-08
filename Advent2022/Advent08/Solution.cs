using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Advent2022.Advent08
{
    public class Solution : ISolution
    {
        public long[][] trees;

        public Solution(string input)
        {
            trees = Input.GetInputLines(input)
                .Select(Helper.AsDigits)
                .ToArray();
        }
        public Solution() : this("Input.txt") { }

        public class Grid
        {
            public static HashSet<Coordinate> visibleTrees = new();

            public long EdgesHeightCount(long[][] trees)
            {
                visibleTrees = new();
                EdgesLeft(trees);
                EdgesRight(trees);
                EdgesTop(trees);
                EdgesBottom(trees);

                return visibleTrees.Count;
            }

            public void EdgesLeft(long[][] trees)
            {
                for (int y = 0; y < trees.Length; y++)
                {
                    long lastHeight = -1;
                    for (int x = 0; x < trees[y].Length; x++)
                    {
                        var treeHeight = trees[y][x];
                        if (treeHeight > lastHeight)
                        {
                            visibleTrees.Add(new(x, y));
                            lastHeight = treeHeight;
                        }
                    }
                }
            }

            public void EdgesRight(long[][] trees)
            {
                for (int y = 0; y < trees.Length; y++)
                {
                    long lastHeight = -1;
                    for (int x = trees[y].Length - 1; x >= 0; x--)
                    {
                        var treeHeight = trees[y][x];
                        if (treeHeight > lastHeight)
                        {
                            visibleTrees.Add(new(x, y));
                            lastHeight = treeHeight;
                        }
                    }
                }
            }

            public void EdgesTop(long[][] trees)
            {
                for (int x = 0; x < trees.Length; x++)
                {
                    long lastHeight = -1;
                    for (int y = 0; y < trees.Length; y++)
                    {
                        var treeHeight = trees[y][x];
                        if (treeHeight > lastHeight)
                        {
                            visibleTrees.Add(new(x, y));
                            lastHeight = treeHeight;
                        }
                    }
                }
            }

            public void EdgesBottom(long[][] trees)
            {
                for (int x = 0; x < trees.Length; x++)
                {
                    long lastHeight = -1;
                    for (int y = trees[x].Length - 1; y >= 0; y--)
                    {
                        var treeHeight = trees[y][x];
                        if (treeHeight > lastHeight)
                        {
                            visibleTrees.Add(new(x, y));
                            lastHeight = treeHeight;
                        }
                    }
                }
            }

            public long HighestScenic(long[][] trees)
            {
                long highestScenic = 0;
                for (int y = 0; y < trees.Length; y++)
                {
                    for (int x = 0; x < trees[y].Length; x++)
                    {
                        var scenic = GetScenic(trees, y, x);
                        if (scenic > highestScenic)
                        {
                            highestScenic = scenic;
                        }
                    }
                }

                return highestScenic;
            }

            public long GetScenic(long[][] trees, int tY, int tX)
            {
                var right = GetScenicRight(trees, tY, tX);
                var left = GetScenicLeft(trees, tY, tX);
                var top = GetScenicTop(trees, tY, tX);
                var bottom = GetScenicBottom(trees, tY, tX);

                return right * left * top * bottom;
            }

            public long GetScenicRight(long[][] trees, int tY, int tX)
            {
                var tHeight = trees[tY][tX];

                int count = 1;
                for (int x = tX + 1; x < trees[tY].Length; x++)
                {
                    var treeHeight = trees[tY][x];
                    if (treeHeight >= tHeight) return count;

                    count++;
                }
                return count - 1;
            }

            public long GetScenicLeft(long[][] trees, int tY, int tX)
            {
                var tHeight = trees[tY][tX];

                int count = 1;
                for (int x = tX - 1; x >= 0; x--)
                {
                    var treeHeight = trees[tY][x];
                    if (treeHeight >= tHeight) return count;

                    count++;
                }
                return count - 1;
            }

            public long GetScenicBottom(long[][] trees, int tY, int tX)
            {
                var tHeight = trees[tY][tX];

                int count = 1;
                for (int y = tY + 1; y < trees.Length; y++)
                {
                    var treeHeight = trees[y][tX];
                    if (treeHeight >= tHeight) return count;

                    count++;
                }
                return count - 1;
            }

            public long GetScenicTop(long[][] trees, int tY, int tX)
            {
                var tHeight = trees[tY][tX];

                int count = 1;
                for (int y = tY - 1; y >= 0; y--)
                {
                    var treeHeight = trees[y][tX];
                    if (treeHeight >= tHeight) return count;

                    count++;
                }
                return count - 1;
            }
        }

        public object GetResult1()
        {
            return new Grid().EdgesHeightCount(trees);
        }

        public object GetResult2()
        {
            // 126 too low
            // not 169

            return new Grid().HighestScenic(trees);
        }
    }
}
