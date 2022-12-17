using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.AdventActive
{
    class Tests
    {
        [TestCase(example, 1651)]
        [TestCase(simple11, 53)]
        [TestCase(simple21, 81)]
        [TestCase(d2_12, 77)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 1707)]
        [TestCase(simple11, 44)]
        [TestCase(simple21, 66)]
        [TestCase(d2_12, 64)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string simple11 = @"Valve AA has flow rate=0; tunnels lead to valves BB,CC
Valve BB has flow rate=1; tunnel leads to valve AA
Valve CC has flow rate=1; tunnel leads to valve AA";

        public const string simple21 = @"Valve AA has flow rate=0; tunnels lead to valves BB,CC
Valve BB has flow rate=2; tunnel leads to valve AA
Valve CC has flow rate=1; tunnel leads to valve AA";

        public const string d2_12 = @"Valve AA has flow rate=0; tunnels lead to valves BB,CC
Valve BB has flow rate=0; tunnels lead to valves AA, DD
Valve CC has flow rate=1; tunnel leads to valve AA
Valve DD has flow rate=2; tunnel leads to valve BB";

        public const string example = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";
    }
}
