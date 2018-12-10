using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent
{
    class Advent10
    {
        List<Vector> GetInput()
        {
            string resourceName = "Advent.Input.Advent10Input.txt";
            var input = typeof(Program).Assembly.GetManifestResourceStream(resourceName);

            var vectors = new List<Vector>();
            using (var txt = new StreamReader(input))
            {
                while (!txt.EndOfStream)
                    vectors.Add(Vector.Parse(txt.ReadLine()));
            }

            return vectors;
        }

        private class Vector
        {
            public int baseX;
            public int baseY;
            public int velocityX;
            public int velocityY;

            public static Vector Parse(string input)
            {
                var split = input.Split(new char[] { ' ', '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
                return new Vector()
                {
                    baseX = int.Parse(split[1]),
                    baseY = int.Parse(split[2]),
                    velocityX = int.Parse(split[4]),
                    velocityY = int.Parse(split[5])
                };
            }
        }

        int GetXCrossingTime(Vector v1, Vector v2)
        {
            var basediff = v2.baseX - v1.baseX;
            var veldiff = v1.velocityX - v2.velocityX;

            return basediff / veldiff;
        }

        private class AdjacencyBucket
        {
            private List<AdjacencyXYCoord> coords;

            public AdjacencyBucket(IEnumerable<Vector> vectors, int time)
            {
                coords = new List<AdjacencyXYCoord>();
                foreach (var vector in vectors)
                {
                    coords.Add(new AdjacencyXYCoord()
                    {
                        X = vector.baseX + time * vector.velocityX,
                        Y = vector.baseY + time * vector.velocityY
                    });
                }
                foreach (var coord in coords)
                {
                    foreach (var adjacentCoord in coord.GetAdjacent())
                    {
                        var matchingCoords = coords.Where(c => c.Equals(adjacentCoord));
                        if (matchingCoords.Count() > 0)
                        {
                            coord.IsAdjacent = true;
                            foreach (var matchingCoord in matchingCoords) matchingCoord.IsAdjacent = true;
                        }
                    }
                }
            }

            public int GetNumAdjacent()
            {
                return coords.Where(c => c.IsAdjacent).Count();
            }

            public void Print()
            {
                var xMin = coords.Select(c => c.X).Min();
                var yMin = coords.Select(c => c.Y).Min();

                Console.WriteLine("adjacencyBucket:");
                for (int y = yMin; y < yMin + 60; y++)
                {
                    for (int x = xMin; x < xMin + 60; x++)
                    {
                        Console.Write((coords.Where(c => c.X == x && c.Y == y).Count() > 0) ? "X" : " ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private class AdjacencyXYCoord
        {
            public int X;
            public int Y;
            public bool IsAdjacent = false;

            public IEnumerable<AdjacencyXYCoord> GetAdjacent()
            {
                var adjacent = new List<AdjacencyXYCoord>();
                adjacent.Add(new AdjacencyXYCoord() { X = X + 1, Y = Y });
                adjacent.Add(new AdjacencyXYCoord() { X = X - 1, Y = Y });
                adjacent.Add(new AdjacencyXYCoord() { X = X , Y = Y + 1});
                adjacent.Add(new AdjacencyXYCoord() { X = X , Y = Y - 1});

                return adjacent;
            }

            public override int GetHashCode() { return X + Y; }
            public override bool Equals(object obj)
            {
                var other = obj as AdjacencyXYCoord;
                return other.X == X && other.Y == Y;
            }
        }

        public void WriteResult()
        {
            var vectors = GetInput();

            var v1 = vectors[0];
            var v2 = vectors.Where(v => v.velocityX != v1.velocityX).First();
            var startingTime = GetXCrossingTime(v1, v2);

            for (int time = startingTime - 5; time < startingTime + 5; time++)
            {
                var adjacencyBucket = new AdjacencyBucket(vectors, time);
                if (adjacencyBucket.GetNumAdjacent() > vectors.Count * .8)
                {
                    adjacencyBucket.Print();
                    Console.WriteLine("dit werkt, op time " + time + " met startingTime " + startingTime);
                }
            }
        }
    }
}
