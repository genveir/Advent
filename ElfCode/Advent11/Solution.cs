using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Advent11
{
    class Solution : ISolution
    {
        private static int gridSerialNumber;

        private class Grid
        {
            public int[,,] power;
            public XYZCoord mostPowerCoord;
            public int mostPower;

            public Grid(int maxZ)
            {
                power = new int[300, 300, maxZ];

                for (int x = 0; x < 300; x++)
                {
                    for (int y = 0; y < 300; y++)
                    {
                        power[x, y, 0] = CalcPower(x + 1, y + 1);
                    }
                }
            }

            private int CalcPower(int x, int y)
            {
                int rackId = x + 10;
                var Power = y * rackId;
                Power = Power + gridSerialNumber;
                Power = Power * rackId;
                Power = (Power / 100) % 10;
                Power = Power - 5;

                return Power;
            }

            public void AggregatePowerP1()
            {
                for (int z = 0; z < 3; z++)
                { 
                    for (int x = 0; x < 300; x++)
                    { 
                        for (int y = 0; y < 300; y++)
                        {
                            CalcAggregate(x, y, z);
                        }
                    }
                }
            }

            public void AggregatePowerP2()
            {
                for (int z = 0; z < 300; z++)
                {
                    if (z % 100 == 0) Console.Write("x");
                    else if (z % 10 == 0) Console.Write(".");

                    for (int x = 0; x < 300; x++)
                    {
                        for (int y = 0; y < 300; y++)
                        {
                            CalcAggregate(x, y, z);
                        }
                    }
                }
                Console.WriteLine();
            }

            public void CalcAggregate(int x, int y, int z)
            {
                if (z > x || z > y) return;

                if (z == 0) return;
                else
                {
                    power[x, y, z] = power[x - 1, y - 1, z - 1];
                    for (int X = x - z; X < x; X++)
                    {
                        power[x, y, z] += power[X, y, 0];
                    }
                    for (int Y = y - z; Y < y; Y++)
                    {
                        power[x, y, z] += power[x, Y, 0];
                    }
                    power[x, y, z] += power[x, y, 0];
                }

                if (power[x, y, z] > mostPower)
                {
                    mostPower = power[x, y, z];
                    mostPowerCoord = new XYZCoord(x + 1, y + 1, z + 1);
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

        public void WriteResult()
        {
            //Tests();

            gridSerialNumber = 9798;

            var grid = new Grid(3);
            grid.AggregatePowerP1();
            // oeps, grid geeft bottomright coord en je moet topleft hebben
            Console.WriteLine((grid.mostPowerCoord.X - grid.mostPowerCoord.Z + 1) + "," + (grid.mostPowerCoord.Y - grid.mostPowerCoord.Z + 1) + "," + grid.mostPowerCoord.Z);

            var grid2 = new Grid(300);
            grid2.AggregatePowerP2();
            Console.WriteLine((grid2.mostPowerCoord.X - grid2.mostPowerCoord.Z + 1) + "," + (grid2.mostPowerCoord.Y - grid2.mostPowerCoord.Z + 1) + "," + grid2.mostPowerCoord.Z);
        }

        private void Tests()
        {
            gridSerialNumber = 18;
            var g0 = new Grid(300);
            g0.AggregatePowerP2();
            Console.WriteLine((g0.mostPowerCoord.X - g0.mostPowerCoord.Z + 1) + "," + (g0.mostPowerCoord.Y - g0.mostPowerCoord.Z + 1) + "," + g0.mostPowerCoord.Z);


            gridSerialNumber = 42;
            var g1 = new Grid(300);
            g1.AggregatePowerP2();
            Console.WriteLine((g1.mostPowerCoord.X - g1.mostPowerCoord.Z + 1) + "," + (g1.mostPowerCoord.Y - g1.mostPowerCoord.Z + 1) + "," + g1.mostPowerCoord.Z + 1);

            Console.WriteLine(g0.mostPowerCoord + " should be 90,269,16 with power " + g0.mostPower + " which should be 113");
            Console.WriteLine(g1.mostPowerCoord + " should be 232,251,12 with power " + g1.mostPower + " which should be 119");
        }
    }
}
