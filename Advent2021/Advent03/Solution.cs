using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent03
{
    public class Solution : ISolution
    {
        string[] numbers;

        public Solution(string input)
        {
            numbers = Input.GetInputLines(input).ToArray();
            
        }
        public Solution() : this("Input.txt") { }

        public long P1()
        {
            char[] gamma = GetGamma(numbers);
            char[] episilon = GetEpsilon(numbers);
            
            string gammaString = new string(gamma);
            string epsilonString = new string(episilon);

            return Convert.ToInt64(gammaString, 2) * Convert.ToInt64(epsilonString, 2);
        }

        public char[] GetGamma(IEnumerable<string> input)
        {
            var numbers = input.ToArray();

            char[] gamma = new char[numbers[0].Length];
            for (int n = 0; n < numbers[0].Length; n++)
            {
                int numOnes = 0;
                int numZeros = 0;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i][n] == '1') numOnes++;
                    else numZeros++;
                }

                gamma[n] = (numOnes > numZeros) ? '1' : '0';
                if (numOnes == numZeros) gamma[n] = '1';
            }

            return gamma;
        }

        public char[] GetEpsilon(IEnumerable<string> input)
        {
            var numbers = input.ToArray();

            char[] epsilon = new char[numbers[0].Length];
            for (int n = 0; n < numbers[0].Length; n++)
            {
                int numOnes = 0;
                int numZeros = 0;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i][n] == '0') numOnes++;
                    else numZeros++;
                }

                epsilon[n] = (numOnes > numZeros) ? '1' : '0';
                if (numOnes == numZeros) epsilon[n] = '0';
            }

            return epsilon;
        }

        public long P2()
        {
            List<string> oxy = numbers.ToList();
            List<string> co2 = numbers.ToList();

            for (int n = 0; n < oxy[0].Length; n++)
            {
                char[] gamma = GetGamma(oxy);
                char[] episilon = GetEpsilon(co2);

                List<string> nextOxy = new List<string>();
                List<string> nextCo2 = new List<string>();

                if (oxy.Count == 1)
                {
                    nextOxy.Add(oxy.Single());
                }
                else
                {
                    for (int i = 0; i < oxy.Count; i++)
                    {
                        if (oxy[i][n] == gamma[n]) nextOxy.Add(oxy[i]);
                    }
                }
                oxy = nextOxy;

                if (co2.Count == 1)
                {
                    nextCo2.Add(co2.Single());
                }
                else
                {
                    for (int i = 0; i < co2.Count; i++)
                    {
                        if (co2[i][n] == episilon[n]) nextCo2.Add(co2[i]);
                    }
                }
                co2 = nextCo2;
            }

            var oxyString = oxy.Single();
            var co2String = co2.Single();

            return Convert.ToInt64(oxyString, 2) * Convert.ToInt64(co2String, 2);
        }

        public object GetResult1()
        {
            return P1();
        }

        public object GetResult2()
        {
            return P2();
        }
    }
}
