namespace Advent2025.Shared;
public class Range
{
    public long LowerBound { get; set; }
    public long UpperBound { get; set; }

    [ComplexParserTarget("lower-upper")]
    public Range(long lowerBound, long upperBound)
    {
        LowerBound = lowerBound;
        UpperBound = upperBound;
    }

    public bool Contains(long value)
    {
        return value >= LowerBound && value <= UpperBound;
    }

    public bool Overlaps(Range other) =>
        (other.LowerBound >= LowerBound && other.LowerBound <= UpperBound) ||
        (other.UpperBound >= LowerBound && other.UpperBound <= UpperBound) ||
        (LowerBound >= other.LowerBound && LowerBound <= other.UpperBound) ||
        (UpperBound >= other.LowerBound && UpperBound <= other.UpperBound);

    public bool TryCombine(Range other, out Range combined)
    {
        if (other.Overlaps(this))
        {
            var lowerBound = Math.Min(LowerBound, other.LowerBound);
            var upperBound = Math.Max(UpperBound, other.UpperBound);
            combined = new Range(lowerBound, upperBound);
            return true;
        }
        combined = null;
        return false;
    }

    public long Size() => UpperBound - LowerBound + 1;

    public override string ToString() => $"{LowerBound}-{UpperBound}";
}
