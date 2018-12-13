using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent13
{
    public enum Direction { North, East, South, West }

    static class DirectionExtension
    {
        public static Direction Opposite(this Direction input)
        {
            switch (input)
            {
                case Direction.North: return Direction.South;
                case Direction.East: return Direction.West;
                case Direction.South: return Direction.North;
                default: return Direction.East;
            }
        }
    }

    class Solution : ISolution
    {
        public List<Cart> Carts;

        void ParseInput()
        {
            string resourceName = "Advent.Advent13.Input.txt";
            //resourceName = "Advent.Input.a13test.txt";
            var input = this.GetType().Assembly.GetManifestResourceStream(resourceName);

            var factory = new TrackFactory();
            Carts = new List<Cart>();

            using (var txt = new StreamReader(input))
            {
                int y = 0;
                while (!txt.EndOfStream)
                {
                    var line = txt.ReadLine();
                    for (int x = 0; x < line.Length; x++)
                    {
                        Parse(factory, x, y, line[x]);
                    }
                    y++;
                }
            }
        }

        private void Parse(TrackFactory factory, int x, int y, char input)
        {
            var coord = new XYCoord(x, y);

            Cart cart = null;
            TrainTrack track = null;
            switch (input)
            {
                case '<': cart = new Cart(coord, Direction.West); input = '-'; break;
                case '>': cart = new Cart(coord, Direction.East); input = '-'; break;
                case '^': cart = new Cart(coord, Direction.North); input = '|'; break;
                case 'v': cart = new Cart(coord, Direction.South); input = '|'; break;
            }

            if (input != ' ') track = factory.Parse(coord, input);

            if (cart != null)
            {
                Carts.Add(cart);
                cart.track = track;
            }
        }

        public void WriteResult()
        {
            ParseInput();

            var turn = 0;
            while(true)
            {
                var carts = Carts.OrderBy(c => c.track.coord).ToList();

                foreach (var cart in carts)
                {
                    if (!Carts.Contains(cart)) continue; // al gecrasht deze beurt

                    var crash = cart.Move();
                    if (crash)
                    {
                        Console.WriteLine("crash in turn " + turn + " at coord " + cart.track.coord);
                        Carts.Remove(cart);
                        var otherCart = Carts.Where(c => c.track.coord.Equals(cart.track.coord)).Single();
                        Carts.Remove(otherCart);
                        Cart.RemoveFrom(cart.track.coord);

                        Console.WriteLine(Carts.Count + " carts left");
                    }
                }

                if (Carts.Count == 1)
                {
                    Console.WriteLine("last cart is " + Carts.Single());
                    return;
                }

                turn++;
            }
        }
    }
}
