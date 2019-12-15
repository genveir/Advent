using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2018.Advent14
{
    class Solution : ISolution
    {
        private List<int> Input = new List<int>() { 3, 7 };

        private IEnumerable<int> Step(List<int> input, int[] cursors)
        {
            var sum = cursors.Sum(c => input[c]);

            var addedDigits = GetDigits(sum);

            input.AddRange(addedDigits);

            for (int c = 0; c < cursors.Length; c++) cursors[c] = (cursors[c] + input[cursors[c]] + 1) % input.Count;

            return addedDigits;
        }

        private IEnumerable<int> GetDigits(int input)
        {
            var digits = new List<int>();
            if (input == 0) digits.Add(0);
            while (input > 0)
            {
                digits.Add(input % 10);
                input = input / 10;
            }
            digits.Reverse();
            return digits;
        }

        private void Print(List<int> input, int[] cursors)
        {
            for (int n = 0; n < input.Count; n++)
            {
                Console.Write((cursors[0] == n) ? "(" : (cursors[1] == n) ? "[" : " ");
                Console.Write(input[n]);
                Console.Write((cursors[0] == n) ? ")" : (cursors[1] == n) ? "]" : " ");
            }
            Console.WriteLine();
        }

        private List<int> GenerateTo(int listCount, bool print)
        {
            var cursors = new int[2] { 0, 1 };
            var output = Input.GetRange(0, Input.Count).ToList();

            if (print) Print(output, cursors);
            while (output.Count < listCount)
            {
                Step(output, cursors);
                if (print) Print(output, cursors);
            }

            return output;
        }

        private List<int> GenerateUntil(List<int> condition)
        {
            var cursors = new int[2] { 0, 1 };
            var output = Input.GetRange(0, Input.Count).ToList();

            var conditionCount = condition.Count;
            var conditionCursor = 0;

            while (true)
            {
                var digitsAdded = Step(output, cursors);
                foreach (var digit in digitsAdded)
                {
                    if (digit == condition[conditionCursor])
                    {
                        conditionCursor++;
                        if (conditionCursor == condition.Count) return output;
                    }
                    else conditionCursor = digit == condition[0] ? 1 : 0;
                }
            }
        }

        private string GetResult1(int get10After, bool print = false)
        {
            var output = GenerateTo(get10After + 10, print);

            var builder = new StringBuilder();
            foreach (var i in output.GetRange(get10After, 10)) builder.Append(i);
            return builder.ToString();
        }

        private int GetResult2(string condition)
        {
            var actualCondition = condition.Select(c => int.Parse(c.ToString())).ToList();
            var resultingList = GenerateUntil(actualCondition);

            var count = resultingList.Count - actualCondition.Count;

            if (!resultingList.TakeLast(actualCondition.Count).SequenceEqual(actualCondition)) count--;

            return count;
        }

        public void WriteResult()
        {
            Tests();

            Console.WriteLine("part1: " + GetResult1(286051));
            Console.WriteLine("part2: " + GetResult2("286051"));
        }

        private void Tests()
        {
            Console.WriteLine("5158916779 <= " + GetResult1(9));
            Console.WriteLine("0124515891 <= " + GetResult1(5));
            Console.WriteLine("9251071085 <= " + GetResult1(18));
            Console.WriteLine("5941429882 <= " + GetResult1(2018));
            Console.WriteLine();
            Console.WriteLine(9 + " <= " + GetResult2("51589"));
            Console.WriteLine(5 + " <= " + GetResult2("01245"));
            Console.WriteLine(18 + " <= " + GetResult2("92510"));
            Console.WriteLine(2018 + " <= " + GetResult2("59414"));
            Console.WriteLine();
        }
    }
}
