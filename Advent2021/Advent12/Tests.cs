using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent12
{
    class Tests
    {
        [TestCase(example, 10)]
        [TestCase(simpleCase, 2)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 36)]
        [TestCase(simpleCase, 3)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void TestAllMyRoutesAreGood()
        {
            var sol = new Solution(example);

            var myResults = sol.EnumeratePaths2().ToList();
            var exampleResults = visits.Split(Environment.NewLine).ToList();

            foreach(var route in myResults)
            {
                Assert.That(exampleResults.Any(res => res == route.ToString()));
            }
        }

        [Test]
        public void IHaveAllRoutes()
        {
            var sol = new Solution(example);

            var myResults = sol.EnumeratePaths2().ToList();
            var exampleResults = visits.Split(Environment.NewLine).ToList();

            List<string> missing = new List<string>();
            foreach (var route in exampleResults)
            {
                if (!myResults.Any(res => res.ToString() == route)) missing.Add(route);
            }

            Assert.That(missing.Count == 0, $"missing:{Environment.NewLine} {string.Join(Environment.NewLine, missing)}");
        }

        public const string simpleCase = @"start-a
start-b
a-c
c-d
a-end
b-end";

        public const string example = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

        public const string visits = @"start,A,b,A,b,A,c,A,end
start,A,b,A,b,A,end
start,A,b,A,b,end
start,A,b,A,c,A,b,A,end
start,A,b,A,c,A,b,end
start,A,b,A,c,A,c,A,end
start,A,b,A,c,A,end
start,A,b,A,end
start,A,b,d,b,A,c,A,end
start,A,b,d,b,A,end
start,A,b,d,b,end
start,A,b,end
start,A,c,A,b,A,b,A,end
start,A,c,A,b,A,b,end
start,A,c,A,b,A,c,A,end
start,A,c,A,b,A,end
start,A,c,A,b,d,b,A,end
start,A,c,A,b,d,b,end
start,A,c,A,b,end
start,A,c,A,c,A,b,A,end
start,A,c,A,c,A,b,end
start,A,c,A,c,A,end
start,A,c,A,end
start,A,end
start,b,A,b,A,c,A,end
start,b,A,b,A,end
start,b,A,b,end
start,b,A,c,A,b,A,end
start,b,A,c,A,b,end
start,b,A,c,A,c,A,end
start,b,A,c,A,end
start,b,A,end
start,b,d,b,A,c,A,end
start,b,d,b,A,end
start,b,d,b,end
start,b,end";
    }
}