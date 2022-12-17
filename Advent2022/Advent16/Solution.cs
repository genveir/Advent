using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using Advent2022.Shared.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent16
{
    public class Solution : ISolution
    {
        public List<Valve> valves;
        public Dictionary<string, Valve> valveMap;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var singleParser = new InputParser<Valve>(false, 3, "Valve ", " has flow rate=", "; tunnel leads to valve ");
            var inputParser = new InputParser<Valve>(false, 3, "Valve ", " has flow rate=", "; tunnels lead to valves ");

            valves = lines.Select(l =>
            {
                if (l.Contains("valves")) return inputParser.Parse(l);
                return singleParser.Parse(l);
            }).ToList();

            valves.Add(new Valve("SKIP", 0, Array.Empty<string>()));

            valveMap = valves.ToDictionary(m => m.Name, m => m);

            foreach (var valve in valves) valve.SetTargets(valveMap);
            foreach (var valve in valves) valve.SetDistances();
        }
        public Solution() : this("Input.txt") { }

        public class Valve
        {
            public string Name;
            public long FlowRate;
            public string[] TargetNames;

            public Valve[] Targets;
            public Dictionary<Valve, long> Distances;

            [ComplexParserConstructor]
            public Valve(string name, long flowRate, string[] targets)
            {
                this.Name = name;
                this.FlowRate = flowRate;
                this.TargetNames = targets.Select(tn => tn.Trim()).ToArray();
            }

            public void SetTargets(Dictionary<string, Valve> targets)
            {
                Targets = TargetNames.Select(tn => targets[tn]).ToArray();
            }

            public void SetDistances()
            {
                Distances = new();

                Queue<(Valve, long)> queue = new();
                queue.Enqueue((this, 0));

                HashSet<Valve> seen = new();
                while (queue.Count > 0)
                {
                    var (valve, distance) = queue.Dequeue();

                    if (seen.Contains(valve)) continue;
                    seen.Add(valve);

                    if (valve.FlowRate > 0)
                        Distances.Add(valve, distance);

                    foreach (var target in valve.Targets)
                    {
                        queue.Enqueue((target, distance + 1));
                    }
                }
            }

            public override string ToString()
            {
                return $"Valve {Name}";
            }
        }

        public class State
        {
            public Actor[] Actors;
            public Actor Me => Actors[0];
            public Actor Elephant => Actors[1];

            public Valve[] OpenValves;
            public Valve[] HandledValves => OpenValves.Append(Me.Target).Append(Elephant.Target).ToArray();
            public long TurnsLeft;

            public string StateString;

            public State(Actor[] actors, IEnumerable<Valve> openValves, long turnsLeft)
            {
                Actors = actors;
                OpenValves = openValves.ToArray();
                TurnsLeft = turnsLeft;

                StateString = $"{ActorString(Actors)}_{ValveString(OpenValves)}_{TurnsLeft}";
            }

            private string ActorString(IEnumerable<Actor> actors) =>
                string.Join("_", actors
                    .Select(ActorString));

            private string ActorString(Actor actor) => $"{actor.Target?.Name ?? "skip"}_{actor.TurnsLeftOnPath}";

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
                List<Transition> transitions = new();

                var nextActorStates = GetNextStates(Actors[0], Actors[1].Target);

                foreach(var nextActorState in nextActorStates)
                {
                    var newActors = new Actor[2];
                    var order = nextActorState.TurnsLeftOnPath <= Actors[1].TurnsLeftOnPath ? 1 : 0;
                    newActors[1 - order] = new(nextActorState.Target, nextActorState.TurnsLeftOnPath);
                    newActors[order] = new(Actors[1].Target, Actors[1].TurnsLeftOnPath);

                    var turnsToTake = newActors.Min(act => act.TurnsLeftOnPath);
                    newActors[0].TurnsLeftOnPath = 0;
                    newActors[1].TurnsLeftOnPath -= turnsToTake;

                    var newOpenValves = OpenValves.Append(newActors[0].Target);

                    var newState = new State(newActors, newOpenValves, TurnsLeft - turnsToTake);

                    var value = FlowPerTurn * turnsToTake;

                    transitions.Add(new(newState, value));
                }

                return transitions.ToArray();
            }

            public Actor[] GetNextStates(Actor actor, Valve disallowed)
            {
                var position = actor.Target;

                // move to a place and open a valve
                var possibleDestinations = position.Distances.Keys
                    .Except(OpenValves.Append(disallowed));

                List<Actor> newActors = new();
                foreach (var destination in possibleDestinations)
                {
                    var turnsRequired = position.Distances[destination] + 1;

                    if (TurnsLeft - turnsRequired < 0) continue;

                    var newActor = new Actor(destination, turnsRequired);

                    newActors.Add(newActor);
                }

                // can't go anywhere in the remaining time: skip to the end
                if (newActors.Count == 0)
                {
                    var skipToEnd = new Actor(actor.Target, TurnsLeft);

                    newActors.Add(skipToEnd);
                }

                return newActors.ToArray();
            }

            public long FlowPerTurn => OpenValves.Sum(ov => ov.FlowRate);

            public override string ToString() => $"State {StateString}";
        }

        public class Actor
        {
            public long TurnsLeftOnPath;
            public Valve Target;

            public Actor(Valve target, long turnsLeftOnPath)
            {
                Target = target;
                TurnsLeftOnPath = turnsLeftOnPath;
            }

            public Actor SubtractTurns(long turns)
            {
                return new(Target, TurnsLeftOnPath - turns);
            }

            public override int GetHashCode()
            {
                return (int)TurnsLeftOnPath + Target.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as Actor;
                return other.TurnsLeftOnPath == this.TurnsLeftOnPath &&
                    other.Target == this.Target;
            }

            public override string ToString()
            {
                return $"Actor {Target} in {TurnsLeftOnPath}";
            }
        }

        public class Transition
        {
            public State NewState;
            public long TransitionValue;

            public Transition(State newState, long transitionValue)
            {
                NewState = newState;
                TransitionValue = transitionValue;
            }

            public override string ToString() =>
               $"{TransitionValue} -> {NewState}";

            public override int GetHashCode()
            {
                return NewState.StateString.GetHashCode() + (int)TransitionValue;
            }

            public override bool Equals(object obj)
            {
                var other = obj as Transition;
                return other.TransitionValue == TransitionValue &&
                    other.NewState.StateString == NewState.StateString;
            }
        }

        public Dictionary<string, long> bestValues = new();
        public long DP(State state)
        {
            if (bestValues.TryGetValue(state.StateString, out long value)) return value;

            long bestSubResult = -1;

            if (state.TurnsLeft == 0) bestSubResult = 0;
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

        public long DoPart1(long turns)
        {
            var me = new Actor(valveMap["AA"], 0);
            var elephant = new Actor(valveMap["SKIP"], turns);
            return DP(new State(new[] { me, elephant }, Array.Empty<Valve>(), 30));
        }

        public long DoPart2()
        {
            var me = new Actor(valveMap["AA"], 0);
            var elephant = new Actor(valveMap["AA"], 0);

            return DP(new State(new[] { me, elephant }, Array.Empty<Valve>(), 26));
        }

        public object GetResult1()
        {
            return DoPart1(30);
        }

        public object GetResult2()
        {
            return DoPart2();
        }
    }
}
