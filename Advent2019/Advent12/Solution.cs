using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent12
{
    public class Solution : ISolution
    {
        public static Moon[] moons;

        private Input.InputMode inputMode;
        private string input;

        public Solution(Input.InputMode inputMode, string input)
        {
            this.inputMode = inputMode;
            this.input = input;

            Parse();
        }
        public void Parse()
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            moons = Moon.Parse(lines).ToArray();
        }

        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public int numSteps = 1000;


        public class Moon
        {
            public long Id;
            public Coordinate coordinate;
            public Coordinate velocity;

            public Coordinate originalCoordinate;

            public Coordinate periods;

            public static IEnumerable<Moon> Parse(IEnumerable<string> lines)
            {
                var parsedInputs = new List<Moon>();

                long id = 0;
                foreach(var line in lines)
                {
                    var coords = line.Split(new char[] { '<', 'x', '=', ',', ' ', 'y', 'z', '>' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(a => long.Parse(a)).ToArray();

                    var pi = new Moon()
                    {
                        coordinate = new Coordinate(coords[0], coords[1], coords[2]),
                        originalCoordinate = new Coordinate(coords[0], coords[1], coords[2]),
                        velocity = new Coordinate(0, 0, 0),
                        periods = new Coordinate(0, 0, 0),
                        Id = id++
                    };

                    parsedInputs.Add(pi);
                }

                return parsedInputs;
            }

            public void ApplyGravity()
            {
                foreach(var moon in moons)
                {
                    if (moon.Id == Id) continue;

                    if (moon.coordinate.X > this.coordinate.X) this.velocity.X++;
                    if (moon.coordinate.Y > this.coordinate.Y) this.velocity.Y++;
                    if (moon.coordinate.Z > this.coordinate.Z) this.velocity.Z++;

                    if (moon.coordinate.X < this.coordinate.X) this.velocity.X--;
                    if (moon.coordinate.Y < this.coordinate.Y) this.velocity.Y--;
                    if (moon.coordinate.Z < this.coordinate.Z) this.velocity.Z--;
                }
            }

            public void ApplyVelocity()
            {
                coordinate.X += velocity.X;
                coordinate.Y += velocity.Y;
                coordinate.Z += velocity.Z;
            }

            public long PotentialEnergy {
                get
                {
                    return Math.Abs(coordinate.X) +
                        Math.Abs(coordinate.Y) +
                        Math.Abs(coordinate.Z.Value);
                }
            }

            public long KineticEnergy
            {
                get
                {
                    return Math.Abs(velocity.X) +
                        Math.Abs(velocity.Y) +
                        Math.Abs(velocity.Z.Value);
                }
            }

            public bool MatchesStart
            {
                get
                {
                    return coordinate.Equals(originalCoordinate) && velocity.Equals(new Coordinate(0, 0, 0));
                }
            }

            public override int GetHashCode()
            {
                return (int)Id;
            }

            public override bool Equals(object obj)
            {
                return ((Moon)obj).Id == Id;
            }

            public override string ToString()
            {
                return string.Format("moon {6}: pos=<x={0}, y={1}, z={2}>, vel=<x={3}, y={4}, z={5}>",
                    coordinate.X, coordinate.Y, coordinate.Z,
                    velocity.X, velocity.Y, velocity.Z, Id);
            }
        }

        public void SimulateStep(bool verbose = false)
        {
            foreach (var moon in moons) moon.ApplyGravity();
            foreach (var moon in moons) moon.ApplyVelocity();

            if (verbose)
            {
                foreach (var moon in moons) Console.WriteLine(moon);
                Console.WriteLine();
                Console.ReadLine();
            }
        }

        public void SetPeriods()
        {
            long counter = 0;

            while (moons.Any(moon => moon.periods.X == 0 || moon.periods.Y == 0 || moon.periods.Z == 0))
            {
                SimulateStep();
                counter++;
             
                foreach(var moon in moons)
                {
                    if (moon.periods.X == 0 && moon.originalCoordinate.X == moon.coordinate.X && moon.velocity.X == 0) moon.periods.X = counter;
                    if (moon.periods.Y == 0 && moon.originalCoordinate.Y == moon.coordinate.Y && moon.velocity.Y == 0) moon.periods.Y = counter;
                    if (moon.periods.Z == 0 && moon.originalCoordinate.Z == moon.coordinate.Z && moon.velocity.Z == 0) moon.periods.Z = counter;
                }
            }
        }

        public string GetResult1()
        {
            for (int n = 0; n < numSteps; n++) SimulateStep();

            long totalEnergy = moons.Sum(m => m.KineticEnergy * m.PotentialEnergy);

            return totalEnergy.ToString();
            // not 1109
        }

        public string GetResult2()
        {
            Parse();

            SetPeriods();

            var xPeriods = moons.OrderBy(m => m.periods.X).Select(m => m.periods.X).ToArray();
            var yPeriods = moons.OrderBy(m => m.periods.Y).Select(m => m.periods.Y).ToArray();
            var zPeriods = moons.OrderBy(m => m.periods.Z).Select(m => m.periods.Z.Value).ToArray();

            var xLCM = Helper.LCM(xPeriods[0], xPeriods[1]);
            xLCM = Helper.LCM(xLCM, xPeriods[2]);
            xLCM = Helper.LCM(xLCM, xPeriods[3]);

            Parse();
            for (int n = 0; n < xLCM; n++)
            {
                SimulateStep();
                if (n > 0 && moons.All(moon => moon.originalCoordinate.X == moon.coordinate.X && moon.velocity.X == 0)) xLCM = n + 1;
            }
            Console.WriteLine(xLCM);
            foreach (var moon in moons) Console.WriteLine(moon);

            var yLCM = Helper.LCM(yPeriods[0], yPeriods[1]);
            yLCM = Helper.LCM(yLCM, yPeriods[2]);
            yLCM = Helper.LCM(yLCM, yPeriods[3]);

            Parse();
            for (int n = 0; n < yLCM; n++)
            {
                if (n > 0 && moons.All(moon => moon.originalCoordinate.Y == moon.coordinate.Y && moon.velocity.Y == 0)) yLCM = n;
                SimulateStep();
            }
            foreach (var moon in moons) Console.WriteLine(moon);

            var zLCM = Helper.LCM(zPeriods[0], zPeriods[1]);
            zLCM = Helper.LCM(zLCM, zPeriods[2]);
            zLCM = Helper.LCM(zLCM, zPeriods[3]);

            Parse();
            for (int n = 0; n < zLCM; n++)
            {
                if (n > 0 && moons.All(moon => moon.originalCoordinate.Z == moon.coordinate.Z && moon.velocity.Z == 0)) zLCM = n;
                SimulateStep();
            }
            foreach (var moon in moons) Console.WriteLine(moon);

            var totalLCM = Helper.LCM(xLCM, yLCM);
            totalLCM = Helper.LCM(totalLCM, zLCM);

            return totalLCM.ToString();
            // not 64704171707646560
            // 16100228207840 too low
        }
    }
}
