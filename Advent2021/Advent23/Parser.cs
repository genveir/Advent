using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent23
{
    public static class Parser
    {
        public static (World, State) ParseInput(string[] lines)
        {
            var world = new World();
            
            var roomList = new List<Room>();
            var rooms = new Room[lines.Length][];
            var positions = new List<(int index, char creature)>();

            int index = 0;
            Room room;
            for (int y = 0; y < lines.Length; y++)
            {
                rooms[y] = new Room[lines[y].Length];
                for (int x = 0; x < lines[y].Length; x++)
                {
                    switch (lines[y][x])
                    {
                        case '.':
                            room = new Room(index++, y, x);
                            AddRoom(room, y, x, rooms);
                            roomList.Add(room);
                            break;
                        case 'A':
                        case 'B':
                        case 'C':
                        case 'D':
                            if (lines[y - 1][x] == '.') rooms[y - 1][x].blocked = true;

                            room = new Room(index++, y, x);
                            AddRoom(room, y, x, rooms);
                            positions.Add((room.index, lines[y][x]));
                            roomList.Add(room);
                            break;
                    }
                }
            }

            world.placeType = new int[roomList.Count];
            for (int n = 0; n < roomList.Count; n++) if (roomList[n].blocked) world.placeType[n] = World.PLACETYPE_BLOCKED;
            for (int n = 0; n < positions.Count; n++) world.placeType[positions[n].index] = World.PLACETYPE_TARGET;

            world.distanceMatrix = GenerateDistanceMatrix(roomList);

            world.linkMatrix = new int[roomList.Count][];
            for (int n = 0; n < world.linkMatrix.Length; n++)
            {
                world.linkMatrix[n] = roomList[n].links.ToArray();
            }

            var targets = positions.Select(p => roomList[p.index]).GroupBy(p => p.x).ToArray();

            world.targets = targets[0].Select(t => t.index)
                    .Concat(targets[1].Select(t => t.index))
                    .Concat(targets[2].Select(t => t.index))
                    .Concat(targets[3].Select(t => t.index))
                    .ToArray();

            world.numOfEach = positions.Count / 4;
            world.costToMove = new int[positions.Count];
            world.typeOfAmphipod = new int[positions.Count];
            int cost = 1;
            for (int n = 0; n < 4; n++)
            {
                for (int i = 0; i < world.numOfEach; i++)
                {
                    world.typeOfAmphipod[n * world.numOfEach + i] = n;
                    world.costToMove[n * world.numOfEach + i] = cost;
                }
                cost *= 10;
            }

            var stateOccupier = new bool[roomList.Count];
            var aPositions = new List<int>();
            var bPositions = new List<int>();
            var cPositions = new List<int>();
            var dPositions = new List<int>();
            foreach (var p in positions)
            {
                switch (p.creature)
                {
                    case 'A': aPositions.Add(p.index); break;
                    case 'B': bPositions.Add(p.index); break;
                    case 'C': cPositions.Add(p.index); break;
                    case 'D': dPositions.Add(p.index); break;
                };
            }

            var statePositions =
                        MatchAndSwap(targets[0].Select(i => i.index).ToArray(), aPositions.ToArray())
                .Concat(MatchAndSwap(targets[1].Select(i => i.index).ToArray(), bPositions.ToArray()))
                .Concat(MatchAndSwap(targets[2].Select(i => i.index).ToArray(), cPositions.ToArray()))
                .Concat(MatchAndSwap(targets[3].Select(i => i.index).ToArray(), dPositions.ToArray()))
                .ToArray();

            var stateOccupiers = new int[roomList.Count];
            for (int n = 0; n < stateOccupiers.Length; n++) stateOccupiers[n] = -1;
            for (int n = 0; n < statePositions.Length; n++) stateOccupiers[statePositions[n]] = n;

            world.hashMults = new int[statePositions.Length];
            for (int n = 0; n < statePositions.Length; n++) world.hashMults[n] = world.costToMove[n] * 10000000;

            var state = new State(null, world, statePositions, stateOccupiers, 0);
            state.LockValues();

            world.maxConsiderableCost = 3 * state.TotalCost / 2;

            return (world, state);
        }

        public static int[] MatchAndSwap(int[] targets, int[] positions)
        {
            for (int n = 0; n < targets.Length; n++)
            {
                var targetInPosition = targets[n];

                int indexOfMatch = -1;
                for (int i = 0; i < positions.Length;i++)
                {
                    if (positions[i] == targetInPosition) indexOfMatch = i;
                }

                if (indexOfMatch > -1)
                {
                    positions[indexOfMatch] = positions[n];
                    positions[n] = targetInPosition;
                }
            }

            return positions;
        }

        public static int[][] GenerateDistanceMatrix(List<Room> roomList)
        {
            var distanceMatrix = new int[roomList.Count][];

            for (int r = 0; r < roomList.Count; r++)
            {
                distanceMatrix[r] = new int[roomList.Count];
                FindDistances(roomList, r, distanceMatrix[r], 0, r);
            }

            return distanceMatrix;
        }

        public static void FindDistances(List<Room> roomList, int current, int[] distances, int distance, int isFor)
        {
            if (roomList[current].visited == isFor) return;
            roomList[current].visited = isFor;

            distances[current] = distance;

            foreach (var linked in roomList[current].links) FindDistances(roomList, linked, distances, distance + 1, isFor);
        }

        private static void AddRoom(Room room, int y, int x, Room[][] rooms)
        {
            rooms[y][x] = room;
            if (rooms[y - 1][x] != null) { room.links.Add(rooms[y - 1][x].index); rooms[y - 1][x].links.Add(room.index); }
            if (rooms[y][x - 1] != null) { room.links.Add(rooms[y][x - 1].index); rooms[y][x - 1].links.Add(room.index); }
        }

        public class Room
        {
            public int index;
            public bool blocked;
            public int y, x;

            public Room(int index, int y, int x)
            {
                this.index = index;
                this.y = y;
                this.x = x;

                visited = -1;
            }

            public int visited;
            public List<int> links = new List<int>();


            public override string ToString()
            {
                return blocked ? "x" : ".";
            }
        }
    }
}
