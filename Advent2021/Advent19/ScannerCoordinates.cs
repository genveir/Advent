using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent19
{
    public class ScannerCoordinates
    {
        public Coordinate[] coordinates;
        public Coordinate[][] relativeCoordinates;
        public Coordinate[][] absoluteRelativeCoordinates;
        public Coordinate[][][] rotatedAbsoluteRelativeCoordinates;


        public ScannerCoordinates(Coordinate[] coordinates)
        {
            this.coordinates = coordinates;

            CalculateRelativePositions();
            CalculateAbsoluteRelativePositions();
            CalculateRotatedAbsoluteRelativePositions();
        }

        public void CalculateRelativePositions()
        {
            relativeCoordinates = new Coordinate[coordinates.Length][];

            for (int @base = 0; @base < coordinates.Length; @base++)
            {
                var baseProbe = coordinates[@base];

                relativeCoordinates[@base] = new Coordinate[coordinates.Length];
                for (int target = 0; target < coordinates.Length; target++)
                {
                    var targetProbe = coordinates[target];

                    relativeCoordinates[@base][target] =
                        new Coordinate(targetProbe.X - baseProbe.X, targetProbe.Y - baseProbe.Y, targetProbe.Z.Value - baseProbe.Z.Value);
                }
            }
        }

        public void CalculateAbsoluteRelativePositions()
        {
            absoluteRelativeCoordinates = new Coordinate[coordinates.Length][];

            for (int @base = 0; @base < coordinates.Length; @base++)
            {
                absoluteRelativeCoordinates[@base] = new Coordinate[coordinates.Length];
                for (int target = 0; target < coordinates.Length; target++)
                {
                    var baseProbe = relativeCoordinates[@base][target];

                    absoluteRelativeCoordinates[@base][target] =
                        new Coordinate(Math.Abs(baseProbe.X), Math.Abs(baseProbe.Y), Math.Abs(baseProbe.Z.Value));
                }
            }
        }

        public void CalculateRotatedAbsoluteRelativePositions()
        {
            rotatedAbsoluteRelativeCoordinates = new Coordinate[coordinates.Length][][];

            for (int n = 0; n < coordinates.Length; n++)
            {
                rotatedAbsoluteRelativeCoordinates[n] = new Coordinate[6][];
                for (int i = 0; i < 6; i++)
                {
                    rotatedAbsoluteRelativeCoordinates[n][i] = new Coordinate[coordinates.Length];
                }
            }

            for (int @base = 0; @base < coordinates.Length; @base++)
            {
                for (int target = 0; target < coordinates.Length; target++)
                {
                    var baseProbe = absoluteRelativeCoordinates[@base][target];

                    rotatedAbsoluteRelativeCoordinates[@base][0][target] = new Coordinate(baseProbe.X, baseProbe.Y, baseProbe.Z);
                    rotatedAbsoluteRelativeCoordinates[@base][1][target] = new Coordinate(baseProbe.Z.Value, baseProbe.X, baseProbe.Y);
                    rotatedAbsoluteRelativeCoordinates[@base][2][target] = new Coordinate(baseProbe.Y, baseProbe.Z.Value, baseProbe.X);
                    
                    rotatedAbsoluteRelativeCoordinates[@base][3][target] = new Coordinate(baseProbe.X, baseProbe.Z.Value, baseProbe.Y);
                    rotatedAbsoluteRelativeCoordinates[@base][4][target] = new Coordinate(baseProbe.Z.Value, baseProbe.Y, baseProbe.X);
                    rotatedAbsoluteRelativeCoordinates[@base][5][target] = new Coordinate(baseProbe.Y, baseProbe.X, baseProbe.Z.Value);
                }
            }
        }
    }
}
