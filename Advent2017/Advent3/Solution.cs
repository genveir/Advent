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
            return "";
        }
    }
}
