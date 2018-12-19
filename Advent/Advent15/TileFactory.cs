using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Advent.Advent15
{
    class TileFactory
    {
        private Dictionary<XYCoord, Tile> AllPositions = new Dictionary<XYCoord, Tile>();

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
                case 'e': newTile = new Elf(coord, tileNorth, tileWest) { IsStaticTestGuy = true }; break;
            }

            if (newTile != null) AllPositions.Add(coord, newTile);

            return newTile;
        }
    }
}
