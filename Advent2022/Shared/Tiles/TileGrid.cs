using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tiles
{
    public class TileGrid<TInput, TTileType> : IEnumerable<TTileType>
        where TTileType : BaseTile<TTileType>
    {
        public List<TTileType> AllTiles = new();
        public Dictionary<Coordinate, TTileType> TileMap = new();

        public enum LinkMode { Orthogonal, Diagonal }

        public TileGrid(TInput[][] grid, Func<TInput, TTileType> constructTile, LinkMode linkMode = LinkMode.Orthogonal)
        {
            AllTiles = new();
            TileMap = new();
            
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    TInput input = grid[y][x];

                    var tile = constructTile(input);
                    var coordinate = new Coordinate(x, y);

                    AllTiles.Add(tile);
                    TileMap.Add(coordinate, tile);

                    if (x > 0) tile.Link(TileMap[coordinate.ShiftX(-1)], true);
                    if (y > 0) tile.Link(TileMap[coordinate.ShiftY(-1)], true);
                    if (x > 0 && y > 0 && linkMode == LinkMode.Diagonal) tile.Link(TileMap[coordinate.Shift(-1, -1)], true);
                    if (x < grid[y].Length - 1 && y > 0 && linkMode == LinkMode.Diagonal) tile.Link(TileMap[coordinate.Shift(1, -1)], true);
                }
            }
        }

        public void AddTile(TTileType tile) 
        { 
            AllTiles.Add(tile);
        }

        public IEnumerator<TTileType> GetEnumerator()
        {
            return ((IEnumerable<TTileType>)AllTiles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)AllTiles).GetEnumerator();
        }
    }
}
