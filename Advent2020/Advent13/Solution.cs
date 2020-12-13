using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent13
{
    public class Solution : ISolution
    {
        public long arrivalTimestamp;
        public List<Bus> buses = new List<Bus>();

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            arrivalTimestamp = long.Parse(lines[0]);
            var busses = lines[1].Split(",");

            for (int n = 0; n < busses.Length; n++)
            {
                if (busses[n] != "x") buses.Add(new Bus(long.Parse(busses[n]), n));
            }
        }
        public Solution() : this("Input.txt") { }

        public class Bus
        {
            public long firstMomentItCanLeave = 0;
            public long period;
            public long leaveOffset;

            public Bus(long period, long leaveOffset)
            {
                this.period = period;
                this.leaveOffset = leaveOffset % period;
            }

            public long GetTimeToWait(long timeStamp)
            {
                var posMod = timeStamp % period;

                return period - (posMod);
            }

            public override string ToString()
            {
                return $"Bus {period} offset {leaveOffset} fl {firstMomentItCanLeave}";
            }
        }

        public object GetResult1()
        {
            long lowestTime = long.MaxValue;
            long answer = 0;
            for (int n = 0; n < buses.Count; n++)
            {
                var timeToWait = buses[n].GetTimeToWait(arrivalTimestamp);
                if (timeToWait < lowestTime)
                {
                    lowestTime = timeToWait;
                    answer = timeToWait * buses[n].period;
                }
            }

            return answer;
        }

        public object GetResult2()
        {
            var theMagicBus = buses.Aggregate((one, two) => CreateMagicCombinerBus(one, two));

            return theMagicBus.firstMomentItCanLeave;
        }

        private Bus CreateMagicCombinerBus(Bus one, Bus two)
        {
            Console.WriteLine("checking " + one);
            Console.WriteLine("against " + two);

            long timeStamp = one.firstMomentItCanLeave;
            while(true)
            {
                var timeTowait = two.GetTimeToWait(timeStamp);
                if (timeTowait == two.leaveOffset) break;

                timeStamp += one.period;

                if (timeStamp > 10_000_000_000_000_000) return null;
            }

            var period = one.period * two.period;

            return new Bus(period, 0) { firstMomentItCanLeave = timeStamp };
        }
    }
}
