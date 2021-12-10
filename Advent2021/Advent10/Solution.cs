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

            public int _corruptionIndex = 0;
            public long _corruptionScore = -1;
            public long CorruptionScore
            {
                get
                {
                    if (_corruptionScore == -1)
                    {
                        Stack<char> opened = new Stack<char>();
                        for (int n = 0; n < input.Length; n++)
                        {

                            if (isOpening(input[n])) opened.Push(input[n]);
                            else
                            {
                                var corruptionScore = GetCorrupted(opened, input[n]);

                                if (corruptionScore > 0)
                                {
                                    _corruptionScore = corruptionScore;
                                    _corruptionIndex = n;
                                    break;
                                }
                            }
                        }
                    }
                    if (_corruptionScore == -1) _corruptionScore = 0;

                    return _corruptionScore;
                }
            }

            private long GetCorrupted(Stack<char> opened, char c)
            {
                var toClose = opened.Pop();
                switch (c)
                {
                    case ')':
                        if (toClose != '(') return 3;
                        break;
                    case ']':
                        if (toClose != '[') return 57;
                        break;
                    case '}':
                        if (toClose != '{') return 1197;
                        break;
                    case '>':
                        if (toClose != '<') return 25137;
                        break;
                }

                return 0;

            }

            public long GetCompletionScore()
            {
                Stack<char> opened = new Stack<char>();
                for (int n = 0; n < input.Length; n++)
                {
                    if (isOpening(input[n])) opened.Push(input[n]);
                    else
                    {
                        opened.Pop();
                    }
                }

                long score = 0;
                while(opened.Count > 0)
                {
                    score *= 5;
                    var toClose = opened.Pop();

                    switch(toClose)
                    {
                        case '(': score += 1;break;
                        case '[': score += 2; break;
                        case '{': score += 3; break;
                        case '<': score += 4; break;
                    }
                }

                return score;
            }

            public bool isOpening(char c) => "([{<".Contains(c);

            public override string ToString()
            {
                return $"{_corruptionIndex}, {_corruptionScore } {input}";
            }
        }

        public object GetResult1()
        {
            return modules.Sum(m => m.CorruptionScore);

            ;
        }

        public object GetResult2()
        {
            var incomplete = modules
                .Where(m => m.CorruptionScore == 0)
                .Select(m => m.GetCompletionScore())
                .OrderBy(m => m)
                .ToList();

            return incomplete[incomplete.Count / 2];
        }
    }
}
