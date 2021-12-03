using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent04
{
    public class Solution : ISolution
    {
        public IEnumerable<string> commonNames;

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input).ToArray();

            var inputParser = new InputParser<int, int, string, string>("num)    num stars  name");

            List<List<string>> names = new List<List<string>>();
            foreach (var lines in blocks) 
            {
                List<string> blockNames = new List<string>();
                foreach( var line in lines)
                {
                    var (_, _, _, name) = inputParser.Parse(line);

                    blockNames.Add(name);
                };
                names.Add(blockNames);
            }

            commonNames = names[0].Intersect(names[1]);
        }
        public Solution() : this("Input.txt") { }


        public object GetResult1()
        {
            return string.Join(Environment.NewLine, commonNames);
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
