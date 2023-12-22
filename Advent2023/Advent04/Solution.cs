using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent04;

public class Solution : ISolution
{
    public List<Card> cards;

    public Solution(string input)
    {
        var lines = Input
            .GetInputLines(input)
            .Select(l => l.Substring(5))
            .ToArray();

        var inputParser = new InputParser<Card>("line");

        cards = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class Card
    {
        public long Id { get; set; }
        public long Amount { get; set; } = 1;

        public long[] Winning { get; set; }
        public long[] Actual { get; set; }

        [ComplexParserConstructor("id: winning | actual", ArrayDelimiters = new[] {' '})]
        public Card(long id, long[] winning, long[] actual)
        {
            Id = id;
            Winning = winning;
            Actual = actual;
        }

        private long _numWins = -1;
        public long NumWins()
        {
            if (_numWins == -1)
            {
                _numWins = 0;
                for (int n = 0; n < Winning.Length; n++)
                {
                    if (Actual.Contains(Winning[n]))
                    {
                        _numWins++;
                    }
                }
            }
            return _numWins;
        }

        public long Value()
        {
            var numWins = NumWins();
            return numWins == 0 ? 0 : (long)Math.Pow(2, numWins - 1);
        }
    }

    public object GetResult1()
    {
        return cards.Sum(c => c.Value());
    }

    public object GetResult2()
    {
        int upperBound = cards.Count - 1;
        for (int n = 0; n <  cards.Count; n++)
        {
            var numWins = cards[n].NumWins();

            for (int i = 1; i <= numWins; i++) 
            {
                if (n + i > upperBound) break;
                cards[n + i].Amount += cards[n].Amount;
            }
        }

        return cards.Sum(c => c.Amount);
    }
}
