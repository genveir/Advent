using Advent2019.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent20
{
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
            foreach (var neighbour in neighbours)
            {
                neighbour.neighbours.Remove(this);
            }
            this.neighbours = null;
        }

        public override string ToString()
        {
            return "PortalTile " + name + " " + coord;
        }
    }

    public class ProvisionalPortal : PortalTile
    {
        public ProvisionalPortal(PortalTile portal) : base(portal.coord, 'a')
        {
            this.name = portal.name;
            foreach (var neighbour in portal.neighbours) LinkTo(neighbour);
        }

        public override string ToString()
        {
            return "PROV_" + base.ToString();
        }
    }
}
