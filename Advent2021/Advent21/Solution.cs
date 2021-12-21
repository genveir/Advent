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
        public State startingState;

        public Solution() : this(4,6) { }
        public Solution(string input) : this() { }
        public Solution(long p1Pos = 4, long p2Pos = 6)
        {
            startingState = new State()
            {
                pActivePos = p1Pos,
                pActiveScore = 0,
                pOffPos = p2Pos,
                pOffScore = 0
            };

            results = new Dictionary<State, (long p1Wins, long p2Wins)>();
        }

        public class State
        {
            private long _pOffPos;
            public long pOffPos 
            { 
                get => _pOffPos;
                set { _pOffPos = value; while (_pOffPos > 10) _pOffPos -= 10; }
            }


            public long pOffScore;

            public long pActivePos;
            public long pActiveScore;

            public State Copy()
            {
                return new State()
                {
                    _pOffPos = _pOffPos,
                    pOffScore = pOffScore,
                    pActivePos = pActivePos,
                    pActiveScore = pActiveScore
                };
            }

            // hm dit flipt gewoon
            public State Next(int totalDieRoll)
            {
                var newState = Copy();
                newState.pOffPos = pActivePos + totalDieRoll;
                newState.pOffScore = pActiveScore + newState.pOffPos;

                newState.pActivePos = pOffPos;
                newState.pActiveScore = pOffScore;

                return newState;
            }

            public override int GetHashCode()
            {
                return (int)(pActivePos * _pOffPos + 398479 * pActiveScore * pOffScore);
            }

            public override bool Equals(object obj)
            {
                var other = (State)obj;
                return other.pActivePos == this.pActivePos &&
                    other._pOffPos == this._pOffPos &&
                    other.pActiveScore == this.pActiveScore &&
                    other.pOffScore == this.pOffScore;
            }

            public override string ToString()
            {
                return $"pAct at {pActivePos} with {pActiveScore}, pOff at {_pOffPos} with {pOffScore}";
            }
        }

        public class DeterministicDie
        {
            public int numRolls { get; set; } = -1;

            public int Roll => (++numRolls % 100) + 1;
        }

        public void TakeTurn(ref State state, DeterministicDie die)
        {
            var totalRoll = die.Roll + die.Roll + die.Roll;
            state = state.Next(totalRoll);
        }

        Dictionary<State, (long p1Wins, long p2Wins)> results;
        public (long pActWins, long pOffWins) CalculateWins(State state)
        {
            // eerder gezien -> result terug
            if (results.TryGetValue(state, out (long p1Wins, long pw2Wins) val)) return val;

            // gewonnen -> +1 win
            if (state.pOffScore >= 21) return (1, 0);

            // mogelijke nieuwe states is deze state met dobbels
            (long pActWins, long pOffWins) result = (0, 0);
            for (var die1 = 1; die1 <= 3; die1++)
            {
                for (var die2 = 1; die2 <= 3; die2++)
                {
                    for (var die3 = 1; die3 <= 3; die3++)
                    {
                        var totalVal = die1 + die2 + die3;

                        // flipperoonie
                        var newState = state.Next(totalVal);

                        var subResult = CalculateWins(newState);

                        result.pActWins = result.pActWins + subResult.pOffWins;
                        result.pOffWins = result.pOffWins + subResult.pActWins;
                    }
                }
            }

            results[state] = result;

            return result;
        }


        public object GetResult1()
        {
            var state = startingState.Copy();
            var die = new DeterministicDie();

            while(true)
            {
                TakeTurn(ref state, die);

                if (state.pOffScore >= 1000) return state.pActiveScore * (die.numRolls + 1);
            }
        }

        public object GetResult2()
        {
            // eindscore is 21,22, 23
            // vanuit 20 + 1,2,3
            //        19 + 2,3
            //        18 + 3

            // alle states -> positions, scores, turn, verder niks = 10 * 10 * 23 * 23 * 2 = 50k ofzo
            // kan makkelijk in een dict

            var startState = startingState.Copy();

            results = new Dictionary<State, (long p1Wins, long p2Wins)>();

            var res = CalculateWins(startState);

            return (res.pActWins > res.pOffWins) ? res.pActWins : res.pOffWins;
        }
    }
}
