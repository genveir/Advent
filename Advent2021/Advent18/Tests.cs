using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent18
{
    class Tests
    {
        [TestCase(example, 4140)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 3993)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [TestCase("[1,2]")]
        [TestCase("[[1,2],3]")]
        [TestCase("[1,[2,3]]")]
        [TestCase("[[[[[9,8],1],2],3],4]")]
        [TestCase("[7,[6,[5,[4,[3,2]]]]]")]
        [TestCase("[[6,[5,[4,[3,2]]]],1]")]
        [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]")]
        [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
        public void ParseWorks(string input)
        {
            var sol = new Solution(input);

            Assert.AreEqual(input, sol.wholeValues.Single().pair.ToString());
        }

        [TestCase("[1,2]", "[1,2]")]
        [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
        [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        public void ExplodeWorks(string input, string expected)
        {
            var sol = new Solution(input);

            var wv = sol.wholeValues.Single();

            wv.pair.DoExplosion(0);

            Assert.AreEqual(expected, wv.pair.ToString());
        }

        [TestCase(addExample1, "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
        [TestCase(addExample2, "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
        [TestCase(addExample3, "[[[[5,0],[7,4]],[5,5]],[6,6]]")]
        [TestCase(addExmaple4, "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]")]
        public void AggregateWorks(string input, string expected)
        {
            var sol = new Solution(input);

            var agg = sol.AggregateAdd();

            Assert.AreEqual(expected, agg.pair.ToString());
        }

        [Test]
        public void Testp2Case()
        {
            var fline = "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]";
            var sline = "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]";

            var first = Parser.ParseLine(fline);
            var second = Parser.ParseLine(sline);

            var sum = WholeValue.Add(first, second);

            Assert.AreEqual("[[[[7,8],[6,6]],[[6,0],[7,7]]],[[[7,8],[8,8]],[[7,9],[0,6]]]]", sum.pair.ToString());

            Assert.AreEqual(3993, sum.Magnitude);
        }

        public const string addExample1 = @"[1,1]
[2,2]
[3,3]
[4,4]";

        public const string addExample2 = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]";

        public const string addExample3 = @"[1,1]
[2,2]
[3,3]
[4,4]
[5,5]
[6,6]";

        public const string addExmaple4 = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";

        public const string example = @"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
[[[5,[2,8]],4],[5,[[9,9],0]]]
[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
[[[[5,4],[7,7]],8],[[8,3],8]]
[[9,3],[[9,9],[6,[4,9]]]]
[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
    }
}
