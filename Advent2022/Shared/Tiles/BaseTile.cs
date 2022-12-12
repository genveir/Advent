using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tiles
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
