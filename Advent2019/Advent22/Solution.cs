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

        public long numCards = 10007;
        public int[] cards;

        public string[] lines;

        public Solution(Input.InputMode inputMode, string input)
        {
            lines = Input.GetInputLines(inputMode, input).ToArray();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Setup(long numCards)
        {
            this.numCards = numCards;
            techniques = Technique.Parse(lines, numCards);

            _CombinedTechniques = (1, 0);
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
            BigInteger internalMult = first.mult;
            BigInteger internalAdd = first.add;
            BigInteger externalMult = second.mult;
            BigInteger externalAdd = second.add;

            BigInteger newMult = (internalMult * externalMult);
            BigInteger newAdd = (externalMult * internalAdd + externalAdd);

            if (newMult < 0) newMult = (newMult % numCards) + numCards;
            if (newAdd < 0) newAdd = (newAdd % numCards) + numCards;

            newMult = newMult % numCards;
            newAdd = newAdd % numCards;

            var longMult = (long)newMult;
            var longAdd = (long)newAdd;

            return (longMult, longAdd);
        }

        public (long mult, long add) _CombinedTechniques = (1,0);
        public (long mult, long add) CombinedTechniques
        {
            get
            {
                if (_CombinedTechniques.mult == 1 && _CombinedTechniques.add == 0)
                {
                    for (int n = techniques.Length - 1; n >= 0; n--)
                    {
                        var technique = techniques[n];
                        var multadd = technique.BackTrackForm();

                        _CombinedTechniques = Combine(_CombinedTechniques, multadd);
                    }
                }

                return _CombinedTechniques;
            }
        }

        public long BackTrack(long cardIndex, long numberOfSteps)
        {
            var maxNeeded = (int)Math.Log(numberOfSteps, 2) + 1;
            var requiredFunctions = new (long mult, long add)[maxNeeded];
            var powers = new long[maxNeeded];

            requiredFunctions[0] = CombinedTechniques;
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
            Setup(10007);

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
            Setup(119315717514047);

            return BackTrack(2020, 101741582076661).ToString();
        }
    }

    // 27462094789084 too low
    // 37889219674304
    // 99954129567610 "not right"
    // 102460576295153 too high
}
