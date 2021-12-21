using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent21
{
    public class Solution : ISolution
    {
        // Player 1 starting position: 4
        // Player 2 starting position: 6
        public Player player1;
        public Player player2;

        public Solution(string input) : this() { }
        public Solution()
        {
            die.numRolls = -1;

            player1 = new Player();
            player2 = new Player();

            player1.position = 4;
            player2.position = 6;

            results = new Dictionary<State, (long p1Wins, long p2Wins)>();
        }

        static Solution()
        {
            die = new DeterministicDie();
        }

        public static DeterministicDie die;

        public class DeterministicDie
        {
            public long numRolls { get; set; }

            public DeterministicDie()
            {
                this.numRolls = -1;
            }

            public long Roll
            {
                get
                {
                    return (++numRolls % 100) + 1;
                }
            }
        }

        public class Player
        {
            public long score;

            public long position;

            public long TakeTurn()
            {
                var totalRoll = die.Roll + die.Roll + die.Roll;
                position = position + totalRoll;
                while (position > 10) position -= 10;

                score += position;

                return score;
            }
        }

        public object GetResult1()
        {
            while(true)
            {
                if (player1.TakeTurn() >= 1000) return player2.score * (die.numRolls + 1);

                if (player2.TakeTurn() >= 1000) return player1.score * (die.numRolls + 1);
            }
        }

        public class State
        {
            public long p1Pos;
            public long p2Pos;

            public long p1Score;
            public long p2Score;

            public bool p1sTurn;

            public override int GetHashCode()
            {
                return (int)(p1Pos * p2Pos + (10928181 * p1Score * p2Score) * (p1sTurn ? 7 : 3));
            }

            public override bool Equals(object obj)
            {
                var other = (State)obj;
                return other.p1Pos == this.p1Pos &&
                    other.p2Pos == this.p2Pos &&
                    other.p1Score == this.p1Score &&
                    other.p2Score == this.p2Score &&
                    other.p1sTurn == this.p1sTurn;
            }

            public override string ToString()
            {
                var turn = p1sTurn ? "(P1)" : "(P2)";
                return $"{turn} p1 at {p1Pos} with {p1Score}, p2 at {p2Pos} with {p2Score}";
            }
        }

        Dictionary<State, (long p1Wins, long p2Wins)> results;
        public (long p1Wins, long p2Wins) CalculateWins(State state)
        {
            // eerder gezien -> result terug
            if (results.TryGetValue(state, out (long p1Wins, long pw2Wins) val)) return val;

            // gewonnen -> +1 win
            if (state.p1Score >= 21) return (1, 0);
            if (state.p2Score >= 21) return (0, 1);

            // mogelijke nieuwe states is deze state met dobbels
            (long p1Wins, long p2Wins) result = (0, 0);
            for (var die1 = 1; die1 <= 3; die1++ )
            {
                for (var die2 = 1; die2 <= 3; die2++)
                {
                    for (var die3 = 1; die3 <= 3; die3++)
                    {
                        var totalVal = die1 + die2 + die3;

                        var newState = GetNewState(state, totalVal);

                        var subResult = CalculateWins(newState);

                        result.p1Wins = result.p1Wins + subResult.p1Wins;
                        result.p2Wins = result.p2Wins + subResult.p2Wins;
                    }
                }
            }

            results[state] = result;

            return result;
        }

        public object GetResult2()
        {
            // eindscore is 21,22, 23
            // vanuit 20 + 1,2,3
            //        19 + 2,3
            //        18 + 3

            // alle states -> positions, scores, turn, verder niks = 10 * 10 * 23 * 23 * 2 = 50k ofzo
            // kan makkelijk in een dict

            var startState = new State()
            {
                p1Pos = 4,
                p1Score = 0,
                p2Pos = 6,
                p2Score = 0,
                p1sTurn = true
            };

            results = new Dictionary<State, (long p1Wins, long p2Wins)>();

            var res = CalculateWins(startState);

            return (res.p1Wins > res.p2Wins) ? res.p1Wins : res.p2Wins;
        }

        public State GetNewState(State state, int totalVal)
        {
            State newState;

            if (state.p1sTurn)
            {
                var p1Pos = state.p1Pos + totalVal;
                while (p1Pos > 10) p1Pos -= 10;

                var p1Score = state.p1Score + p1Pos;

                newState = new State()
                {
                    p1Pos = p1Pos,
                    p1Score = p1Score,
                    p2Pos = state.p2Pos,
                    p2Score = state.p2Score,
                    p1sTurn = false
                };
            }
            else
            {
                var p2Pos = state.p2Pos + totalVal;
                while (p2Pos > 10) p2Pos -= 10;

                var p2Score = state.p2Score + p2Pos;

                newState = new State()
                {
                    p1Pos = state.p1Pos,
                    p1Score = state.p1Score,
                    p2Pos = p2Pos,
                    p2Score = p2Score,
                    p1sTurn = true
                };
            }

            return newState;
        }
    }
}
