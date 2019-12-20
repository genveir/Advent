using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent20
{
    public class Solution : ISolution
    {
        Tile start;
        Tile end;

        string[] lines;

        List<ProvisionalPortal> provPortals;

        public Solution(Input.InputMode inputMode, string input)
        {
            lines = Input.GetInputLines(inputMode, input).ToArray();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Parse(bool levelZero, bool recursive)
        {
            var allTiles = new Dictionary<Coordinate, Tile>();
            var partialPortals = new List<PortalTile>();

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var coord = new Coordinate(x, y);

                    Tile tile = null;
                    if (lines[y][x] == '.')
                    {
                        tile = new RegularTile(coord);
                    }
                    if (lines[y][x] >= 'A' && lines[y][x] <= 'Z')
                    {
                        var ptile = new PortalTile(coord, lines[y][x]);
                        partialPortals.Add(ptile);
                        tile = ptile;
                    }

                    Tile toTheLeft = null;
                    Tile up = null;
                    allTiles.TryGetValue(new Coordinate(x - 1, y), out toTheLeft);
                    allTiles.TryGetValue(new Coordinate(x, y - 1), out up);

                    if (tile != null)
                    {
                        if (toTheLeft != null) tile.LinkTo(toTheLeft);
                        if (up != null) tile.LinkTo(up);
                    }

                    allTiles.Add(coord, tile);
                }
            }

            if (recursive)
            {
                ParseRecursePortals(partialPortals, levelZero);
            }
            else
            {
                ParseRegularPortals(partialPortals);
            }
        }

        public void ParseRegularPortals(List<PortalTile> partialPortals)
        {
            var actualPortals = partialPortals.Where(pp => pp.name.Length == 2);
            var groupedPortals = actualPortals.GroupBy(ap => ap.name);

            foreach(var portalGroup in groupedPortals)
            {
                if (portalGroup.Key == "AA")
                {
                    start = portalGroup.Single().neighbours.Single();
                    portalGroup.Single().DeLink();
                }
                else if (portalGroup.Key == "ZZ")
                {
                    end = portalGroup.Single().neighbours.Single();
                    portalGroup.Single().DeLink();
                }
                else
                {
                    var ptiles = portalGroup.ToList();
                    ptiles[0].LinkTo(ptiles[1]);
                    var fullPortal = ptiles[0];

                    var neighbours = fullPortal.neighbours.ToList();
                    neighbours[0].LinkTo(neighbours[1]);
                    fullPortal.DeLink();
                }
            }
        }

        public void ParseRecursePortals(List<PortalTile> partialPortals, bool levelZero)
        {
            var actualPortals = partialPortals.Where(pp => pp.name.Length == 2);

            var minY = actualPortals.Min(ap => ap.coord.Y);
            var maxY = actualPortals.Max(ap => ap.coord.Y);
            var minX = actualPortals.Min(ap => ap.coord.X);
            var maxX = actualPortals.Max(ap => ap.coord.X);

            var outerPortals = actualPortals.Where(ap =>
                ap.coord.X == minX || ap.coord.X == maxX ||
                ap.coord.Y == minY || ap.coord.Y == maxY)
                .ToList();

            foreach (var portal in outerPortals)
            {
                if (portal.name == "AA")
                {
                    if (levelZero) start = portal.neighbours.Single();

                    portal.DeLink();
                }
                else if (portal.name == "ZZ")
                {
                    if (levelZero) end = portal.neighbours.Single();
                    portal.DeLink();
                }
                else
                {
                    if (levelZero) portal.DeLink();
                    else
                    {
                        var provPortal = provPortals.Single(pp => pp.name == portal.name);
                        portal.LinkTo(provPortal);

                        var neighbours = portal.neighbours.ToList();
                        neighbours[0].LinkTo(neighbours[1]);
                        portal.DeLink();
                    }
                }
            }

            provPortals = new List<ProvisionalPortal>();
            var innerPortals = actualPortals.Except(outerPortals);

            foreach(var portal in innerPortals)
            {
                var provPortal = new ProvisionalPortal(portal);
                portal.DeLink();
                provPortals.Add(provPortal);
            }
        }

        

        public static int searchnum = 1;
        public int Search(bool recurse)
        {
            Parse(true, recurse);

            var tiles = new Queue<Tile>();
            start.searchnum = ++searchnum;
            tiles.Enqueue(start);

            while (tiles.Count > 0)
            {
                var tile = tiles.Dequeue();

                if (tile.coord == end.coord) return tile.distance;

                if (tile.neighbours.Any(n => n is ProvisionalPortal)) Parse(false, true);

                foreach (var neighbour in tile.neighbours)
                {
                    if (neighbour.searchnum != searchnum)
                    {
                        neighbour.searchnum = searchnum;
                        neighbour.distance = tile.distance + 1;
                        tiles.Enqueue(neighbour);
                    }
                }
            }

            return -1;
        }


        public string GetResult1()
        {
            return Search(false).ToString();
        }

        public string GetResult2()
        {
            return Search(true).ToString();
        }
    }
}
