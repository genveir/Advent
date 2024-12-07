namespace Advent2024.AdventBase;

public class Solution
{
    public List<ParsedInput> modules;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<ParsedInput>("line");

        modules = inputParser.Parse(lines);
    }
    public Solution() : this("Input.txt") { }

    public class ParsedInput
    {
        [ComplexParserTarget("line")]
        public ParsedInput()
        {

        }
    }

    public object GetResult1()
    {
        return "";
    }

    public object GetResult2()
    {
        return "";
    }
}
