using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent22;

public class Solution : ISolution
{
    public StateObj State;

    public class StateObj
    {
        public Dictionary<Coordinate, Brick> BricksByCoordinate = new();
        public List<Brick> Bricks = new();

        public StateObj(Dictionary<Coordinate, Brick> bricksByCoordinate)
        {
            BricksByCoordinate = bricksByCoordinate;
            Bricks = BricksByCoordinate.Values.Distinct().ToList();
        }

        public StateObj CopyWithout(Brick dontCopy)
        {
            var newDict = new Dictionary<Coordinate, Brick>();
            foreach (var brick in Bricks)
            {
                if (brick == dontCopy) continue;

                var newBrick = brick.Copy();

                foreach (var coord in newBrick.AllCoords)
                {
                    newDict.Add(coord, newBrick);
                }
            }

            return new StateObj(newDict);
        }
    }

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<Brick>("line");

        var bricks = inputParser.Parse(lines);

        Dictionary<Coordinate, Brick> bricksByCoordinate = new();
        foreach (var brick in bricks)
        {
            foreach (var coord in brick.AllCoords)
                bricksByCoordinate.Add(coord, brick);
        }

        State = new StateObj(bricksByCoordinate);
    }
    public Solution() : this("Input.txt") { }

    public class Brick
    {
        public static long _index = 0;
        public long index = _index++;

        public List<Coordinate> AllCoords = new();
        public long MinZ => AllCoords.Min(c => c.Z) ?? -1;

        [ComplexParserTarget("first~second")]
        public Brick(Coordinate first, Coordinate second)
        {
            if (first.X != second.X) AddXCoords(first, second);
            if (first.Y != second.Y) AddYCoords(first, second);
            if (first.Z != second.Z) AddZCoords(first, second);

            if (AllCoords.Count == 0) // all the same
                AddXCoords(first, second);
        }

        public Brick() { }

        public Brick Copy()
        {
            return new Brick()
            {
                AllCoords = new(AllCoords)
            };
        }

        public bool Drop(Dictionary<Coordinate, Brick> bricksByCoordinate)
        {
            var space = FindSpace(bricksByCoordinate);

            if (space == 0) return false;

            var newCoords = new List<Coordinate>();
            foreach (var coord in AllCoords)
            {
                var newCoord = coord.ShiftZ(space);

                bricksByCoordinate.Remove(coord);
                newCoords.Add(newCoord);
                bricksByCoordinate.Add(newCoord, this);
            }

            AllCoords = newCoords;

            return true;
        }

        public long FindSpace(Dictionary<Coordinate, Brick> bricksByCoordinate)
        {
            var shift = -1;

            while (true)
            {
                var below = AllCoords.Select(lc => lc.ShiftZ(shift));

                if (below.Any(b => b.Z < 1)) return shift + 1;

                if (below.Any(ob => bricksByCoordinate.TryGetValue(ob, out Brick b) && b != this)) return shift + 1;

                shift -= 1;
            }
        }

        public void AddXCoords(Coordinate first, Coordinate second)
        {
            if (first.X > second.X) AddXCoords(second, first);
            else
            {
                for (long x = first.X; x <= second.X; x++)
                {
                    AllCoords.Add(new(x, first.Y, first.Z));
                }
            }
        }

        public void AddYCoords(Coordinate first, Coordinate second)
        {
            if (first.Y > second.Y) AddYCoords(second, first);
            else
            {
                for (long y = first.Y; y <= second.Y; y++)
                {
                    AllCoords.Add(new(first.X, y, first.Z));
                }
            }
        }

        public void AddZCoords(Coordinate first, Coordinate second)
        {
            if (first.Z > second.Z) AddZCoords(second, first);
            else
            {
                for (long z = first.Z ?? 0; z <= (second.Z ?? 0); z++)
                {
                    AllCoords.Add(new(first.X, first.Y, z));
                }
            }
        }

        public override string ToString() => $"Brick {index} {AllCoords.First()}-{AllCoords.Last()}";
    }

    public bool DropBricksOne(StateObj state, HashSet<long> fallen)
    {
        var bricks = state.BricksByCoordinate.Values;

        bool anyFallen = false;
        foreach (var brick in bricks.OrderBy(b => b.MinZ))
        {
            if (brick.Drop(state.BricksByCoordinate))
            {
                anyFallen = true;
                fallen.Add(brick.index);
            };
        }
        return anyFallen;
    }

    public int DropBricksAllTheWay(StateObj state)
    {
        HashSet<long> fallen = new();
        while (DropBricksOne(state, fallen)) { }

        return fallen.Count;
    }

    public object GetResult1()
    {
        DropBricksAllTheWay(State);

        long result = 0;
        foreach (var brick in State.Bricks)
        {
            var brickCoords = brick.AllCoords;

            foreach (var brickCoord in brickCoords) State.BricksByCoordinate.Remove(brickCoord);

            var otherBricks = State.Bricks.Where(b => b != brick).ToArray();

            bool canDisintegrate = true;
            foreach (var other in otherBricks)
            {
                if (other.FindSpace(State.BricksByCoordinate) != 0) canDisintegrate = false;
            }
            if (canDisintegrate) result++;

            foreach (var brickCoord in brickCoords) State.BricksByCoordinate.Add(brickCoord, brick);
        }

        return result;
    }

    public object GetResult2()
    {
        DropBricksAllTheWay(State);

        long result = 0;
        foreach (var brick in State.Bricks)
        {
            var copy = State.CopyWithout(brick);

            result += DropBricksAllTheWay(copy);
        }

        return result;
    }
}
