using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2018.Advent9
{
    public class Solution : ISolution
    {
        //479 players; last marble is worth 71035 points
        public void WriteResult()
        {
            Console.Write("part1: ");
            WriteWinningScore(479, 71035);
            Console.Write("part2: ");
            WriteWinningScore(479, 7103500);
        }


        private void WriteWinningScore(int numPlayers, int numMarbles)
        {
            var game = new Game(numPlayers);
            for (int n = 0; n < numMarbles; n++)
            {
                game.Place();
            }

            Console.WriteLine(game.GetHighscore());
        }

        // players zijn 0-based, moet 1 hoger voor output
        private class Game
        {
            private CircularLinkedList<int> Marbles { get; set; }
            private int numPlayers;
            private Dictionary<int, long> scores;

            private int currentPlayer;
            private int currentMarble;
            

            public Game(int numPlayers)
            {
                Marbles = new CircularLinkedList<int>(0);
                scores = new Dictionary<int, long>();
                this.numPlayers = numPlayers;
                this.currentMarble = 1;
                for (int n = 0; n < numPlayers; n++) scores.Add(n, 0);
            }

            public void Place()
            {
                if (currentMarble % 23 == 0)
                {
                    scores[currentPlayer] += currentMarble;
                    scores[currentPlayer] += Marbles.RemoveCounterClockWise(7);
                }
                else
                {
                    Marbles.AddClockwise(2, currentMarble);
                }

                currentMarble++;
                currentPlayer++;
                if (currentPlayer == numPlayers) currentPlayer = 0;
            }

            public long GetHighscore()
            {
                return scores.OrderByDescending(kv => kv.Value).First().Value;
            }
        }

        private class CircularLinkedList<T>
        {
            private LinkedList<T> inner;
            private LinkedListNode<T> cursor;

            public CircularLinkedList(T firstItem)
            {
                inner = new LinkedList<T>();
                cursor = inner.AddFirst(firstItem);
            }

            public T RemoveCounterClockWise(int numPositions)
            {
                for (int n = 0; n < numPositions; n++)
                {
                    GoCounterClockwise();
                }
                return RemoveCurrent();
            }

            private T RemoveCurrent()
            {
                var toRemove = cursor;
                GoClockwise();
                inner.Remove(toRemove);

                return toRemove.Value;
            }

            public void AddClockwise(int numPositions, T value)
            {
                for (int n = 1; n < numPositions; n++)
                {
                    GoClockwise();
                }
                inner.AddAfter(cursor, value);
                GoClockwise();
            }

            private T GoClockwise()
            {
                if (cursor.Next == null) cursor = inner.First;
                else cursor = cursor.Next;
                return cursor.Value;
            }

            private T GoCounterClockwise()
            {
                if (cursor.Previous == null) cursor = inner.Last;
                else cursor = cursor.Previous;
                return cursor.Value;
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                foreach (var item in inner)
                {
                    if (item.Equals(cursor.Value)) builder.Append("(" + item + ") ");
                    else builder.Append(item + " ");
                }
                return builder.ToString();
            }
        }
    }
}
