using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Advent2019.Advent22
{
    public abstract class Technique
    {
        public abstract int[] Apply(int[] input);
        public long BackTrack(long cardIndex, long numCards)
        {
            (var mult, var add) = BackTrackForm();

            return (numCards + (mult * cardIndex + add)) % numCards;
        }
        public abstract (long mult, long add) BackTrackForm();

        public static Technique[] Parse(IEnumerable<string> lines, long numCards)
        {
            var parsedInputs = new List<Technique>();

            foreach (var line in lines)
            {
                var split = line.Split(' ');

                Technique pi = null;
                if (split[0] == "cut") pi = new Cut(split[1], numCards);
                else if (split[0] == "deal")
                {
                    if (split[3] == "stack") pi = new Revert();
                    else pi = new Deal(split[3], numCards);
                }
                else throw new Exception("unparseable line");

                parsedInputs.Add(pi);
            }

            return parsedInputs.ToArray();
        }
    }

    public class Revert : Technique
    {
        public override int[] Apply(int[] input)
        {
            var result = new int[input.Length];
            for (int n = 0; n < input.Length; n++)
            {
                result[input.Length - 1 - n] = input[n];
            }
            return result;
        }

        public override (long mult, long add) BackTrackForm()
        {
            return (-1, -1);
        }

        public override string ToString()
        {
            return "Revert";
        }
    }

    public class Cut : Technique
    {
        long iVal;

        public Cut(string cutVal, long numCards)
        {
            iVal = int.Parse(cutVal);
            if (iVal < 0)
            {
                iVal = numCards + iVal;
            }

        }

        public override int[] Apply(int[] input)
        {
            int[] result = new int[input.Length];

            Array.Copy(input, 0, result, result.Length - iVal, iVal);
            Array.Copy(input, iVal, result, 0, result.Length - iVal);

            return result;
        }

        public override (long mult, long add) BackTrackForm()
        {
            return (1, iVal);
        }

        public override string ToString()
        {
            return "Cut " + iVal;
        }
    }

    public class Deal : Technique
    {
        public int increment;
        public long multInv;

        public Deal(string increment, long numCards)
        {
            this.increment = int.Parse(increment);
            multInv = GroupPower(this.increment, numCards - 2, numCards);
        }

        public override int[] Apply(int[] input)
        {
            var pos = 0;
            var result = new int[input.Length];

            for (int n = 0; n < input.Length; n++)
            {
                result[pos] = input[n];

                pos += increment;
                pos = pos % input.Length;
            }

            return result;
        }

        public override (long mult, long add) BackTrackForm()
        {
            return (multInv, 0);
        }

        public long GroupPower(BigInteger num, BigInteger exponent, BigInteger modulo)
        {
            if (exponent == 0) return 1;

            BigInteger power = GroupPower(num, exponent / 2, modulo) % modulo;
            power = (power * power) % modulo;

            if (exponent % 2 == 0) return (long)power;
            else
            {
                var bigResult = (num * power) % modulo;

                return (long)bigResult;
            }
        }

        public override string ToString()
        {
            return "Deal " + increment;
        }
    }
}
