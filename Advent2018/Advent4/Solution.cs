using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2018.Advent4
{
    class Solution : ISolution
    {
        private List<Event> GetInput()
        {
            var adventNum = this.GetType().Name.ToCharArray().Last();
            var input = typeof(Program).Assembly.GetManifestResourceStream("Advent2018.Advent4.Input.txt");

            var vals = new List<Event>();
            using (var txt = new StreamReader(input))
            {
                while (!txt.EndOfStream)
                    vals.Add(Event.Parse(txt.ReadLine()));
            }

            vals = vals.OrderBy(v => v.Moment).ToList();
            long id = -1;
            foreach (var val in vals)
            {
                if (val.Id == -1) val.Id = id;
                else id = val.Id;
            }

            return vals;
        }

        private abstract class Event
        {
            public int Moment;
            public long Id = -1;

            public Event(int moment)
            {
                this.Moment = moment;
            }

            public abstract void Update(ref List<Guard> guards);

            public static Event Parse(string input)
            {
                var split = input.Split(new char[] { ']', ' ', '-', ':', '#' }, StringSplitOptions.RemoveEmptyEntries);
                var month = int.Parse(split[1]) * 31 * 24 * 60;
                var day = int.Parse(split[2]) * 24 * 60;
                var hour = int.Parse(split[3]) * 60;
                var minute = int.Parse(split[4]);

                var moment = month + day + hour + minute;

                switch (split[5])
                {
                    case "falls": return new SleepEvent(moment);
                    case "wakes": return new WakeEvent(moment);
                    case "Guard": return new GuardEvent(moment, long.Parse(split[6]));
                    default: throw new InvalidDataException();
                }
            }
        }


        private class GuardEvent : Event
        {
            public GuardEvent(int moment, long id) : base(moment) { Id = id; }

            public override void Update(ref List<Guard> guards)
            {
                if (!guards.Any(g => g.Id == Id)) guards.Add(new Guard(this.Id));
            }
        }

        private class SleepEvent : Event
        {
            public SleepEvent(int moment) : base(moment) { }

            public override void Update(ref List<Guard> guards)
            {
                var guard = guards.Where(g => g.Id == Id).Single();
                guard.isAsleep = true;
            }
        }

        private class WakeEvent : Event
        {
            public WakeEvent(int moment) : base(moment) { }

            public override void Update(ref List<Guard> guards)
            {
                var guard = guards.Where(g => g.Id == Id).Single();
                guard.isAsleep = false;
            }
        }

        private class Guard
        {
            public Guard(long id) { this.Id = id; }

            public long Id;

            public bool isAsleep = false;

            public List<int> sleepMoments = new List<int>();

            public long sleepDuration { get { return sleepMoments.Count(); } }

            public void Update(int moment)
            {
                if (isAsleep)
                {
                    sleepMoments.Add(moment);
                }
            }

            public long minuteMostAsleepCount
            {
                get
                {
                    if (sleepMoments.Count == 0) return 0;
                    var grouped = sleepMoments.GroupBy(sm => sm % 60);
                    var ordered = grouped.OrderBy(g => g.Count());
                    return ordered.Last().ToList().Count;
                }
            }
            public long minuteMostAsleep
            {
                get
                {
                    var grouped = sleepMoments.GroupBy(sm => sm % 60);
                    var ordered = grouped.OrderBy(g => g.Count());
                    return ordered.Last().Key;
                }
            }

            public override string ToString()
            {
                return string.Format("Guard ({0}): {1}", Id, sleepDuration);
            }
        }

        private List<Guard> GetGuards()
        {
            var input = GetInput();

            var guards = new List<Guard>();

            var firstMoment = input.First().Moment;
            var lastMoment = input.Last().Moment;

            for (int m = firstMoment; m <= lastMoment; m++)
            {
                var eventsToHandle = input.Where(i => i.Moment == m);
                foreach (var e in eventsToHandle) e.Update(ref guards);

                foreach (var guard in guards) guard.Update(m);
            }

            return guards;
        }

        public long GetMostAsleepId()
        {
            var guards = GetGuards();

            var inOrder = guards.OrderBy(g => g.sleepDuration);
            var mostAsleep = inOrder.Last();

            var best = mostAsleep.minuteMostAsleep;

            return mostAsleep.Id * best;
        }

        public long GetMostAsleepMinute()
        {
            var guards = GetGuards();

            var ordered = guards.OrderBy(g => g.minuteMostAsleepCount);
            var mostMinutes = ordered.Last();

            var best = mostMinutes.minuteMostAsleep;

            return mostMinutes.Id * best;
        }

        public void WriteResult()
        {
            Console.WriteLine("part 1: " + GetMostAsleepId());
            Console.WriteLine("part 2: " + GetMostAsleepMinute());
        }
    }
}
