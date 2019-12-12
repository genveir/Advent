using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2019.Advent10
{
    public class Solution : ISolution
    {
        public List<Asteroid> asteroids;
        bool[] locations = new bool[5000];

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            asteroids = Asteroid.Parse(lines);
            foreach (var asteroid in asteroids) locations[asteroid.X * 100 + asteroid.Y] = true;

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

        private int[] GCDs = new int[5000];
        private int GetGCD(int xShift, int yShift)
        {
            int key = xShift * 100 + yShift;
            int result = GCDs[key];
            if (result == 0)
            {
                result = (int)Helper.GCD(xShift, yShift);
                GCDs[key] = result;
            }
            return result;
        }


        public void SetVisible (int asteroidIndex)
        {
            var handled = new bool[10000];

            asteroids[asteroidIndex].Visible.Clear();

            for (int secondIndex = 0; secondIndex < asteroids.Count; secondIndex++)
            {
                if (asteroidIndex == secondIndex) continue;

                var xShift = asteroids[secondIndex].X - asteroids[asteroidIndex].X;
                var yShift = asteroids[secondIndex].Y - asteroids[asteroidIndex].Y;

                var GCD = GetGCD(Math.Abs(xShift), Math.Abs(yShift));

                xShift = xShift == 0 ? 0 : xShift / GCD;
                yShift = yShift == 0 ? 0 : yShift / GCD;

                if (handled[(xShift + 40) * 100 +  yShift]) continue;
                handled[(xShift + 40) * 100 +  yShift] = true;

                int newX = asteroids[asteroidIndex].X + xShift;
                int newY = asteroids[asteroidIndex].Y + yShift;
                for (int n = 0; n < GCD; n++)
                {
                    if (locations[newX * 100 + newY])
                    {
                        asteroids[asteroidIndex].Visible.Add(asteroids[secondIndex]);
                        break;
                    }

                    newX += xShift;
                    newY += yShift;
                }
            }
        }

        public void SetAngles(int asteroidIndex)
        {
            var stationAst = asteroids[asteroidIndex];

            foreach (var visible in stationAst.Visible)
            {
                visible.AngleFromStation = Helper.GetAngle((stationAst.X, stationAst.Y), (visible.X, visible.Y));
            }
        }

        int highestVisible = 0;
        int station = -1;
        public void FindStation()
        {
            Parallel.For(0, asteroids.Count, (firstIndex) =>
            {
                SetVisible(firstIndex);
                if (asteroids[firstIndex].Visible.Count > highestVisible)
                {
                    highestVisible = asteroids[firstIndex].Visible.Count;
                    station = firstIndex;
                }
            });
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
