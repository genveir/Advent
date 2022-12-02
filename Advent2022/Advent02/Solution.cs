using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent02
{
    public class Solution : ISolution
    {
        public List<Round> rounds;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<Round>("A X");

            rounds = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public enum RPS
        {
            Rock = 0,
            Paper = 1,
            Scissors = 2
        }

        public enum WLS { Win, Lose, Draw }

        public class Round
        {
            private RPS Other;
            private RPS Yours;
            private WLS WLS;

            [ComplexParserConstructor]
            public Round(char other, char yours)
            {
                Other = other switch
                {
                    'A' => RPS.Rock,
                    'B' => RPS.Paper,
                    'C' => RPS.Scissors
                };
                Yours = yours switch
                {
                    'X' => RPS.Rock,
                    'Y' => RPS.Paper,
                    'Z' => RPS.Scissors
                };

                WLS = yours switch
                {
                    'X' => WLS.Lose,
                    'Y' => WLS.Draw,
                    'Z' => WLS.Win
                };

            }

            public int Score()
            {
                var typeScore = Yours switch
                {
                    RPS.Rock => 1,
                    RPS.Paper => 2,
                    RPS.Scissors => 3
                };

                var comp = (Other - Yours + 2) % 3;

                return typeScore + comp * 3;
            }

            public void SetMineFromWLS()
            {
                var otherVal = (int)Other;

                int newVal = WLS switch
                {
                    WLS.Win => (otherVal + 1) % 3,
                    WLS.Draw => otherVal,
                    WLS.Lose => (otherVal + 2) % 3
                };

                Yours = (RPS)newVal;
            }
        }

        public object GetResult1()
        {
            return rounds.Sum(round => round.Score());
        }

        public object GetResult2()
        {
            foreach (var round in rounds) round.SetMineFromWLS();

            return rounds.Sum(round => round.Score());
        }
    }
}
