namespace Advent2023.Shared.Tiles;

public class DecoratedTile<TValue> : BaseTile<DecoratedTile<TValue>>
{
    public TValue Value { get; set; }

    public DecoratedTile(TValue value)
    {
        Value = value;
    }
}
