using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent23
{
    public class Solution : ISolution
    {
        public long[] startingCups;

        public CupCircle Cups;

        public Solution(string input)
        {
            startingCups = Input.GetNumbers(input);
        }
        public Solution() : this("Input.txt") { }

        public class CupCircle
        {
            public Cup Current { get; set; }

            public Cup[] Cups { get; set; }

            public CupCircle(long[] cups)
            {
                var builtCups = new Cup[cups.Length];
                var numCups = new Cup[cups.Length];

                builtCups[0] = new Cup(cups[0]);
                for (int n = 1; n < cups.Length; n++)
                {
                    builtCups[n] = new Cup(cups[n]);
                    builtCups[n].LinkCounterClockwise(builtCups[n - 1]);
                    numCups[cups[n] - 1] = builtCups[n];
                }
                builtCups[0].LinkCounterClockwise(builtCups[builtCups.Length - 1]);
                numCups[cups[0] - 1] = builtCups[0];

                for (int n = 1; n < numCups.Length; n++)
                {
                    numCups[n].LinkDown(numCups[n - 1]);
                }
                numCups[0].LinkDown(numCups[numCups.Length - 1]);

                Cups = numCups;
                Current = builtCups[0];
            }
        }

        public class Cup
        {
            public Cup CounterClockwise { get; set; }
            public Cup ClockWise { get; set; }
            public Cup OneLower { get; set; }

            public long Number { get; set; }

            public bool PickedUp { get; set; }

            public Cup(long number) { this.Number = number; }

            public void LinkCounterClockwise(Cup cupToLink)
            {
                CounterClockwise = cupToLink;
                cupToLink.ClockWise = this;
            }

            public void LinkDown(Cup cup)
            {
                OneLower = cup;
            }
        }

        public void Step()
        {
            var current = Cups.Current;

            var leftMostPickedUp = current.ClockWise;
            var middlePickedUp = leftMostPickedUp.ClockWise;
            var rightMostPickedUp = middlePickedUp.ClockWise;

            rightMostPickedUp.ClockWise.LinkCounterClockwise(current);

            leftMostPickedUp.PickedUp = true;
            middlePickedUp.PickedUp = true;
            rightMostPickedUp.PickedUp = true;

            var destination = current.OneLower;
            while (destination.PickedUp) destination = destination.OneLower;

            destination.ClockWise.LinkCounterClockwise(rightMostPickedUp);
            leftMostPickedUp.LinkCounterClockwise(destination);

            leftMostPickedUp.PickedUp = false;
            middlePickedUp.PickedUp = false;
            rightMostPickedUp.PickedUp = false;

            Cups.Current = current.ClockWise;
        }

        public void RunFor(long rounds)
        {
            for (long r = 0; r < rounds; r++)
            {
                Step();
            }
        }

        public string FormatResult()
        {
            string result = "";

            var cup = Cups.Cups[0];
            for (int n = 0; n < Cups.Cups.Length - 1; n++)
            {
                cup = cup.ClockWise;
                result += cup.Number;
            }

            return result;
        }

        public object GetResult1()
        {
            Cups = new CupCircle(startingCups);

            RunFor(100);

            return FormatResult();
        }

        public object GetResult2()
        {
            var allCups = new long[1000000];
            for (int n = 0; n < allCups.Length; n++) allCups[n] = n + 1;
            for (int n = 0; n < startingCups.Length; n++) allCups[n] = startingCups[n];

            Cups = new CupCircle(allCups);

            RunFor(10000000);

            var cup1 = Cups.Cups[0];
            return cup1.ClockWise.Number * cup1.ClockWise.ClockWise.Number;
        }
    }
}
