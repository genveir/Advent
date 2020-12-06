using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent6
{
    public class AltSolution : ISolution
    {
        public object GetResult1()
        {
            return Input.GetInput("input").Split(Environment.NewLine + Environment.NewLine).Select(group => group.Replace(Environment.NewLine, "").Distinct().Count()).Sum();
        }

        public object GetResult2()
        {
            return Input.GetInput("input").Split(Environment.NewLine + Environment.NewLine).Select(group => group.Split(Environment.NewLine).Aggregate((p1, p2) => new string(p1.Intersect(p2).ToArray())).Length).Sum();
        }
    }
}
