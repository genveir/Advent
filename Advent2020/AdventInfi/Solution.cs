using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.AdventInfi
{
    public class Solution : ISolution
    {
        private Octagon GetRequiredOctagon(long numPeople)
        {
            int baseSize = 1;
            while (true)
            {
                var octagon = new Octagon(baseSize);

                var result = octagon.Surface();

                if (result >= numPeople) return octagon;

                baseSize++;
            }
        }

        public object GetResult1()
        {
            return GetRequiredOctagon(17_473_259).Size; ;
        }

        public object GetResult2()
        {
            var requiredOctagons = new long[]
            {
                4_541_398_757,
                1_340_956_891,
                747_700_240,
                430_853_830,
                369_033_904,
                42_707_129
            }.Select(np => GetRequiredOctagon(np));

            return requiredOctagons.Select(ro => ro.Circumference()).Sum();
        }
    }
}
