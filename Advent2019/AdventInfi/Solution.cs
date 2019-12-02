using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.AdventInfi
{
    public class Solution : ISolution
    {
        static (int X, int Y) kerstman;

        Dictionary<int, int> flats;
        List<Sprong> sprongen;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input, new char[] { ' ', ':', 's', '}', '{', '\\', '\n', '\r', '\"' }).ToArray();

            flats = Flat.Parse(lines[1].Split(new char[] { ',', ']', '[' }, StringSplitOptions.RemoveEmptyEntries));
            sprongen = Sprong.Parse(lines[3].Split(new char[] { ',', ']', '[' }, StringSplitOptions.RemoveEmptyEntries));
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class Flat
        {
            public static Dictionary<int, int> Parse(string[] lines)
            {
                var flats = new Dictionary<int, int>();

                for (int n = 0; n < lines.Length; n += 2)
                {
                    int X = int.Parse(lines[n]);
                    int Y = int.Parse(lines[n + 1]);

                    if (n == 0) kerstman = (X, Y);

                    flats.Add(X, Y);
                }

                return flats;
            }
        }

        private class Sprong
        {
            public int X;
            public int Y;

            public static List<Sprong> Parse(string[] lines)
            {
                var sprongen = new List<Sprong>();

                for (int n = 0; n < lines.Length; n += 2)
                {
                    var s = new Sprong()
                    {
                        X = int.Parse(lines[n]),
                        Y = int.Parse(lines[n + 1])
                    };

                    sprongen.Add(s);
                }

                return sprongen;
            }
        }

        public string GetResult1()
        {
            for (int n = 0; n < sprongen.Count; n++)
            {
                kerstman.X++;
                kerstman.X += sprongen[n].X;
                kerstman.Y += sprongen[n].Y;

                if (flats.ContainsKey(kerstman.X) && flats[kerstman.X] <= kerstman.Y) kerstman.Y = flats[kerstman.X];
                else return (n + 1).ToString();
            }

            return "0";
        }

        public string GetResult2()
        {


            return "";
        }
    }
}
