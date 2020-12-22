using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent22
{
    public class Solution : ISolution
    {
        public Deck[] InitialDecks;

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input);

            InitialDecks = new Deck[blocks.Length];

            for (int n = 0; n < InitialDecks.Length; n++)
            {
                InitialDecks[n] = new Deck(n);
                for (int card = 1; card < blocks[n].Length; card++) InitialDecks[n].Cards.Enqueue(int.Parse(blocks[n][card]));
                InitialDecks[n].PlayedCard = InitialDecks[n].Cards.Count;
            }
        }
        public Solution() : this("Input.txt") { }

        public class Deck
        {
            public int Player;

            public Queue<int> Cards = new Queue<int>();

            public int PlayedCard;

            public Deck(int player) { this.Player = player; }

            public Deck(Deck prevRound) : this(prevRound.Player)
            {
                var allCards = prevRound.Cards.ToArray();
                var numToGet = prevRound.PlayedCard;

                for (int n = 0; n < numToGet; n++)
                {
                    Cards.Enqueue(allCards[n]);
                }
            }

            public override string ToString()
            {
                return string.Join(",", Cards);
            }
        }

        public void RunRound(Deck[] decks)
        {
            foreach (var deck in decks) deck.PlayedCard = deck.Cards.Dequeue();

            var inOrder = decks.OrderByDescending(d => d.PlayedCard);

            var winner = inOrder.First();

            foreach (var deck in inOrder) winner.Cards.Enqueue(deck.PlayedCard);
        }

        public int RunRecursiveRound(Deck[] decks)
        {
            foreach (var deck in decks) deck.PlayedCard = deck.Cards.Dequeue();

            int winner;
            if (decks.All(d => d.PlayedCard <= d.Cards.Count))
            {
                var newDecks = decks.Select(d => new Deck(d)).ToArray();

                winner = RunRecursiveGame(newDecks);
            }
            else
            {
                var inOrder = decks.OrderByDescending(d => d.PlayedCard);

                winner = inOrder.First().Player;
            }

            decks[winner].Cards.Enqueue(decks[winner].PlayedCard);
            decks[winner].Cards.Enqueue(decks[1 - winner].PlayedCard);

            return winner;
        }

        public int RunRecursiveGame(Deck[] decks)
        {
            HashSet<string> gameStates = new HashSet<string>();

            while (decks[0].Cards.Count * decks[1].Cards.Count > 0)
            {
                var gameState = string.Join('.', decks.Select(d => d.ToString()));

                if (gameStates.Contains(gameState)) return 0;
                else gameStates.Add(gameState);

                RunRecursiveRound(decks);
            }

            var winner = decks.Where(d => d.Cards.Count > 0).Single().Player;

            return winner;
        }

        public object GetResult1()
        {
            var Decks = InitialDecks.Select(d => new Deck(d)).ToArray();

            while (Decks[0].Cards.Count * Decks[1].Cards.Count > 0)
            {
                RunRound(Decks);
            }
            var winner = Decks.Where(d => d.Cards.Count > 0).Single().Cards;

            int result = 0;
            for (int n = winner.Count; n >= 1; n--)
            {
                result += winner.Dequeue() * n;
            }

            return result;
        }

        public object GetResult2()
        {
            var Decks = InitialDecks.Select(d => new Deck(d)).ToArray();

            var winner = Decks[RunRecursiveGame(Decks)].Cards;

            int result = 0;
            for (int n = winner.Count; n >= 1; n--)
            {
                result += winner.Dequeue() * n;
            }

            return result;
        }
    }
}
