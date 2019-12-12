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

        public Coordinate periods;

        public void SetPeriods()
        {
            long counter = 0;

            periods = new Coordinate(0, 0, 0);
            while (periods.X == 0 || periods.Y == 0 || periods.Z == 0)
            {
                SimulateStep();
                counter++;

                if (periods.X == 0 && moons.All(moon => moon.originalCoordinate.X == moon.coordinate.X && moon.velocity.X == 0)) periods.X = counter;
                if (periods.Y == 0 && moons.All(moon => moon.originalCoordinate.Y == moon.coordinate.Y && moon.velocity.Y == 0)) periods.Y = counter;
                if (periods.Z == 0 && moons.All(moon => moon.originalCoordinate.Z == moon.coordinate.Z && moon.velocity.Z == 0)) periods.Z = counter;
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

            var totalLCM = Helper.LCM(periods.X, periods.Y);
            totalLCM = Helper.LCM(totalLCM, periods.Z.Value);

            return totalLCM.ToString();
            // not 64704171707646560
            // 16100228207840 too low
        }
    }
}
