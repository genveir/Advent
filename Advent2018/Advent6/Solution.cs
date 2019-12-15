using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent6
{
    class Solution : ISolution
    {
        private List<SearchItem> GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent2018.Advent6.Input.txt");

            var vals = new List<SearchItem>();
            using (var txt = new StreamReader(input))
            {
                while (!txt.EndOfStream)
                    vals.Add(SearchItem.Parse(txt.ReadLine()));
            }

            return vals;
        }

        private class Grid
        {
            public Grid(int XLowest, int XHighest, int YLowest, int YHighest)
            {
                this.XLowest = XLowest;
                this.XHighest = XHighest;
                this.YLowest = YLowest;
                this.YHighest = YHighest;

                GridCoords = new GridCoord[XHighest - XLowest + 1, YHighest - YLowest + 1];
            }

            public int XLowest;
            public int YLowest;
            public int XHighest;
            public int YHighest;

            private GridCoord[,] GridCoords;

            public GridCoord Coord(int X, int Y)
            {
                if (GridCoords[X - XLowest, Y - YLowest] == null)
                    GridCoords[X - XLowest, Y - YLowest] = new GridCoord() { X = X, Y = Y };

                return GridCoords[X - XLowest, Y - YLowest];
            }
        }

        private class GridCoord
        {
            public int X;
            public int Y;

            public SearchItem closest;

            public int CalcDistance(GridCoord other)
            {
                var xDist = Math.Abs(X - other.X);
                var yDist = Math.Abs(Y - other.Y);

                return xDist + yDist;
            }

            public int CalcDistance(int X, int Y)
            {
                var xDist = Math.Abs(this.X - X);
                var yDist = Math.Abs(this.Y - Y);

                return xDist + yDist;
            }
        }

        private class SearchItem : GridCoord
        {
            public int numClosest;
            public bool IsInfinite = false;

            public static SearchItem Parse(string input)
            {
                var coords = input.Split(",").Select(i => int.Parse(i)).ToArray();
                return new SearchItem()
                {
                    X = coords[0],
                    Y = coords[1]
                };
            }
        }

        public int GetLargestArea()
        {
            var searchItems = GetInput();

            int XLowest = searchItems.Min(s => s.X);
            int XHighest = searchItems.Max(s => s.X);
            int YLowest = searchItems.Min(s => s.Y);
            int YHighest = searchItems.Max(s => s.Y);

            var grid = new Grid(
                XLowest,
                XHighest,
                YLowest,
                YHighest
            );

            for (int x = XLowest; x <= XHighest; x++)
            {
                for (int y = YLowest; y <= YHighest; y++)
                {
                    var gridCoord = grid.Coord(x, y);

                    int lowestDist = int.MaxValue;
                    SearchItem lowestItem = null;
                    foreach (var item in searchItems)
                    {
                        var dist = item.CalcDistance(gridCoord);
                        if (dist < lowestDist)
                        {
                            lowestDist = dist;
                            lowestItem = item;
                        }
                        else if (dist == lowestDist) lowestItem = null;
                    }
                    gridCoord.closest = lowestItem;
                    if (lowestItem != null)
                    {
                        lowestItem.numClosest++;
                        if (x == XLowest || x == XHighest || y == YLowest || y == YHighest) lowestItem.IsInfinite = true;
                    }
                }
            }

            return searchItems.Where(s => !s.IsInfinite).Max(s => s.numClosest);
        }

        public int GetAreaSize()
        {
            var searchItems = GetInput();

            int inArea = 0;
            for (int x = -1000; x <= 1000; x++)
            {
                for (int y = -1000; y <= 1000; y++)
                {
                    if (searchItems.Sum(s => s.CalcDistance(x, y)) < 10000) inArea++;
                }
            }

            return inArea;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetLargestArea());
            Console.WriteLine("part2: " + GetAreaSize());
        }
    }
}
