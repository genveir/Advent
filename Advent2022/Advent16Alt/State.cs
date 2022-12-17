using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent16Alt
{
    public class State
    {
        public Valve Position;
        public Valve[] OpenValves;
        public Valve[] BlockedValves;
        public long TurnsLeft;

        public string StateString;

        public State(Valve position, IEnumerable<Valve> openValves, IEnumerable<Valve> blockedValves, long turnsLeft)
        {
            Position = position;
            OpenValves = openValves.ToArray();
            BlockedValves = blockedValves.ToArray();
            TurnsLeft = turnsLeft;

            StateString = $"{Position.Name}_{ValveString(OpenValves)}_{ValveString(BlockedValves)}_{TurnsLeft}";
        }

        private string ValveString(IEnumerable<Valve> valves) => valves.Count() switch
        {
            0 => "",
            1 => valves.Single().Name,
            _ => valves
                .Select(ov => ov.Name)
                .OrderBy(ov => ov)
                .Aggregate((a, b) => a + b)
        };

        public Transition[] GetTransitions()
        {
            // move to a place and open a valve
            var possibleDestinations = Position.Distances.Keys
                .Except(OpenValves)
                .Except(BlockedValves)
                .ToArray();

            var transitions = new Transition[possibleDestinations.Length];
            Parallel.For(0, transitions.Length, n =>
            {
                var destination = possibleDestinations[n];

                var turnsRequired = Position.Distances[destination] + 1;

                if (TurnsLeft - turnsRequired < 0) return;

                var newState = new State(destination, OpenValves.Append(destination), BlockedValves, TurnsLeft - turnsRequired);
                var value = FlowPerTurn * turnsRequired;

                transitions[n] = (new(newState, value));
            });

            var nonNullTransitions = transitions.Where(t => t != null).ToList();

            //skip to the end
            var newState = new State(Position, OpenValves, BlockedValves, 0);

            var skipToEnd = new Transition(newState, FlowPerTurn * TurnsLeft);

            nonNullTransitions.Add(skipToEnd);


            return nonNullTransitions.ToArray();
        }

        public long FlowPerTurn => OpenValves.Sum(ov => ov.FlowRate);

        public override string ToString() => $"State {StateString}";
    }
}
