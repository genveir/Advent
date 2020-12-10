using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent10
{
    public class Solution : ISolution
    {
        List<Adapter> adapters;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            adapters = lines.Select(line => new Adapter(int.Parse(line))).ToList();

            foreach (var ad in adapters) ad.SetPossibleConnections(adapters);
        }
        public Solution() : this("Input.txt") { }

        public class Adapter
        {
            public int ratedJoltage;

            public Adapter(int ratedJoltage)
            {
                this.ratedJoltage = ratedJoltage;
            }

            private List<Adapter> PossibleConnections = new List<Adapter>();

            public void SetPossibleConnections(List<Adapter> adapters)
            {
                foreach(var adapter in adapters)
                {
                    if (adapter.ratedJoltage > this.ratedJoltage)
                    {
                        if (adapter.ratedJoltage - this.ratedJoltage <= 3) PossibleConnections.Add(adapter);
                    }
                }
            }

            public long _counted = -1;
            public long CountLeaves()
            {
                if (PossibleConnections.Count == 0) return 1;

                if (_counted == -1)
                {
                    _counted = PossibleConnections.Select(pc => pc.CountLeaves()).Sum();
                }
                return _counted;
            }

            public override string ToString()
            {
                return "Adapter: " + ratedJoltage;
            }
        }

        public object GetResult1()
        {
            var sorted = adapters.OrderBy(ad => ad.ratedJoltage).ToList();
            var lastJoltage = 0;
            var ones = 0;
            var threes = 1;
            for (int n = 0; n < sorted.Count; n++)
            {
                var diff = sorted[n].ratedJoltage - lastJoltage;
                if (diff == 1) ones++;
                else if (diff == 3) threes++;
                else throw new InvalidOperationException();

                lastJoltage = sorted[n].ratedJoltage;
            }

            return ones * threes;
        }

        public object GetResult2()
        {
            var availableFromStart = adapters.Where(ad => ad.ratedJoltage <= 3);

            return availableFromStart.Select(ad => ad.CountLeaves()).Sum();
        }
    }
}
