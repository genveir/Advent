using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent06;

public class Solution : ISolution
{
    public Race longRace;
    public List<Race> races;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var times = lines[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse)
            .ToArray();
        var distances = lines[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse)
            .ToArray();

        races = new();
        for (int n = 0; n < times.Length; n++)
        {
            races.Add(new(times[n], distances[n]));
        }

        var longTime = long.Parse(lines[0].Split(' ', 2).Skip(1).Single().Replace(" ", ""));
        var longDistance = long.Parse(lines[1].Split(' ', 2).Skip(1).Single().Replace(" ", ""));

        longRace = new Race(longTime, longDistance);
    }
    public Solution() : this("Input.txt") { }

    public class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }

        public Race(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }

        public long DistanceAtPushTime(long pushTime) => pushTime * (Time - pushTime);

        public bool WinsAtPushTime(long pushTime) => DistanceAtPushTime(pushTime) > Distance;

        public long FindFirstThatWins() => FindFirstThatWins(Time / 4, Time / 4);

        public long FindFirstThatWins(long pushTime, long stepSize)
        {
            if (stepSize == 0)
            {
                if (WinsAtPushTime(pushTime))
                {
                    return WinsAtPushTime(pushTime - 1) ? (pushTime - 1) : pushTime;
                }
                else return pushTime + 1;
            }

            if (WinsAtPushTime(pushTime)) return FindFirstThatWins(pushTime - stepSize, stepSize / 2);
            return FindFirstThatWins(pushTime + stepSize, stepSize / 2);
        }

        public long NumThatWin => (Time + 1) - 2 * FindFirstThatWins();
    }

    public object GetResult1()
    {
        var firstThatWins = races.Select(r => r.FindFirstThatWins()).ToArray();
        var numsThatWin = races.Select(r => r.NumThatWin).ToList();

        return races.Select(r => r.NumThatWin).Aggregate((a, b) => a * b);
    }

    public object GetResult2()
    {
        return longRace.NumThatWin;
    }
}
