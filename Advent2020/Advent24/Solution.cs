using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent24
{
    public class Solution : ISolution
    {
        Route[] Routes;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            Routes = lines.Select(line =>
            {
                var Path = new List<Route.Direction>();

                for (int n = 0; n < line.Length; n++)
                {
                    switch (line[n])
                    {
                        case 'e': Path.Add(Route.Direction.East); break;
                        case 'w': Path.Add(Route.Direction.West); break;
                        case 'n':
                            n++;
                            if (line[n] == 'e') Path.Add(Route.Direction.NorthEast);
                            else Path.Add(Route.Direction.NorthWest);
                            break;
                        case 's':
                            n++;
                            if (line[n] == 'e') Path.Add(Route.Direction.SouthEast);
                            else Path.Add(Route.Direction.SouthWest);
                            break;
                    }
                }
                return new Route(Path);
            }).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public class Route
        {
            public enum Direction { East, SouthEast, SouthWest, West, NorthWest, NorthEast };

            public List<Direction> Path;

            public Route(List<Direction> path)
            {
                this.Path = path;
            }

            public Tile Walk(Tile startingTile)
            {
                Tile currentTile = startingTile;
                for (int n = 0; n < Path.Count;n++)
                {
                    switch(Path[n])
                    {
                        case Direction.East: currentTile = currentTile.East(); break;
                        case Direction.SouthEast: currentTile = currentTile.SouthEast(); break;
                        case Direction.SouthWest: currentTile = currentTile.SouthWest(); break;
                        case Direction.West: currentTile = currentTile.West(); break;
                        case Direction.NorthWest: currentTile = currentTile.NorthWest(); break;
                        case Direction.NorthEast: currentTile = currentTile.NorthEast(); break;
                    }
                }
                return currentTile;
            }
        }

        public class TileFloor
        {
            public HashSet<Tile> BlackTiles;
            public Dictionary<Coordinate, Tile> AllTiles;
            public Tile CenterTile;

            public TileFloor()
            {
                this.BlackTiles = new HashSet<Tile>();
                this.AllTiles = new Dictionary<Coordinate, Tile>();
                this.CenterTile = new Tile(this, new Coordinate(0, 0, 0));
            }

            public void AddTile(Tile tile)
            {
                this.AllTiles.Add(tile.Location, tile);
            }

            public Tile GetTile(ref Tile cache, Coordinate coordinate)
            {
                if (cache == null)
                {
                    if (!AllTiles.TryGetValue(coordinate, out cache))
                    {
                        cache = new Tile(this, coordinate);
                    }
                }
                return cache;
            }
        }

        public class Tile
        {
            public TileFloor Floor { get; }
            public Coordinate Location { get; }
            public bool IsBlack { get; set; }

            public void Flip()
            {
                IsBlack = !IsBlack;

                if (IsBlack) Floor.BlackTiles.Add(this);
                else Floor.BlackTiles.Remove(this);
            }

            public Tile(TileFloor floor, Coordinate location)
            {
                this.Location = location;
                this.Floor = floor;

                this.Floor.AddTile(this);
            }

            private Tile _east, _west, _northEast, _southEast, _northWest, _southWest;

            public Tile East() => Floor.GetTile(ref _east, Location.Shift(0, -1, 1)); 
            public Tile West() => Floor.GetTile(ref _west, Location.Shift(0, 1, -1)); 
            public Tile NorthEast() => Floor.GetTile(ref _northEast, Location.Shift(1, 0, 1));
            public Tile SouthEast() => Floor.GetTile(ref _southEast, Location.Shift(-1, -1, 0));
            public Tile NorthWest() => Floor.GetTile(ref _northWest, Location.Shift(1, 1, 0));
            public Tile SouthWest() => Floor.GetTile(ref _southWest, Location.Shift(-1, 0, -1));

            public Tile[] Neighbours => new Tile[]
            {
                East(), SouthEast(), SouthWest(), West(), NorthWest(), NorthEast()
            };

            public void DetermineFlipsForSelfAndNeighbours(int turn, HashSet<Tile> flipTiles)
            {
                DetermineFlip(turn, flipTiles);
                foreach (var neighbour in Neighbours) neighbour.DetermineFlip(turn, flipTiles);
            }

            int checkedFlip = -1;
            public void DetermineFlip(int turn, HashSet<Tile> flipTiles)
            {
                if (turn == checkedFlip) return;
                checkedFlip = turn;

                bool shouldFlip;
                var blackNeighbours = Neighbours.Where(nb => nb.IsBlack).Count();
                if (IsBlack)
                {
                    shouldFlip = blackNeighbours == 0 || blackNeighbours > 2;
                }
                else
                {
                    shouldFlip = blackNeighbours == 2;
                }

                if (shouldFlip) flipTiles.Add(this);
            }

            public override string ToString()
            {
                return $"Tile {Location} {(IsBlack ? "black" : "white")}";
            }
        }

        public object GetResult1()
        {
            var floor = new TileFloor();

            for (int n = 0; n < Routes.Count(); n++)
            {
                var tile = Routes[n].Walk(floor.CenterTile);
                tile.Flip();
            }

            return floor.BlackTiles.Count;
        }

        public object GetResult2()
        {
            var floor = new TileFloor();

            for (int n = 0; n < Routes.Count(); n++)
            {
                var tile = Routes[n].Walk(floor.CenterTile);
                tile.Flip();
            }

            for (int n = 0; n < 100; n++)
            {
                var flipTiles = new HashSet<Tile>();
                foreach (var tile in floor.BlackTiles) tile.DetermineFlipsForSelfAndNeighbours(n, flipTiles);
                foreach (var tile in flipTiles) tile.Flip();
            }

            return floor.BlackTiles.Count;
        }
    }
}
