using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent19
{
    public interface IRule
    {
        IEnumerable<int> Matches(string toMatch, int offset);

        void Link(IRule[][] rules);

        string IndirectToString();
    }

    public class TerminalRule : IRule
    {
        private int index; // alleen voor toString
        private char production;

        public TerminalRule(int index, char production)
        {
            this.index = index;
            this.production = production;
        }

        public IEnumerable<int> Matches(string toMatch, int offset)
        {
            if (offset >= toMatch.Length) yield break;

            if (toMatch[offset] == production)
            {
                yield return 1;
            }
            yield break;
        }

        public void Link(IRule[][] rules)
        {
            throw new NotImplementedException();
        }

        public string IndirectToString() => index.ToString();

        public override string ToString()
        {
            return $"{index}: {production}";
        }
    }

    public class Rule : IRule
    {
        private int index; // alleen voor toString
        public IRule[][] Products;

        public Rule(int index)
        {
            this.index = index;
        }

        public IEnumerable<int> Matches(string toMatch, int offset)
        {
            if (offset >= toMatch.Length) yield break;

            for (var disjunct = 0; disjunct < Products.Length; disjunct++)
            {
                var enumerator = GetMatch(toMatch, offset, disjunct, 0).GetEnumerator();

                while (enumerator.MoveNext()) yield return enumerator.Current;
            }

            yield break;
        }

        private IEnumerable<int> GetMatch(string toMatch, int offset, int disjunct, int rule)
        {
            var enumerator = Products[disjunct][rule].Matches(toMatch, offset).GetEnumerator();

            if (rule == Products[disjunct].Length - 1)
            {
                while (enumerator.MoveNext()) yield return enumerator.Current;
            }
            else
            {
                while (enumerator.MoveNext())
                {
                    var matchLength = enumerator.Current;
                    var nextEnum = GetMatch(toMatch, offset + matchLength, disjunct, rule + 1).GetEnumerator();

                    while (nextEnum.MoveNext()) yield return matchLength + nextEnum.Current;
                }
            }
        }

        public void Link(IRule[][] products)
        {
            this.Products = products;
        }

        public string IndirectToString() => index.ToString();

        private string FormatProducts()
        {
            var innerArrays = Products.Select(pArray => string.Join(" ", pArray.Select(p => p.IndirectToString())));
            return string.Join(" | ", innerArrays);
        }

        public override string ToString()
        {
            return $"{index}: ({FormatProducts()})";
        }
    }
}
