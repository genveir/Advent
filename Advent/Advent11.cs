using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent
{
    class Advent11
    {
        private static int gridSerialNumber;

        private class Grid
        {
            public Cell[,,] cells;
            public XYZCoord mostPowerCoord;
            public int mostPower;

            public Grid()
            {
                cells = new Cell[300, 300, 300];

                for (int x = 0; x < 300; x++)
                {
                    for (int y = 0; y < 300; y++)
                    {
                        for (int z = 0; z < 300; z++)
                        {
                            cells[x, y, z] = new Cell(x + 1, y + 1, z);
                        }
                    }
                }
            }

            public void AggregatePowerP1()
            {
                for (int x = 0; x < 300; x++)
                {
                    for (int y = 0; y < 300; y++)
                    {
                        CalcAggregate(x, y, 3);
                    }
                }
            }

            public void CalcAggregate(int x, int y, int z)
            {
                var cell = cells[x, y, 0];
                for (int xOff = -z + 1; xOff <= 0; xOff++)
                {
                    for (int yOff = -z + 1; yOff <= 0; yOff++)
                    {
                        SafeAdd(x + xOff, y + yOff, z, cell.Power);
                    }
                }
            }

            private void SafeAdd(int x, int y, int z, int Power)
            {
                if (x < 0 || y < 0 || x > cells.GetUpperBound(0) || y > cells.GetUpperBound(1)) return;
                cells[x, y, z].PowerAggregate += Power;
                if (cells[x, y, z].PowerAggregate > mostPower)
                {
                    mostPower = cells[x, y, z].PowerAggregate;
                    mostPowerCoord = new XYZCoord(x + 1, y + 1, z);
                }
            }
        }

        private class XYZCoord
        {
            public int X, Y, Z;
            public XYZCoord(int x, int y, int z) { X = x; Y = y; Z = z; }

            public override string ToString()
            {
                return X + "," + Y + "," + Z;
            }
        }

        private class Cell
        {
            public int Power { get; set; }
            public int PowerAggregate { get; set; }

            public Cell(int x, int y, int z)
            {
                if (z == 0)
                {
                    int rackId = x + 10;
                    Power = y * rackId;
                    Power = Power + gridSerialNumber;
                    Power = Power * rackId;
                    Power = (Power / 100) % 10;
                    Power = Power - 5;
                }
            }
        }

        public void WriteResult()
        {
            gridSerialNumber = 9798;
            var grid = new Grid();
            grid.AggregatePowerP1();

            var mostPower = grid.mostPowerCoord;

            Console.WriteLine(grid.mostPowerCoord);
        }
    }
}
