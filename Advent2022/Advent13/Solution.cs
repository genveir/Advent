using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using Newtonsoft.Json.Linq;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Advent2022.AdventActive
{
    public class Solution : ISolution
    {
        public List<PacketPair> packetPairs;

        public Solution(string input)
        {
            var blocks = Input.GetBlockLines(input).ToArray();

            PacketPair.indexCounter = 1;
            packetPairs = blocks.Select(b => new PacketPair(b)).ToList();
        }
        public Solution() : this("Input.txt") { }

        public class PacketPair
        {
            public static int indexCounter = 1;
            public int index;

            public Packet Left;
            public Packet Right;

            public PacketPair(string[] pair)
            {
                index = indexCounter++;

                Left = Packet.Parse(pair[0]);
                Right = Packet.Parse(pair[1]);
            }

            public bool AreInTheRightOrder()
            {
                var comparison = Left.CompareTo(Right);                

                return comparison == 1;
            }

            public override string ToString()
            {
                return $"{Left}{Environment.NewLine}{Right}";
            }
        }

        public object GetResult1()
        {
            var inOrder = packetPairs.Where(pp => pp.AreInTheRightOrder());

            return inOrder.Sum(pp => pp.index);
        }

        public object GetResult2()
        {
            var allPackets = packetPairs.SelectMany(p => new[] { p.Left, p.Right }).ToList();

            var dividers = new PacketPair(new[] { "[[2]]", "[[6]]" });
            allPackets.Add(dividers.Left);
            allPackets.Add(dividers.Right);

            var inOrder = allPackets.OrderByDescending(p => p).ToArray();

            List<int> dividerPositions = new();
            for (int n = 1; n <= allPackets.Count; n++)
            {
                var packet = inOrder[n - 1];

                if (packet.Equals(dividers.Left) || packet.Equals(dividers.Right))
                    dividerPositions.Add(n);
            }

            return dividerPositions[0] * dividerPositions[1];
        }
    }
}
