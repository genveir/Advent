using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent
{
    class Advent1
    {
        private List<long> GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent.Input.Advent" + adventNum + "Input.txt");

            var vals = new List<long>();

            using (var txt = new StreamReader(input))
            {
                while(!txt.EndOfStream)
                    vals.Add(long.Parse(txt.ReadLine().Replace("+", "")));
            }

            return vals;
        }

        public long FirstDouble()
        {
            var vals = GetInput();

            var previous = new List<long>();
            var sum = 0L;
            while (true)
            {
                for (int n = 0; n < vals.Count; n++)
                {
                    previous.Add(sum);

                    sum += vals[n];

                    if (previous.Contains(sum)) return sum;
                }
            }
        }

        public long Calc()
        {
            var input = GetInput();

            return input.Sum();
        }
    }
}