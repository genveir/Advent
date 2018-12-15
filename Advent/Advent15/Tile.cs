using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.Advent15
{
    public enum TileType { Floor, Wall, Elf, Goblin }

    public static class TileTypeExtension
    {
        public static TileType Opponent(this TileType input)
        {
            switch(input)
            {
                case TileType.Elf: return TileType.Goblin;
                case TileType.Goblin: return TileType.Elf;
                default: return TileType.Floor;
            }
        }
    }

    class Tile
    {
        public TileType Type;

        public XYCoord coord;
        // inserts zijn in order (N, W, E, S)
        protected List<Tile> Neighbours;

        public Tile(XYCoord coord, Tile north, Tile west, TileType type)
        {
            HP = 0;
            AP = 0;
            Type = type;

            this.coord = coord;
            Neighbours = new List<Tile>();

            if (north != null) Link(north, true);
            if (west != null) Link(west, true);
        }

        public void Link(Tile tile, bool backlink)
        {
            Neighbours.Add(tile);
            if (backlink) tile.Link(this, false);
        }

        public int HP;
        public int AP;
        public bool IsAlive { get { return HP > 0; } }
        public bool IsStaticTestGuy { get; set; }

        // returnt true als het iets killt
        public bool Attack()
        {
            var target = Neighbours.Where(n => n.Type == this.Type.Opponent()).OrderBy(n => n.HP).Distinct().FirstOrDefault();
            if (target != null)
            {
                target.HP -= AP;
                if (target.HP < 0)
                {
                    target.HP = 0;
                    target.Type = TileType.Floor;
                    return true;
                }
            }
            return false;
        }

        public Tile WalkTo(Tile tile)
        {
            tile.HP = this.HP;
            tile.AP = this.AP;
            tile.Type = this.Type;

            this.HP = 0;
            this.AP = 0;
            this.Type = TileType.Floor;

            return tile;
        }

        public Tile Move()
        {
            if (IsStaticTestGuy) return this;

            var tile = GetPathToNearest(this.Type.Opponent());

            if (tile != null) return WalkTo(tile);
            else return this;
        }

        public int FoundBy;
        private static int FindCounter = 0;
        public int Depth;

        public Tile GetPathToNearest(TileType tileType)
        {
            FindCounter++;

            var front = new Queue<Tile>();

            FoundBy = FindCounter;
            Depth = 0;

            foreach (var n in Neighbours)
            {
                if (n.Type == tileType) return null;

                if (n.Type == TileType.Floor)
                {
                    n.FoundBy = FindCounter;
                    n.Depth = 1;
                    front.Enqueue(n);
                }
            }

            var currentDepth = 0;
            Dictionary<Tile, Tile> TargetToPathMap = new Dictionary<Tile, Tile>();
            while (front.Count > 0)
            {
                var toEval = front.Dequeue();
                if (toEval.Depth != currentDepth)
                {
                    if (TargetToPathMap.Count != 0) return GetReturnValue(TargetToPathMap);
                    else currentDepth = toEval.Depth;
                }

                if (toEval.Type == tileType) TargetToPathMap.Add(toEval, toEval.RunDepthChain());
                if (toEval.Type == TileType.Floor)
                {
                    foreach (var n in toEval.Neighbours)
                    {
                        if (n.FoundBy != FindCounter)
                        {
                            n.FoundBy = FindCounter;
                            n.Depth = toEval.Depth + 1;
                            front.Enqueue(n);
                        }
                    }
                }
            }

            if (TargetToPathMap.Count != 0) return GetReturnValue(TargetToPathMap);
            return null;
        }

        private Tile GetReturnValue(Dictionary<Tile, Tile> TargetToPathMap)
        {
            return TargetToPathMap.OrderBy(o => o.Key.coord).First().Value;
        }

        public Tile RunDepthChain()
        {
            if (Depth == 1) return this;
            else return Neighbours
                    .Where(
                        n => n.FoundBy == FindCounter && 
                        n.Depth == Depth - 1 &&
                        n.Type == TileType.Floor)
                    .First()
                    .RunDepthChain();
        }

        public override string ToString()
        {
            return Type + " " + ((IsAlive) ? ("" + HP) : "") + " " + coord;
        }
    }

    class Goblin : Tile
    {
        public Goblin(XYCoord coord, Tile north, Tile west) : base(coord, north, west, TileType.Goblin)
        {
            HP = Solution.GOBLIN_HP;
            AP = Solution.GOBLIN_AP;
        }
    }

    class Elf : Tile
    {
        public Elf(XYCoord coord, Tile north, Tile west) : base(coord, north, west, TileType.Elf)
        {
            HP = Solution.ELF_HP;
            AP = Solution.ELF_AP;
        }
    }
}
