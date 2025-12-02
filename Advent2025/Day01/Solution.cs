namespace Advent2025.Day01;

public class Solution
{
    public List<int> rotations = new();

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        foreach (var line in lines)
        {
            if (line.StartsWith("L"))
                rotations.Add(-int.Parse(line[1..]));
            else if (line.StartsWith("R"))
                rotations.Add(int.Parse(line[1..]));
        }
    }
    public Solution() : this("Input.txt") { }

    public int timesAtZero = 0;
    public int timesPassingZero = 0;

    public void Simulate()
    {
        timesAtZero = 0;
        timesPassingZero = 0;

        int currentRotation = 50;

        for (int n = 0; n < rotations.Count; n++)
        {
            var atZero = currentRotation % 100 == 0;
            var previousFace = GetFace(currentRotation);

            timesPassingZero += Math.Abs(rotations[n]) / 100;
            currentRotation += rotations[n] % 100;

            var face = GetFace(currentRotation);

            if (face != previousFace && !atZero)
            {
                timesPassingZero++;
            }
            else if (currentRotation % 100 == 0)
                timesPassingZero++;

            if (atZero)
                timesAtZero++;
        }
    }

    public static int GetFace(int rotation) => rotation / 100 + (rotation < 0 ? -1 : 0);

    public object GetResult1()
    {
        Simulate();

        return timesAtZero;
    }

    public object GetResult2()
    {
        Simulate();

        return timesPassingZero;
    }
}
