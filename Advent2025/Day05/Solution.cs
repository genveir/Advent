namespace Advent2025.Day05;

public class Solution
{
    public List<Shared.Range> ranges;
    public List<long> ingredients;

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        var parser = new InputParser<Shared.Range>("range");
        ranges = blocks[0].Select(parser.Parse).ToList();

        ingredients = blocks[1].Select(long.Parse).ToList();
    }
    public Solution() : this("Input.txt") { }

    public object GetResult1()
    {
        int count = 0;

        for (int n = 0; n < ingredients.Count; n++)
        {
            if (ranges.Any(r => r.Contains(ingredients[n])))
            {
                count++;
            }
        }

        return count;
    }

    public class DecoratedRange
    {
        public bool DoneProcessing { get; set; } = false;

        public Shared.Range Range { get; set; }

        public DecoratedRange(Shared.Range range)
        {
            Range = range;
        }

        public override string ToString() => (DoneProcessing ? "+" : "-") + Range.ToString();
    }

    public object GetResult2()
    {
        List<DecoratedRange> _ranges = new List<DecoratedRange>();

        foreach (var range in ranges)
        {
            _ranges.Add(new DecoratedRange(range));
        }

        while (_ranges.Any(r => !r.DoneProcessing))
        {
            var toProcess = _ranges.First(r => !r.DoneProcessing);

            _ranges.Remove(toProcess);

            var unProcessed = _ranges.Where(r => !r.DoneProcessing).ToList();

            bool didCombine = false;
            foreach (var other in unProcessed)
            {
                if (toProcess.Range.TryCombine(other.Range, out var combined))
                {
                    didCombine = true;
                    _ranges.Remove(other);
                    toProcess.Range = combined;
                }
            }

            toProcess.DoneProcessing = !didCombine;
            _ranges.Add(toProcess);
        }

        return _ranges.Select(r => r.Range).Sum(r => r.Size());
    }
}
