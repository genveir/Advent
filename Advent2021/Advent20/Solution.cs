using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent20
{
    public class Solution : ISolution
    {
        public State dark, light;

        public State currentState;

        public Solution(string input)
        {
            var lines = Input.GetBlockLines(input).ToArray();

            dark = new State(false);
            light = new State(true);

            dark.otherState = light;
            light.otherState = dark;

            dark.enhancement = lines[0][0].Select(c => c == '#').ToArray();
            light.enhancement = dark.enhancement.DeepCopy();

            for(int y = 0; y < lines[1].Length; y++)
            {
                for (int x = 0; x < lines[1][0].Length; x++)
                {
                    var c = lines[1][y][x] == '#';
                    dark.AddToFront(key(x, y), c);
                }
            }

            currentState = dark;
        }
        public Solution() : this("Input.txt") { }

        public static long key(long x, long y) => x * xMult + y;
        public static long xMult = 1000000000;

        public class State {
            public bool[] enhancement;
            public Dictionary<long, bool> front;
            public bool defaultColor;

            public State otherState;

            public State(bool defaultColor)
            {
                this.defaultColor = defaultColor;
                this.front = new Dictionary<long, bool>();
            }

            public State DoStep()
            {
                otherState.front.Clear();

                foreach (var coord in front.Keys)
                {
                    var value = Convert(Lookaround(coord));

                    var toSet = enhancement[value];

                    otherState.AddToFront(coord, toSet);
                }

                return otherState;
            }

            public void AddToFront(long coord, bool toSet)
            {
                var isActive = toSet != defaultColor;

                if (!isActive) return;

                var around = Lookaround(coord);

                if (around[0] == null) front.Add(coord - 1 - xMult, defaultColor);
                if (around[1] == null) front.Add(coord - 1, defaultColor);
                if (around[2] == null) front.Add(coord - 1 + xMult, defaultColor);

                if (around[3] == null) front.Add(coord - xMult, defaultColor);
                front[coord] = toSet;
                if (around[5] == null) front.Add(coord + xMult, defaultColor);

                if (around[6] == null) front.Add(coord + 1 - xMult, defaultColor);
                if (around[7] == null) front.Add(coord + 1, defaultColor);
                if (around[8] == null) front.Add(coord + 1 + xMult, defaultColor);
            }

            public bool?[] Lookaround(long coord)
            {
                bool[] fromSet = new bool[9];
                bool[] isSet = new bool[9];

                isSet[0] = front.TryGetValue(coord - 1 - xMult, out fromSet[0]);
                isSet[1] = front.TryGetValue(coord - 1, out fromSet[1]);
                isSet[2] = front.TryGetValue(coord - 1 + xMult, out fromSet[2]);

                isSet[3] = front.TryGetValue(coord - xMult, out fromSet[3]);
                isSet[4] = front.TryGetValue(coord, out fromSet[4]);
                isSet[5] = front.TryGetValue(coord + xMult, out fromSet[5]);

                isSet[6] = front.TryGetValue(coord + 1 - xMult, out fromSet[6]);
                isSet[7] = front.TryGetValue(coord + 1, out fromSet[7]);
                isSet[8] = front.TryGetValue(coord + 1 + xMult, out fromSet[8]);

                var result = new bool?[9];
                for (int n = 0; n < 9; n++) result[n] = isSet[n] ? fromSet[n] : null;

                return result;
            }

            public long Convert(bool?[] bits)
            {
                long result = 0;
                if (bits[8] ?? defaultColor) result += 1;
                if (bits[7] ?? defaultColor) result += 2;
                if (bits[6] ?? defaultColor) result += 4;
                if (bits[5] ?? defaultColor) result += 8;
                if (bits[4] ?? defaultColor) result += 16;
                if (bits[3] ?? defaultColor) result += 32;
                if (bits[2] ?? defaultColor) result += 64;
                if (bits[1] ?? defaultColor) result += 128;
                if (bits[0] ?? defaultColor) result += 256;

                return result;
            }
        }

        
        public void DoStep()
        {
            currentState = currentState.DoStep();
        }

        public object GetResult1()
        {
            // not 714, not 10009

            DoStep();
            DoStep();

            var numLight = dark.front.Where(f => f.Value).Count();

            return numLight;
        }

        public object GetResult2()
        {
            for (int n = 0; n < 48; n++) DoStep();

            var numLight = dark.front.Where(f => f.Value).Count();

            return numLight;
        }
    }
}
