using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent3
{
    class Solution : ISolution
    {
        private List<Patch> GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent2018.Advent3.Input.txt");

            var vals = new List<Patch>();
            using (var txt = new StreamReader(input))
            {
                while (!txt.EndOfStream)
                    vals.Add(Patch.Parse(txt.ReadLine()));
            }

            return vals;
        }

        private class Patch
        {
            public long Id { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
            public long XSize { get; set; }
            public long YSize { get; set; }

            public static Patch Parse(string input)
            {
                input = input.Replace("#", "");
                var split = input.Split(new char[] { '@', ',', ':', 'x' }).Select(s => long.Parse(s)).ToArray();

                var patch = new Patch()
                {
                    Id = split[0],
                    X = split[1],
                    Y = split[2],
                    XSize = split[3],
                    YSize = split[4]
                };

                return patch;
            }
        }

        public long GetOverlap()
        {
            var patches = GetInput();

            var overlap = 0L;
            var occupied = new Dictionary<long, long>();
            var noOverlap = new List<Patch>();
            foreach (var patch in patches)
            {
                bool hasAnyOverlap = false;

                for (int xOffset = 0; xOffset < patch.XSize; xOffset++)
                {
                    for (int yOffset = 0; yOffset < patch.YSize; yOffset++)
                    {
                        long coord = 1000 * (patch.X + xOffset) + (patch.Y + yOffset);

                        if (occupied.ContainsKey(coord))
                        {
                            hasAnyOverlap = true;
                            occupied[coord] = occupied[coord] + 1;
                            if (occupied[coord] == 2) overlap++;
                        }
                        else
                        {
                            occupied.Add(coord, 1);
                        }
                    }
                }

                if (!hasAnyOverlap) noOverlap.Add(patch);
            }

            foreach (var patch in noOverlap)
            {
                bool stillNoOverlap = true;
                for (int xOffset = 0; xOffset < patch.XSize; xOffset++)
                {
                    for (int yOffset = 0; yOffset < patch.YSize; yOffset++)
                    {
                        long coord = 1000 * (patch.X + xOffset) + (patch.Y + yOffset);

                        if (occupied[coord] > 1) stillNoOverlap = false;
                    }
                }
                if (stillNoOverlap == true) Console.WriteLine("no overlap: " + patch.Id);
            }

            return overlap;
        }

        public void WriteResult()
        {
            Console.WriteLine("part2: " + GetOverlap());
        }
    }
}
