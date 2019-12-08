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

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            digits = lines.Single().ToCharArray().Select(c => c - 48).ToArray();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public string GetResult1()
        {
            int numLayers = digits.Length / (25 * 6);

            int minZeroes = int.MaxValue;
            int returnValue = -1;
            for (int n = 0; n < numLayers; n++)
            {
                var types = new int[3];

                int start = n * 25 * 6;
                for (int p =0; p < 25 * 6; p++)
                {
                    types[digits[start + p]]++;
                }
                if (types[0] < minZeroes)
                {
                    minZeroes = types[0];
                    returnValue = types[1] * types[2];
                }
            }

            return returnValue.ToString();
        }

        public string GetResult2()
        {
            var output = new StringBuilder();
            output.AppendLine();

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
                            if (digits[position] == 1) output.Append("X");
                            if (digits[position] == 0) output.Append(" ");
                            break;
                        }
                    }
                }
                output.AppendLine();
            }

            return output.ToString();
        }
    }
}
