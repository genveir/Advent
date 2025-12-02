namespace Advent2025.Day02;

public class Solution
{
    public List<IdRange> idRanges;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input, splitOn: ['\n', '\r', ',']);

        var inputParser = new InputParser<IdRange>("line");

        idRanges = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class IdRange
    {
        public string First { get; }
        public string Last { get; }

        [ComplexParserTarget("first-last")]
        public IdRange(string first, string last)
        {
            First = first;
            Last = last;
        }

        public IEnumerable<long> FindInvalidIds()
        {
            long first = long.Parse(First);
            long last = long.Parse(Last);

            for (long id = first; id <= last; id++)
            {
                var asString = id.ToString();

                if (asString.Length % 2 != 0)
                    continue;

                if (IsSuperInvalid(asString, asString.Length / 2))
                {
                    yield return id;
                }
            }
        }

        public IEnumerable<long> FindSuperInvalidIds()
        {
            long first = long.Parse(First);
            long last = long.Parse(Last);

            for (long id = first; id <= last; id++)
            {
                var asString = id.ToString();
                for (int n = 1; n <= asString.Length / 2; n++)
                {
                    if (IsSuperInvalid(asString, n))
                    {
                        yield return id;
                        break;
                    }
                }
            }
        }

        private bool IsSuperInvalid(string idString, int subStringLength)
        {
            if (idString.Length % subStringLength != 0)
                return false;

            var subString = idString.Substring(0, subStringLength);
            for (int pos = subStringLength; pos < idString.Length; pos += subStringLength)
            {
                if (idString.Substring(pos, subStringLength) != subString)
                    return false;
            }

            return true;
        }

        public override string ToString() => $"{First}-{Last}";
    }

    // total space is only 2302978 items
    public object GetResult1()
    {
        List<long> invalidIds = [];

        foreach (var idRange in idRanges)
        {
            invalidIds.AddRange(idRange.FindInvalidIds());
        }

        return invalidIds.Sum();
    }

    public object GetResult2()
    {
        List<long> invalidIds = [];

        foreach (var idRange in idRanges)
        {
            invalidIds.AddRange(idRange.FindSuperInvalidIds());
        }

        return invalidIds.Sum();
    }
}
