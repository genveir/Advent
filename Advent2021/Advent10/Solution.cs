using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent10
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input);

            modules = lines.Select(l => new ParsedInput(l)).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public char[] input;

            [ComplexParserConstructor]
            public ParsedInput(string input)
            {
                this.input = input.ToCharArray();
            }

            static ParsedInput()
            {
                corruptionMap = new long[255][];

                corruptionMap['('] = new long[255];
                corruptionMap['['] = new long[255];
                corruptionMap['{'] = new long[255];
                corruptionMap['<'] = new long[255];

                corruptionMap[')'] = new long[255];
                corruptionMap[')']['['] = 3;
                corruptionMap[')']['{'] = 3;
                corruptionMap[')']['<'] = 3;

                corruptionMap[']'] = new long[255];
                corruptionMap[']']['('] = 57;
                corruptionMap[']']['{'] = 57;
                corruptionMap[']']['<'] = 57;

                corruptionMap['}'] = new long[255];
                corruptionMap['}']['('] = 1197;
                corruptionMap['}']['['] = 1197;
                corruptionMap['}']['<'] = 1197;

                corruptionMap['>'] = new long[255];
                corruptionMap['>']['('] = 25137;
                corruptionMap['>']['['] = 25137;
                corruptionMap['>']['{'] = 25137;

                numPops = new long[255];
                numPops['('] = 0;
                numPops[')'] = 2;
                numPops['['] = 0;
                numPops[']'] = 2;
                numPops['{'] = 0;
                numPops['}'] = 2;
                numPops['<'] = 0;
                numPops['>'] = 2;

                scoreMap = new long[255];
                scoreMap['('] = 1;
                scoreMap['['] = 2;
                scoreMap['{'] = 3;
                scoreMap['<'] = 4;
            }

            public static long[][] corruptionMap;
            private static long[] numPops;
            private static long[] scoreMap;

            public long CorruptionScore => _corruptionScores.corruption;

            public (Stack<char> open, long corruption) _corruptionScores;
            public void CalculateCorruptionScore()
            {
                var opened = new Stack<char>(50);

                long acc = 0;
                long mult = 1;
                for (int index = 0; index < input.Length; index++)
                {
                    var c = input[index];

                    opened.Push(c);

                    char toCheck = (char)0;
                    for (int n = 0; n < numPops[c]; n++) toCheck = opened.Pop();

                    var corruption = corruptionMap[c][toCheck];

                    acc = acc + (corruption * mult);
                    mult = 1 - (acc & 1);
                }

                _corruptionScores = (opened, acc);
            }

            public long Score { get; set; }
            public void CalculateCompletionScore()
            {
                (var opened, var corruption) = _corruptionScores;

                var mult = 1 - (corruption & 1);

                Score = 0;
                while (opened.Count > 0)
                {
                    Score *= 5;
                    var toClose = opened.Pop();

                    Score += scoreMap[toClose];
                }

                Score = Score * mult;
            }
        }

        bool p1Run = false;
        public object GetResult1()
        {
            Parallel.ForEach(modules, m => m.CalculateCorruptionScore());

            p1Run = true;
            return modules.Sum(m => m.CorruptionScore);
        }

        public object GetResult2()
        {
            if (!p1Run) Parallel.ForEach(modules, m => m.CalculateCorruptionScore());

            Parallel.ForEach(modules, m => m.CalculateCompletionScore());

            var incomplete = modules.Select(m => m.Score).ToList();
            var filtered = incomplete.Where(m => m > 0).ToList();
            var ordered = filtered
                .OrderBy(m => m)
                .ToList();

            return ordered[ordered.Count / 2];
        }
    }
}
