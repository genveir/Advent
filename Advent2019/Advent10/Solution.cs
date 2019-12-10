using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent10
{
    public class Solution : ISolution
    {
        public List<Asteroid> asteroids;
        public Dictionary<(int x, int y), Asteroid> locations;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            asteroids = Asteroid.Parse(lines);
            locations = new Dictionary<(int x, int y), Asteroid>();
            foreach (var asteroid in asteroids) locations.Add((asteroid.X, asteroid.Y), asteroid);

            FindStation();
            Find200th();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public class Asteroid
        {
            public int X;
            public int Y;
            public List<Asteroid> Visible = new List<Asteroid>();
            public double AngleFromStation;

            public static List<Asteroid> Parse(string[] lines)
            {
                var asteroids = new List<Asteroid>();

                for (int y = 0; y < lines.Count(); y++)
                {
                    var line = lines[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '.') continue;
                        else
                        {
                            asteroids.Add(new Asteroid() { X = x, Y = y });
                        }
                    }
                }

                return asteroids;
            }

            public override string ToString()
            {
                return "Asteroid (" + X + ", " + Y + ")";
            }
        }

        public int GetGCD(int first, int second)
        {
            if (first == 1 || second == 1) return 1;

            return GetNon1GCD(first, second);
        }

        public int GetNon1GCD(int first, int second)
        {
            if (second == 0) return first;
            if (first == 0) return second;

            if (second > first) return GetNon1GCD(second % first, first);
            return GetNon1GCD(first, first % second);
        }

        public void SetVisible (int asteroidIndex)
        {
            var handled = new HashSet<(int, int)>();

            asteroids[asteroidIndex].Visible.Clear();

            for (int secondIndex = 0; secondIndex < asteroids.Count; secondIndex++)
            {
                if (asteroidIndex == secondIndex) continue;

                var xShift = asteroids[secondIndex].X - asteroids[asteroidIndex].X;
                var yShift = asteroids[secondIndex].Y - asteroids[asteroidIndex].Y;

                var GCD = GetGCD(Math.Abs(xShift), Math.Abs(yShift));

                xShift = xShift == 0 ? 0 : xShift / GCD;
                yShift = yShift == 0 ? 0 : yShift / GCD;

                if (handled.Contains((xShift, yShift))) continue;
                handled.Add((xShift, yShift));

                int newX = asteroids[asteroidIndex].X + xShift;
                int newY = asteroids[asteroidIndex].Y + yShift;
                for (int n = 0; n < GCD; n++)
                {
                    if (locations.ContainsKey((newX, newY)))
                    {
                        asteroids[asteroidIndex].Visible.Add(asteroids[secondIndex]);
                        break;
                    }

                    newX += xShift;
                    newY += yShift;
                }
            }
        }

        public double GetAngle(int X, int Y)
        {
            var angle = ((Math.Atan2(Y, X) + 0.5 * Math.PI) + 2.0d * Math.PI) % (2.0d * Math.PI);
            return angle;
        }

        public void SetAngles(int asteroidIndex)
        {
            var stationAst = asteroids[asteroidIndex];

            foreach (var visible in stationAst.Visible)
            {
                visible.AngleFromStation = GetAngle(visible.X - stationAst.X, visible.Y - stationAst.Y);
            }
        }

        int highestVisible = 0;
        int station = -1;
        public void FindStation()
        {
            for (int firstIndex = 0; firstIndex < asteroids.Count; firstIndex++)
            {
                SetVisible(firstIndex);
                if (asteroids[firstIndex].Visible.Count > highestVisible)
                {
                    highestVisible = asteroids[firstIndex].Visible.Count;
                    station = firstIndex;
                }
            }
        }

        Asteroid asteroid200;
        public void Find200th()
        {
            var stationAsteroid = asteroids[station];

            SetAngles(station);
            var inOrder = stationAsteroid.Visible.OrderBy(ast => ast.AngleFromStation).ToArray();

            if (asteroids.Count > 200)
                asteroid200 = inOrder[199];
        }

        public string GetResult1()
        {
            return highestVisible.ToString();
        }

        public string GetResult2()
        {
            return (asteroid200.X * 100 + asteroid200.Y).ToString();
        }
    }
}
