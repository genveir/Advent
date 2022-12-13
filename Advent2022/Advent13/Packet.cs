using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent2022.AdventActive.Solution;

namespace Advent2022.Advent13
{
    public class Packet : IComparable<Packet>
    {
        public long Value { get; set; }
        public bool IsValue { get; set; }

        public List<Packet> SubPackets { get; set; } = new();

        public static Packet Parse(string data)
        {
            return PacketParser.Parse(data);
        }

        public override int GetHashCode()
        {
            return (int)Value * SubPackets.Count * SubPackets.Select(sp => sp.GetHashCode()).Sum();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Packet;

            if (other == null) return false;
            if (IsValue != other.IsValue) return false;
            if (IsValue) return Value == other.Value;

            if (other.SubPackets.Count != SubPackets.Count) return false;
            for (int n = 0; n < SubPackets.Count; n++)
            {
                if (!other.SubPackets[n].Equals(SubPackets[n])) return false;
            }
            return true;
        }

        public override string ToString()
        {
            return IsValue ? Value.ToString() :
                $"[{string.Join(",", SubPackets)}]";
        }

        public int CompareTo(Packet other)
        {
            var leftItems = SubPackets.ToArray();
            var rightItems = other.SubPackets.ToArray();

            for (int n = 0; n < leftItems.Length; n++)
            {
                if (n == rightItems.Length) return -1;
                var leftValue = leftItems[n];
                var rightValue = rightItems[n];

                if (leftValue.IsValue && rightValue.IsValue)
                {
                    if (leftValue.Value < rightValue.Value) return 1;
                    if (leftValue.Value > rightValue.Value) return -1;
                }
                else if (!leftValue.IsValue && !rightValue.IsValue)
                {
                    var listCompareResult = leftValue.CompareTo(rightValue);
                    if (listCompareResult != 0) return listCompareResult;
                }
                else if (leftValue.IsValue)
                {
                    var asList = new Packet() { SubPackets = new List<Packet>() { leftValue } };

                    var listCompareResult = asList.CompareTo(rightValue);
                    if (listCompareResult != 0) return listCompareResult;
                }
                else if (rightValue.IsValue)
                {
                    var asList = new Packet() { SubPackets = new List<Packet>() { rightValue } };

                    var listCompareResult = leftValue.CompareTo(asList);
                    if (listCompareResult != 0) return listCompareResult;
                }
            }
            if (rightItems.Length == leftItems.Length) return 0;
            return 1; // left ran out
        }
    }
}
