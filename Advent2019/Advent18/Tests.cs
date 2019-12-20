using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Advent18
{
    class Tests
    {
        public const string small = @"#########
#b.A.@.a#
#########";

        public const string larger = @"########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################";

        public const string more1 = @"########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################";

        public const string more2 = @"#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################";

        public const string more3 = @"########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################";

        [TestCase(small, "8")]
        [TestCase(larger, "86")]
        [TestCase(more1, "132")]
        [TestCase(more2, "136")]
        [TestCase(more3, "81")]
        public void Test1(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("", "2140")]
        public void Test2(string input, string output)
        {
            var sol = new Solution(Shared.Input.InputMode.String, input);

            Assert.AreEqual(output, sol.GetResult2());
        }
    }
}
