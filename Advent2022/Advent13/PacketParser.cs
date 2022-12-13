using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.AdventActive
{
    public static class PacketParser
    {
        public static int index;
        public static string data;

        public static Packet Parse(string data)
        {
            index = 0;
            PacketParser.data = data;

            return ParseList();
        }

        public static Packet ParseList()
        {
            index++;

            List<Packet> subPackets = new List<Packet>();
            do
            {
                if (data[index] == ']')
                {
                    index++;
                    break;
                }

                if (data[index] == '[')
                {
                    subPackets.Add(ParseList());
                }
                else
                {
                    subPackets.Add(ParseValue());
                }
            } while (data[index++] == ',');

            return new Packet() { IsValue = false, SubPackets = subPackets };
        }

        public static Packet ParseValue()
        {
            List<char> intData = new();

            while (data[index] != ',' && data[index] != ']') intData.Add(data[index++]);

            var value = long.Parse(new string(intData.ToArray()));

            return new Packet() { IsValue = true, Value = value };
        }
    }
}
