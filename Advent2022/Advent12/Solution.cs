using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using Advent2022.Shared.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Advent2022.Advent12
{
    public class Solution : ISolution
    {
        public Tile start;
        public TileGrid<char, Tile> tileGrid;

        public Solution(string input)
        {
            var grid = Input.GetLetterGrid(input).ToArray();

            var constructor = (char c) => c switch
            {
                'S' => new Tile(0, isStart: true),
                'E' => new Tile(25, isEnd: true),
                _ => new Tile(c - 'a')
            };

            tileGrid = new(grid, constructor);
            start = tileGrid.Single(t => t.IsStart);
        }
        public Solution() : this("Input.txt") { }

        public class Tile : BaseTile<Tile>
        {
            public long Height;
            public bool IsStart;
            public bool IsEnd;

            public long Exploration = 0;
            public long ExploreLength = 0;

            [ComplexParserConstructor]
            public Tile(long height, bool isStart = false, bool isEnd = false)
            {
                Height = height;
                IsStart = isStart;
                IsEnd = isEnd;
            }

            public IEnumerable<Tile> Explore(long exploration)
            {
                if (Exploration == exploration) return Enumerable.Empty<Tile>();
                Exploration = exploration;

                var adjacent = Neighbours
                    .Where(n => n.Height - 1 <= this.Height);

                foreach (var adjacentTile in adjacent) 
                    adjacentTile.BeExplored(this);

                return adjacent;
            }

            public void BeExplored(Tile explorer)
            {
                ExploreLength = explorer.ExploreLength + 1;
            }
        }

        private long BFSNum = 0;
        public long BFSToGoal(Tile bfsStart)
        {
            BFSNum++;
            bfsStart.ExploreLength = 0;

            Queue<Tile> tiles= new Queue<Tile>();

            tiles.Enqueue(bfsStart);
            while (tiles.Count > 0)
            {
                var tile = tiles.Dequeue();

                if (tile.IsEnd) return tile.ExploreLength;

                var newTiles = tile.Explore(BFSNum);
                foreach (var newTile in newTiles) tiles.Enqueue(newTile);
            }

            return long.MaxValue;
        }

        public object GetResult1()
        {
            return BFSToGoal(start);
        }

        public object GetResult2()
        {
            List<long> results = new();
            foreach(var tile in tileGrid.Where(t => t.Height == 0))
            {
                results.Add(BFSToGoal(tile));
            }
            // 396 too high
            return results.Min();
        }
    }
}
