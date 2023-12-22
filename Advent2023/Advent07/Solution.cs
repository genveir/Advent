using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent07;

public class Solution : ISolution
{
    public static Dictionary<char, Card> CardLookup = new();

    static Solution()
    {
        for (int n = 0; n < 10; n++) CardLookup.Add((char)('0' + n), new Card((char)('0' + n)));
        CardLookup.Add('T', new Card('T'));
        CardLookup.Add('J', new Card('J'));
        CardLookup.Add('Q', new Card('Q'));
        CardLookup.Add('K', new Card('K'));
        CardLookup.Add('A', new Card('A'));
    }


    public List<Hand> Hands;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<Hand>("line");

        Hands = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class Hand
    {
        public string Initial { get; set; }

        public Card[] Cards { get; set; }
        public long Bid { get; set; }

        public string HandType { get; set; }

        public string TypeValue { get; set; }
        public string CardValue { get; set; }
        public long TotalValue { get; set; }
        public long P2Value { get; set; }

        [ComplexParserTarget("cards bid")]
        public Hand(string cards, long bid)
        {
            Initial = cards;
            Cards = cards.Select(c => CardLookup[c]).ToArray();
            Bid = bid;

            SetHandType();
            SetCardValue();
            TotalValue = long.Parse(TypeValue + CardValue);

            P2Value = OptimizeForP2();
        }

        public void SetHandType()
        {
            var cardGroups = Cards.GroupBy(c => c);
            if (cardGroups.Count() == 1)
            {
                HandType = "Five of a kind";
                TypeValue = "9";
            }
            else if (cardGroups.Any(c => c.Count() == 4))
            {
                HandType = "Four of a kind";
                TypeValue = "8";
            }
            else if (cardGroups.Any(c => c.Count() == 3))
            {
                if (cardGroups.Any(c => c.Count() == 2))
                {
                    HandType = "Full House";
                    TypeValue = "7";
                }
                else
                {
                    HandType = "Three of a kind";
                    TypeValue = "6";
                }
            }
            else if (cardGroups.Any(c => c.Count() == 2))
            {
                if (cardGroups.Count() == 3)
                {
                    HandType = "Two pair";
                    TypeValue = "5";
                }
                else
                {
                    HandType = "One pair";
                    TypeValue = "4";
                }
            }
            else
            {
                HandType = "High card";
                TypeValue = "3";
            }
        }

        public void SetCardValue()
        {
            CardValue = Cards.Select(c => c.ValueString).Aggregate((a, b) => a + b);
        }

        public string P2CardValue()
        {
            return Cards.Select(c => c.P2ValueString).Aggregate((a, b) => a + b);
        }

        public long OptimizeForP2()
        {
            if (Cards.Any(c => c.Representation == 'J'))
            {
                var p2Value = P2CardValue();

                List<Hand> newHands = new();

                foreach(var c in "123456789TQKA")
                {
                    var newRep = Initial.Replace('J', c);
                    var newHand = new Hand(newRep, Bid);
                    newHand.TotalValue = long.Parse(newHand.TypeValue + p2Value);
                    newHands.Add(newHand);
                }

                return newHands.Max(nh => nh.TotalValue);
            }
            else return TotalValue;
        }
    }

    public class Card
    {
        public char Representation { get; set; }

        public string ValueString { get; set; }

        public string P2ValueString { get; set; }

        [ComplexParserTarget("card")]
        public Card(char representation)
        {
            Representation = representation;
            ValueString = GetValue(false);
            P2ValueString = GetValue(true);
        }

        private string GetValue(bool p2) => Representation switch
        {
            'T' => "10",
            'J' => p2 ? "00" : "11",
            'Q' => "12",
            'K' => "13",
            'A' => "14",
            char c => "0" + c
        };
    }

    public object GetResult1()
    {
        var ordered = Hands.OrderBy(h => h.TotalValue).ToArray();
        int rank = 1;
        long sum = 0;
        foreach (var h in ordered)
        {
            sum += h.Bid * rank;
            rank++;
        }
        return sum;
    }

    public object GetResult2()
    {
        var ordered = Hands.OrderBy(h => h.P2Value).ToArray();
        int rank = 1;
        long sum = 0;
        foreach (var h in ordered)
        {
            sum += h.Bid * rank;
            rank++;
        }
        return sum;
    }
}
