using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent7
{
    public class Solution : ISolution
    {
        Dictionary<string, Bag> bagsByName = new Dictionary<string, Bag>();
        List<Bag> bags = new List<Bag>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            bags = lines.Select(line =>
            {
                line = line.Replace(" bags", "");
                line = line.Replace(" bag", "");

                var firstSplit = line.Split("contain");

                string name = firstSplit[0].Trim();

                var bag = GetOrAdd(name);

                var secondsplit = firstSplit[1].Split(new char[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < secondsplit.Length; i++)
                {
                    var b = secondsplit[i];
                    b = b.Trim();

                    if (b != "no other")
                    {
                        var words = b.Split(" ", 2);

                        var num = int.Parse(words[0]);
                        var containedname = words[1].Trim();

                        var contained = GetOrAdd(containedname);

                        bag.Contains.Add((num, contained));
                        contained.CanBeContainedBy.Add(bag);
                    }
                }
                
                return bag;
            }).ToList();
        }
        public Solution() : this("Input.txt") { }

        private Bag GetOrAdd(string name)
        {
            Bag bag;
            if (!bagsByName.TryGetValue(name, out bag))
            {
                bag = new Bag() { name = name };
                bagsByName.Add(name, bag);
            };

            return bag;
        }

        public class Bag
        {
            public string name;

            public List<(int num, Bag bag)> Contains = new List<(int num, Bag bag)>();
            public List<Bag> CanBeContainedBy = new List<Bag>();

            public IEnumerable<Bag> TransitiveCanBeContained()
            {
                return CanBeContainedBy.Union(CanBeContainedBy.SelectMany(b => b.TransitiveCanBeContained()));
            }

            public long TransitiveNumInBag()
            {
                long result = 1;
                for (int i = 0; i < Contains.Count; i++)
                {
                    (int num, Bag bag) = Contains[i];

                    result += num * bag.TransitiveNumInBag();
                }

                return result;
            }
        }

        public object GetResult1()
        {
            var shinyGoldBag = GetOrAdd("shiny gold");

            return shinyGoldBag.TransitiveCanBeContained().Count();
        }

        public object GetResult2()
        {
            var shinyGoldBag = GetOrAdd("shiny gold");

            return shinyGoldBag.TransitiveNumInBag() - 1;
        }
    }
}
