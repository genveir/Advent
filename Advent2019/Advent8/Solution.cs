using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent8
{
    public class Solution : ISolution
    {
        int[] digits;
        EncodedImage image;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            digits = lines.Single().ToCharArray().Select(c => c - 48).ToArray(); ;
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        private class EncodedImage
        {
            
        }

        public string GetResult1()
        {
            int numLayers = digits.Length / (25 * 6);

            var layers = new layerdata[numLayers];
            for (int n = 0; n < numLayers; n++)
            {
                layers[n] = new layerdata();
                int start = n * 25 * 6;
                for (int p =0; p < 25 * 6; p++)
                {
                    switch(digits[start + p])
                    {
                        case 0: layers[n].zeroes++; break;
                        case 1: layers[n].ones++; break;
                        case 2: layers[n].twos++; break;
                    }
                }
            }

            var min = layers.Min(l => l.zeroes);
            var minLayer = layers.Where(l => l.zeroes == min).Single();
            return (minLayer.ones * minLayer.twos).ToString();
        }

        struct layerdata
        {
            public int zeroes;
            public int ones;
            public int twos;
        }

        public string GetResult2()
        {
            int numLayers = digits.Length / (25 * 6);
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    for (int layer = 0; layer < numLayers; layer++)
                    {
                        int position = layer * 25 * 6 + x + y * 25;
                        if (digits[position] == 2) continue;
                        else
                        {
                            if (digits[position] == 1) Console.Write("X");
                            if (digits[position] == 0) Console.Write(" ");
                            break;
                        }
                    }
                }
                Console.WriteLine();
            }

            return "";
        }
    }
}
