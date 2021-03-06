﻿using System.Linq;

namespace Advent2020.Advent6
{
    public class AltSolution : Shared.ISolution
    {
        public object GetResult1() => Shared.Input.GetInput("input").Split("\r\n\r\n").Select(group => group.Replace("\r\n", "").Distinct().Count()).Sum();
        public object GetResult2() => Shared.Input.GetInput("input").Split("\r\n\r\n").Select(group => group.Split("\r\n").Aggregate((p1, p2) => new string(p1.Intersect(p2).ToArray())).Length).Sum();
    }
}
