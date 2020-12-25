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

        public long BruteforceLoopSize(long pubKey) 
        {
            long loopSize = 0;
            long curVal = 7;

            while(curVal != pubKey)
            {
                curVal *= 7;
                curVal = curVal % 20201227;

                loopSize++;
            }

            return loopSize;
        }

        public long CalculateEncryptionKey(long loopSize, long pubKey)
        {
            long curVal = pubKey;

            for (int n = 0; n < loopSize; n++)
            {
                curVal *= pubKey;
                curVal = curVal % 20201227;
            }

            return curVal;
        }

        public object GetResult1()
        {
            var loopSize = BruteforceLoopSize(cardNum);
            var encKey = CalculateEncryptionKey(loopSize, doorNum);

            return encKey;
        }

        public object GetResult2()
        {
            return "";
        }
    }
}
