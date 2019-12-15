using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent15
{
    public class Solution : ISolution
    {
        public const int GOBLIN_HP = 200;
        public const int GOBLIN_AP = 3;
        public const int ELF_HP = 200;
        public static int ELF_AP = 3;

        private List<Tile> Creatures;
        private Dictionary<TileType, int> numOfEach;

        private string Input;
        public enum InputMode { String, File }

        public Solution() : this("Input", InputMode.File) { }
        public Solution(string input, InputMode mode)
        {
            if (mode == InputMode.File) ParseInput(input);
            else
            {
                Input = input;
                ParseInput();
            }
        }

        private void ParseInput(string fileName)
        {
            string resourceName = "Advent.Advent15." + fileName + ".txt";
            var inputFile = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            using (var txt = new StreamReader(inputFile))
            {
                Input = txt.ReadToEnd();
            }

            ParseInput();
        }

        private void ParseInput()
        {
            var factory = new TileFactory();
            Creatures = new List<Tile>();
            numOfEach = new Dictionary<TileType, int>();
            numOfEach[TileType.Elf] = 0;
            numOfEach[TileType.Goblin] = 0;

            var input = Input.Replace("\r", "").Split('\n');

            for (int y = 0; y < input.Length; y++)
            {
                var line = input[y].Trim();
                for (int x = 0; x < line.Length; x++)
                {
                    var tile = factory.Parse(new XYCoord(x, y), line[x]);
                    if (tile != null && tile.IsAlive)
                    {
                        Creatures.Add(tile);
                        numOfEach[tile.Type]++;
                    }
                }
            }
        }
        
        public bool Step()
        {
            var creatures = Creatures.OrderBy(c => c.coord).ToList();

            var newCreatures = new List<Tile>();
            foreach (var creature in creatures)
            {
                if (!creature.IsAlive) continue;
                if (numOfEach[creature.Type.Opponent()] == 0) return false;

                var moved = creature.Move();
                if (moved.Attack()) numOfEach[moved.Type.Opponent()]--;
                newCreatures.Add(moved);
            }

            Creatures = newCreatures.Where(c => c.IsAlive).ToList();
            return true;
        }

        public class RunResult
        {
            public bool hasResult = true;
            public int turn;
            public int hps;
        }

        public RunResult Run(bool stopIfElfDies = false)
        {
            var numElves = numOfEach[TileType.Elf];

            int turn = 0;
            while (Step() && turn < 1000)
            {
                turn++;

                if (stopIfElfDies)
                {
                    if (numOfEach[TileType.Elf] != numElves)
                    {
                        return new RunResult() { hasResult = false };
                    }
                }
            }

            if (turn == 1000) return new RunResult() { hasResult = false };
            return new RunResult() { turn = turn, hps = Creatures.Sum(c => c.HP) };
        }

        public void WriteResult()
        {
            ParseInput();

            var result = Run(false);

            if (result.hasResult)
            {
                Console.Write("part1: ");
                Console.WriteLine(FormattedResult(result));
            }

            ELF_AP = 4;
            for (ELF_AP = 4; true; ELF_AP++)
            { 
                ParseInput();

                result = Run(true);

                if (result.hasResult) break;
            }

            Console.Write("part2: ");
            Console.WriteLine(FormattedResult(result));
            return;
        }

        private string FormattedResult(RunResult result)
        {
            return string.Format("fight ended on turn {0} with {1} total hps, for an outcome of {2}",
                result.turn,
                result.hps,
                result.turn * result.hps);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            FindCounter--;
            var tiles = new List<Tile>();
            foreach(var creature in Creatures) RecursiveGetNeighbours(tiles, creature);

            var maxX = tiles.Max(k => k.coord.X);
            var maxY = tiles.Max(k => k.coord.Y);
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    var tile = tiles.Where(t => t.coord.Equals(new XYCoord(x, y))).SingleOrDefault();

                    if (tile == null) builder.Append('#');
                    else switch (tile.Type)
                    {
                        case TileType.Floor: builder.Append('.'); break;
                        case TileType.Goblin: builder.Append('G'); break;
                        case TileType.Elf: builder.Append('E'); break;
                        default: builder.Append('#'); break;
                    }
                }
                if (y != maxY) builder.Append('\n');
            }

            return builder.ToString();
        }

        private static int FindCounter = -1;
        private void RecursiveGetNeighbours(List<Tile> soFar, Tile tile)
        {
            if (!(tile.FoundBy == FindCounter)) soFar.Add(tile);

            tile.FoundBy = FindCounter;
            foreach(var neighbour in tile.Neighbours)
            {
                if (neighbour.FoundBy != FindCounter) RecursiveGetNeighbours(soFar, neighbour);
            }
        }
    }
}
