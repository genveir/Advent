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

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            Parse(lines);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Parse(string[] lines)
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

        public abstract class Tile
        {
            public Coordinate coord;
            public List<Tile> neighbours;
            public int searchnum;
            public int distance;

            public Tile()
            {
                this.neighbours = new List<Tile>();
            }

            public virtual void LinkTo(Tile tile)
            {
                neighbours.Add(tile);
                tile.neighbours.Add(this);
            }
        }

        public class RegularTile : Tile
        {
            public RegularTile(Coordinate coord) : base()
            {
                this.coord = coord;       
            }

            public override string ToString()
            {
                return "Tile: " + coord.ToString();
            }
        }

        public class PortalTile : RegularTile
        {
            public string name;

            public PortalTile(Coordinate coord, char name) : base(coord)
            {
                this.name = name.ToString();
            }

            public override void LinkTo(Tile tile)
            {
                var other = tile as PortalTile;
                if (other != null)
                {
                    this.name = other.name + this.name;
                    this.neighbours.AddRange(other.neighbours);
                    other.DeLink();
                }
                else base.LinkTo(tile);
            }

            public void DeLink()
            {
                foreach(var neighbour in neighbours)
                {
                    neighbour.neighbours.Remove(this);
                }
                this.neighbours = null;
            }

            public override string ToString()
            {
                return "PortalTile " + name;
            }
        }

        public static int searchnum = 1;

        public string GetResult1()
        {
            var tiles = new Queue<Tile>();
            start.searchnum = ++searchnum;
            tiles.Enqueue(start);
            
            while (tiles.Count > 0)
            {
                var tile = tiles.Dequeue();

                if (tile.coord == end.coord) return tile.distance.ToString();

                foreach(var neighbour in tile.neighbours)
                {
                    if (neighbour.searchnum != searchnum)
                    {
                        neighbour.searchnum = searchnum;
                        neighbour.distance = tile.distance + 1;
                        tiles.Enqueue(neighbour);
                    }
                }
            }

            // not 4610

            return "no result";
        }

        public string GetResult2()
        {
            return "";
        }
    }
}
