using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent2020.Advent4
{
    public class Solution : ISolution
    {
        List<ParsedInput> modules;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser<string, string>("key:value");

            modules = new List<ParsedInput>();
            ParsedInput currentModule = new ParsedInput();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    modules.Add(currentModule);
                    currentModule = new ParsedInput();
                }
                
                var fields = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int n = 0; n < fields.Length; n++)
                {
                    var split = inputParser.Parse(fields[n]);
                    currentModule.passportValues.Add(split.Item1, split.Item2);
                }
            }
            modules.Add(currentModule);
        }
        public Solution() : this("Input.txt") { }

        public class ParsedInput
        {
            public Dictionary<string, string> passportValues = new Dictionary<string, string>();

            public bool IsValid()
            {
                return ValidByr() &&
                    ValidIyr() &&
                    ValidEyr() &&
                    ValidHgt() &&
                    ValidHcl() &&
                    ValidEcl() &&
                    ValidPid();
            }

            private bool ValidByr()
            {
                if (!passportValues.ContainsKey("byr")) return false;
                int val;
                if (!int.TryParse(passportValues["byr"], out val)) return false;
                return val >= 1920 && val <= 2002;
            }

            private bool ValidIyr()
            {
                if (!passportValues.ContainsKey("iyr")) return false;
                int val;
                if (!int.TryParse(passportValues["iyr"], out val)) return false;
                return val >= 2010 && val <= 2020;
            }

            private bool ValidEyr()
            {
                if (!passportValues.ContainsKey("eyr")) return false;
                int val;
                if (!int.TryParse(passportValues["eyr"], out val)) return false;
                return val >= 2020 && val <= 2030;
            }

            private bool ValidHgt()
            {
                if (!passportValues.ContainsKey("hgt")) return false;
                var hgt = passportValues["hgt"];
                if (hgt.EndsWith("cm"))
                {
                    int val;
                    if (!int.TryParse(hgt.Substring(0, 3), out val)) return false;
                    return val >= 150 && val <= 193;
                }
                if (hgt.EndsWith("in"))
                {
                    int val;
                    if (!int.TryParse(hgt.Substring(0, 2), out val)) return false;
                    return val >= 59 && val <= 76;
                }
                return false;
            }

            private bool ValidHcl()
            {
                if (!passportValues.ContainsKey("hcl")) return false;
                var hcl = passportValues["hcl"];
                return Regex.IsMatch(hcl, "\\#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]");
            }

            private bool ValidEcl()
            {
                if (!passportValues.ContainsKey("ecl")) return false;
                var ecl = passportValues["ecl"];
                return new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(ecl);
            }

            private bool ValidPid()
            {
                if (!passportValues.ContainsKey("pid")) return false;
                var pid = passportValues["pid"];
                if (pid.Length != 9) return false;
                return Regex.IsMatch(pid, "[0-9]*");
            }
        }

        public string GetResult1()
        {
            return modules
                .Where(m => m.passportValues.ContainsKey("byr"))
                .Where(m => m.passportValues.ContainsKey("iyr"))
                .Where(m => m.passportValues.ContainsKey("eyr"))
                .Where(m => m.passportValues.ContainsKey("hgt"))
                .Where(m => m.passportValues.ContainsKey("hcl"))
                .Where(m => m.passportValues.ContainsKey("ecl"))
                .Where(m => m.passportValues.ContainsKey("pid"))
                .Count().ToString();
        }

        public string GetResult2()
        {
            return modules.Where(m => m.IsValid())
                .Count().ToString();
        }
    }
}
