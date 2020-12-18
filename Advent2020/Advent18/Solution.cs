using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2020.Advent18
{
    public class Solution : ISolution
    {
        public IEnumerable<Expression> expressions;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            expressions = lines
                .Select(line => ParseExpression(line))
                .ToList();
        }
        public Solution() : this("Input.txt") { }

        public static Expression ParseExpression(string stringRepresentation)
        {
            stringRepresentation = Regex.Replace(stringRepresentation, "\\s", "");

            return ParseExpression(stringRepresentation, 0, stringRepresentation.Length);
        }
        public static Expression ParseExpression(string stringRepresentation, int start, int end)
        {
            if (end - start == 1) return new LongExpression(stringRepresentation[start] - 48);

            int ellipses = 0;
            for (int n = end - 1; n >= start; n--)
            {
                char current = stringRepresentation[n];

                if (current == '(') ellipses++;
                else if (current == ')') ellipses--;
                else
                {
                    if (ellipses == 0)
                    {
                        if (current == '+') return new AddExpression(
                            ParseExpression(stringRepresentation, start, n),
                            ParseExpression(stringRepresentation, n + 1, end)
                            );
                        else if (current == '*') return new MultiplyExpression(
                            ParseExpression(stringRepresentation, start, n),
                            ParseExpression(stringRepresentation, n + 1, end)
                            );
                    }
                }
            }

            return new UnarySubExpression(ParseExpression(stringRepresentation, start + 1, end - 1));
        }

        public interface Expression
        {
            long Evaluate();

            Expression Rewrite();
        }

        public class UnarySubExpression : Expression
        {
            public Expression Expression;

            public UnarySubExpression(Expression expression)
            {
                this.Expression = expression;
            }

            public long Evaluate() { return Expression.Evaluate(); }

            public Expression Rewrite()
            {
                return new UnarySubExpression(Expression.Rewrite());
            }

            public override string ToString()
            {
                return "(" + Expression + ")";
            }
        }

        public class LongExpression : Expression
        {
            public long Value;

            public LongExpression(long value)
            {
                this.Value = value;
            }

            public long Evaluate() { return Value; }

            public Expression Rewrite() => this;

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class MultiplyExpression : Expression
        {
            public Expression left;
            public Expression right;

            public MultiplyExpression(Expression left, Expression right)
            {
                this.left = left;
                this.right = right;
            }

            public long Evaluate() { return left.Evaluate() * right.Evaluate(); }

            public Expression Rewrite()
            {
                var lAdd = left as AddExpression;

                return new MultiplyExpression(left.Rewrite(), right.Rewrite());
            }

            public override string ToString()
            {
                return "(" + left + " * " + right + ")";
            }
        }

        public class AddExpression : Expression
        {
            public Expression left;
            public Expression right;

            public AddExpression(Expression left, Expression right)
            {
                this.left = left;
                this.right = right;
            }

            public long Evaluate() { return left.Evaluate() + right.Evaluate(); }

            public Expression Rewrite()
            {
                var lMul = left as MultiplyExpression;

                if (lMul != null)
                {
                    return new MultiplyExpression(lMul.left.Rewrite(), new AddExpression(lMul.right.Rewrite(), this.right.Rewrite()));
                }

                return new AddExpression(left.Rewrite(), right.Rewrite());
            }

            public override string ToString()
            {
                return "(" + left + " + " + right + ")";
            }
        }

        public object GetResult1()
        {
            return expressions.Select(exp => exp.Evaluate()).Sum();
        }

        public object GetResult2()
        {
            for (int i = 0; i < 100; i++) expressions = expressions.Select(exp => exp.Rewrite());

            return expressions.Select(exp => exp.Evaluate()).Sum();
        }
    }
}
