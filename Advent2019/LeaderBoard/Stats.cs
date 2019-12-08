using Advent2019.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.LeaderBoard
{
    public class Stats
    {
        public Stats()
        {
            var json = Input.GetInput(Input.InputMode.Embedded, "LeaderBoard.txt");

            var model = JsonConvert.DeserializeObject<LeaderBoardModel>(json);

            int lastDay = 0;
            var personalStats = new List<PersonalStats>();
            foreach (var member in model.Members)
            {
                var memberDayStats = new List<DayStats>();
                foreach(var cdl in member.Value.completion_day_level)
                {
                    var dayStats = new DayStats()
                    {
                        Day = int.Parse(cdl.Key)
                    };
                    if (dayStats.Day > lastDay) lastDay = dayStats.Day;
                    StarModel sm;

                    cdl.Value.TryGetValue("1", out sm);
                    if (sm != default(StarModel)) dayStats.finish1 = sm.get_star_ts;
                    cdl.Value.TryGetValue("2", out sm);
                    if (sm != default(StarModel)) dayStats.finish2 = sm.get_star_ts;

                    memberDayStats.Add(dayStats);
                }

                personalStats.Add(new PersonalStats()
                {
                    Name = member.Value.Name,
                    DayStats = memberDayStats
                });
            }

            ;

            for (int n = 1; n <= lastDay; n++)
            {
                Console.WriteLine("Day " + n);
                Console.WriteLine("seconds to do part 2:");
                foreach (var person in personalStats)
                {
                    var dayStat = person.DayStats.SingleOrDefault(ds => ds.Day == n);

                    string timeTaken;
                    if (dayStat?.finish2 == null) timeTaken = "DNS";
                    else if (dayStat.finish2 == 0) timeTaken = "DNF";
                    else timeTaken = (dayStat.finish2 - dayStat.finish1).ToString();

                    Console.WriteLine(person.Name.PadRight(20) + timeTaken);
                }
                Console.WriteLine();
            }
        }


        private class PersonalStats
        {
            public string Name { get; set; }
            public IEnumerable<DayStats> DayStats { get; set; }

            public override string ToString()
            {
                return "PersonalStats " + Name;
            }
        }

        private class DayStats
        {
            public int Day { get; set; }
            
            public long finish1;
            public long finish2;
        }


        private class LeaderBoardModel
        {
            public Dictionary<string, MemberModel> Members { get; set; }
        }

        private class MemberModel
        {
            public string Name { get; set; }

            public Dictionary<string, Dictionary<string, StarModel>> completion_day_level { get; set; }
        }

        private class StarModel
        {
            public long get_star_ts { get; set; }
        }
    }
}
