using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent19
{
    public class Solution : ISolution
    {
        public List<Scanner> scanners = new List<Scanner>();

        public Solution(string input, int requiredMatches = 12)
        {
            var lines = Input.GetBlockLines(input).ToArray();

            foreach (var block in lines)
            {
                var split = block[0].Split(' ');
                var num = long.Parse(split[2]);
                Coordinate[] coords = new Coordinate[block.Length - 1];
                for (int n = 1; n < block.Length; n++)
                {
                    var nums = block[n].Split(',').Select(long.Parse).ToArray();
                    coords[n - 1] = new Coordinate(nums);
                }

                scanners.Add(new Scanner(num, coords, requiredMatches));
            }

            foreach (var scanner in scanners) scanner.FindMatchingScanners(scanners);

            scanners[0].HasBeenNormalized = true;

            NormalizeAllScanners();
        }
        public Solution() : this("Input.txt") { }

        public void NormalizeAllScanners()
        {
            var toNormalizeAgainst = new Queue<Scanner>();
            toNormalizeAgainst.Enqueue(scanners[0]);

            while(toNormalizeAgainst.Count > 0)
            {
                var scanner = toNormalizeAgainst.Dequeue();

                var matches = scanner.scannerMatches;

                var unnormalizedScanners = matches
                    .Select(m => m.otherScanner)
                    .Where(s => !s.HasBeenNormalized);

                foreach(var unnormalized in unnormalizedScanners)
                {
                    unnormalized.NormalizeAgainst(scanner);
                    toNormalizeAgainst.Enqueue(unnormalized);
                }
            }
        }

        public List<Coordinate> GetAllBeacons()
        {
            return scanners
                .Where(s => s.HasBeenNormalized) // this means things are broken.
                .SelectMany(s => s.normalizedCoordinates)
                .Distinct()
                .OrderBy(c => c.X)
                .ThenBy(c => c.Y)
                .ThenBy(c => c.Z)
                .ToList();
        }

        public object GetResult1()
        {
            return GetAllBeacons().Count();
        }

        public object GetResult2()
        {
            long highest = 0;
            for (int n = 0; n < scanners.Count; n++)
            {
                for (int i = 0; i < scanners.Count; i++)
                {
                    var diffX = Math.Abs(scanners[n].OwnPosition().X - scanners[i].OwnPosition().X);
                    var diffY = Math.Abs(scanners[n].OwnPosition().Y - scanners[i].OwnPosition().Y);
                    var diffZ = Math.Abs(scanners[n].OwnPosition().Z.Value - scanners[i].OwnPosition().Z.Value);

                    var distance = diffX + diffY + diffZ;

                    if (distance > highest) highest = distance; //10870 too high
                }
            }

            return highest;
        }
    }
}
