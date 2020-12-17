using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advent2020.Advent17
{
    public class Solution : ISolution
    {
        public SwappingGrid grid;

        private string[] lines;

        public Solution(string input)
        {
            this.lines = Input.GetInputLines(input).ToArray();
        }

        public void SetGrid(int dimensions)
        {
            grid = new SwappingGrid();

            for (int n = 0; n < 2; n++)
            {
                var builder = new InfiniteAdjacencyGridBuilder<int, _4DGridPosition>();
                if (dimensions > 3) builder.AddLayer(igp => igp.W, (igp, val) => igp.W = val);
                if (dimensions > 2) builder.AddLayer(igp => igp.Z, (igp, val) => igp.Z = val);
                builder.AddLayer(igp => igp.Y, (igp, val) => igp.Y = val);
                builder.AddLayer(igp => igp.X, (igp, val) => igp.X = val);
                grid.Current = builder.Complete();

                grid.Swap();
            }

            var wCoord = W_Value.Get(0);
            var zCoord = Z_Value.Get(0);
            for (int y = 0; y < lines.Length; y++)
            {
                var yCoord = Y_Value.Get(y);

                for (int x = 0; x < lines[y].Length; x++)
                {
                    var xCoord = X_Value.Get(x);

                    var pos = new _4DGridPosition(xCoord, yCoord, zCoord, wCoord);

                    int val = 0;
                    if (lines[y][x] == '#')
                    {
                        grid.Front.Add(pos);
                        val = 10000;
                    }
                    
                    grid.Current.Set(pos, val);
                }
            }
            grid.Front.Swap();

            ofInterest = new HashSet<_4DGridPosition>();
        }
        public Solution() : this("Input.txt") { }

        public HashSet<_4DGridPosition> ofInterest;
        public void RunUpdate()
        {
            var active = grid.Front.Get().OrderBy(pos => pos.Z).ThenBy(pos => pos.Y).ThenBy(pos => pos.X).ToArray();

            foreach (var pos in active)
            {
                var adjacent = grid.Current.GetAdjacentPositions(pos);

                foreach (var adj in adjacent)
                {
                    grid.Current.Update(adj, i => i + 1);

                    ofInterest.Add(adj);
                }
            }

            foreach (var pos in ofInterest)
            {
                var element = grid.Current.Get(pos);

                if (element == 10003 || element == 10004 || element == 3)
                {
                    grid.Next.Set(pos, 10000);

                    grid.Front.Add(pos);
                }
            }

            grid.Swap();
        }
    
        public object GetResult1()
        {
            SetGrid(3);
            for (int n = 0; n < 6; n++)
            {
                RunUpdate();
            }

            return grid.Front.Get().Count();
        }

        public object GetResult2()
        {
            SetGrid(4);
            for (int n = 0; n < 6; n++)
            {
                RunUpdate();
            }

            return grid.Front.Get().Count();
        }
    }
}
