using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Advent23
{
    class Solution : ISolution
    {
        public List<NanoBot> bots;

        public enum InputMode { String, File }

        public Solution() : this("Input", InputMode.File) { }
        public Solution(string input, InputMode mode)
        {
            if (mode == InputMode.File) ParseInput(input);
            else
            {
                _ParseInput(input);
            }
        }

        private void ParseInput(string fileName)
        {
            string resourceName = "Advent.Advent23." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        Voxel NormalizedZero = new Voxel();
        private void _ParseInput(string input)
        {
            bots = new List<NanoBot>();

            var lines = input.Split('\n');
            foreach (var line in lines)
            {
                bots.Add(NanoBot.FromString(line));
            }

            var minX = bots.Min(b => b.X);
            var minY = bots.Min(b => b.Y);
            var minZ = bots.Min(b => b.Z);

            bots = bots
                .Select(b => new NanoBot() { X = b.X - minX, Y = b.Y - minY, Z = b.Z - minZ, SignalStrength = b.SignalStrength })
                .ToList();

            NormalizedZero = new Voxel()
            {
                X = -minX,
                Y = -minY,
                Z = -minZ
            };
        }

        public int GetNumInRangeOfStrongest()
        {
            int numInRange = 0;

            var strongestSignalBot = bots.OrderBy(b => b.SignalStrength).Last();
            foreach( var bot in bots)
            {
                if (bot.IsInRangeOf(strongestSignalBot)) numInRange++;
            }

            return numInRange;
        }



        public long GetPointInRangeOfMost()
        {
            int inflateStart = 1000000000;
            int inflateShift = 10;

            var voxelsAt = new ConcurrentStack<Voxel>[1001];
            InitVoxels(voxelsAt, inflateStart);
                
            for (int inflateFactor = inflateStart / inflateShift; inflateFactor >= 1; inflateFactor /= inflateShift)
            {
                RunRound(voxelsAt, inflateFactor, inflateShift);                
            }

            int result = 0;
            for (int n = voxelsAt.Length - 1; n > 0; n--) if (voxelsAt[n].Count > 0) { result = n; break; };
            var results = voxelsAt[result]
                .Select(v => v.DistanceTo(NormalizedZero))
                .Distinct();

            return results
                .Single();
        }

        public void InitVoxels(ConcurrentStack<Voxel>[] voxelsAt, int inflateStart)
        {
            for (int n = 0; n < voxelsAt.Length; n++) voxelsAt[n] = new ConcurrentStack<Voxel>();

            var testPoint = new Voxel();

            var smallBots = bots.Select(b => b.ReduceByFactor(inflateStart)).ToList();

            var minX = smallBots.Min(b => b.X);
            var maxX = smallBots.Max(b => b.X);
            var minY = smallBots.Min(b => b.Y);
            var maxY = smallBots.Max(b => b.Y);
            var minZ = smallBots.Min(b => b.Z);
            var maxZ = smallBots.Max(b => b.Z);

            Console.WriteLine("largest range is now " + smallBots.Max(b => b.SignalStrength));
            for (long z = minZ; z <= maxZ; z++)
            {
                testPoint.Z = z;
                for (long y = minY; y <= maxY; y++)
                {
                    testPoint.Y = y;
                    for (long x = minX; x <= maxX; x++)
                    {
                        testPoint.X = x;
                        int numInRange = 0;
                        foreach (var bot in smallBots) if (testPoint.IsInRangeOf(bot)) numInRange++;

                        voxelsAt[numInRange].Push(new Voxel()
                        {
                            X = testPoint.X,
                            Y = testPoint.Y,
                            Z = testPoint.Z
                        });
                    }
                }
            }
        }

        public ConcurrentStack<Voxel> GetHighest(ConcurrentStack<Voxel>[] voxelsAt)
        {
            int highest = 0;
            for (int n = voxelsAt.Length - 1; n > 0; n--) if (voxelsAt[n].Count > 0) { highest = n; break; };

            Console.WriteLine("highest was " + highest);

            return voxelsAt[highest];
        }

        public void RunRound(ConcurrentStack<Voxel>[] voxelsAt, int inflateFactor, int inflateShift)
        {
            var voxelsFound = GetHighest(voxelsAt);
            var voxelsToCheck = voxelsFound.SelectMany(vah => vah.Inflate(inflateShift)).ToList();
            for (int n = 0; n < voxelsAt.Length; n++) voxelsAt[n] = new ConcurrentStack<Voxel>();

            var smallBots = bots.Select(b => b.ReduceByFactor(inflateFactor)).ToList();

            Console.WriteLine("checking " + voxelsToCheck.Count() + " voxels");
            Console.WriteLine("inflate factor is " + inflateFactor);
            Console.WriteLine("largest range is now " + smallBots.Max(b => b.SignalStrength));
            Parallel.ForEach(voxelsToCheck, voxelToCheck =>
            {
                var numInRange = CheckVoxel(voxelToCheck, smallBots);

                voxelsAt[numInRange].Push(voxelToCheck);
            });
        }

        public int CheckVoxel(Voxel voxel, List<NanoBot> smallBots)
        {
            int numInRange = 0;
            foreach (var bot in smallBots) if (voxel.IsInRangeOf(bot)) numInRange++;

            return numInRange;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetNumInRangeOfStrongest());
            Console.WriteLine("part2: " + GetPointInRangeOfMost());
        }
    }
}
