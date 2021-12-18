using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18
{
    public static class Parser
    {
        public static string inputString;
        public static int cursor;

        public static WholeValue ParseLine(string line)
        {
            Parser.inputString = line;
            Parser.cursor = 0;

            return new WholeValue(ParsePair());
        }

        public static TreeNode ParsePair()
        {
            Pair left;
            Pair right;

            read(); // leading bracket

            left = ReadMember();

            read(); // comma

            right = ReadMember();

            read(); // comma or final bracket

            return new TreeNode(left, right);
        }

        private static Pair ReadMember()
        {
            Pair result;

            var val = peek();

            var intValue = (long)val - 48;
            if (intValue >= 0 && intValue < 10)
            {
                read(); // get the number off the queue
                result = new LeafNode(intValue);
            }
            else result = ParsePair();

            return result;
        }

        public static char read()
        {
            var result = peek();
            cursor++;

            return result;
        }

        public static char peek() => inputString[cursor];
    }
}
