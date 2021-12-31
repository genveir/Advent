using Advent2021.Advent24.Constraints;
using Advent2021.Advent24.Expressions;
using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent24.Tests
{
    class General
    {
        [TestCase(example, "")]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            var str = sol.Z.Value.PrintToDepth(10);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, "")]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        //  (SET[[0.1]: 18][[0.2]: 19][[0.3]: 20][[0.4]: 21][[0.5]: 22][[0.6]: 23][[0.7]: 24][[0.8]: 25][[0.9]: 26]) 
        //  == 
        //  (SET[[1.1]: 1][[1.2]: 2][[1.3]: 3][[1.4]: 4][[1.5]: 5][[1.6]: 6][[1.7]: 7][[1.8]: 8][[1.9]: 9])
        [Test]
        public void CollapseDisjunctEquality()
        {
            var set1 = ParseSimpleSetFromOutput("(SET[[0.1]: 18][[0.2]: 19][[0.3]: 20][[0.4]: 21][[0.5]: 22][[0.6]: 23][[0.7]: 24][[0.8]: 25][[0.9]: 26])");
            var set2 = ParseSimpleSetFromOutput("(SET[[1.1]: 1][[1.2]: 2][[1.3]: 3][[1.4]: 4][[1.5]: 5][[1.6]: 6][[1.7]: 7][[1.8]: 8][[1.9]: 9])");

            var expr = new Eql(set1, set2);

            var collapsed = expr.Simplify();

            Assert.That(collapsed.IsEquivalentTo(new Constant(0, Constraint.None()), true));
        }

        private Set ParseSimpleSetFromOutput(string output)
        {
            var parts = output.Split(new char[] { '(', ')', 'S', 'E', 'T', '[', ']', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var elements = new Constant[9];
            for (int n = 0; n < 18; n += 2)
            {
                var constraintVals = parts[n].Split(".").Select(int.Parse).ToArray();
                var constraint = new AndConstraint(constraintVals[0], constraintVals[1]);

                var value = int.Parse(parts[n + 1]);

                elements[n / 2] = new Constant(value, constraint);
            }

            return new Set(elements);
        }

        public const string example = @"inp w
mul x 0
add x z
mod x 26
div z 1
add x 11
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y 6
mul y x
add z y
inp w
mul x 0
add x z
mod x 26
div z 1
add x 11
eql x w
eql x 0
mul y 0
add y 25
mul y x
add y 1
mul z y
mul y 0
add y w
add y 12
mul y x
add z y";
    }
}
