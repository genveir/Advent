using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2022.Advent04
{
    public class Solution : ISolution
    {
        public List<Assignment> assignments;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<Assignment>("a-b,c-d");

            assignments = inputParser.Parse(lines);
        }
        public Solution() : this("Input.txt") { }

        public class Assignment
        {
            private int sectionStart;
            private int sectionEnd;
            private int section2Start;
            private int section2End;

            [ComplexParserConstructor]
            public Assignment(int sectionStart, int sectionEnd, int section2Start, int section2End)
            {
                this.sectionStart = sectionStart;
                this.sectionEnd = sectionEnd;
                this.section2Start = section2Start;
                this.section2End = section2End;
            }

            public bool HasRangeThatFullyCoversTheOther()
            {
                return FullyCoversTheOther(sectionStart, sectionEnd, section2Start, section2End) ||
                    FullyCoversTheOther(section2Start, section2End, sectionStart, sectionEnd);
            }

            public static bool FullyCoversTheOther(int start, int end, int otherStart, int otherEnd)
            {
                return start <= otherStart && end >= otherEnd;
            }

            public bool HasOverlap()
            {
                return HasRangeThatFullyCoversTheOther() ||
                    (sectionStart <= section2End && sectionEnd >= section2End) ||
                    (sectionEnd >= section2Start && sectionStart <= section2Start);
            }
        }

        public object GetResult1()
        {
            return assignments.Count(a => a.HasRangeThatFullyCoversTheOther());
        }

        public object GetResult2()
        {
            return assignments.Count(a => a.HasOverlap());
        }
    }
}
