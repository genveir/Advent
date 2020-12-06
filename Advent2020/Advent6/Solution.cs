using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent6
{
    public class Solution : ISolution
    {
        List<Group> groups;

        public Solution(string input)
        {
            var perGroup = Input.GetInput(input).Split(Environment.NewLine + Environment.NewLine);

            groups = new List<Group>();
            foreach(var groupLines in perGroup)
            {
                var group = new Group();
                var persons = groupLines.Split(Environment.NewLine);
                foreach(var personLine in persons)
                {
                    group.Answers.Add(personLine);
                }

                groups.Add(group);
            }
        }
        public Solution() : this("Input.txt") { }

        public class Group
        {
            public List<IEnumerable<char>> Answers { get; set; } = new List<IEnumerable<char>>();

            public int CountAllGivenAnswers()
            {
                return Answers.Aggregate((a1, a2) => a1.Union(a2)).Count();
            }

            public int CountSharedAnswers()
            {
                return Answers.Aggregate((a1, a2) => a1.Intersect(a2)).Count();
            }
        }

        public object GetResult1()
        {
            return groups.Select(g => g.CountAllGivenAnswers()).Sum();
        }

        public object GetResult2()
        {
            return groups.Select(g => g.CountSharedAnswers()).Sum();
        }
    }
}
