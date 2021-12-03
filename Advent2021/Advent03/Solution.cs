using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent03
{
    public class Solution : ISolution
    {
        public List<List<int>> numbers;

        public Solution(string input)
        {
            numbers = Input
                .GetInputLines(input)
                .Select(s => s.AsDigits())
                .Pivot();
        }
        public Solution() : this("Input.txt") { }

        public static int[] GetMatch(IEnumerable<IEnumerable<int>> input, int match) =>
            input.Select(inp => (inp.Sum() >= inp.Count() / 2.0d) ? match : 1 - match).ToArray();

        public static List<int> GetGasMatch(IEnumerable<IEnumerable<int>> input, int match, int digit)
        {
            var gases = input.Pivot();

            var matchNum = GetMatch(input, match)[digit];
            var nextSet = gases.Where(gas => gas[digit] == matchNum);

            return nextSet.Count() == 1 ? nextSet.Single() : GetGasMatch(nextSet.Pivot(), match, digit + 1);
        }

        public long CalculateResult(Func<int, IEnumerable<int>> matchFunc) =>
            Enumerable.Range(0, 2)
                .Select(matchFunc)
                .Select(ints => Convert.ToInt64(string.Join("", ints), 2))
                .Aggregate((a, b) => a * b);
            
        public object GetResult1() => CalculateResult(m => GetMatch(numbers, m));
        public object GetResult2() => CalculateResult(m => GetGasMatch(numbers, m, 0));
    }
}
