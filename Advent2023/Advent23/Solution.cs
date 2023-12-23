using System;
using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;
using Advent2023.Shared.InputParsing;

namespace Advent2023.Advent23;

public class Solution : ISolution
{
    public Intersection Start;
    public Intersection End;

    public Solution(string input)
    {
        var grid = Input.GetLetterGrid(input);

        Dictionary<Coordinate, Tile> Tiles = new();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '#') continue;

                var coord = new Coordinate(x, y);
                var tile = new Tile(coord, grid[y][x]);

                Tiles.Add(coord, tile);
            }
        }

        var start = Tiles.Values.Single(t => t.Location.Y == 0);
        Start = new Intersection(start.Location);

        var intersections = new Dictionary<Coordinate, Intersection>() { { start.Location, Start } };
        Start.ParsePaths(Tiles, intersections);

        End = intersections.Values.Single(i => i.Location.Y == Tiles.Max(t => t.Key.Y));
    }
    public Solution() : this("Input.txt") { }

    public class Tile
    {
        public Coordinate Location { get; set; }
        public char Representation { get; set; }

        public Tile(Coordinate coord, char representation)
        {
            Location = coord;
            Representation = representation;
        }
    }

    public class Intersection
    {
        public Coordinate Location { get; set; }
        public List<Path> Paths { get; set; } = new();
        public bool Parsed = false;

        public Intersection(Coordinate location)
        {
            Location = location;
        }

        public void ParsePaths(Dictionary<Coordinate, Tile> tiles, Dictionary<Coordinate, Intersection> intersections)
        {
            if (Parsed) return;
            Parsed = true;

            Tile pathStart;
            if (tiles.TryGetValue(Location.ShiftY(-1), out pathStart))
                Paths.Add(Path.Parse(tiles, intersections, pathStart, new(0, -1)));
            if (tiles.TryGetValue(Location.ShiftX(-1), out pathStart))
                Paths.Add(Path.Parse(tiles, intersections, pathStart, new(-1, 0)));
            if (tiles.TryGetValue(Location.ShiftY(1), out pathStart))
                Paths.Add(Path.Parse(tiles, intersections, pathStart, new(0, 1)));
            if (tiles.TryGetValue(Location.ShiftX(1), out pathStart))
                Paths.Add(Path.Parse(tiles, intersections, pathStart, new(1, 0)));

            Paths = Paths.Where(p => p != null).ToList();

            foreach (var intersection in Paths.Select(p => p.End))
            {
                intersection.ParsePaths(tiles, intersections);
            }
        }

        public override string ToString() => $"{Location}";
    }

    public class PathParseData
    {
        public bool ThisWay = true;
        public Coordinate OtherIntersection = null;
        public long Length = 0;
    }

    public class Path
    {
        public long Length { get; set; }

        public Intersection End { get; set; }

        public bool Part2 { get; set; }

        public Path(Intersection end, long length, bool part2)
        {
            End = end;
            Length = length;
            Part2 = part2;
        }

        public static Path Parse(Dictionary<Coordinate, Tile> tiles, Dictionary<Coordinate, Intersection> intersections,
            Tile pathStart, Coordinate direction)
        {
            PathParseData parseData = new();
            ParseToIntersection(tiles, pathStart, direction, parseData);

            if (!intersections.TryGetValue(parseData.OtherIntersection, out var intersection))
            {
                intersection = new Intersection(parseData.OtherIntersection);
                intersections.Add(parseData.OtherIntersection, intersection);
            }

            return new Path(intersection, parseData.Length, !parseData.ThisWay);
        }

        public static void ParseToIntersection(Dictionary<Coordinate, Tile> tiles,
            Tile tile, Coordinate direction, PathParseData parseData)
        {
            parseData.Length++;

            if (direction == new Coordinate(1, 0) && tile.Representation == '<')
                parseData.ThisWay = false;
            if (direction == new Coordinate(-1, 0) && tile.Representation == '>')
                parseData.ThisWay = false;
            if (direction == new Coordinate(0, 1) && tile.Representation == '^')
                parseData.ThisWay = false;
            if (direction == new Coordinate(0, -1) && tile.Representation == 'v')
                parseData.ThisWay = false;

            var neighbours = GetNeighbours(tiles, tile.Location, tile.Location - direction);

            if (neighbours.Count == 0) // dead end
            {
                // check for end
                if (tile.Location.Y != tiles.Keys.Max(k => k.Y))
                {
                    parseData.ThisWay = false;
                }

                parseData.OtherIntersection = tile.Location;
                return;
            }
            if (neighbours.Count > 1)
            {
                parseData.OtherIntersection = tile.Location;
                return;
            }
            else
            {
                ParseToIntersection(tiles, neighbours.Single(), neighbours.Single().Location - tile.Location, parseData);
            }
        }

        public static List<Tile> GetNeighbours(Dictionary<Coordinate, Tile> tiles, Coordinate start, Coordinate disallowed)
        {
            List<Tile> result = new();
            Tile tile;
            if (tiles.TryGetValue(start.ShiftX(1), out tile)) result.Add(tile);
            if (tiles.TryGetValue(start.ShiftX(-1), out tile)) result.Add(tile);
            if (tiles.TryGetValue(start.ShiftY(1), out tile)) result.Add(tile);
            if (tiles.TryGetValue(start.ShiftY(-1), out tile)) result.Add(tile);

            return result.Where(r => r.Location != disallowed).ToList();
        }

        public override string ToString()
        {
            return $"{Length} to {End}";
        }
    }

    public struct Route
    {
        public long Length = 0;
        public Intersection[] Visited;
        public Intersection Location;

        public Route(Intersection location, long pathLength, Intersection[] visited)
        {
            Location = location;
            Length = pathLength;
            Visited = visited;
        }

        public Route VisitNext(Path next)
        {
            return new Route(next.End, Length + next.Length, Visited.Append(Location).ToArray());
        }
    }

    public long FindRoute(bool part2)
    {
        var current = new Route(Start, 0, new[] { Start });

        Queue<Route> routes = new();
        routes.Enqueue(current);

        List<Route> finished = new();
        while (routes.Count > 0)
        {
            current = routes.Dequeue();

            if (current.Location == End)
            {
                finished.Add(current);
                continue;
            }

            var availablePaths = current.Location.Paths;
            if (!part2) availablePaths = availablePaths.Where(p => !p.Part2).ToList();

            foreach (var path in availablePaths)
            {
                var newRoute = current.VisitNext(path);

                if (!newRoute.Visited.Contains(newRoute.Location))
                    routes.Enqueue(newRoute);
            }
        }

        return finished.Max(r => r.Length);
    }

    public object GetResult1()
    {
        return FindRoute(false);
    }

    public object GetResult2()
    {
        return FindRoute(true);
    }
}
