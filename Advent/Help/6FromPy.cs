using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent
{
    class _6FromPy
    {
        public void DoTheThing()
        {
            var inputFile = typeof(Program).Assembly.GetManifestResourceStream("Advent.Input.Advent6Input.txt");
            using (var txt = new StreamReader(inputFile))
            {
                var input = txt.ReadToEnd();
                var lines = input.Split('\n');
                //X = list(map(lambda line: int(re.findall("-?\d+", line)[0]), lines))
                var X = lines.Select(line => int.Parse(Regex.Matches(line, @"-?\d+")[0].Value)).ToArray();
                //Y = list(map(lambda line: int(re.findall("-?\d+", line)[1]), lines))
                var Y = lines.Select(line => int.Parse(Regex.Matches(line, @"-?\d+")[1].Value)).ToArray();

                //coords = [(X[i], Y[i]) for i in range(len(X))]
                var coords = new List<XYTuple>();
                for(int i = 0; i < X.Count(); i++) coords.Add(new XYTuple(X[i], Y[i]));

                //minX, maxX, minY, maxY = min(X), max(X), min(Y), max(Y)
                var minX = X.Min(); var maxX = X.Max(); var minY = Y.Min(); var maxY = Y.Max();

                //#Using only the Manhattan distance, determine the area around each coordinate by counting the number of integer X,Y locations that are closest to that coordinate (and aren't tied in distance to any other coordinate).
                //infiniteCoords = set([i for i in range(len(coords)) if X[i] in (minX, maxX) or Y[i] in [minY, maxY]])
                var infiniteCoords = new HashSet<int>();
                for (int i = 0; i < coords.Count(); i++)
                {
                    if (coords[i].X == minX || coords[i].X == maxX || coords[i].Y == minY || coords[i].Y == maxY) infiniteCoords.Add(i);
                }

                //closestPointsASCII = {}
                var closestPointASCII = new Dictionary<XYTuple, char>();

                //closestPoints = {}
                var closestPoints = new Dictionary<XYTuple, int>();

                //pointsCount = defaultdict(int)
                var pointsCount = new Dictionary<int, int>();

                //for x in range(minX, maxX + 1) :
                for (int x = minX; x < maxX + 1; x++)
                {
                    //for y in range(minY, maxY + 1) :
                    for (int y = minY; y < maxY + 1; y++)
                    {
                        //dists = [(abs(x - X[i]) + abs(y - Y[i]), i) for i in range(len(X))] # format: (distance, pointIndex)
                        var dists = new List<Dist>();
                        for (int i = 0; i < X.Length; i++) dists.Add(new Dist(Math.Abs(x - X[i]) + Math.Abs(y - Y[i]), i));

                        //minDist = min(dists)
                        var minDist = dists.Min();

                        //uniqueMaxDists = sum([dist[0] == minDist[0] for dist in dists]) == 1 # only one distance with maxDistance
                        bool uniqueMaxDists = dists.Where(dist => dist[0] == minDist[0]).Count() == 1;

                        //if uniqueMaxDists:
                        if (uniqueMaxDists)
                        {
                            //closestPointsASCII[(x, y)] = chr(65 + minDist[1])
                            closestPointASCII.Add(new XYTuple(x, y), (char)(65 + minDist[1]));

                            //closestPoints[(x, y)] = minDist[1]
                            closestPoints.Add(new XYTuple(x, y), minDist[1]);

                            //pointsCount[minDist[1]] += 1
                            if (!pointsCount.ContainsKey(minDist[1])) pointsCount.Add(minDist[1], 0);
                            pointsCount[minDist[1]] += 1;
                        }
                        //else:
                        else
                        {
                            //closestPointsASCII[(x, y)] = '-'
                            closestPointASCII.Add(new XYTuple(x, y), '-');

                            //closestPoints[(x, y)] = -1
                            closestPoints.Add(new XYTuple(x, y), -1);
                        }
                    }
                }

                //print(
                //  max(
                //      [
                //          (pointsCount[index], coords[index]) 
                //          for index in range(len(X)) if index not in infiniteCoords
                //      ]
                //  )
                //)

                List<PointCoordTuple> tuples = new List<PointCoordTuple>();
                for (int index = 0; index < X.Length; index++)
                {
                    if (infiniteCoords.Contains(index)) continue;
                    tuples.Add(new PointCoordTuple(pointsCount[index], coords[index]));
                }
                Console.WriteLine(tuples.Max());
            }
        }

        private class XYTuple
        {
            public XYTuple(int x, int y) { X = x; Y = y; }

            public int X;
            public int Y;

            public override string ToString()
            {
                return String.Format("({0},{1})", X, Y);
            }
        }

        private class Dist : IComparable
        {
            public Dist(int distance, int pointIndex) { Distance = distance; PointIndex = pointIndex; }

            public int this[int i] // even zodat ik de python syntax kan kopieren
            {
                get { return i == 0 ? Distance : PointIndex; }
            }

            public int Distance;
            public int PointIndex;

            public int CompareTo(object obj)
            {
                return this.Distance.CompareTo(((Dist)obj).Distance);
            }

            public override string ToString()
            {
                return string.Format("distance {0}, pointIndex {1}", Distance, PointIndex);
            }
        }

        private class PointCoordTuple : IComparable
        {
            public PointCoordTuple(int pointCount, XYTuple coord) { PointCount = pointCount; Coord = coord; }

            public int PointCount;
            public XYTuple Coord;
            public int CompareTo(object obj)
            {
                return this.PointCount.CompareTo(((PointCoordTuple)obj).PointCount);
            }

            public override string ToString()
            {
                return string.Format("pointcount {0}, coord {1}", PointCount, Coord);
            }
        }
    }
}