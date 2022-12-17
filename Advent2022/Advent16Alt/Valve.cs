using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent16Alt
{
    public class Valve
    {
        public string Name;
        public long FlowRate;
        public string[] TargetNames;

        public Valve[] Targets;
        public Dictionary<Valve, long> Distances;

        [ComplexParserConstructor]
        public Valve(string name, long flowRate, string[] targets)
        {
            Name = name;
            FlowRate = flowRate;
            TargetNames = targets.Select(tn => tn.Trim()).ToArray();
        }

        public void SetTargets(Dictionary<string, Valve> targets)
        {
            Targets = TargetNames.Select(tn => targets[tn]).ToArray();
        }

        public void SetDistances()
        {
            Distances = new();

            Queue<(Valve, long)> queue = new();
            queue.Enqueue((this, 0));

            HashSet<Valve> seen = new();
            while (queue.Count > 0)
            {
                var (valve, distance) = queue.Dequeue();

                if (seen.Contains(valve)) continue;
                seen.Add(valve);

                if (valve.FlowRate > 0)
                    Distances.Add(valve, distance);

                foreach (var target in valve.Targets)
                {
                    queue.Enqueue((target, distance + 1));
                }
            }
        }

        public override string ToString()
        {
            return $"Valve {Name}";
        }
    }
}
