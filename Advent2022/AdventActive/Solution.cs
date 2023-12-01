using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.AdventActive
{
    public class Solution : ISolution
    {
        public List<int> inputs;

        public Coordinate[] grid;
        public long[] heights = new long[7];

        public Solution(string input)
        {
            var chars = Input.GetInput(input).ToArray();

            inputs = chars.Select(c => c switch
            {
                '<' => -1,
                '>' => 1,
                _ => throw new NotSupportedException("unexpected things gonna unexpect")
            }).ToList();
        }
        public Solution() : this("Input.txt") { }



        private void Simulate(long numberOfBlocks)
        {

        }

        public object GetResult1()
        {
            Simulate(2022);

            return heights.Max();
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
