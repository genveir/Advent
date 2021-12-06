using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent06
{
    public class Solution : ISolution
    {
        List<long> numbers;
        public List<LanternFish> fishes;

        public long[] fishArray = new long[9];

        public Solution(string input)
        {
            numbers = Input.GetNumbers(input, ',').ToList();

            fishes = numbers.Select(n => new LanternFish(n)).ToList();
            foreach (var n in numbers) fishArray[n]++;
        }
        public Solution() : this("Input.txt") { }

        public class LanternFish
        {
            public long currentAge;

            public LanternFish(long currentAge)
            {
                this.currentAge = currentAge;
            }

            public void Tick(List<LanternFish> nextGen)
            {
                if (currentAge == 0)
                {
                    nextGen.Add(new LanternFish(8));
                    currentAge = 7;
                }

                currentAge--;
            }

            public override string ToString()
            {
                return currentAge.ToString();
            }
        }

        public static long days = 80;
        public object GetResult1()
        {
            for (int n = 0; n < days; n++)
            {
                var nextGen = new List<LanternFish>();

                foreach(var fish in fishes)
                {
                    fish.Tick(nextGen);
                }
                fishes.AddRange(nextGen);
                ;
            }
            return fishes.Count;
        }

        public static long days2 = 256;
        public object GetResult2()
        {
            for (int n = 0; n < days2; n++)
            {
                var newArray = new long[9];
                newArray[0] = fishArray[1];
                newArray[1] = fishArray[2];
                newArray[2] = fishArray[3];
                newArray[3] = fishArray[4];
                newArray[4] = fishArray[5];
                newArray[5] = fishArray[6];
                newArray[6] = fishArray[7] + fishArray[0];
                newArray[7] = fishArray[8];
                newArray[8] = fishArray[0];

                fishArray = newArray;
            }
            return fishArray.Sum();
        }
    }
}
