using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent22
{
    class Tests
    {
        [TestCase(XOnly, 1)]
        [TestCase(XAndY, 5)]
        [TestCase(smallExample, 39)]
        [TestCase(example, 590784)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, "")]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        public const string XOnly = @"on x=10..12,y=1..1,z=1..1
off x=11..13,y=1..1,z=1..1";

        [Test]
        public void CanDoSimpleSplit()
        {
            var sol = new Solution(XOnly);

            var output = sol.cubes[0].SplitOnOverlap(sol.cubes[1]).Single();

            // remainder without the overlap
            Assert.AreEqual(10, output.minX);
            Assert.AreEqual(10, output.maxX);
            Assert.AreEqual(true, output.on);

            // unaltered 1
            Assert.AreEqual(11, sol.cubes[1].minX);
            Assert.AreEqual(13, sol.cubes[1].maxX);
            Assert.AreEqual(false, sol.cubes[1].on);
        }

        [Test]
        public void CanDoSimpleTheOtherWay()
        {
            var sol = new Solution(XOnly);

            var output = sol.cubes[1].SplitOnOverlap(sol.cubes[0]).Single();

            // remainder without the overlap
            Assert.AreEqual(13, output.minX);
            Assert.AreEqual(13, output.maxX);
            Assert.AreEqual(false, output.on);

            // unaltered 1
            Assert.AreEqual(10, sol.cubes[0].minX);
            Assert.AreEqual(12, sol.cubes[0].maxX);
            Assert.AreEqual(true, sol.cubes[0].on);
        }

        public const string XAndY = @"on x=10..12,y=10..12,z=1..1
off x=11..13,y=11..13,z=1..1";

        [Test]
        public void CanDoPlaneSplit()
        {
            var sol = new Solution(XAndY);

            var output = sol.cubes[0].SplitOnOverlap(sol.cubes[1]);

            Assert.AreEqual(2, output.Count);

            // bar with x-remainder
            var xRemainder = output[0];
            Assert.AreEqual(10, xRemainder.minX);
            Assert.AreEqual(10, xRemainder.maxX);
            Assert.AreEqual(10, xRemainder.minY);
            Assert.AreEqual(12, xRemainder.maxY);
            Assert.AreEqual(true, xRemainder.on);

            // bar with y-remainder
            var yRemainder = output[1];
            Assert.AreEqual(11, yRemainder.minX);
            Assert.AreEqual(12, yRemainder.maxX);
            Assert.AreEqual(10, yRemainder.minY);
            Assert.AreEqual(10, yRemainder.maxY);
            Assert.AreEqual(true, yRemainder.on);

            // unaltered 1
            Assert.AreEqual(11, sol.cubes[1].minX);
            Assert.AreEqual(13, sol.cubes[1].maxX);
            Assert.AreEqual(11, sol.cubes[1].minY);
            Assert.AreEqual(13, sol.cubes[1].maxY);
            Assert.AreEqual(false, sol.cubes[1].on);
        }

        public const string example = @"on x=-20..26,y=-36..17,z=-47..7
on x=-20..33,y=-21..23,z=-26..28
on x=-22..28,y=-29..23,z=-38..16
on x=-46..7,y=-6..46,z=-50..-1
on x=-49..1,y=-3..46,z=-24..28
on x=2..47,y=-22..22,z=-23..27
on x=-27..23,y=-28..26,z=-21..29
on x=-39..5,y=-6..47,z=-3..44
on x=-30..21,y=-8..43,z=-13..34
on x=-22..26,y=-27..20,z=-29..19
off x=-48..-32,y=26..41,z=-47..-37
on x=-12..35,y=6..50,z=-50..-2
off x=-48..-32,y=-32..-16,z=-15..-5
on x=-18..26,y=-33..15,z=-7..46
off x=-40..-22,y=-38..-28,z=23..41
on x=-16..35,y=-41..10,z=-47..6
off x=-32..-23,y=11..30,z=-14..3
on x=-49..-5,y=-3..45,z=-29..18
off x=18..30,y=-20..-8,z=-3..13
on x=-41..9,y=-7..43,z=-33..15
on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
on x=967..23432,y=45373..81175,z=27513..53682";

        public const string smallExample = @"on x=10..12,y=10..12,z=10..12
on x=11..13,y=11..13,z=11..13
off x=9..11,y=9..11,z=9..11
on x=10..10,y=10..10,z=10..10";
    }
}
