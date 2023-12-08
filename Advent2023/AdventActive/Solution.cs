using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.AdventActive;

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

        [ComplexParserConstructor("loc = (left, right)")]
        public Location(string name, string leftName, string rightName)
        {
            Name = name;
            LeftName = leftName;
            RightName = rightName;
        }

        public void LinkUp(Dictionary<string, Location> locations)
        {
            Left = locations[LeftName];
            Right = locations[RightName];
        }

        public Location Step(char direction) =>
            (direction == 'L') ? Left : Right;
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
        List<Location> current = locations.Where(l => l.Name.EndsWith('A')).ToList();

        // run each path until it cycles
        // find lengths in cycle between Zs
        // CRT over all paths

        return "";
    }
}
