using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Advent2022.Advent14
{
    public class Solution : ISolution
    {
        public List<RockLines> rockLines;
        public HashSet<Coordinate> rock;
        public HashSet<Coordinate> sand;

        public long highestY;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            rockLines = new();
            foreach (var line in lines)
            {
                rockLines.Add(new(line.Split(" -> ").Select(l => l.Parse<Coordinate>("coord")).ToArray()));
            }

            rock = new();
            foreach (var rockLine in rockLines) rockLine.AddToHashSet(rock);

            highestY = rock.Max(r => r.Y);

            sand = new();
        }
        public Solution() : this("Input.txt") { }

        public class RockLines
        {
            public Coordinate[] Coordinates;

            [ComplexParserConstructor]
            public RockLines(Coordinate[] coordinates)
            {
                Coordinates = coordinates;
            }

            public void AddToHashSet(HashSet<Coordinate> set)
            {
                for (int n = 0; n < Coordinates.Length - 1; n++)
                {
                    AddToHashSet(set, Coordinates[n], Coordinates[n+1]);
                }
            }

            public void AddToHashSet(HashSet<Coordinate> set, Coordinate start, Coordinate end)
            {
                if (start.X == end.X)
                {
                    var minY = Math.Min(start.Y, end.Y);
                    var maxY = Math.Max(start.Y, end.Y);
                    for (long y = minY; y <= maxY; y++)
                    {
                        set.Add(new(start.X, y));
                    }
                }
                else if (start.Y == end.Y)
                {
                    var minX = Math.Min(start.X, end.X);
                    var maxX = Math.Max(start.X, end.X);
                    for (long x = minX; x <= maxX; x++)
                    {
                        set.Add(new(x, start.Y));
                    }
                }
                else
                {
                    throw new NotSupportedException("can only do lines");
                }
            }
        }

        public bool Drop1Step(Coordinate sandPosition, out Coordinate newPosition)
        {
            var oneDown = sandPosition.ShiftY(1);

            if (CanPlace(oneDown))
            {
                newPosition = oneDown;
                return true;
            }

            var shiftLeft = oneDown.ShiftX(-1);

            if (CanPlace(shiftLeft))
            {
                newPosition = shiftLeft;
                return true;
            }

            var shiftRight = oneDown.ShiftX(1);

            if (CanPlace(shiftRight))
            {
                newPosition = shiftRight;
                return true;
            }

            newPosition = sandPosition;
            return false;
        }

        private bool CanPlace(Coordinate sandPosition) =>
            !(rock.Contains(sandPosition) || sand.Contains(sandPosition));

        // could maintain a "drop path" and just drop from one position back on the queue each time
        // could maintain a set of y-coordinates for each x-coordinate and move down in 1 step
        public bool Drop1Sand(Coordinate dropFrom)
        {
            Coordinate sandPosition = dropFrom;
            if (!CanPlace(sandPosition)) return false;

            while(sandPosition.Y < highestY + 5)
            {
                if (!Drop1Step(sandPosition, out Coordinate newPosition))
                {
                    sand.Add(sandPosition);
                    return true;
                }

                sandPosition = newPosition;
            }
            return false;
        }

        public object GetResult1()
        {
            while(Drop1Sand(new(500,0))) { }

            return sand.Count;
        }

        public object GetResult2()
        {
            for (int x = -100000; x < 100000; x++)
            {
                rock.Add(new(x, highestY + 2));
            }

            while(Drop1Sand(new(500,0))) { }

            return sand.Count;
        }
    }
}
