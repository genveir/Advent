using Advent2022.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Advent2022.Advent13
{
    class Tests
    {
        private static object[][] TestCases = new object[][]
        {
            new object[] {"[]", new Packet() },
            new object[] {"[3]",
                new Packet()
                {
                    SubPackets = new()
                    {
                        new() {IsValue = true, Value = 3}
                    }
                }
            },
            new object[] {"[3,4]",
                new Packet()
                {
                    SubPackets = new()
                    {
                        new() {IsValue = true, Value = 3},
                        new() {IsValue = true, Value = 4}
                    }
                }
            },
            new object[] {"[[1,2,3],19,[4,5,6]]",
                new Packet()
                {
                    SubPackets = new()
                    {
                        new()
                        {
                            SubPackets = new()
                            {
                                new() {IsValue = true, Value = 1},
                                new() {IsValue = true, Value = 2},
                                new() {IsValue = true, Value = 3}
                            }
                        },
                        new() {IsValue = true, Value = 19},
                        new()
                        {
                            SubPackets = new()
                            {
                                new() {IsValue = true, Value = 4},
                                new() {IsValue = true, Value = 5},
                                new() {IsValue = true, Value = 6}
                            }
                        }
                    }
                }
            }
        };

        [TestCase(example, 13)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 140)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [TestCaseSource(nameof(TestCases))]
        public void ParsingTests(string data, Packet packet)
        {
            var result = Packet.Parse(data);

            Assert.AreEqual(packet, result);
        }

        [TestCase("[]", "[3]", true)] // left runs out
        [TestCase("[3]", "[]", false)] // right runs out
        [TestCase("[1,2,3]", "[1,2,3,4]", true)] // left runs out
        [TestCase("[1,2,3,4]", "[1,2,3]", false)] // right runs out
        [TestCase("[1]", "[2]", true)] // left is lower
        [TestCase("[2]", "[1]", false)] // right is lower
        [TestCase("[1,1]", "[1,2]", true)] // left is lower
        [TestCase("[1,2]", "[1,1]", false)] // right is lower
        [TestCase("[1,2,3,4,[1,1]]", "[1,2,3,4,[1,2]]", true)] // left is lower
        [TestCase("[1,2,3,4,[1,2]]", "[1,2,3,4,[1,1]]", false)] // right is lower
        [TestCase("[1]", "[[2]]", true)] // value-fallthrough right
        [TestCase("[[2]]", "[1]", false)] // value-fallthrough left
        [TestCase("[1,2]", "[1,[3]]", true)] // value-fallthrough right
        [TestCase("[1,[2]]", "[1,3]", true)] // value-fallthrough left
        [TestCase("[1,2,3]", "[1,[2,3]]", true)] // single value fallthrough
        [TestCase("[1,[2,3]]", "[1,2,3]", false)] // single value fallthrough
        [TestCase("[[1],1]", "[[1],2]", true)] // continue after equal list
        [TestCase("[[1],2]", "[[1],1]", false)] // continue after equal list
        public void ComparisonTests(string first, string second, bool inOrder)
        {
            var pair = new Solution.PacketPair(new[] { first, second });

            var comparison = pair.AreInTheRightOrder();

            Assert.AreEqual(inOrder, comparison);
        }

        public const string example = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
    }
}
