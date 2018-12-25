using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent25
{
    class Solution : ISolution
    {
        public enum InputMode { String, File }

        public Solution() : this("Input", InputMode.File) { }
        public Solution(string input, InputMode mode)
        {
            if (mode == InputMode.File) ParseInput(input);
            else
            {
                _ParseInput(input);
            }
        }

        private void ParseInput(string fileName)
        {
            string resourceName = "Advent.Advent25." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        List<Star> AllStars = new List<Star>();
        public void _ParseInput(string input)
        {
            var lines = input.Replace("\r", "").Split("\n");
            for (int n = 0; n < lines.Length; n++)
            {
                var line = lines[n];
                var asInts = line.Split(',').Select(l => int.Parse(l)).ToArray();
                var star = new Star
                {
                    X = asInts[0],
                    Y = asInts[1],
                    Z = asInts[2],
                    T = asInts[3]
                };
                AllStars.Add(star);
            }
        }

        public int GetPart1()
        {
            var constellations = new List<Constellation>();

            foreach(var star in AllStars)
            {
                var constellation = new Constellation();
                constellation.Stars.Add(star);
                constellations.Add(constellation);
            }

            while(JoinSome(constellations)) { }
            
            return constellations.Count;
        }

        private bool JoinSome(List<Constellation> constellations)
        {
            List<Constellation> toRemove = new List<Constellation>();

            int numConstellations = constellations.Count;
            for (int n = 0; n < numConstellations; n++)
            {
                var constellation1 = constellations[n];
                bool joinedSome = false;
                for (int i = n + 1; i < numConstellations; i++)
                {
                    var constellation2 = constellations[i];

                    var stargroup1 = constellation1.Stars;
                    var stargroup2 = constellation2.Stars;

                    if (IsCloseEnough(stargroup1, stargroup2))
                    {
                        toRemove.Add(constellation2);
                        constellation2.Join(constellation1);
                        joinedSome = true;
                    }
                }
                if (joinedSome)
                {
                    foreach (var constellation in toRemove) constellations.Remove(constellation);
                    return true;
                }
            }
            return false;
        }

        private bool IsCloseEnough(IEnumerable<Star> stargroup1, IEnumerable<Star> stargroup2)
        {
            foreach (var star1 in stargroup1)
            {
                foreach (var star2 in stargroup2)
                {
                    if (star1.DistanceTo(star2) <= 3) return true;
                }
            }

            return false;
        }

        public int GetPart2()
        {

            return 0;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetPart1());
            Console.WriteLine("part2: " + GetPart2());
        }
    }
}
