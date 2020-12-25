using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent25
{
    public class Solution : ISolution
    {
        long cardNum;
        long doorNum;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            cardNum = long.Parse(lines[0]);
            doorNum = long.Parse(lines[1]);
        }
        public Solution() : this("Input.txt") { }

        public long CalculateEncryptionKey(long pubKey1, long pubKey2)
        {
            long encKey = pubKey1;
            long testVal = 7;

            while(testVal != pubKey2)
            {
                encKey *= pubKey1;
                testVal *= 7;

                encKey = encKey % 20201227;
                testVal = testVal % 20201227;
            }

            return encKey;
        }

        public object GetResult1()
        {
            var encKey = CalculateEncryptionKey(cardNum, doorNum);

            return encKey;
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
