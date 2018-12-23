using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.Advent23
{
    class Voxel
    {
        public long X;
        public long Y;
        public long Z;

        public bool IsInRangeOf(NanoBot bot)
        {
            var distance = DistanceTo(bot);

            return distance <= bot.SignalStrength;
        }

        public long DistanceTo(Voxel voxel)
        {
            return Math.Abs(X - voxel.X) + Math.Abs(Y - voxel.Y) + Math.Abs(Z - voxel.Z);
        }

        public IEnumerable<Voxel> Inflate(int factor)
        {
            var voxels = new List<Voxel>();

            for (int z = 0; z < factor; z++)
            {
                for (int y = 0; y < factor; y++)
                {
                    for (int x = 0; x < factor; x++)
                    {
                        voxels.Add(new Voxel()
                        {
                            X = X * factor + x ,
                            Y = Y * factor + y ,
                            Z = Z * factor + z
                        });
                    }
                }
            }

            return voxels;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
    }

    class NanoBot : Voxel
    {
        public long SignalStrength;

        public static NanoBot FromString(string input)
        {
            var parts = input.Replace("\r", "").Split(new char[] { '<', '>', ',', '=' }, StringSplitOptions.RemoveEmptyEntries);

            return new NanoBot()
            {
                X = long.Parse(parts[1]),
                Y = long.Parse(parts[2]),
                Z = long.Parse(parts[3]),
                SignalStrength = long.Parse(parts[5])
            };
        }

        public NanoBot ReduceByFactor(int factor)
        {
            return new NanoBot()
            {
                X = X / factor,
                Y = Y / factor,
                Z = Z / factor,
                SignalStrength = (long)Math.Ceiling((double)SignalStrength / (double)factor)
            };
        }

        public override string ToString()
        {
            return base.ToString() + "{" + SignalStrength + "}";
        }
    }
}
