using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent04
{
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

                rowPicks[coordinate.Y]++;
                columnPicks[coordinate.X]++;

                if (rowPicks[coordinate.Y] == 5) MarkWin(turn, number);
                if (columnPicks[coordinate.X] == 5) MarkWin(turn, number);
            }
        }

        public long score;
        public long turn;
        public void MarkWin(long turn, long number)
        {
            this.won = true;

            this.turn = turn;
            this.score = number * boardValue;
        }
    }
}
