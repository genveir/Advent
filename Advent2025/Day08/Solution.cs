namespace Advent2025.Day08;

public class Solution
{
    public List<JunctionBox> junctionBoxes;
    public List<DistanceAndPair> distancesAndPairs;
    public List<Circuit> circuits;

    public static int NumLinksToMake = 1000;

    public Solution(string input)
    {
        var lines = Input.GetInputLines(input).ToArray();

        var inputParser = new InputParser<JunctionBox>("junctionbox");

        junctionBoxes = inputParser.Parse(lines);

        circuits = new();
        foreach (var box in junctionBoxes)
        {
            circuits.Add(box.Circuit);
        }

        distancesAndPairs = new();
        for (int n = 0; n < junctionBoxes.Count; n++)
        {
            for (int i = n + 1; i < junctionBoxes.Count; i++)
            {
                var boxA = junctionBoxes[n];
                var boxB = junctionBoxes[i];

                var distance = boxA.Position.Distance(boxB.Position);

                distancesAndPairs.Add(new DistanceAndPair()
                {
                    Distance = distance,
                    Pair = (boxA, boxB)
                });
            }
        }

        distancesAndPairs = distancesAndPairs.OrderBy(d => d.Distance).ToList();
    }
    public Solution() : this("Input.txt") { }

    public class DistanceAndPair
    {
        public double Distance { get; set; }
        public (JunctionBox, JunctionBox) Pair { get; set; }

        public override string ToString() => $"Distance {Distance} between {Pair.Item1} and {Pair.Item2}";
    }

    public class JunctionBox
    {
        public Coordinate3D Position { get; set; }

        public Circuit Circuit { get; set; }

        [ComplexParserTarget("coord")]
        public JunctionBox(Coordinate3D position)
        {
            Position = position;
            Circuit = new();
            Circuit.JunctionBoxes.Add(this);
        }

        public void LinkTo(JunctionBox other)
        {
            other.Circuit.SwapAllTo(Circuit);
        }

        public override string ToString() => $"Box at {Position} in {Circuit}";
    }

    public class Circuit
    {
        private static long _id = 0;
        public long Id { get; set; }

        public List<JunctionBox> JunctionBoxes = new();

        public Circuit()
        {
            Id = _id++;
        }

        public void SwapAllTo(Circuit circuit)
        {
            if (circuit == this) return;

            foreach (var box in JunctionBoxes)
            {
                box.Circuit = circuit;
                circuit.JunctionBoxes.Add(box);
            }
            JunctionBoxes.Clear();
        }

        public long Size => JunctionBoxes.Count;

        public override string ToString() => $"Circuit {Id} with {Size} boxes";
    }

    public object GetResult1()
    {
        for (int n = 0; n < NumLinksToMake; n++)
        {
            distancesAndPairs[n].Pair.Item1.LinkTo(distancesAndPairs[n].Pair.Item2);
        }

        circuits = circuits.OrderByDescending(c => c.Size).ToList();

        return circuits[0].Size * circuits[1].Size * circuits[2].Size;
    }

    public object GetResult2()
    {
        int index = 0;
        JunctionBox first = null, second = null;

        circuits = circuits.Where(c => c.Size > 0).ToList();

        while (circuits.Count > 1)
        {
            (first, second) = distancesAndPairs[index].Pair;

            if (first.Circuit != second.Circuit)
            {
                var circuitToRemove = second.Circuit;
                first.LinkTo(second);

                circuits.Remove(circuitToRemove);
            }

            index++;
        }

        return first.Position.X * second.Position.X;
    }
}
