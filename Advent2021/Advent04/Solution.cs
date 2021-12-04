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
            numbers = Input.GetNumbers(numberInput, ',');

            var blocks = Input.GetBlockLines(input);

            bingoBoards = blocks.Select(b => new BingoBoard(b)).ToArray();

            RunBingo();
        }
        public Solution() : this("Input.txt") { }

        private void RunBingo()
        {
            for (int turn = 0; turn < numbers.Length; turn++)
            {
                var number = numbers[turn];

                foreach (var board in bingoBoards)
                {
                    board.Pick(turn, number);
                }
            }
        }

        public class BingoBoard
        {
            public long[][] fields;

            public long boardValue;

            public long[] rowPicks;
            public long[] columnPicks;

            Dictionary<long, Coordinate> byNum = new Dictionary<long, Coordinate>();

            public BingoBoard(string[] input)
            {
                fields = new long[5][];
                rowPicks = new long[5];
                columnPicks = new long[5];

                for (int y = 0; y < 5; y++)
                {
                    fields[y] = new long[5];

                    var nums = input[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < 5; x++)
                    {
                        var num = long.Parse(nums[x]);
                        boardValue += num;

                        fields[y][x] = num;

                        var coord = new Coordinate(x, y);
                        byNum.Add(num, coord);
                    }
                }
            }

            public bool won = false;
            public void Pick(long turn, long number)
            {
                if (won) return;

                Coordinate coordinate;
                if (byNum.TryGetValue(number, out coordinate))
                {
                    boardValue -= number;

                    if (++rowPicks[coordinate.Y] == 5) MarkWin(turn, number);
                    if (++columnPicks[coordinate.X] == 5) MarkWin(turn, number);
                }
            }

            public long score;
            public long wonOnTurn;
            public void MarkWin(long turn, long number)
            {
                this.won = true;

                this.wonOnTurn = turn;
                this.score = number * boardValue;
            }
        }

        public object GetResult1()
        {
            return bingoBoards.OrderBy(b => b.wonOnTurn).First().score;
        }

        public object GetResult2()
        {
            return bingoBoards.OrderBy(b => b.wonOnTurn).Last().score;
        }
    }
}
