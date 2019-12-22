using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Advent2019.Advent22
{
    public class Solution : ISolution
    {
        public Technique[] techniques;

        public static long numCards = 10007;
        public int[] cards;

        public string[] lines;

        public Solution(Input.InputMode inputMode, string input)
        {
            var lines = Input.GetInputLines(inputMode, input).ToArray();

            techniques = Technique.Parse(lines, numCards);
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

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
                        else pi = new Deal(split[3]);
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
                for(int n = 0; n < input.Length; n++)
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
                    iVal = numCards + iVal ;
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

            public Deal(string increment)
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

            public long GroupPower(long num, long exponent, long modulo)
            {
                if (exponent == 0) return 1;

                long power = GroupPower(num, exponent / 2, modulo) % modulo;
                power = (power * power) % modulo;

                if (exponent % 2 == 0) return power;
                else return (num * power) % modulo;
            }

            public override string ToString()
            {
                return "Deal " + increment;
            }
        }

        public void ResetDeck()
        {
            cards = new int[numCards];
            for (int n = 0; n < cards.Length; n++)
            {
                cards[n] = n;
            }
        }

        public void ApplyTechniques()
        {
            foreach (var technique in techniques)
            {
                cards = technique.Apply(cards);
            }
        }

        public (long mult, long add) Combine((long mult, long add) first, (long mult, long add) second)
        {
            var internalMult = first.mult;
            var internalAdd = first.add;
            var externalMult = second.mult;
            var externalAdd = second.add;

            BigInteger newMult = (internalMult * externalMult);
            BigInteger newAdd = (externalMult * internalAdd + externalAdd);

            while (newMult < 0) newMult += numCards;
            while (newAdd < 0) newAdd += numCards;

            newMult = newMult % numCards;
            newAdd = newAdd % numCards;

            var longMult = (long)newMult;
            var longAdd = (long)newAdd;

            return (longMult, longAdd);
        }

        public (long mult, long add) _CombinedFunction = (1,0);
        public (long mult, long add) CombinedFunction
        {
            get
            {
                if (_CombinedFunction.mult == 1 && _CombinedFunction.add == 0)
                {
                    for (int n = techniques.Length - 1; n >= 0; n--)
                    {
                        var technique = techniques[n];
                        var multadd = technique.BackTrackForm();

                        _CombinedFunction = Combine(_CombinedFunction, multadd);
                    }
                }

                return _CombinedFunction;
            }
        }

        public long BackTrack(long cardIndex)
        {
            var val = (CombinedFunction.mult * cardIndex + CombinedFunction.add);

            return val % numCards;
        }

        public long BackTrack(long cardIndex, long numberOfSteps)
        {
            var maxNeeded = (int)Math.Log(numberOfSteps, 2) + 1;
            var requiredFunctions = new (long mult, long add)[maxNeeded];
            var powers = new long[maxNeeded];

            requiredFunctions[0] = CombinedFunction;
            powers[0] = 1;
            for (int n = 1; n < maxNeeded; n++)
            {
                requiredFunctions[n] = Combine(requiredFunctions[n - 1], requiredFunctions[n - 1]);
                powers[n] = powers[n - 1] * 2;
            }

            long stepsLeft = numberOfSteps;
            (long mult, long add) totalFunction = (1, 0);
            for (int n = maxNeeded - 1; n >= 0; n--)
            {
                if (stepsLeft >= powers[n])
                {
                    totalFunction = Combine(totalFunction, requiredFunctions[n]);
                    stepsLeft -= powers[n];
                }
            }

            var val = (totalFunction.mult * cardIndex + totalFunction.add);

            return val % numCards;
        }

        public bool IsSorted()
        {
            for (int n = 0; n < cards.Length; n++)
            {
                if (cards[n] != n) return false;
            }

            return true;
        }

        public string GetResult1()
        {
            numCards = 10007;
            ResetDeck();
            ApplyTechniques();

            for (int n = 0; n < cards.Length; n++)
            {
                if (cards[n] == 2019) return n.ToString();
            }

            return "no result";
        }

        public string GetResult2()
        {
            numCards = 119315717514047;

            return BackTrack(2020, 101741582076661).ToString();
        }
    }

    // 27462094789084 too low
    // 102460576295153 too high
}
