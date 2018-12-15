using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Advent.Advent15
{
    class TileFactory
    {
        public Dictionary<XYCoord, Tile> AllPositions = new Dictionary<XYCoord, Tile>();

        public Tile Parse(XYCoord coord, char input)
        {
            Tile tileNorth;
            Tile tileWest;
            AllPositions.TryGetValue(new XYCoord(coord.X, coord.Y - 1), out tileNorth);
            AllPositions.TryGetValue(new XYCoord(coord.X - 1, coord.Y), out tileWest);

            Tile newTile = null;
            switch (input)
            {
                case 'G': newTile = new Goblin(coord, tileNorth, tileWest); break;
                case 'E': newTile = new Elf(coord, tileNorth, tileWest); break;
                case '.': newTile = new Tile(coord, tileNorth, tileWest, TileType.Floor); break;
                case '#': newTile = new Tile(coord, tileNorth, tileWest, TileType.Wall); break;
                case 'e': newTile = new Elf(coord, tileNorth, tileWest) { IsStaticTestGuy = true }; break;
                default: Debug.WriteLine("unknown char at " + coord); break;
            }

            AllPositions.Add(coord, newTile);

            return newTile;
        }
    }
}
