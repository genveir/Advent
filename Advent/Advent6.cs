﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent
{
    class Advent6
    {
        private List<Prereq> GetInput()
        {
            var vals = new List<Prereq>();
            //using (var txt = new StreamReader(new FileStream(@"D:\repos\Advent\Advent\Input\a6test.txt", FileMode.Open)))
            using (var txt = new StreamReader(new FileStream(@"D:\repos\Advent\Advent\Input\Advent6Input.txt", FileMode.Open)))
            {
                while (!txt.EndOfStream)
                    vals.Add(Prereq.Parse(txt.ReadLine()));
            }

            return vals;
        }

        private class Prereq
        {
            public static Prereq Parse(string input)
            {
                return new Prereq()
                {
                    Prerequisite = input[5],
                    After = input[36]
                };
            }

            public char Prerequisite;
            public char After;
        }

        private class PlanStep
        {
            public List<PlanStep> Prerequisite = new List<PlanStep>();
            public List<PlanStep> After = new List<PlanStep>();

            public void Take()
            {
                foreach (var step in After) step.Prerequisite.Remove(this);
            }

            public int TimeTaken { get; set; }

            public bool IsAvailable()
            {
                return Prerequisite.Count == 0;
            }
        }

        private class Plan
        {
            public List<PlanStep> available;

            private Dictionary<char, PlanStep> steps = new Dictionary<char, PlanStep>();

            public Plan(List<Prereq> input)
            {
                available = new List<PlanStep>();

                foreach (var prereq in input)
                {
                    PlanStep prerequisite = GetOrCreate(prereq.Prerequisite);
                    PlanStep after = GetOrCreate(prereq.After);

                    prerequisite.After.Add(after);
                    after.Prerequisite.Add(prerequisite);
                }
            }

            public int GetNumAvailable()
            {
                return steps.Where(s => s.Value.IsAvailable()).Count();
            }

            public IEnumerable<PlanStep> GetAvailable(int num)
            {
                var relevantSteps = steps.Where(s => s.Value.IsAvailable())
                    .OrderBy(kv => kv.Key)
                    .Take(num)
                    .ToList();

                foreach (var relevantStep in relevantSteps) steps.Remove(relevantStep.Key);
                return relevantSteps.Select(kv => kv.Value);
            }

            public char? TakeNext()
            {
                var availableSteps = steps.Where(s => s.Value.IsAvailable());
                if (availableSteps.Count() == 0) return null;

                var next = (availableSteps.Count() > 1) ?
                    availableSteps.OrderBy(kv => kv.Key).First() :
                    availableSteps.Single();

                steps.Remove(next.Key);

                next.Value.Take();
                return next.Key;
            }

            private PlanStep GetOrCreate(char key)
            {
                PlanStep step;
                steps.TryGetValue(key, out step);
                if (step == null)
                {
                    step = new PlanStep();
                    step.TimeTaken = key - 'A' + 61; // haha, side effects much
                    steps.Add(key, step);
                }
                return step;
            }
        }

        public void WritePlan()
        {
            var input = GetInput();

            var plan = new Plan(input);

            using (var writer = new StreamWriter(new FileStream(@"D:\temp\plan.txt", FileMode.Create)))
            {
                for (char? c = plan.TakeNext(); c != null; c = plan.TakeNext())
                {
                    writer.Write(c);
                    Console.Write(c);
                }
            }
        }

        public void MultiTask()
        {
            var input = GetInput();

            var plan = new Plan(input);

            int moment = 0;
            int availableElves = 5;
            List<TimeEvent> TimeLine = new List<TimeEvent>();
            TimeLine.Add(new TimeEvent() { taskComplete = null, moment = 0 });

            while (TimeLine.Count > 0)
            {
                var currentEvent = TimeLine.First();
                TimeLine.Remove(currentEvent);
                if (currentEvent.taskComplete != null)
                {
                    currentEvent.taskComplete.Take();
                    availableElves++;
                }
                moment = currentEvent.moment;

                var numTasks = Math.Min(plan.GetNumAvailable(), availableElves);
                var startedTasks = plan.GetAvailable(numTasks);
                foreach (var startedTask in startedTasks)
                {
                    TimeLine.Add(new TimeEvent() { taskComplete = startedTask, moment = moment + startedTask.TimeTaken });
                    availableElves--;
                }
                TimeLine = TimeLine.OrderBy(tl => tl.moment).ToList();
            }

            Console.WriteLine(moment);
        }

        private class TimeEvent
        {
            public int moment;
            public PlanStep taskComplete;
        }
    }
}
