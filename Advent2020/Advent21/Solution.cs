using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent21
{
    public class Solution : ISolution
    {
        Dictionary<string, Allergen> allergens = new Dictionary<string, Allergen>();
        Dictionary<string, int> ingredients = new Dictionary<string, int>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            foreach(var line in lines)
            {
                var firstSplit = line.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                var ingreds = firstSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach(var ingred in ingreds)
                {
                    int num;
                    if (!ingredients.TryGetValue(ingred, out num)) ingredients[ingred] = 0;

                    ingredients[ingred]++;
                }

                var allergenStrings = firstSplit[1].Replace("contains", "").Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(var allergenString in allergenStrings)
                {
                    Allergen allergen;
                    allergens.TryGetValue(allergenString, out allergen);
                    if (allergen == null)
                    {
                        allergen = new Allergen() { Name = allergenString };
                        allergens.Add(allergenString, allergen);
                    }

                    if (allergen.MightBeIn.Count == 0) allergen.MightBeIn.AddRange(ingreds);
                    else allergen.MightBeIn = allergen.MightBeIn.Intersect(ingreds).ToList();
                }
            }
        }
        public Solution() : this("Input.txt") { }

        public class Allergen
        {
            public string Name;

            public List<string> MightBeIn = new List<string>();

            public override string ToString()
            {
                return Name + " MBI " + string.Join(',', MightBeIn);
            }
        }

        private IEnumerable<string> GetCleanIngreds()
        {
            var allMightBeIns = allergens.SelectMany(al => al.Value.MightBeIn).Distinct();

            var cleanIngreds = ingredients.Keys.Except(allMightBeIns);

            return cleanIngreds;
        }

        public object GetResult1()
        {
            var cleanIngreds = GetCleanIngreds();

            int numClean = 0;
            foreach (var cleanIngred in cleanIngreds) numClean += ingredients[cleanIngred];

            return numClean;
        }

        public object GetResult2()
        {
            var cleanIngreds = GetCleanIngreds();

            foreach(var allergen in allergens.Values)
            {
                allergen.MightBeIn = allergen.MightBeIn.Except(cleanIngreds).ToList();
            }

            Dictionary<string, string> resolved = new Dictionary<string, string>();
            while(resolved.Count < allergens.Count)
            {
                foreach(var allergen in allergens)
                {
                    if (allergen.Value.MightBeIn.Count == 1)
                    {
                        resolved.Add(allergen.Value.Name, allergen.Value.MightBeIn.Single());
                    }
                }

                foreach(var allergen in allergens)
                {
                    allergen.Value.MightBeIn = allergen.Value.MightBeIn.Except(resolved.Values).ToList();
                }
            }

            return string.Join(',', resolved.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
        }
    }
}
