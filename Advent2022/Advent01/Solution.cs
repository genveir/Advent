using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent01
{
    public class Solution : ISolution
    {
        public List<Elf> elves = new();

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input).ToArray();

            foreach (var block in blocks) elves.Add(new(block));
        }
        public Solution() : this("Input.txt") { }

        public class Elf
        {
            public int[] calories;

            [ComplexParserConstructor]
            public Elf(string[] lines)
            {
                calories = lines.Select(l => int.Parse(l)).ToArray();
            }

            public int TotalCalories => calories.Sum();
        }

        public object GetResult1()
        {
            return elves
                .Max(m => m.TotalCalories);
        }

        public object GetResult2()
        {
            return elves
                .Select(m => m.TotalCalories)
                .OrderByDescending(m => m)
                .Take(3)
                .Sum();
        }
    }
}
