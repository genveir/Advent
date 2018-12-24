using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent24
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
            string resourceName = "Advent.Advent24." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            string input;
            using (var txt = new StreamReader(inputFile))
            {
                input = txt.ReadToEnd();
            }

            _ParseInput(input);
        }

        public void _ParseInput(string input)
        {
            var immuneSystem = new List<Group>();
            var infection = new List<Group>();
            var currentAffiliation = Affiliation.ImmuneSystem;

            var lines = input.Replace("\r", "").Split('\n');
            for (int n = 0; n < lines.Length; n++)
            {
                var line = lines[n].Trim();

                if (line == "Immune System:") currentAffiliation = Affiliation.ImmuneSystem;
                else if (line == "Infection:") currentAffiliation = Affiliation.Infection;
                else if (line != "")
                {
                    if (currentAffiliation == Affiliation.ImmuneSystem) immuneSystem.Add(Group.Parse(line, currentAffiliation));
                    else infection.Add(Group.Parse(line, currentAffiliation));
                }
            }

            fight = new Fight(immuneSystem, infection);
        }

        public Fight fight;

        public int GetPart1(bool print = false)
        {
            while (fight.DoRound(print)) { }

            return fight.immuneSystemArmy.Union(fight.infectionArmy).Sum(g => g.numUnits);
        }

        public int GetPart2()
        {
            for (int n = 0; n < 100000; n++)
            {
                fight.Reset();
                fight.Boost(n);
                while (fight.DoRound(false)) { }
                if (fight.Winner == Affiliation.ImmuneSystem) return fight.immuneSystemArmy.Sum(g => g.numUnits);
            }
            return -1;
        }

        public void WriteResult()
        {
            Console.WriteLine("part1: " + GetPart1(false));
            Console.WriteLine("part2: " + GetPart2());
        }
    }
}
