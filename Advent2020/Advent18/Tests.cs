using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2020.Advent18
{
    class Tests
    {
        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [TestCase("2 * 3 + (4 * 5)", 26)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [TestCase("2 * 3 + (4 * 5)", 46)]
        [TestCase("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [TestCase("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [TestCase("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void CanParseSingleLong()
        {
            string expression = "1";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(1, parsed.Evaluate());
        }

        [Test]
        public void CanParseLongInParenthesis()
        {
            string expression = "(1)";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(1, parsed.Evaluate());
        }

        [Test]
        public void CanParseSum()
        {
            string expression = "1+2";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(3, parsed.Evaluate());
        }

        [Test]
        public void CanParseProduct()
        {
            string expression = "1*2";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(2, parsed.Evaluate());
        }        

        [Test]
        public void CanParseMultipleOperators()
        {
            string expression = "1+1+1";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(3, parsed.Evaluate());
        }

        [Test]
        public void CanParseSubExpressions()
        {
            string expression = "(1*1)+(1*1)";

            var parsed = Solution.ParseExpression(expression);

            Assert.AreEqual(2, parsed.Evaluate());
        }

        [Test]
        public void ParsingIsLeftAssociative()
        {
            var expression = Solution.ParseExpression("1+2*3");
            var exprLeft = Solution.ParseExpression("(1+2)*3");
            var exprRight = Solution.ParseExpression("1+(2*3)");

            Assert.AreNotEqual(exprRight.Evaluate(), expression.Evaluate());
            Assert.AreEqual(exprLeft.Evaluate(), expression.Evaluate());
            Assert.AreEqual(9, expression.Evaluate());
        }

        [Test]
        public void CanRewrite()
        {
            var expression = Solution.ParseExpression("6 * 2 + 3");

            Assert.AreEqual(15, expression.Evaluate());

            var rewrite = expression.Rewrite();

            Assert.AreEqual(30, rewrite.Evaluate());
        }
    }
}
