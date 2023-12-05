using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent05;

public class Solution : ISolution
{
    public long[] Seeds;
    public RangeMap SeedToSoil;
    public RangeMap SoilToFertilizer;
    public RangeMap FertilizerToWater;
    public RangeMap WaterToLight;
    public RangeMap LightToTemperature;
    public RangeMap TemperatureToHumidity;
    public RangeMap HumidityToLocation;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        Seeds = new InputParser<long[]>(true, 1) { ArrayDelimiters = new char[] {' '} }
            .Parse<long[]>(lines[0].Substring(7));

        int lineIndex = 1;
        SeedToSoil = MapNext(lines, ref lineIndex);
        SoilToFertilizer = MapNext(lines, ref lineIndex);
        FertilizerToWater = MapNext(lines, ref lineIndex);
        WaterToLight = MapNext(lines, ref lineIndex);
        LightToTemperature = MapNext(lines, ref lineIndex);
        TemperatureToHumidity = MapNext(lines, ref lineIndex);
        HumidityToLocation = MapNext(lines, ref lineIndex);
    }
    public Solution() : this("Input.txt") { }

    RangeMap MapNext(string[] lines, ref int lineIndex)
    {
        while (!(lines[lineIndex].Length > 0 && lines[lineIndex][0] >= '0' && lines[lineIndex][0] <= '9')) lineIndex++;

        var map = new RangeMap();
        while (lineIndex < lines.Length && lines[lineIndex].Length > 0)
        {
            var values = lines[lineIndex].Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            map.AddRange(values[0], values[1], values[2]);

            lineIndex++;
        }

        map.SortRange();

        return map;
    }

    public class RangeMap
    {
        public List<long[]> Segments { get; set; } = new() { new[] { 10_000_000_000, 0 } };

        public void AddRange(long destinationStart, long sourceStart, long length)
        {
            // destStart 20
            // sourceStart 10
            // length 5
            // result should be a segment 10-14, value 10
            // segments do not overlap, so add a segment 9,0
            // then add a segment 14, 10

            var difference = destinationStart - sourceStart;
            Segments.Add(new[] { sourceStart - 1L, 0 });
            Segments.Add(new[] { sourceStart + length - 1L, difference });
        }

        public void SortRange()
        {
            Segments = Segments.OrderBy(a => a[0]).ToList();

            var previous = Segments[0];
            List<long[]> toRemove = new();
            for (int n = 1; n < Segments.Count; n++)
            {
                if (Segments[n][0] == previous[0])
                {
                    toRemove.Add(previous[1] == 0 ? previous : Segments[n]);
                }
                previous = Segments[n];
            }

            foreach (var segment in toRemove) Segments.Remove(segment);
        }

        public long MapValue(long source)
        {
            for (int n = 0; n < Segments.Count; n++)
            {
                if (source <= Segments[n][0]) return source + Segments[n][1];
            }

            throw new InvalidOperationException("cannot map value");
        }

        public List<long[]> MapSegment(long[] sourceSegment) => MapSegment(sourceSegment, 0);

        public List<long[]> MapSegment(long[] sourceSegment, int segmentIndex)
        {
            List<long[]> result = new();

            long start = 0, end;
            for (; segmentIndex < Segments.Count; segmentIndex++)
            {
                if (sourceSegment[0] <= Segments[segmentIndex][0])
                {
                    start = sourceSegment[0] + Segments[segmentIndex][1];
                    break;
                }
            }

            if (sourceSegment[1] <= Segments[segmentIndex][0])
            {
                end = sourceSegment[1] + Segments[segmentIndex][1];
                result.Add(new[] { start, end });
                return result;
            }
            else
            {
                result.Add(new[] { start, Segments[segmentIndex][0] + Segments[segmentIndex][1] });
                result.AddRange(MapSegment(new[] { Segments[segmentIndex][0] + 1L, sourceSegment[1] }, segmentIndex + 1));
                return result;
            }
        }
    }

    public long MapSeed(long value)
    {
        value = SeedToSoil.MapValue(value);
        value = SoilToFertilizer.MapValue(value);
        value = FertilizerToWater.MapValue(value);
        value = WaterToLight.MapValue(value);
        value = LightToTemperature.MapValue(value);
        value = TemperatureToHumidity.MapValue(value);
        value = HumidityToLocation.MapValue(value);

        return value;
    }

    public long MapSeedRange(long[][] ranges)
    {
        var soilSegments = ranges.SelectMany(s => SeedToSoil.MapSegment(s)).ToArray();
        var fertilizerSegments = soilSegments.SelectMany(s => SoilToFertilizer.MapSegment(s)).ToArray();
        var waterSegments = fertilizerSegments.SelectMany(f => FertilizerToWater.MapSegment(f)).ToArray();
        var lightSegments = waterSegments.SelectMany(w => WaterToLight.MapSegment(w)).ToArray();
        var temperatureSegments = lightSegments.SelectMany(l => LightToTemperature.MapSegment(l)).ToArray();
        var humiditySegments = temperatureSegments.SelectMany(t => TemperatureToHumidity.MapSegment(t)).ToArray();
        var locationSegments = humiditySegments.SelectMany(h => HumidityToLocation.MapSegment(h)).ToArray();

        return locationSegments.Min(l => l[0]);
    }

    public object GetResult1()
    {
        return Seeds.Min(MapSeed);
    }

    public object GetResult2()
    {
        long[][] ranges = new long[Seeds.Length / 2][];
        for (int n = 0; n < Seeds.Length; n += 2)
        {
            ranges[n / 2] = new long[] { Seeds[n], Seeds[n] + Seeds[n + 1] - 1 };
        }

        return MapSeedRange(ranges);
    }
}
