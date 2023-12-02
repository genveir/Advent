using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Advent2023.Shared;

namespace Advent2023.Advent02;

public class Solution : ISolution
{
    public List<Game> games;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input)
            .Select(l => l.Substring(5))
            .ToArray();

        var inputParser = new InputParser<Game>("game");

        games = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class Game
    {
        public long Id { get; }
        public List<Draw> Draws { get; }

        [ComplexParserConstructor("num: draws")]
        public Game(long id, Draw[] draws)
        {
            Id = id;
            Draws = draws.ToList();
        }
    }

    public class Draw
    {
        public List<DieGroup> DieGroups { get; }

        [ComplexParserConstructor("dieGroups", ArrayDelimiters = new[] {';'})]
        public Draw(DieGroup[] dieGroups)
        {
            DieGroups = dieGroups.ToList();
        }
    }

    public class DieGroup
    {
        public string Color { get; }
        public long Number { get; }

        [ComplexParserConstructor("number color")]
        public DieGroup(long number, string color)
        {
            Color = color;
            Number = number;
        }
    }

    public object GetResult1()
    {
        long sum = 0;

        foreach(var game in games)
        {
            if (game.Draws.Any(d => d.DieGroups.Any(dg => dg.Color == "red" && dg.Number > 12))) continue;
            if (game.Draws.Any(d => d.DieGroups.Any(dg => dg.Color == "green" && dg.Number > 13))) continue;
            if (game.Draws.Any(d => d.DieGroups.Any(dg => dg.Color == "blue" && dg.Number > 14))) continue;
            sum += game.Id;
        }

        return sum;
    }

    public object GetResult2()
    {
        long sum = 0;

        foreach(var game in games)
        {
            long red = game.Draws.SelectMany(d => d.DieGroups.Where(dg => dg.Color == "red")).Max(dg => dg.Number);
            long green = game.Draws.SelectMany(d => d.DieGroups.Where(dg => dg.Color == "green")).Max(dg => dg.Number);
            long blue = game.Draws.SelectMany(d => d.DieGroups.Where(dg => dg.Color == "blue")).Max(dg => dg.Number);

            var power = red * green * blue;

            sum += power;
        }

        return sum;
    }
}
