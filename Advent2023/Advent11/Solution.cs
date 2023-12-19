using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent11;

public class Solution : ISolution
{
    public HashSet<Coordinate> Galaxies { get; set; } = new();

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        for (int y = 0; y < lines.Length; y++)
            for (int x = 0; x < lines[y].Length; x++)
                if (lines[y][x] == '#') Galaxies.Add(new(x, y));
    }
    public Solution() : this("Input.txt") { }

    
    public HashSet<Coordinate> ExpandedUniverse(HashSet<Coordinate> universe, long shiftSize)
    {
        var expandedOverX = Expand(universe, c => c.X, (c, s) => c.ShiftX(s), shiftSize);
        return Expand(expandedOverX, c => c.Y, (c, s) => c.ShiftY(s), shiftSize);
    }

    public HashSet<Coordinate> Expand(HashSet<Coordinate> universe, Func<Coordinate, long> getKey, Func<Coordinate, long, Coordinate> shiftCoord, long shiftSize)
    {
        var byKey = new Dictionary<long, List<Coordinate>>();
        foreach(var coord in universe)
        {
            var key = getKey(coord);
            if (byKey.ContainsKey(key))
                byKey[key].Add(coord);
            else
                byKey[key] = new() { coord };
        }

        var max = byKey.Keys.Max();
        long shift = 0;
        var expanded = new HashSet<Coordinate>();

        for (long n = 0; n <= max; n++)
        {
            if (byKey.ContainsKey(n))
            {
                foreach (var coord in byKey[n])
                    expanded.Add(shiftCoord(coord, shift));
            }
            else
            {
                shift += shiftSize;
            }
        }
        return expanded;
    }

    public object GetResult1()
    {
        var expanded = ExpandedUniverse(Galaxies, 1).ToList();

        long sum = 0;
        for (int n = 0; n < expanded.Count; n++)
            for (int i = n + 1; i < expanded.Count; i++)
                sum += expanded[n].ManhattanDistance(expanded[i]);

        return sum;
    }

    public object GetResult2()
    {
        var expanded = ExpandedUniverse(Galaxies, 1000000 - 1).ToList();

        long sum = 0;
        for (int n = 0; n < expanded.Count; n++)
            for (int i = n + 1; i < expanded.Count; i++)
                sum += expanded[n].ManhattanDistance(expanded[i]);

        return sum;
    }
}
