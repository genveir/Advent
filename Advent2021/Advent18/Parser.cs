using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent18b
{
    public static class Parser
    {
        public static string inputString;
        public static int cursor;

        public static TreeNode ParseLine(string line)
        {
            Parser.inputString = line;
            Parser.cursor = 0;

            return ParsePair();
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

            var newNode = new TreeNode(left, right);

            left.Parent = newNode;
            right.Parent = newNode;

            return newNode;
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

    public abstract class Pair
    {
        public TreeNode Parent;
    }

    public class TreeNode : Pair
    {
        public Pair left;
        public Pair right;



        public TreeNode(Pair left, Pair right)
        {
            this.left = left;
            this.right = right;
        }
    }

    public class LeafNode : Pair
    {
        public long _value;

        public LeafNode(long value)
        {
            _value = value;
        }
    }
}
