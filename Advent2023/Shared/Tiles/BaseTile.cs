using System.Collections.Generic;

namespace Advent2023.Shared.Tiles
{
    public abstract class BaseTile<TImplementationType> 
        where TImplementationType : BaseTile<TImplementationType>
    {
        public List<TImplementationType> Neighbours = new();

        public void Link(TImplementationType tile, bool linkBack)
        {
            Neighbours.Add(tile);
            if (linkBack) tile.Link((TImplementationType)this, false);
        }
    }
}
