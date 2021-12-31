using Advent2021.Advent24.Constraints;
using Advent2021.Advent24.Expressions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent24.Tests
{
    public class AddTests
    {
        [Test]
        public void CanAddUnconstrainedConstants()
        {
            var left = new Constant(10, Constraint.None());
            var right = new Constant(2, Constraint.None());

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, Constraint.None()), true));
        }

        [Test]
        public void CanAddLeftSimpleConstrainedConstants()
        {
            var left = new Constant(10, new AndConstraint(0, 1));
            var right = new Constant(2, Constraint.None());

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, new AndConstraint(0, 1)), true));
        }

        [Test]
        public void CanAddRightSimpleConstrainedConstants()
        {
            var left = new Constant(10, Constraint.None());
            var right = new Constant(2, new AndConstraint(0, 1));

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, new AndConstraint(0, 1)), true));
        }

        [Test]
        public void CanAddConstantsWithSameSimpleConstraint()
        {
            var left = new Constant(10, new AndConstraint(0, 1));
            var right = new Constant(2, new AndConstraint(0, 1));

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, new AndConstraint(0, 1)), true));
        }

        [Test]
        public void CanAddConstantsWithDifferentSimpleConstraints()
        {
            var left = new Constant(10, new AndConstraint(0, 1));
            var right = new Constant(2, new AndConstraint(1, 5));

            var sum = new Add(left, right).Simplify();

            var constraint = new AndConstraint(0, 1).And(new AndConstraint(1, 5));

            Assert.That(sum.IsEquivalentTo(new Constant(12, constraint), true));
        }

        [Test]
        public void CanAddConstantsWithOrLeftAndNoneRight()
        {
            var or = BuildOrConstraint((0, 1), (1, 5));
            var left = new Constant(10, or);
            var right = new Constant(2, Constraint.None());

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, or), true));
        }

        [Test]
        public void CanAddConstantsWithOrRightAndNoneLeft()
        {
            var or = BuildOrConstraint((0, 1), (1, 5));
            var left = new Constant(10, Constraint.None());
            var right = new Constant(2, or);

            var sum = new Add(left, right).Simplify();

            Assert.That(sum.IsEquivalentTo(new Constant(12, or), true));
        }

        [Test]
        public void CanAddConstantsWithOrLeftAndMatchingSimpleRight()
        {
            var or = BuildOrConstraint((0, 1), (1, 5));
            var left = new Constant(10, or);
            var right = new Constant(2, new AndConstraint(0, 1));

            var sum = new Add(left, right).Simplify();

            var sumConstraints = new List<Constraint>()
            {
                or
            };
            var newOr = new OrConstraint(sumConstraints);

            Assert.That(sum.IsEquivalentTo(new Constant(12, newOr), true));
        }

        [Test]
        public void ConstructedExample()
        {
            var input = @"inp w
eql w 1
add x w
mul x 10
add y w
mul y 2
inp w
eql w 5
mul w 10";
            var sol = new Solution(input);

            var w = sol.W.Value.PrintToDepth(100);
            var x = sol.X.Value.PrintToDepth(100);
            var y = sol.Y.Value.PrintToDepth(100);
            var z = sol.Z.Value.PrintToDepth(100);

            var wrong = new Add(sol.X.Value, sol.W.Value);

            var simplify = wrong.Simplify();
        }

        private AndConstraint BuildAndConstraint(params (int input, int value)[] andConstraintValues)
        {
            var constraints = andConstraintValues.Select(acv => new AndConstraint(acv.input, acv.value));

            return constraints.Aggregate((a, b) => a.And(b));
        }

        private OrConstraint BuildOrConstraint(params (int input, int value)[] andConstraintValues)
        {
            return new OrConstraint(andConstraintValues.Select(acv => new AndConstraint(acv.input, acv.value)));
        }
    }
}
