using Advent2024.Shared;
using Advent2024.Shared.InputParsing;

namespace Advent2024.Day02;

public class Solution : ISolution
{
    public List<ParsedInput> modules;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        modules = inputParser.Parse(lines);
    }

    public Solution() : this("Input.txt")
    {
    }

    public class ParsedInput
    {
        private readonly long[] nums;

        [ComplexParserTarget("line", ArrayDelimiters = [' '])]
        public ParsedInput(long[] nums)
        {
            this.nums = nums;
        }

        public bool IsValid()
        {
            bool increasing = false;

            if (nums[1] > nums[0])
            {
                increasing = true;
            }

            for (int n = 1; n < nums.Length; n++)
            {
                if (increasing && nums[n] < nums[n - 1])
                {
                    return false;
                }
                if (!increasing && nums[n] > nums[n - 1])
                {
                    return false;
                }

                var diff = Math.Abs(nums[n] - nums[n - 1]);

                if (diff < 1) return false;
                if (diff > 3) return false;
            }

            return true;
        }

        public bool IsDampenerValid(bool increasing, bool skipFirst)
        {
            bool dampened = skipFirst; // <-- aughh
            long current = nums[skipFirst ? 1 : 0];

            for (int n = skipFirst ? 2 : 1; n < nums.Length; n++)
            {
                if (increasing && nums[n] < current)
                {
                    if (dampened == false)
                    {
                        dampened = true;
                        continue;
                    }
                    else return false;
                }
                if (!increasing && nums[n] > current)
                {
                    if (dampened == false)
                    {
                        dampened = true;
                        continue;
                    }
                    else return false;
                }

                var diff = Math.Abs(nums[n] - current);

                if (diff < 1)
                {
                    if (dampened == false)
                    {
                        dampened = true;
                        continue;
                    }
                    else return false;
                }
                if (diff > 3)
                {
                    if (dampened == false)
                    {
                        dampened = true;
                        continue;
                    }
                    else return false;
                }

                current = nums[n];
            }

            return true;
        }
    }

    public object GetResult1()
    {
        return modules.Where(m => m.IsValid()).Count();
    }

    public object GetResult2()
    {
        // not 545, not 549, not 551, not 643
        return modules.Where(m =>
            m.IsDampenerValid(true, true) ||
            m.IsDampenerValid(true, false) ||
            m.IsDampenerValid(false, true) ||
            m.IsDampenerValid(false, false)).Count();
    }
}