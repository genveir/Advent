using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent4
{
    public class Solution : ISolution
    {
        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public bool Test(int num, bool is2)
        {
            var digits = ('a' + num.ToString() + "a").ToCharArray();

            bool hasdouble = false;
            for (int n = 1; n < digits.Length - 2; n++)
            {
                int dign0 = digits[n - 1]- 48;
                int dign = digits[n] - 48;
                int dign1 = digits[n + 1] - 48;
                int dign2 = digits[n + 2] - 48;

                var before = dign != dign0 || !is2;
                var areeq = dign == dign1;
                var arenteq = dign != dign2;

                if (before && areeq && arenteq) hasdouble = true;
                if (digits[n] > digits[n + 1]) return false;
            }

            return hasdouble;
        }

        public int low = 145852;
        public int high = 616942;

        public string GetResult1()
        {
            int bla = 0;

            int num = low;
            while (num <= high)
            {
                if (Test(num, false)) bla++;
                num++;
            }

            return bla.ToString();
        }

        public string GetResult2()
        {
            int bla = 0;

            int num = low;
            while (num <= high)
            {
                if (Test(num, true)) bla++;
                num++;
            }

            // not 1517
            return bla.ToString();
        }
    }
}
