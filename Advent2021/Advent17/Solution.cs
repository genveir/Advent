using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent17
{
    public class Solution : ISolution
    {
        public TargetArea target;

        public Solution(string input)
        {
            var line = Input.GetInputLines(input).ToArray().Single();

            var split = line.Split(new char[] { ' ', '=', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var minX = long.Parse(split[3]);
            var maxX = long.Parse(split[4]);
            var minY = long.Parse(split[6]);
            var maxY = long.Parse(split[7]);

            target = new TargetArea(minX, maxX, minY, maxY);
        }
        public Solution() : this("Input.txt") { }

        public class TargetArea
        {
            public long xMin;
            public long xMax;
            public long yMin;
            public long yMax;

            [ComplexParserConstructor]
            public TargetArea(long xMin, long xMax, long yMin, long yMax)
            {
                this.xMin = xMin;
                this.xMax = xMax;
                this.yMin = yMin;
                this.yMax = yMax;
            }

            public Coordinate CalculatePosition(long xVelocity, long yVelocity, long turns)
            {

                var tri = triangle(turns - 1);

                long newX;
                if (turns > xVelocity)
                {
                    newX = xVelocity * xVelocity - triangle(xVelocity - 1);                    
                }
                else newX = xVelocity * turns - tri;

                var newY = yVelocity * turns - tri;

                return new Coordinate(newX, newY);
            }

            public int ValidatePosition(long xVelocity, long yVelocity, long turns)
            {
                var position = CalculatePosition(xVelocity, yVelocity, turns);

                if (position.X < xMin) return -1; 
                if (position.X > xMax) return 1; 
                if (position.Y < yMin) return 1; 
                if (position.Y > yMax) return -1;
                else return 0;
            }

            public bool IsValidShot(long xVelocity, long yVelocity)
            {
                var turns = 0;
                var stepSize = 256;
                var stepUp = true;
                
                while(stepSize > 0)
                {
                    if (stepUp) turns = turns + stepSize;
                    else turns = turns - stepSize;
                    stepSize = stepSize / 2;

                    var validation = ValidatePosition(xVelocity, yVelocity, turns);
                    if (validation == 0) return true;
                    stepUp = (validation < 1);
                }

                return false;
            }

            public long getMaxY(long xVelocity, long yVelocity)
            {
                long turns = yVelocity;
                if (yVelocity < 0) turns = 0;

                return CalculatePosition(xVelocity, yVelocity, turns).Y;
            }

            public long triangle(long input) => input * (input + 1) / 2;
        }

        public object GetResult1()
        {
            long highest = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    if (target.IsValidShot(x, y))
                    {
                        var maxY = target.getMaxY(x, y);
                        if (maxY > highest) highest = maxY;
                    }
                }
            }

            return highest;
        }

        public object GetResult2()
        {
            long num = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = -1000; y < 1000; y++)
                {
                    if (target.IsValidShot(x, y))
                    {
                        num++;
                    }
                }
            }

            return num;
        }
    }
}
