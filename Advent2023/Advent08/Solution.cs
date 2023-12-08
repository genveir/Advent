using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Advent2023.Shared;

namespace Advent2023.Advent08;

public class Solution : ISolution
{
    public CircularList<char> Instructions;
    public List<Location> locations;
    Dictionary<string, Location> allLocations = new();

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        Instructions = new(lines[0]);

        var inputParser = new InputParser<Location>("line");
        locations = inputParser.Parse(lines.Skip(2));

        foreach (var location in locations)
            allLocations.Add(location.Name, location);

        foreach (var location in locations)
            location.LinkUp(allLocations);
    }
    public Solution() : this("Input.txt") { }

    public class Location
    {
        public string Name { get; set; }
        public string LeftName { get; set; }
        public string RightName { get; set; }

        public Location Left { get; set; }
        public Location Right { get; set; }

        public bool EndsOnZ { get; set; }
        public long CycleStart { get; set; }
        public long CycleLength { get; set; }

        [ComplexParserConstructor("loc = (left, right)")]
        public Location(string name, string leftName, string rightName)
        {
            Name = name;
            LeftName = leftName;
            RightName = rightName;

            EndsOnZ = name.EndsWith('Z');
        }

        public void LinkUp(Dictionary<string, Location> locations)
        {
            Left = locations[LeftName];
            Right = locations[RightName];
        }

        public Location Step(char direction) =>
            (direction == 'L') ? Left : Right;

        public override string ToString()
        {
            return $"{Name} => {LeftName}, {RightName}";
        }
    }

    public object GetResult1()
    {
        Location current = allLocations["AAA"];

        int pathLength;
        for (pathLength = 0; current.Name != "ZZZ"; pathLength++)
        {
            var step = Instructions[pathLength];
            current = current.Step(step);
        }
        return pathLength;
    }

    public object GetResult2()
    {
        List<Location> starts = locations.Where(l => l.Name.EndsWith('A')).ToList();

        // run each path until it cycles
        // find lengths in cycle between Zs
        foreach (var start in starts)
        {
            Location current = start;

            Location end = null;
            int firstHit = 0;
            int secondHit = 0;
            for (int n = 0; true; n++)
            {
                if (current.EndsOnZ)
                {
                    if (firstHit == 0)
                    {
                        firstHit = n;
                        end = current;
                    }
                    else
                    {
                        if (end != current)
                            throw new InvalidOperationException("unexpected: more ends for one start");

                        if (firstHit % Instructions.Count == n % Instructions.Count)
                        {
                            secondHit = n;
                        }
                        break;
                    }
                }
                var step = Instructions[n];
                current = current.Step(step);
            }
            start.CycleStart = firstHit;
            start.CycleLength = secondHit - firstHit;
        }

        // don't need CRT, cycle length is identical from A-Z and Z-Z, so LCM will do
        return starts.Select(s => s.CycleLength).Aggregate(Helper.LCM);
    }
}
