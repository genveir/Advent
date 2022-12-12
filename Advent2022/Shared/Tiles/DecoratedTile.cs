using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tiles
{
    public class DecoratedTile<TValue> : BaseTile<DecoratedTile<TValue>>
    {
        public TValue Value { get; set; }

        public DecoratedTile(TValue value)
        {
            Value = value;
        }
    }
}
