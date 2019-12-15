using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent22
{
    public class Tile
    {
        public (int x, int y) coord;
        private TileCollection tiles;

        public Tile(TileCollection tiles, (int x, int y) coord, int multiplyOfOthers, int depth, (int x, int y) target)
        {
            this.tiles = tiles;
            this.coord = coord;
            this.erosionLevel = (multiplyOfOthers + depth) % 20183;
            if (multiplyOfOthers == -1) this.erosionLevel = -1;
            HeuristicDistance = Math.Abs(coord.x - target.x) + Math.Abs(coord.y - target.y);
        }

        public int erosionLevel;
        public int Type
        {
            get
            {
                return erosionLevel % 3;
            }
        }

        public List<Tile> Neighbours
        {
            get
            {
                var neighbours = new List<Tile>();
                neighbours.Add(tiles.GetTile(coord.x - 1, coord.y));
                neighbours.Add(tiles.GetTile(coord.x, coord.y - 1));
                neighbours.Add(tiles.GetTile(coord.x + 1, coord.y));
                neighbours.Add(tiles.GetTile(coord.x, coord.y + 1));

                return neighbours;
            }
        }

        public int HeuristicDistance { get; set; }

        public override int GetHashCode()
        {
            return 1000 * coord.y + coord.x;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Tile;
            return coord.x == other.coord.x && coord.y == other.coord.y;
        }

        public override string ToString()
        {
            string typeString;
            switch (Type)
            {
                case 0: typeString = "Rocky"; break;
                case 1: typeString = "Wet"; break;
                case 2: typeString = "Narrow"; break;
                default: typeString = "Impassable"; break;
            }
            return string.Format("({0}, {1}): {2}", coord.x, coord.y, typeString);
        }
    }
}
