using Advent2021.Shared;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent23
{
    public class Solution : ISolution
    {
        public string input;

        public World world;
        public State startingState;

        public Solution(string input)
        {
            this.input = input;

            var lines = Input.GetInputLines(input).ToArray();

            (world, startingState) = Parser.ParseInput(lines);
        }
        public Solution() : this("Input.txt") { }

        public object GetResult1()
        {
            var solve = Solve1();

            //PrintResult(solve);

            return solve?.cost ?? -1;
        }

        public void PrintResult(State state)
        {
            if (state.parent != null) PrintResult(state.parent);
            Console.WriteLine(state.PrettyPrintP1());
        }

        public State Solve1()
        {
            var queue = new PriorityQueue<State>();
            queue.Add(startingState);

            HashSet<State> seenAtLowerCost = new HashSet<State>();
            while (queue.Size > 0)
            {
                var state = queue.Poll();

                if (seenAtLowerCost.Contains(state)) continue;
                seenAtLowerCost.Add(state);

                if (state.IsWin())
                {
                    return state;
                }
                else
                {
                    state.GenerateTransitions(queue);
                }
            }
            return null;
        }

        public object GetResult2() // not 44208
        {
            var lines = Input.GetInputLines(input).ToArray();

            var actualLines = new string[lines.Length + 2];
            for (int n = 0; n < 3; n++)
            {
                actualLines[n] = lines[n];
            }
            actualLines[3] = "  #D#C#B#A#";
            actualLines[4] = "  #D#B#A#C#";
            for (int n = 3; n < lines.Length; n++)
            {
                actualLines[n + 2] = lines[n];
            }

            (world, startingState) = Parser.ParseInput(actualLines);

            var solve = Solve1();

            //PrintResult(solve);

            return solve?.cost ?? -1;
        }
    }

    public class World
    {
        public const int PLACETYPE_NORMAL = 0;
        public const int PLACETYPE_BLOCKED = 1;
        public const int PLACETYPE_TARGET = 2;

        public int[] typeOfAmphipod; // 0 = A, 1 = B, 2 = C, 3 = D

        public int[] costToMove; // the cost to move each creature

        public int[][] distanceMatrix; // distance to each room, for heuristics

        public int[][] linkMatrix; // rooms directly linked to each room

        public int[] targets; // target spots for each amphipod.

        public int[] placeType; // 0 = regular room, 1 = blocked room, 2 = target room

        public int numOfEach; // number of each creature

        public int[] hashMults; // multiplier for each amphipod to calculate equality hash

        public long maxConsiderableCost; // don't bother adding transitions that are more expensive than this
    }

    public class State : IComparable<State>
    {
        public State parent;
        public World world;
        public int[] Positions; // positions of the creatures. Divide by world.numOfEach to get the type (0 = A, 1 = B, 2 = C, 3 = D)
        public int[] Occupier;
        public int cost = 0;

        private LockedValues Values { get; set; }

        public State(State parent, World world, int[] positions, int[] occupied, int cost)
        {
            this.parent = parent;
            this.world = world;
            this.Positions = positions;
            this.Occupier = occupied;
            this.cost = cost;

            visited = new int[Occupier.Length];
            burrowIsFree = new bool[4];
            for (int n = 0; n < visited.Length; n++) visited[n] = -1;
        }

        private class LockedValues
        {
            public bool[] LockedBurrows { get; }
            public long HeuristicDistance { get; }
            public long TotalCost { get; }
            public int _HashCode { get; }

            public LockedValues(long heuristicDistance, long totalCost, int hashCode, bool[] lockedBurrows)
            {
                HeuristicDistance = heuristicDistance;
                TotalCost = totalCost;
                _HashCode = hashCode;
                LockedBurrows = lockedBurrows;
            }
        }

        public void LockValues()
        {
            var heuristic = CalculateHeuristic();

            this.Values = new LockedValues(
                heuristic,
                cost + heuristic,
                CalculateHashCode(),
                CalculateLockedBurrows());
        }

        public State Copy()
        {
            return new State(this, world, Positions.DeepCopy(), Occupier.DeepCopy(), cost);
        }

        public bool[] LockedBurrows => Values.LockedBurrows;
        public bool[] CalculateLockedBurrows()
        {
            bool[] lockedBurrows = new bool[4];
            for (int ampType = 0; ampType < 4; ampType++)
            {
                for (int num = 0; num < world.numOfEach; num++)
                {
                    var spotToCheck = world.targets[ampType * world.numOfEach + num];
                    var ampInPlace = Occupier[spotToCheck];
                    if (ampInPlace > 0 && world.typeOfAmphipod[ampInPlace] != ampType) lockedBurrows[ampType] = true;
                }
            }

            return lockedBurrows;
        }

        public void GenerateTransitions(PriorityQueue<State> states)
        {
            for (int n = 0; n < Positions.Length; n++)
                GenerateTransitions(states, n);
        }

        bool[] burrowIsFree;
        public void GenerateTransitions(PriorityQueue<State> possibleStates, int amphipod)
        {
            var canOnlyMoveHome = world.placeType[Positions[amphipod]] == World.PLACETYPE_NORMAL;
            visited[Positions[amphipod]] = amphipod;

            MoveOn(possibleStates, amphipod, Positions[amphipod], canOnlyMoveHome);
        }

        int[] visited;
        public void CheckMove(PriorityQueue<State> possibleStates, int amphipod, int position, bool canOnlyMoveHome)
        {
            if (visited[position] == amphipod) return;
            visited[position] = amphipod;

            // space is occupied
            if (Occupier[position] > -1)
            {
                // if it's by a amphipod with the same cost in its target position that's fine.
                // can't go here though, so don't add a state and move on.
                if (world.costToMove[Occupier[position]] == world.costToMove[amphipod] &&
                    position == world.targets[Occupier[position]])
                {
                    MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
                    return;
                }
                else return;
            }

            // if we can only go home and this is not our target we can't stay
            if (canOnlyMoveHome && position != world.targets[amphipod])
            {
                MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
                return;
            }

            // can't stay on a blocked square, move on.
            if (world.placeType[position] == World.PLACETYPE_BLOCKED)
            {
                MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
                return;
            }

            if (world.placeType[position] == World.PLACETYPE_TARGET)
            {
                // can't stay on a target square if someone else is in our burrow;
                if (LockedBurrows[world.typeOfAmphipod[amphipod]])
                {
                    MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
                    return;
                }

                // can't stay on a target square that's not ours.
                if (world.targets[amphipod] != position)
                {
                    MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
                    return;
                }
            }
            // otherwise we can go here
            AddMove(possibleStates, amphipod, position);

            MoveOn(possibleStates, amphipod, position, canOnlyMoveHome);
        }

        public void MoveOn(PriorityQueue<State> possibleStates, int amphipod, int position, bool canOnlyMoveHome)
        {
            for (int n = 0; n < world.linkMatrix[position].Length; n++)
            {
                CheckMove(possibleStates, amphipod, world.linkMatrix[position][n], canOnlyMoveHome);
            }
        }

        public void AddMove(PriorityQueue<State> possibleStates, int amphipod, int position)
        {
            var clone = Copy();
            clone.Positions[amphipod] = position;

            clone.Occupier[Positions[amphipod]] = -1;
            clone.Occupier[position] = amphipod;

            var costToMove = world.costToMove[amphipod];
            var distance = world.distanceMatrix[Positions[amphipod]][position];

            clone.cost = clone.cost + costToMove * distance;
            clone.LockValues();

            if (clone.TotalCost <= world.maxConsiderableCost) possibleStates.Add(clone);
        }

        public long HeuristicDistance => Values.HeuristicDistance;
        public long CalculateHeuristic()
        {
            var _heuristicCost = 0;
            for (int n = 0; n < Positions.Length; n++)
            {
                var cost = world.costToMove[n];
                var mySpot = Positions[n];
                var myTarget = world.targets[n];

                var distance = world.distanceMatrix[Positions[n]][world.targets[n]];
                _heuristicCost += cost * distance;
            }
            return _heuristicCost;
        }

        public long TotalCost => Values.TotalCost;

        public bool IsWin()
        {
            return HeuristicDistance == 0;
        }

        public int CompareTo(State other)
        {
            return TotalCost.CompareTo(other.TotalCost);
        }

        public override int GetHashCode() => Values._HashCode;
        public int CalculateHashCode()
        {
            int hash = 0;
            for (int n = 0; n < Positions.Length; n++)
            {
                hash += Positions[n] * world.hashMults[n];
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            var other = obj as State;

            for (int n = 0; n < Positions.Length; n++)
            {
                var myCost = world.costToMove[n];
                var inMyPosition = other.Occupier[Positions[n]];
                if (inMyPosition == -1) return false;
                var hisCost = world.costToMove[inMyPosition];

                if (myCost != hisCost) return false;
            }
            return true;
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(TotalCost);
            builder.Append("  ");
            builder.Append(Inline());

            return builder.ToString();
        }

        public string Inline()
        {
            StringBuilder builder = new StringBuilder();
            for (int n = 0; n < Occupier.Length; n++)
            {
                var c = GetChar(Occupier[n]);
                builder.Append(c);

                if (n == 10) builder.Append(" | ");
                if (n > 10) builder.Append(" ");
                if (n == 14) builder.Append(" | ");
                if (n == 18) builder.Append(" | ");
                if (n == 22) builder.Append(" | ");
            }

            return builder.ToString().Trim();
        }

        public char GetChar(int amphipod) => (Occupier.Length == 8) ? GetP1Char(amphipod) : GetP2Char(amphipod);

        public char GetP1Char(int amphipod)
        {
            return amphipod switch
            {
                -1 => '.',
                0 => 'A',
                1 => 'a',
                2 => 'B',
                3 => 'b',
                4 => 'C',
                5 => 'c',
                6 => 'D',
                7 => 'd',
                _ => '?'
            };
        }

        public char GetP2Char(int amphipod)
        {
            return amphipod switch
            {
                -1 => '.',
                0 => 'A',
                1 => 'a',
                2 => 'E',
                3 => 'e',
                4 => 'B',
                5 => 'b',
                6 => 'F',
                7 => 'f',
                8 => 'C',
                9 => 'c',
                10 => 'G',
                11 => 'g',
                12 => 'D',
                13 => 'd',
                14 => 'H',
                15 => 'h',
                _ => '?'
            };
        }

        public string PrettyPrintP1()
        {
            var template = @"
#############
#...........#
###.#.#.#.###
  #.#.#.#.#  
  #.#.#.#.#  
  #.#.#.#.#  
  #########  ".ToCharArray();

            Dictionary<int, int> CoordinateMap = new Dictionary<int, int>();
            for (int n = 0; n < 11; n++) CoordinateMap[n] = 18 + n;
            for (int n = 0; n < 4; n++) CoordinateMap[n + 11] = 35 + 2 * n;
            for (int n = 0; n < 4; n++) CoordinateMap[n + 15] = 50 + 2 * n;
            for (int n = 0; n < 4; n++) CoordinateMap[n + 19] = 65 + 2 * n;
            for (int n = 0; n < 4; n++) CoordinateMap[n + 23] = 80 + 2 * n;

            for (int n = 0; n < Occupier.Length; n++)
            {
                char c = GetChar(Occupier[n]);
                var occupier = Occupier[n];
                if (occupier >= 0)
                {
                    var position = Positions[occupier];

                    template[CoordinateMap[Positions[occupier]]] = c;
                }
            }

            return new string(template);
        }
    }
}
