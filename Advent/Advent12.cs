using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent
{
    class Advent12
    {
        PlantLine current;

        static Dictionary<int, bool> rules;
        static int[] powers;

        void ParseInput()
        {
            string resourceName = "Advent.Input.Advent12Input.txt";
            //resourceName = "Advent.Input.a12test.txt";
            var input = typeof(Program).Assembly.GetManifestResourceStream(resourceName);

            powers = new int[5];
            powers[0] = 1;
            powers[1] = 2;
            powers[2] = 4;
            powers[3] = 8;
            powers[4] = 16;

            using (var txt = new StreamReader(input))
            {
                var initialString = txt.ReadLine().Split(' ')[2];
                var initial = initialString.Select(c => c == '#').ToArray();
                current = new PlantLine(initial, 0);

                txt.ReadLine();

                rules = new Dictionary<int, bool>();
                for (int n = 0; n < 32; n++)
                {
                    rules.Add(n, false);
                }

                while (!txt.EndOfStream)
                {
                    var line = txt.ReadLine();
                    if (line[9] == '#')
                    {
                        int ruleKey = 0;
                        for (int n = 0; n < 5; n++)
                        {
                            if (line[n] == '#') ruleKey += powers[n];
                        }
                        rules[ruleKey] = true;
                    }
                }
            }
        }
        private class PlantLine
        {
            public HashSet<int> plants;
            public int minIndex = int.MinValue;
            public int maxIndex;
            public int startInt;

            public int Value
            {
                get
                {
                    int result = 0;
                    foreach (var plant in plants) result += plant - minIndex + startInt;
                    return result;
                }
            }

            public PlantLine(bool[] input, int startInt)
            {
                plants = new HashSet<int>();

                for (int n = 0; n < input.Count(); n++)
                {
                    if (input[n])
                    {
                        plants.Add(n);
                        if (minIndex == int.MinValue)
                        {
                            minIndex = n;
                            this.startInt = startInt + n;
                        }
                        maxIndex = n;
                    }
                }
            }

            public PlantLine GetNext()
            {
                bool[] nextState = new bool[maxIndex - minIndex + 5];
                for (int n = minIndex - 2; n <= maxIndex + 2; n++)
                {
                    int ruleKey = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (plants.Contains(n + i - 2)) ruleKey += powers[i];
                    }
                    nextState[n - minIndex + 2] = rules[ruleKey];
                }

                return new PlantLine(nextState, startInt - 2);
            }
        }

        private void TakeStep()
        {
            current = current.GetNext();
        }

        private void Print()
        {
            for (int n = current.minIndex; n <= current.maxIndex; n++)
            {
                Console.Write((current.plants.Contains(n) ? '#' : '.'));
            }
            Console.WriteLine();
        }

        public void WriteResult()
        {
            ParseInput();

            Print();
            for (int n = 0; n < 20; n++)
            {
                Console.Write(current.startInt.ToString().PadRight(2) + ": ");
                TakeStep();
                Print();
            }
            var result = current.Value;
            Console.WriteLine("sum of all plants is " + result);

            int lastValue = 0;
            int lastDiff = 0;
            int previousDiff = 0;
            int generation = 20;
            while (true) 
            {
                TakeStep();
                generation++;
                var diff = (current.Value - lastValue);
                if (diff == lastDiff && diff == previousDiff) break; // zo is het wel repeating genoeg

                lastDiff = diff;
                previousDiff = lastDiff;
                lastValue = current.Value;
            }
            
            var curVal = current.Value;
            long genDiff = 50L * 1000L * 1000L * 1000L - generation;
            var longtime = curVal + lastDiff * genDiff;

            using (var writer = new StreamWriter(new FileStream(@"d:\temp\output.txt", FileMode.Create)))
            {
                writer.WriteLine(longtime);
                Console.WriteLine(longtime);
            }
        }
    }
}
