using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent10
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<ParsedInput>("line");

            modules = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public string input;

            [ComplexParserConstructor]
            public ParsedInput(string input)
            {
                this.input = input;
            }

            static ParsedInput()
            {
                corruptionMap = new long[255][];

                corruptionMap[0] = new long[255];
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

            public long CorruptionScore => CalculateCorruptionScore().corruption;

            long timesToDoCalculation = 1;
            public (Stack<char> open, long corruption) _corruptionScores;
            public (Stack<char> open, long corruption) CalculateCorruptionScore()
            {
                for (int runs = 0; runs < timesToDoCalculation; timesToDoCalculation--)
                {
                    var opened = new Stack<char>();

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
                return _corruptionScores;
            }

            long score;
            long timesToCalculateScore = 1;
            public long GetCompletionScore()
            {
                for (int runs = 0; runs < timesToCalculateScore; timesToCalculateScore--)
                {
                    (var opened, var corruption) = CalculateCorruptionScore();

                    var mult = 1 - (corruption & 1);

                    score = 0;
                    while (opened.Count > 0)
                    {
                        score *= 5;
                        var toClose = opened.Pop();

                        score += scoreMap[toClose];
                    }

                    score = score * mult;
                }
                return score;
            }
        }

        public object GetResult1()
        {
            return modules.Sum(m => m.CorruptionScore);
        }

        public object GetResult2()
        {
            var incomplete = modules.Select(m => m.GetCompletionScore()).ToList();
            var filtered = incomplete.Where(m => m > 0).ToList();
            var ordered = filtered
                .OrderBy(m => m)
                .ToList();

            return ordered[ordered.Count / 2];
        }
    }
}
