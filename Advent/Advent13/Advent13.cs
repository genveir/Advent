using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.Advent13
{
    public enum Direction { North, East, South, West }

    class Advent13Solution
    {
        public static List<Cart> Carts;

        void ParseInput()
        {
            string resourceName = "Advent.Input.Advent13Input.txt";
            //resourceName = "Advent.Input.a13test.txt";
            var input = typeof(Program).Assembly.GetManifestResourceStream(resourceName);

            Carts = new List<Cart>();

            var tracks = new List<TrainTrack>();
            using (var txt = new StreamReader(input))
            {
                int y = 0;
                while (!txt.EndOfStream)
                {
                    var line = txt.ReadLine();
                    for (int x = 0; x < line.Length; x++)
                    {
                        var track = TrainTrack.Parse(x, y, line[x]);
                        if (track != null) tracks.Add(track);
                    }

                    y++;
                }
            }

            TrainTrack.LinkCarts(Carts);
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
