using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent16Alt
{
    public class DynamicProgramming
    {
        public Dictionary<string, long> bestValues = new();

        public State StartState;
        public bool DoubleAgent;

        public DynamicProgramming(State state, bool doubleAgent)
        {
            StartState = state;
            DoubleAgent = doubleAgent;
        }

        public long Execute()
        {
            return DP(StartState);
        }

        public long DP(State state)
        {
            if (bestValues.TryGetValue(state.StateString, out long value)) return value;

            long bestSubResult = -1;

            if (state.TurnsLeft == 0) bestSubResult =
                    DoubleAgent ?
                        RunSubDP(state) :
                        0;
            else
            {
                var transitions = state.GetTransitions();

                foreach (var transition in transitions)
                {
                    var subResult = transition.TransitionValue + DP(transition.NewState);
                    if (subResult > bestSubResult) bestSubResult = subResult;
                }
            }

            bestValues[state.StateString] = bestSubResult;
            return bestSubResult;
        }

        public Dictionary<string, long> SubDpValues = new();
        public long RunSubDP(State state)
        {
            if (SubDpValues.TryGetValue(state.StateString, out long value)) return value;

            var subState = new State(
                StartState.Position,
                Array.Empty<Valve>(),
                state.OpenValves,
                StartState.TurnsLeft);

            var subDp = new DynamicProgramming(subState, false);

            value = subDp.Execute();

            SubDpValues.Add(state.StateString, value);

            return value;
        }
    }
}
