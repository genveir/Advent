using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent16
{
    public class Solution : ISolution
    {
        Packet basePacket;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var bitString = string.Join("", lines.Single().Select(c =>
            {
                return c switch
                {
                    '0' => "0000",
                    '1' => "0001",
                    '2' => "0010",
                    '3' => "0011",
                    '4' => "0100",
                    '5' => "0101",
                    '6' => "0110",
                    '7' => "0111",
                    '8' => "1000",
                    '9' => "1001",
                    'A' => "1010",
                    'B' => "1011",
                    'C' => "1100",
                    'D' => "1101",
                    'E' => "1110",
                    'F' => "1111",
                    _ => "",
                };
            }));

            basePacket = new Packet(bitString, 0);
        }
        public Solution() : this("Input.txt") { }

        public class Packet
        {
            int version;
            int typeId;

            List<string> numbers = new List<string>();
            List<Packet> subPackets = new List<Packet>();

            bool visited;

            static string bitString;
            static int cursor;

            [ComplexParserConstructor]
            public Packet(string bitString, int cursor)
            {
                Packet.bitString = bitString;
                Packet.cursor = cursor;

                ReadPacket();
            }

            public Packet()
            {
                ReadPacket();
            }

            public Packet ReadPacket()
            {
                // the first three bits encode the packet version, and 

                // the next three bits encode the packet type ID.

                //These two values are numbers; all numbers encoded in any packet are represented as binary with the most significant bit first
                version = readnum(3);
                typeId = readnum(3);

                switch (typeId)
                {
                    case 4: ReadNumbers(); break;
                    default: ReadOperator(); break;
                }

                return this;
            }

            public int sumVersions()
            {
                return version + subPackets.Sum(sp => sp.sumVersions());
            }

            public long GetValue()
            {
                switch(typeId)
                {
                    case 0: return subPackets.Sum(sp => sp.GetValue());
                    case 1: return subPackets.Select(sp => sp.GetValue()).Aggregate((a, b) => a * b);
                    case 2: return subPackets.Min(sp => sp.GetValue());
                    case 3: return subPackets.Max(sp => sp.GetValue());
                    case 4: return Convert.ToInt64(string.Join("", numbers), 2);
                    case 5: return subPackets[0].GetValue() > subPackets[1].GetValue() ? 1 : 0;
                    case 6: return subPackets[0].GetValue() < subPackets[1].GetValue() ? 1 : 0;
                    case 7: return subPackets[0].GetValue() == subPackets[1].GetValue() ? 1 : 0;
                    default: throw new InvalidOperationException();
                }
            }

            public string read(int num)
            {
                var result = bitString.Substring(cursor, num);
                cursor += num;

                return result;
            }

            public int readnum(int num) => Convert.ToInt32(read(num), 2);

            public void ReadNumbers()
            {
                var header = read(1);

                bool continueAfter = header == "1";

                numbers.Add(read(4));

                if (continueAfter) ReadNumbers();
            }

            public void ReadOperator()
            {
                var lengthTypeId = read(1);

                // If the length type ID is 0, then the next 15 bits are a number
                // that represents the total length in bits of the sub-packets contained by this packet.
                if (lengthTypeId == "0")
                {
                    var readUntil = readnum(15) + cursor;
                    while (cursor < readUntil)
                    {
                        subPackets.Add(new Packet());
                    }
                }

                // If the length type ID is 1, then the next 11 bits are a number that represents
                // the number of sub-packets immediately contained by this packet.
                else
                {
                    var numPackets = readnum(11);

                    for (int n = 0; n < numPackets; n++)
                    {
                        subPackets.Add(new Packet());
                        
                    }
                }
            }

            public override string ToString()
            {
                return $"Packet {version} {typeId}";
            }
        }

        public object GetResult1()
        {
            return basePacket.sumVersions();
        }

        public object GetResult2()
        {
            return basePacket.GetValue();
        }
    }
}
