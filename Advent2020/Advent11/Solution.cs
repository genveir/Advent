using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent11
{
    public class Solution : ISolution
    {
        public Seat[][] grid;
        public List<Seat> seats = new List<Seat>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            grid = new Seat[lines.Length][];

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                grid[y] = new Seat[line.Length];

                for (int x = 0; x < line.Length; x++)
                {
                    grid[y][x] = new Seat(line[x]);

                    grid[y][x].Link(grid, y, x);

                    seats.Add(grid[y][x]);
                }
            }
        }
        public Solution() : this("Input.txt") { }

        public enum SeatState { NoSeat, Empty, Occupied }

        public class Seat
        {
            private Seat[] neighbours = new Seat[8];
            private Seat[] pt2Neighbours = new Seat[8];

            public SeatState initialState;
            public SeatState state;
            public SeatState nextState;

            public Seat(SeatState state)
            {
                this.state = state;
            }

            public Seat(char state)
            {
                switch(state)
                {
                    case '.': this.state = SeatState.NoSeat; break;
                    case 'L': this.state = SeatState.Empty; break;
                    case '#': this.state = SeatState.Occupied; break;
                }
                this.nextState = this.state;
                this.initialState = this.state;

                neighbours = new Seat[8];
                for (int n = 0; n < 8; n++) neighbours[n] = new Seat(SeatState.NoSeat);

                pt2Neighbours = new Seat[8];
                for (int n = 0; n < 8; n++) pt2Neighbours[n] = new Seat(SeatState.NoSeat);
            }

            public void Reset()
            {
                this.state = this.initialState;
                this.nextState = this.state;
            }

            public void Link(Seat[][] seats, int y, int x)
            {
                for (int dY = -1; dY <= 0; dY++)
                {
                    for (int dX = -1; dX <= 1; dX++)
                    {
                        if (dY == 0 && dX == 0) return;

                        int neighbourIndex = 3 * dY + dX + 4;

                        LinkPt1(seats, y, x, neighbourIndex, dY, dX);

                        LinkPt2(seats, y, x, neighbourIndex, dY, dX);
                    }
                }
            }

            public void LinkPt1(Seat[][] seats, int y, int x, int neighbourIndex, int dY, int dX)
            {
                var nY = y + dY;
                if (nY < 0) return;

                var nX = x + dX;
                if (nX < 0 || nX >= seats[y].Length) return;

                neighbours[neighbourIndex] = seats[nY][nX];
                neighbours[neighbourIndex].neighbours[7 - neighbourIndex] = this;
            }

            public void LinkPt2(Seat[][] seats, int y, int x, int neighbourIndex, int dY, int dX)
            {
                int nY = y;
                int nX = x;

                while (true)
                {
                    nY += dY;
                    if (nY < 0) return;

                    nX += dX;
                    if (nX < 0 || nX >= seats[y].Length) return;

                    if (seats[nY][nX].state != SeatState.NoSeat) break;
                }

                pt2Neighbours[neighbourIndex] = seats[nY][nX];
                pt2Neighbours[neighbourIndex].pt2Neighbours[7 - neighbourIndex] = this;
            }

            public void CalculateNextState(bool isPart2)
            {
                var numOcc = isPart2 ? 5 : 4;
                var activeNB = isPart2 ? pt2Neighbours : neighbours;

                if (state == SeatState.Empty)
                {
                    if (activeNB.Where(nb => nb.state == SeatState.Occupied).Count() == 0) nextState = SeatState.Occupied;
                }
                else if (state == SeatState.Occupied)
                {
                    if (activeNB.Where(nb => nb.state == SeatState.Occupied).Count() >= numOcc) nextState = SeatState.Empty;
                }
            }

            public bool Flip()
            {
                var changes = this.state != this.nextState;

                this.state = this.nextState;

                return changes;
            }
        }

        public void Reset()
        {
            foreach(var seat in seats)
            {
                seat.Reset();
            }
        }

        public bool RunStep(bool isPart2)
        {
            foreach(var seat in seats)
            {
                seat.CalculateNextState(isPart2);
            }

            bool changed = false;
            foreach(var seat in seats)
            {
                changed = seat.Flip() || changed;
            }

            return changed;
        }

        public object GetResult1()
        {
            Reset();

            int numSteps = 1;
            while(RunStep(false)) numSteps++;

            return seats.Where(s => s.state == SeatState.Occupied).Count();
        }

        public object GetResult2()
        {
            Reset();

            int numSteps = 1;
            while (RunStep(true)) numSteps++;

            return seats.Where(s => s.state == SeatState.Occupied).Count();
        }
    }
}
