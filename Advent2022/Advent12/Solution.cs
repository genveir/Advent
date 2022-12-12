using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using Advent2022.Shared.Search;
using Advent2022.Shared.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var constructor = (char c, Coordinate coord) => c switch
            {
                'S' => new Tile(coord, 0, isStart: true),
                'E' => new Tile(coord, 25, isEnd: true),
                _ => new Tile(coord, c - 'a')
            };

            tileGrid = new(grid, constructor);
            start = tileGrid.Single(t => t.IsStart);
        }
        public Solution() : this("Input.txt") { }

        public class Tile : BaseTile<Tile>, IEquatable<Tile>
        {
            public Coordinate Coordinate;
            public long Height;
            public bool IsStart;
            public bool IsEnd;

            public long Exploration = 0;
            public long ExploreLength = 0;

            [ComplexParserConstructor]
            public Tile(Coordinate coordinate, long height, bool isStart = false, bool isEnd = false)
            {
                Coordinate = coordinate;
                Height = height;
                IsStart = isStart;
                IsEnd = isEnd;
            }

            public IEnumerable<Tile> Reachable() =>
                Neighbours.Where(n => n.Height - 1 <= this.Height);

            public IEnumerable<Tile> Explore(long exploration)
            {
                var adjacent = Reachable()
                    .Where(n => n.Exploration != this.Exploration)
                    .ToList();

                foreach (var adjacentTile in adjacent) 
                    adjacentTile.BeExplored(this);

                return adjacent;
            }

            public void BeExplored(Tile explorer)
            {
                Exploration = explorer.Exploration;
                ExploreLength = explorer.ExploreLength + 1;
            }

            public bool Equals(Tile other)
            {
                return other.Coordinate.Equals(Coordinate);
            }

            public override string ToString()
            {
                return $"Tile {Coordinate} Height {Height}";
            }
        }

        public long DijkstraToGoal(IEnumerable<Tile> startNodes)
        {
            var target = tileGrid.Single(t => t.IsEnd);

            var dijkstra = new Dijkstra<Tile>(
                startNodes: startNodes, 
                endNodes: new[] { target },
                transitionCostFunction: (_, _) => 1, 
                heuristicCostFunction: tile => tile.Coordinate.ManhattanDistance(target.Coordinate),
                findNeighbourFunction: tile => tile.Reachable());

            return dijkstra.FindShortest().Cost;
        }


        private long BFSNum = 0;
        public long BFSToGoal(Tile bfsStart)
        {
            BFSNum++;
            bfsStart.ExploreLength = 0;
            bfsStart.Exploration = BFSNum;

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
            return DijkstraToGoal(tileGrid.Where(t => t.Height == 0));
        }
    }
}
