using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent04
{
    public class Solution : ISolution
    {
        long[] numbers;
        BingoBoard[] bingoBoards;

        public static string numberInput = "63,23,2,65,55,94,38,20,22,39,5,98,9,60,80,45,99,68,12,3,6,34,64,10,70,69,95,96,83,81,32,30,42,73,52,48,92,28,37,35,54,7,50,21,74,36,91,97,13,71,86,53,46,58,76,77,14,88,78,1,33,51,89,26,27,31,82,44,61,62,75,66,11,93,49,43,85,0,87,40,24,29,15,59,16,67,19,72,57,41,8,79,56,4,18,17,84,90,47,25";

        public Solution(string input)
        {
            numbers = numberInput.Split(',').Select(long.Parse).ToArray();

            var blocks = Input.GetBlockLines(input).ToArray();

            bingoBoards = blocks.Select(b => new BingoBoard(b)).ToArray();
        }
        public Solution() : this("Input.txt") { }

        public class BingoBoard
        {
            public long[][] fields;
            public bool[][] picked;

            Dictionary<long, Coordinate> byNum = new Dictionary<long, Coordinate>();

            public BingoBoard(string[] input)
            {
                fields = new long[5][];
                picked = new bool[5][];

                for (int y = 0; y < 5; y++)
                {
                    fields[y] = new long[5];
                    picked[y] = new bool[5];

                    var nums = input[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int x =0; x < 5; x++)
                    {
                        var num = long.Parse(nums[x]);
                        fields[y][x] = num;

                        var coord = new Coordinate(x, y);
                        byNum.Add(num, coord);
                    }
                }
            }

            public void Pick(long number)
            {
                Coordinate coordinate;
                if (byNum.TryGetValue(number, out coordinate))
                {
                    picked[coordinate.Y][coordinate.X] = true;
                }
            }

            public bool CheckForWin()
            {
                var pivotted = picked.Pivot();

                for (int y = 0; y < 5; y++) if (picked[y].All(p => p)) return true;
                for (int x = 0; x < 5; x++) if (pivotted[x].All(p => p)) return true;

                return false;
            }

            public long score = -1; // urgl
            public long turn;
            public void MarkWin(long turn, long number)
            {
                if (this.score != -1) return;

                this.turn = turn;
                this.score = number * CalculateScore();
            }

            public long CalculateScore()
            {
                long result = 0;
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if (!picked[y][x]) result += fields[y][x];
                    }
                }
                return result;
            }
        }

        public object GetResult1()
        {
            for (int n = 0; n < numbers.Length; n++)
            {
                var number = numbers[n];

                foreach (var board in bingoBoards)
                {
                    board.Pick(number);

                    if (board.CheckForWin()) return board.CalculateScore() * number;
                }
            }

            return "no answer";
        }

        public object GetResult2()
        {
            for (int n = 0; n < numbers.Length; n++)
            {
                var number = numbers[n];

                foreach (var board in bingoBoards)
                {
                    board.Pick(number);

                    if (board.CheckForWin()) board.MarkWin(n, number);
                }
            }

            var last = bingoBoards.OrderBy(b => b.turn).Last();

            return last.score;
        }
    }
}
