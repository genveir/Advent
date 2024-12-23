namespace Advent2024.Shared;
public static class PairWiseExtension
{
    public static IEnumerable<(T, T)> PairWise<T>(this IEnumerable<T> input)
    {
        T previous = default;
        bool hasPrevious = false;
        foreach (var item in input)
        {
            if (hasPrevious)
            {
                yield return (previous, item);
            }
            previous = item;
            hasPrevious = true;
        }
    }
}
