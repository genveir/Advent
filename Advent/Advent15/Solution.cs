using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent15
{
    public class Solution : ISolution
    {
        public const int GOBLIN_HP = 200;
        public const int GOBLIN_AP = 3;
        public const int ELF_HP = 200;
        public const int ELF_AP = 3;

        private List<Tile> Creatures;
        private Dictionary<TileType, int> numOfEach;
        private TileFactory factory;

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
            factory = new TileFactory();
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

        public RunResult Run(bool print = false)
        {
            int turn = 0;
            while (Step() && turn < 1000)
            {
                turn++;

                if (print)
                {
                    Print(turn);
                    Console.ReadLine();
                }
            }

            if (turn == 1000) return new RunResult() { hasResult = false };
            return new RunResult() { turn = turn, hps = Creatures.Sum(c => c.HP) };
        }

        public void WriteResult()
        {
            ParseInput();

            var result = Run();

            if (result.hasResult)
            {
                Console.WriteLine(FormattedResult(result));
            }
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
            var maxX = factory.AllPositions.Keys.Max(k => k.X);
            var maxY = factory.AllPositions.Keys.Max(k => k.Y);
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    var tile = factory.AllPositions[new XYCoord(x, y)];

                    switch (tile.Type)
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

        public void Print(int turn)
        {
            Console.WriteLine("after turn " + turn + " the status is:");
            Console.WriteLine(this);
        }
    }
}
