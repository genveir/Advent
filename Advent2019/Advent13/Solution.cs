using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent2019.Shared.Search;
using System.Collections.Concurrent;

namespace Advent2019.Advent13
{
    public class Solution : ISolution
    {
        public Executor executor;

        public Dictionary<Coordinate, long> tiles;

        public long BlockCount;
        public long BallX;
        public long PaddleX;
        public long Score;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);

            tiles = new Dictionary<Coordinate, long>();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Print()
        {
            var minX = tiles.Min(t => t.Key.X);
            var maxX = tiles.Max(t => t.Key.X);
            var minY = tiles.Min(t => t.Key.Y);
            var maxY = tiles.Max(t => t.Key.Y);

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    long tile;
                    tiles.TryGetValue(new Coordinate(x, y), out tile);

                    switch(tile)
                    {
                        case 0: Console.Write(" "); break;
                        case 1: Console.Write(Helper.BLOCK); break;
                        case 2: Console.Write("\U00002591"); break;
                        case 3: Console.Write("_");break;
                        case 4: Console.Write("O"); break;
                    }
                }
                Console.WriteLine();
            }
        }

        public void ResetState()
        {
            BlockCount = 0;
            BallX = 0;
            PaddleX = 0;
            Score = 0;
        }

        public void UpdateState()
        {
            while (executor.program.output.Count > 0)
            {
                var x = long.Parse(executor.program.output.Dequeue());
                var y = long.Parse(executor.program.output.Dequeue());
                var tile = long.Parse(executor.program.output.Dequeue());

                UpdateState(x, y, tile);
            }
        }

        public void UpdateState(long x, long y, long tile)
        {
            if (x == -1) Score = tile;
            else
            {
                /*
                 * 0 is an empty tile. No game object appears in this tile.
                 * 1 is a wall tile. Walls are indestructible barriers.
                 * 2 is a block tile. Blocks can be broken by the ball.
                 * 3 is a horizontal paddle tile. The paddle is indestructible.
                 * 4 is a ball tile. The ball moves diagonally and bounces off objects.
                */
                Coordinate coord = new Coordinate(x, y);

                long currentTile;
                tiles.TryGetValue(coord, out currentTile);

                if (tile == 0)
                {
                    if (currentTile == 2) BlockCount--;
                    tiles.Remove(coord);
                }
                if (tile == 2)
                {
                    if (currentTile != tile) BlockCount++;
                }
                if (tile == 3)
                {
                    PaddleX = x;
                }
                if (tile == 4)
                {
                    BallX = x;
                }

                tiles[coord] = tile;
            }
        }

        public string GetResult1()
        {
            ResetState();

            executor.Reset();
            executor.Execute();

            UpdateState();
            Print();

            // not 880 (empty)
            // not 277 (?)
            return BlockCount.ToString();
        }

        public string GetResult2()
        {
            ResetState();

            executor.Reset();
            executor.program.ISetAt(0, 2);

            executor.Execute();
            UpdateState();

            while (executor.program.Blocked)
            {
                long input = PaddleX == BallX ? 0 : (PaddleX < BallX ? 1 : -1);

                executor.AddInput(input);
                UpdateState();

                //Print(); Console.ReadLine();
            }

            return Score.ToString();
        }
    }
}
