using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent25
{
    public class Solution : ISolution
    {
        public Executor executor;

        public Solution(Input.InputMode inputMode, string input)
        {
            var startProg = Input.GetInputLines(inputMode, input, new char[] { ',' }).ToArray();
            executor = new Executor(startProg);
            executor.Execute();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public string GetResult1()
        {
            executor.AddAscInput(@"west
south
take pointer
south
take prime number
west
take coin
east
north
north
east
east
south
south
take space heater
south
take astrolabe
north
north
north
north
take wreath
north
west
take dehydrated water
north
east");
            executor.GetAscOutput();

            var items = new string[] { "pointer", "prime number", "coin", "space heater", "astrolabe", "wreath", "dehydrated water" };

            int maxTry = (int)Math.Pow(2, items.Length);
            var carrying = new List<string>(items);

            for (int toTry = 0; toTry <= maxTry; toTry++)
            {
                foreach (var item in carrying)
                {
                    Drop(item);
                }
                carrying = new List<string>();
                string binary = Convert.ToString(toTry, 2).PadLeft(items.Length, '0');

                for (int n = 0; n < items.Length; n++)
                {
                    if (binary.Length > n && binary[n] == '1')
                    {
                        Take(items[n]);
                        carrying.Add(items[n]);
                    }
                }

                South();

                var output = executor.GetAscOutput();

                if (!output.Contains("Alert! Droids on this ship are heavier than the detected value!") &&
                 !output.Contains("Alert! Droids on this ship are lighter than the detected value!")) return output;
            }

            return "no result";
        }

        public void Drop(string item)
        {
            executor.AddAscInput("drop " + item);
        }

        public void Take(string item)
        {
            executor.AddAscInput("take " + item);
        }

        public void South()
        {
            executor.AddAscInput("south");
        }

        public string GetResult2()
        {
            return "";
        }
    }
}