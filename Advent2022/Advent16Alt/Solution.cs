using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using Advent2022.Shared.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Advent2022.Advent16Alt
{
    public class Solution : ISolution
    {
        public List<Valve> valves;
        public Dictionary<string, Valve> valveMap;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var singleParser = new InputParser<Valve>(false, 3, "Valve ", " has flow rate=", "; tunnel leads to valve ");
            var inputParser = new InputParser<Valve>(false, 3, "Valve ", " has flow rate=", "; tunnels lead to valves ");

            valves = lines.Select(l =>
            {
                if (l.Contains("valves")) return inputParser.Parse(l);
                return singleParser.Parse(l);
            }).ToList();

            valves.Add(new Valve("SKIP", 0, Array.Empty<string>()));

            valveMap = valves.ToDictionary(m => m.Name, m => m);

            foreach (var valve in valves) valve.SetTargets(valveMap);
            foreach (var valve in valves) valve.SetDistances();
        }
        public Solution() : this("Input.txt") { }

        public long DoPart1(long turns)
        {
            var state = new State(
                position: valveMap["AA"],
                openValves: Array.Empty<Valve>(),
                blockedValves: Array.Empty<Valve>(),
                turnsLeft: 30);

            var dp = new DynamicProgramming(state, false);

            return dp.Execute();
        }

        public long DoPart2()
        {
            var state = new State(
                position: valveMap["AA"],
                openValves: Array.Empty<Valve>(),
                blockedValves: Array.Empty<Valve>(),
                turnsLeft: 26);

            var dp = new DynamicProgramming(state, true);

            return dp.Execute();
        }

        public object GetResult1()
        {
            return DoPart1(30);
        }

        public object GetResult2()
        {
            return DoPart2();
        }
    }
}
