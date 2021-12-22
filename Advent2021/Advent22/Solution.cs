using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent22
{
    public class Solution : ISolution
    {
        public List<Cube> cubes;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            cubes = new List<Cube>();
            for (int n = 0; n < lines.Length; n++)
            {
                var line = lines[n];

                var perCoord = line.Split(new char[] { ' ', 'x', 'y', 'z', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var on = perCoord[0] == "on";

                cubes.Add(new Cube(on, 
                    long.Parse(perCoord[1]),
                    long.Parse(perCoord[2]),
                    long.Parse(perCoord[3]),
                    long.Parse(perCoord[4]),
                    long.Parse(perCoord[5]),
                    long.Parse(perCoord[6])));
            }

            ;
        }
        public Solution() : this("Input.txt") { }

        public class Cube
        {
            public bool on;
            public bool off => !on;

            public long minX;
            public long maxX;
            public long minY;
            public long maxY;
            public long minZ;
            public long maxZ;

            public bool isInStartArea;

            public Cube(bool on, long minX, long maxX, long minY, long maxY, long minZ, long maxZ)
            {
                this.on = on;
                this.minX = Math.Min(minX, maxX);
                this.maxX = Math.Max(minX, maxX);
                this.minY = Math.Min(minY, maxY);
                this.maxY = Math.Max(minY, maxY);
                this.minZ = Math.Min(minZ, maxZ);
                this.maxZ = Math.Max(minZ, maxZ);

                isInStartArea = (   minX >= -50 && maxX <= 50 &&
                                    minY >= -50 && maxY <= 50 &&
                                    minZ >= -50 && maxZ <= 50);
            }

            public long Size => ((maxX - minX) + 1) * ((maxY - minY) + 1) * ((maxZ - minZ) + 1);

            public bool Valid
            {
                get
                {
                    return (minX <= maxX && minY <= maxY && minZ <= maxZ);
                }
            }

            public List<Cube> SplitOnOverlap(Cube laterInInstructions)
            {
                var (xSize, xCutPoints) = AxisOverlap(minX, maxX, laterInInstructions.minX, laterInInstructions.maxX);
                var (ySize, yCutPoints) = AxisOverlap(minY, maxY, laterInInstructions.minY, laterInInstructions.maxY);
                var (zSize, zCutPoints) = AxisOverlap(minZ, maxZ, laterInInstructions.minZ, laterInInstructions.maxZ);

                List<Cube> newCubes = new List<Cube>();

                var overlap = xSize * ySize * zSize;

                if (overlap == 0)
                {
                    newCubes.Add(this);
                }
                else if (overlap < Size)
                {
                    // x cut
                    foreach (var cutpoint in xCutPoints)
                    {
                        if (cutpoint.keepLower)
                        {
                            newCubes.Add(new Cube(on, minX, cutpoint.coordinate, minY, maxY, minZ, maxZ));
                            minX = cutpoint.coordinate + 1;
                        }
                        else if (!cutpoint.dropEverything)
                        {
                            newCubes.Add(new Cube(on, cutpoint.coordinate, maxX, minY, maxY, minZ, maxZ));
                            maxX = cutpoint.coordinate - 1;
                        }
                    }

                    // y cut
                    foreach (var cutpoint in yCutPoints)
                    {
                        if (cutpoint.keepLower)
                        {
                            newCubes.Add(new Cube(on, minX, maxX, minY, cutpoint.coordinate, minZ, maxZ));
                            minY = cutpoint.coordinate + 1;
                        }
                        else if(!cutpoint.dropEverything)
                        {
                            newCubes.Add(new Cube(on, minX, maxX, cutpoint.coordinate, maxY, minZ, maxZ));
                            maxY = cutpoint.coordinate - 1;
                        }
                    }

                    // z cut
                    foreach (var cutpoint in zCutPoints)
                    {
                        if (cutpoint.keepLower)
                        {
                            newCubes.Add(new Cube(on, minX, maxX, minY, maxY, minZ, cutpoint.coordinate));
                            minZ = cutpoint.coordinate + 1;
                        }
                        else if (!cutpoint.dropEverything)
                        {
                            newCubes.Add(new Cube(on, minX, maxX, minY, maxY, cutpoint.coordinate, maxZ));
                            maxZ = cutpoint.coordinate - 1;
                        }
                    }
                }

                return newCubes;
            }

            public bool HasOverlap(Cube cube) => Overlap(cube) > 0;
            public long Overlap(Cube cube)
            {
                var (xOverlap, _) = AxisOverlap(minX, maxX, cube.minX, cube.maxX);
                var (yOverlap, _) = AxisOverlap(minY, maxY, cube.minY, cube.maxY);
                var (zOverlap, _) = AxisOverlap(minZ, maxZ, cube.minZ, cube.maxZ);

                var overlap = xOverlap * yOverlap * zOverlap;

                return overlap;
            }

            public record CutPoint
            (
                long coordinate,
                bool keepLower,
                bool dropEverything
            );

            public (long length, CutPoint[] overlapEdges) AxisOverlap(long myMin, long myMax, long hisMin, long hisMax)
            {
                var hisSize = hisMax - hisMin + 1;

                var lowCut = new CutPoint(hisMin - 1, true, false);
                var highCut = new CutPoint(hisMax + 1, false, false);

                if (hisMin <= myMin && hisMax >= myMax)
                {
                    return (myMax - myMin + 1, new[] { new CutPoint(0, false, true) });
                }
                else if (hisMin >= myMin && hisMin <= myMax)
                {
                    if (hisSize < myMax - hisMin + 1) return (hisSize, new[] { lowCut, highCut });
                    return (myMax - hisMin + 1, new[] { lowCut });
                }
                else if (hisMax >= myMin && hisMax <= myMax)
                {
                    return (hisMax - myMin + 1, new[] { highCut });
                }
                return (0, Array.Empty<CutPoint>());
            }

            public override string ToString()
            {
                string onOff = on ? "on" : "off";
                return $"{onOff}: x {minX}-{maxX}, y {minY}-{maxY}, z {minZ}-{maxZ}";
            }
        }

        public long GetCubesThatAreOn(IEnumerable<Cube> input)
        {
            var cubes = input.ToList();

            var distinctCubes = new List<Cube> { cubes[0] };

            for (int n = 1; n < cubes.Count; n++)
            {
                var cube = cubes[n];

                var newSplit = new List<Cube>();
                for (int i = 0; i < distinctCubes.Count; i++)
                { 
                    var distinctCube = distinctCubes[i];

                    var split = distinctCube.SplitOnOverlap(cube);

                    foreach (var s in split) newSplit.Add(s);
                }
                newSplit.Add(cube);
                distinctCubes = newSplit;
            }

            return CalculateOn(distinctCubes);
        }

        public long CalculateOn(List<Cube> distinctCubes)
        {
            long totalCount = 0;
            for (int n = 0; n < distinctCubes.Count; n++)
            {
                var cube = distinctCubes[n];

                if (cube.on) totalCount += cube.Size;
            }

            return totalCount;
        }

        public object GetResult1()
        {
            var cubesInStartingArea = cubes.Where(c => c.isInStartArea);

            return GetCubesThatAreOn(cubesInStartingArea);

            
        }

        public object GetResult2()
        {
            return "";// GetCubesThatAreOn(cubes);
        }
    }
}
