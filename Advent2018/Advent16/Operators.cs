using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2018.Advent16
{
    class Operators
    {
        public Dictionary<int, List<op>> PossibleOperators;

        public delegate void op(ref int[] r, int i1, int i2, int o);

        public List<op> AllOperators;

        public Operators()
        {
            PossibleOperators = new Dictionary<int, List<op>>();
            for (int n = 0; n < 16; n++) PossibleOperators.Add(n, 
                new List<op>() { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr });

            AllOperators = new List<op>() { addr, addi, mulr, muli, banr, bani, borr, bori, setr, seti, gtir, gtri, gtrr, eqir, eqri, eqrr };
        }

        public static void addr(ref int[] r, int a, int b, int c) { r[c] = r[a] + r[b]; }
        public static void addi(ref int[] r, int a, int b, int c) { r[c] = r[a] + b; }
        public static void mulr(ref int[] r, int a, int b, int c) { r[c] = r[a] * r[b]; }
        public static void muli(ref int[] r, int a, int b, int c) { r[c] = r[a] * b; }
        public static void banr(ref int[] r, int a, int b, int c) { r[c] = r[a] & r[b]; }
        public static void bani(ref int[] r, int a, int b, int c) { r[c] = r[a] & b; }
        public static void borr(ref int[] r, int a, int b, int c) { r[c] = r[a] | r[b]; }
        public static void bori(ref int[] r, int a, int b, int c) { r[c] = r[a] | b; }
        public static void setr(ref int[] r, int a, int b, int c) { r[c] = r[a]; }
        public static void seti(ref int[] r, int a, int b, int c) { r[c] = a; }
        public static void gtir(ref int[] r, int a, int b, int c) { r[c] = a > r[b] ? 1 : 0; }
        public static void gtri(ref int[] r, int a, int b, int c) { r[c] = r[a] > b ? 1 : 0; }
        public static void gtrr(ref int[] r, int a, int b, int c) { r[c] = r[a] > r[b] ? 1 : 0; }
        public static void eqir(ref int[] r, int a, int b, int c) { r[c] = a == r[b] ? 1 : 0; }
        public static void eqri(ref int[] r, int a, int b, int c) { r[c] = r[a] == b ? 1 : 0; }
        public static void eqrr(ref int[] r, int a, int b, int c) { r[c] = r[a] == r[b] ? 1 : 0; }

        public int Test(int opcode, int[] r, int a, int b, int c, int[] expected)
        {
            int numOk = 16;

            foreach (var op in AllOperators)
            {
                bool match;
                try
                {
                    match = RunTest(op, r, a, b, c, expected);
                }
                catch (IndexOutOfRangeException)
                {
                    match = false;
                }

                if (!match)
                {
                    numOk--;
                    PossibleOperators[opcode].Remove(op);
                }
            }

            return numOk;
        }
        
        public void Simplify()
        {
            bool runAgain = true;

            while(runAgain)
            {
                runAgain = false;
                foreach(var po in PossibleOperators)
                {
                    if (po.Value.Count == 1)
                    {
                        var op = po.Value.Single();
                        foreach (var po2 in PossibleOperators)
                        {
                            if (po.Key != po2.Key)
                            {
                                if (po2.Value.Contains(op)) runAgain = true;
                                po2.Value.Remove(op);
                            }
                        }
                    }
                }
            }
        }

        private bool RunTest(op op, int[] r, int i1, int i2, int o, int[] expected)
        {
            var t = new int[r.Length];
            r.CopyTo(t, 0);
            op(ref t, i1, i2, o);
            return t.SequenceEqual(expected);
        }

        public void Execute(int opcode, ref int[] r, int a, int b, int c)
        {
            if (PossibleOperators[opcode].Count == 1)
            {
                var op = PossibleOperators[opcode].Single();
                op(ref r, a, b, c);
            }
            else throw new Exception("heb nog geen uniek geval");
        }

        [Test]
        public void TestRemovesWhenNoMatch()
        {
            var operators = new Operators();
            Assert.IsTrue(operators.PossibleOperators[0].Contains(addi));

            operators.Test(0, new int[] { 0, 0, 0, 0 }, 0, 5, 0, new int[] { 0, 0, 0, 0 });

            Assert.IsFalse(operators.PossibleOperators[0].Contains(addi));
        }

        [Test]
        public void TestDoesntRemoveWhenMatch()
        {
            var operators = new Operators();
            Assert.IsTrue(operators.PossibleOperators[0].Contains(addi));

            operators.Test(0, new int[] { 0, 0, 0, 0 }, 0, 5, 0, new int[] { 5, 0, 0, 0 });

            Assert.IsTrue(operators.PossibleOperators[0].Contains(addi));
        }
    }
}
