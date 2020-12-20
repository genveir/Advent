using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent19
{
    public class ProtoRule : IComparable<ProtoRule>
    {
        public int Input;
        public int[][] Products;

        public IRule MatchingRule;

        public ProtoRule(int input, int[][] products)
        {
            this.Input = input;
            this.Products = products;
        }

        public int CompareTo(ProtoRule other)
        {
            var compare = this.Input.CompareTo(other.Input);

            return compare;
        }

        public bool IsTerminal()
        {
            return Products.Length == 1 && Products[0].All(p => p < 0);
        }

        public void LinkRule(IRule[] rules)
        {
            if (IsTerminal()) return;
            else
            {
                var myRule = rules[Input];

                var myProducts = Products.Select(ps => ps.Select(p => rules[p]).ToArray()).ToArray();

                myRule.Link(myProducts);
            }
        }

        private string FormatProducts()
        {
            var innerArrays = Products.Select(pArray => string.Join(" ", pArray));
            return string.Join(" | ", innerArrays);
        }

        public override string ToString()
        {
            return $"{Input}: ({FormatProducts()})";
        }
    }
}
