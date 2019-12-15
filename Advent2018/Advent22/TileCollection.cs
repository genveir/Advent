using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent22
{
    public class TileCollection
    {
        private ConcurrentDictionary<(int x, int y), Tile> Tiles;

        private int depth;
        private (int x, int y) target;

        public TileCollection(int depth, (int x, int y) target)
        {
            this.depth = depth;
            this.target = target;

            Tiles = new ConcurrentDictionary<(int x, int y), Tile>();
        }

        public Tile GetTile(int x, int y)
        {
            if (!Tiles.ContainsKey((x, y)))
            {
                Tile tile;
                if (x < 0 || y < 0) tile = new Tile(this, (x, y), -1, depth, target);
                else if (x == target.x && y == target.y) tile = new Tile(this, (x, y), 0, depth, target);
                else if (y == 0) tile = new Tile(this, (x, y), x * 16807, depth, target);
                else if (x == 0) tile = new Tile(this, (x, y), y * 48271, depth, target);
                else
                {
                    var X = GetTile(x - 1, y).erosionLevel;
                    var Y = GetTile(x, y - 1).erosionLevel;
                    tile = new Tile(this, (x, y), (X * Y), depth, target);
                }

                Tiles.GetOrAdd((x, y), tile);
            }

            return Tiles[(x, y)];
        }
    }
}
