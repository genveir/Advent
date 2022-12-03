using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.AdventActive
{
    public class Solution : ISolution
    {
        public List<Rucksack> rucksacks;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<Rucksack>("line");

            rucksacks = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class Rucksack
        {
            private static int groupCounter;
            public int group;

            public char[] allItems;
            public char[] items1;
            public char[] items2;

            [ComplexParserConstructor]
            public Rucksack(string line)
            {
                allItems = line.ToCharArray();
                items1 = line.Substring(0, line.Length / 2).ToCharArray();
                items2 = line.Substring(line.Length / 2).ToCharArray();

                group = groupCounter / 3;
                groupCounter++;
            }

            public int GetIntersectPriority()
            {
                var intersect = items1
                    .Intersect(items2)
                    .Single();

                return GetPriority(intersect);
            }

            public static int GetLargerIntersectPriority(Rucksack[] rucksacks)
            {
                var intersect = rucksacks[0].allItems
                    .Intersect(rucksacks[1].allItems)
                    .Intersect(rucksacks[2].allItems)
                    .Single();

                return GetPriority(intersect);
            }

            public static int GetPriority(char item) =>
                 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(item) + 1;
        }

        public object GetResult1()
        {
            return rucksacks
                .Sum(m => m.GetIntersectPriority());
        }

        public object GetResult2()
        {
            return rucksacks
                .GroupBy(m => m.group)
                .Sum(g => Rucksack.GetLargerIntersectPriority(g.ToArray()));
        }
    }
}
