using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent17
{
    public class Solution : ISolution
    {
        public TargetArea target;

        public Solution(string input)
        {
            var line = Input.GetInputLines(input).ToArray().Single();

            var split = line.Split(new char[] { ' ', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var minX = long.Parse(split[3]);
            var maxX = long.Parse(split[4]);
            var minY = long.Parse(split[6]);
            var maxY = long.Parse(split[7]);

            target = new TargetArea(minX, maxX, minY, maxY);
        }
        public Solution() : this("Input.txt") { }

        public class TargetArea
        {
            public long xMin;
            public long xMax;
            public long yMin;
            public long yMax;

            [ComplexParserConstructor]
            public TargetArea(long xMin, long xMax, long yMin, long yMax)
            {
                this.xMin = xMin;
                this.xMax = xMax;
                this.yMin = yMin;
                this.yMax = yMax;
            }

            public Coordinate CalculatePosition(long xVelocity, long yVelocity, long turns)
            {
                var newX = CalculateXPosition(xVelocity, turns);
                var newY = CalculateYPosition(yVelocity, turns);

                return new Coordinate(newX, newY);
            }

            public long CalculateXPosition(long xVelocity, long turns)
            {
                long xVal = Math.Min(xVelocity, turns);

                return xVelocity * xVal - triangle(xVal - 1);
            }

            public long CalculateYPosition(long yVelocity, long turns)
            {
                return yVelocity * turns - triangle(turns - 1);
            }

            public int ValidatePosition(long xVelocity, long yVelocity, long turns)
            {
                var validateX = ValidateXPosition(xVelocity, turns);

                if (validateX != 0) return validateX;
                else return ValidateYPosition(yVelocity, turns);
            }

            public int ValidateYPosition(long yVelocity, long turns)
            {
                var position = CalculateYPosition(yVelocity, turns);

                if (position < yMin) return 1; 
                if (position > yMax) return -1;
                else return 0;
            }

            public int ValidateXPosition(long xVelocity, long turns)
            {
                var position = CalculateXPosition(xVelocity, turns);

                if (position < xMin) return -1;
                if (position > xMax) return 1;
                return 0;
            }

            public bool IsValidShot(long xVelocity, long yVelocity)
            {
                var turns = 0;
                var stepSize = 256;
                var stepUp = true;
                
                while(stepSize > 0)
                {
                    if (stepUp) turns = turns + stepSize;
                    else turns = turns - stepSize;
                    stepSize = stepSize / 2;

                    var validation = ValidatePosition(xVelocity, yVelocity, turns);
                    if (validation == 0) return true;
                    stepUp = (validation < 1);
                }

                return false;
            }

            public List<long> TurnsForYVelocity(long yVelocity)
            {
                long turns = 0;
                var stepSize = (long)Math.Pow(2.0d, 12.0d);
                var stepUp = true;

                long hitTurn = -1;
                while (stepSize > 0)
                {
                    if (stepUp) turns = turns + stepSize;
                    else turns = turns - stepSize;
                    stepSize = stepSize / 2;

                    var validation = ValidateYPosition(yVelocity, turns);
                    if (validation == 0)
                    {
                        hitTurn = turns;
                        stepSize = 0;
                    }
                    stepUp = (validation < 1);
                }

                var hits = new List<long>();
                if (hitTurn != -1) hits.Add(hitTurn);

                for (long runBackTurn = hitTurn - 1; runBackTurn > 0 && ValidateYPosition(yVelocity, runBackTurn) == 0 && runBackTurn > 0; runBackTurn--)
                {
                    hits.Add(runBackTurn);
                }
                for (long runForwardTurn = hitTurn + 1; ValidateYPosition(yVelocity, runForwardTurn) == 0; runForwardTurn++)
                {
                    hits.Add(runForwardTurn);
                }

                return hits;
            }

            public List<long> TurnsForXVelocity(long xVelocity)
            {
                long turns = 0;
                var stepSize = (long)Math.Pow(2.0d, 12.0d);
                var stepUp = true;

                long hitTurn = -1;
                while (stepSize > 0)
                {
                    if (stepUp) turns = turns + stepSize;
                    else turns = turns - stepSize;
                    stepSize = stepSize / 2;

                    var validation = ValidateXPosition(xVelocity, turns);
                    if (validation == 0)
                    {
                        hitTurn = turns;
                        stepSize = 0;
                    }
                    stepUp = (validation < 1);
                }

                var hits = new List<long>();
                if (hitTurn != -1) hits.Add(hitTurn);

                for (long runBackTurn = hitTurn - 1; runBackTurn > 0  && ValidateXPosition(xVelocity, runBackTurn) == 0; runBackTurn--)
                {
                    hits.Add(runBackTurn);
                }
                for (long runForwardTurn = hitTurn + 1; runForwardTurn < xMax && ValidateXPosition(xVelocity, runForwardTurn) == 0; runForwardTurn++)
                {
                    hits.Add(runForwardTurn);
                }

                return hits;
            }

            public List<(long xVelocity, long yVelocity, long turn)> EnumeratePossibleHits()
            {
                if (PossibleHits != null) return PossibleHits;

                var xHitsAtTurn = new Dictionary<long, List<long>>();
                for (long xVelocity = 0; xVelocity <= xMax; xVelocity++)
                {
                    var hitTurns = TurnsForXVelocity(xVelocity);
                    foreach (var hitTurn in hitTurns)
                    {
                        if (!xHitsAtTurn.ContainsKey(hitTurn)) xHitsAtTurn.Add(hitTurn, new List<long>());
                        xHitsAtTurn[hitTurn].Add(xVelocity);
                    }
                }

                var yHitsAtTurn = new Dictionary<long, List<long>>();
                for (long yVelocity = yMin; yVelocity <= xMax; yVelocity++)
                {
                    var hitTurns = TurnsForYVelocity(yVelocity);
                    foreach (var hitTurn in hitTurns)
                    {
                        if (!yHitsAtTurn.ContainsKey(hitTurn)) yHitsAtTurn.Add(hitTurn, new List<long>());
                        yHitsAtTurn[hitTurn].Add(yVelocity);
                    }
                }

                var result = new List<(long xVelocity, long yVelocity, long turn)>();

                foreach(var kvp in yHitsAtTurn)
                {
                    var turn = kvp.Key;
                    var yVelocities = kvp.Value;

                    if (xHitsAtTurn.TryGetValue(turn, out List<long> xVelocities))
                    {
                        for (int x = 0; x < xVelocities.Count; x++)
                        {
                            for (int y = 0; y < yVelocities.Count; y++)
                            {
                                result.Add((xVelocities[x], yVelocities[y], turn));
                            }
                        }
                    }
                }

                PossibleHits = result;

                return result;
            }

            public List<(long xVelocity, long yVelocity, long turn)> PossibleHits { get; private set; }

            public long getMaxY(long xVelocity, long yVelocity)
            {
                long turns = yVelocity;
                if (yVelocity < 0) turns = 0;

                return CalculatePosition(xVelocity, yVelocity, turns).Y;
            }

            public Dictionary<long, long> _triangles = new Dictionary<long, long>();
            public long triangle(long input)
            {
                long value;
                if (!_triangles.TryGetValue(input, out value))
                { 
                    value = input * (input + 1) / 2;

                    _triangles[input] = value;
                }
                return value;
            }
        }

        public object GetResult1()
        {
            var hits = target.EnumeratePossibleHits();

            if (hits.Count == 0) return 0;
            var highestYVel = hits.OrderBy(p => p.yVelocity).Last();

            return target.getMaxY(highestYVel.xVelocity, highestYVel.yVelocity);
        }

        public object GetResult2()
        {
            return target.EnumeratePossibleHits()
                .Select(p => (p.xVelocity, p.yVelocity))
                .Distinct()
                .Count();
        }
    }
}
