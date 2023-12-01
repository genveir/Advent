namespace Advent2023.Shared.Tiles
{
    public class CoordinateTile<TValue> : BaseTile<CoordinateTile<TValue>>
    {
        public Coordinate Coordinate { get; set; }
        public TValue Value { get; set; }

        public CoordinateTile(Coordinate coordinate, TValue value)
        {
            Coordinate = coordinate;
            Value = value;
        }
    }
}
