using Advent2017.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2017.Advent3
{
    public class Solution : ISolution
    {
        public Solution(string input)
        {
            
        }
        public Solution() : this("Input.txt") { }

        public Coordinate FindNum(long numToFind)
        {
            long n = 1;
            long square = 1;
            while(square < numToFind)
            {
                n += 2;
                square = n * n;
            }

            var nSteps = (n - 1) / 2;
            var squareCoord = new Coordinate(nSteps, nSteps);
            var shiftLength = n - 1;

            var corner = square;

            if (corner - numToFind > shiftLength) { squareCoord = squareCoord.ShiftX(-shiftLength); corner -= shiftLength; }
            else return squareCoord.ShiftX(numToFind - corner); 

            if (corner - numToFind > shiftLength) { squareCoord = squareCoord.ShiftY(-shiftLength); corner -= shiftLength; }
            else return squareCoord.ShiftY(numToFind - corner);

            if (corner - numToFind > shiftLength) { squareCoord = squareCoord.ShiftX(shiftLength); corner -= shiftLength; }
            else return squareCoord.ShiftX(corner - numToFind);

            return squareCoord.ShiftY(corner - numToFind);
        }

        public object GetResult1()
        {
            var coord = FindNum(277678);
            return coord.ManhattanDistance(new Coordinate(0,0));
        }

        public object GetResult2()
        {
            int stepLength = 1;
            ModNum direction = new ModNum(0, 4);
            Coordinate current = new Coordinate(0, 0);

            Dictionary<Coordinate, int> values = new Dictionary<Coordinate, int>();

            Set(current, 1, values);
            while(true)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int n = 0; n < stepLength; n++)
                    {
                        current = Step(direction, current);
                        var value = values[current];
                        if (value > 277678) return value;
                        Set(current, value, values);
                    }
                    direction++;
                }
                stepLength++;
            }
        }

        private Coordinate Step(ModNum direction, Coordinate current)
        {
            switch(direction.number)
            {
                case 0: return current.ShiftX(1);
                case 1: return current.ShiftY(-1);
                case 2: return current.ShiftX(-1);
                case 3: return current.ShiftY(1);
            }
            throw new InvalidOperationException("direction should be mod 4");
        }

        private void Set(Coordinate coordinate, int value, Dictionary<Coordinate, int> values)
        {
            var neighbours = coordinate.GetNeighbours();
            
            foreach(var neighbour in neighbours)
            {
                if (!values.ContainsKey(neighbour)) values.Add(neighbour, 0);

                values[neighbour] += value;
            }
        }
    }
}
