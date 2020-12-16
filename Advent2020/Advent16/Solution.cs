using Advent2020.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent16
{
    public class Solution : ISolution
    {
        Ticket MyTicket;
        
        List<Ticket> OtherTickets;
        List<ScanRule> ScanRules;

        TicketValidator Validator;

        public Solution(string input)
        {
            var blocks = Input.GetInput(input).Split(Environment.NewLine + Environment.NewLine).Select(b => b.Split(Environment.NewLine)).ToArray();

            var inputParser = new InputParser<string, int, int, int, int>(true, 5, new List<string>() { ": ", "-", " or ", "-" });

            ScanRules = blocks[0].Select(line => new ScanRule(inputParser.Parse(line))).ToList();

            var ticketParser = new InputParser<int[]>("line");

            MyTicket = new Ticket() { values = ticketParser.Parse(blocks[1][1]) };

            OtherTickets = new List<Ticket>();
            for (int n = 1; n < blocks[2].Length; n++)
            {
                OtherTickets.Add(new Ticket() { index = n, values = ticketParser.Parse(blocks[2][n]) });
            }

            Validator = new TicketValidator(ScanRules);
        }
        public Solution() : this("Input.txt") { }

        public class ScanRule
        {
            public bool[] rangeFilter;

            public string name;
            public NumberRange low;
            public NumberRange high;

            public ScanRule((string name, int lowStart, int lowEnd, int highStart, int highEnd) data)
            {
                this.name = data.name;
                this.low = new NumberRange() { low = data.lowStart, high = data.lowEnd };
                this.high = new NumberRange() { low = data.highStart, high = data.highEnd };

                this.rangeFilter = new bool[1000];

                foreach(var num in low.Concat(high))
                {
                    rangeFilter[num] = true;
                }
            }

            public bool IsValidFor(int value)
            {
                return rangeFilter[value];
            }

            public bool[] _isValidForColumn = new bool[0];
            public void CreateColumnValidityCache(int[][] columns)
            {
                _isValidForColumn = new bool[columns.Length];
                for (int n = 0; n < columns.Length; n++) _isValidForColumn[n] = columns[n].All(v => IsValidFor(v));
            }

            public int ValidColumnCount => _isValidForColumn.Where(v => v).Count();
            public bool IsValidForColumn(int index) => _isValidForColumn[index];

            public override string ToString()
            {
                return $"{name} ({low.low}-{low.high}),({high.low}-{high.high})";
            }
        }

        public class Ticket
        {
            public int index;

            public int[] values;

            public override string ToString()
            {
                return $"Ticket {index}";
            }
        }

        public class NumberRange : IEnumerable<int>, IEnumerator<int>
        {
            public int low;
            public int high;

            private int index = -1;

            public int Current => low + index;
            object IEnumerator.Current => low + index;

            public bool MoveNext()
            {
                index++;

                if (Current > high) return false;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            IEnumerator IEnumerable.GetEnumerator() => this;
            public IEnumerator<int> GetEnumerator() => this;
            public void Dispose() { }
        }

        public class TicketValidator
        {
            public bool[] rangeFilter;

            public TicketValidator(IEnumerable<ScanRule> rules)
            {
                rangeFilter = new bool[1000];

                foreach(var rule in rules)
                {
                    for (int n = 0; n < 1000; n++)
                    {
                        if (rule.IsValidFor(n)) rangeFilter[n] = true;
                    }
                }
            }

            public bool Validate(int number)
            {
                return rangeFilter[number];
            }

            public List<int> GetInvalidNumbers(Ticket ticket)
            {
                return ticket.values.Where(v => !Validate(v)).ToList();
            }
        }

        public object GetResult1()
        {
            return OtherTickets.SelectMany(t => Validator.GetInvalidNumbers(t)).Sum();
        }

        public object GetResult2()
        {
            var validTickets = OtherTickets.Where(t => Validator.GetInvalidNumbers(t).Count == 0).ToList();
            validTickets.Add(MyTicket);

            var (success, rules, assignments) = Check(validTickets);

            if (success)
            {
                long result = 1;
                for (int n = 0; n < assignments.Length; n++)
                {
                    var rule = rules[assignments[n]];

                    if (rule.name.StartsWith("departure")) result *= MyTicket.values[n];
                }
                return result;
            }
            else return "no result";
        }

        public (bool, ScanRule[], int[]) Check(IEnumerable<Ticket> validTickets) 
        { 
            int[][] columns = new int[MyTicket.values.Length][];
            for (int n = 0; n < columns.Length; n++)
            {
                columns[n] = validTickets.Select(vt => vt.values[n]).OrderBy(v => v).ToArray();
            }

            var rules = ScanRules.ToArray();
            for (int n = 0; n < rules.Length; n++) rules[n].CreateColumnValidityCache(columns);
            if (rules.Any(r => r.ValidColumnCount == 0)) return (false, rules, null);

            var assignments = new int[rules.Length];
            for (int n = 0; n < assignments.Length; n++) assignments[n] = -1;

            if (MatchResults(assignments, 0, rules))
            {
                return (true, rules, assignments);
            }
            else return (false, rules, assignments);
        }

        public bool MatchResults(int[] ruleAssignments, int columnToSet, ScanRule[] rules)
        {
            ruleAssignments[columnToSet] = -1;
            for (int ruleNum = 0; ruleNum < rules.Length; ruleNum++)
            {
                if (!ruleAssignments.Contains(ruleNum))
                {
                    if (rules[ruleNum].IsValidForColumn(columnToSet))
                    {
                        ruleAssignments[columnToSet] = ruleNum;

                        if (columnToSet == ruleAssignments.Length - 1) return true;
                        else if (MatchResults(ruleAssignments, columnToSet + 1, rules)) return true;
                    }
                }
            }

            ruleAssignments[columnToSet] = -1;
            return false;
        }
    }
}
