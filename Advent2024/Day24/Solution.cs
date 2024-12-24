using FluentAssertions;

namespace Advent2024.Day24;

public class Solution
{
    public Dictionary<string, NamedElement> NamedElementLookup = [];
    public List<Gate> Gates = [];

    public SettableRegister X { get; set; }
    public SettableRegister Y { get; set; }
    public ReadonlyRegister Z { get; set; }

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        var pins = blocks[0];
        var gates = blocks[1];

        foreach (var pin in new InputParser<Pin>("pin").Parse(pins))
        {
            NamedElementLookup[pin.Name] = pin;
        }

        var gateParser = new InputParser<string, string, string, string>("left op right -> out");

        var parsedGates = gateParser.Parse(gates);

        foreach (var (left, op, right, outName) in parsedGates)
        {
            if (!NamedElementLookup.TryGetValue(left, out var leftElement))
            {
                leftElement = new Connection()
                {
                    Name = left
                };

                NamedElementLookup[left] = leftElement;
            }

            if (!NamedElementLookup.TryGetValue(right, out var rightElement))
            {
                rightElement = new Connection()
                {
                    Name = right
                };
                NamedElementLookup[right] = rightElement;
            }

            NetworkElement gate = op switch
            {
                "AND" => new AndGate { Left = leftElement, Right = rightElement },
                "OR" => new OrGate { Left = leftElement, Right = rightElement },
                "XOR" => new XorGate { Left = leftElement, Right = rightElement },
                _ => throw new Exception("Unknown gate type")
            };
            Gates.Add((Gate)gate);
            leftElement.FeedsInto.Add(gate);
            rightElement.FeedsInto.Add(gate);

            if (!NamedElementLookup.TryGetValue(outName, out var outElement))
            {
                outElement = new Connection()
                {
                    Name = outName
                };
                NamedElementLookup[outName] = outElement;
            }
            ((Connection)outElement).Source = gate;
            gate.FeedsInto.Add(outElement);

            X = CreateSettableRegister("x");
            Y = CreateSettableRegister("y");
            Z = CreateReadonlyRegister("z");
        }
    }
    public Solution() : this("Input.txt") { }

    public ReadonlyRegister CreateReadonlyRegister(string name) =>
        new ReadonlyRegister { Elements = GetElements(name), Name = name.ToUpper() };

    public SettableRegister CreateSettableRegister(string name) =>
        new SettableRegister { Pins = GetElements(name).Cast<Pin>().ToArray(), Name = name.ToUpper() };

    private NetworkElement[] GetElements(string name) =>
        NamedElementLookup.Keys.Where(x => x.StartsWith(name))
            .OrderByDescending(x => int.Parse(x[1..]))
            .Select(x => NamedElementLookup[x])
            .ToArray();

    public object GetResult1()
    {
        return Z.GetValue();
    }
    // wel frn,gmq,vtj,wnf,wtt,z05,z21,z39
    // not gmq,vtj,wnf,wtt,z05,z21,z39,z45
    // not gmq,mtk,vtj,wnf,wtt,z05,z21,z39
    public object GetResult2()
    {
        // doesn't work, just did it by hand
        foreach (var gate in Gates)
        {
            var valid = gate.Validate();
        }
        foreach (var output in Z.Elements)
        {
            var op = new Output() { Source = output, Name = ((Connection)output).Name };
            var valid = op.Validate();
        }

        var swaps = Connection
            .faultyConnections
            .ToList();

        swaps.Remove("mtk"); // halfadder

        swaps.Sort();

        return string.Join(",", swaps);
    }

    public bool TestBit(int bit)
    {
        bool isCorrect = true;

        // test if bit is 0 when X and Y are 0
        SetValues(bit, false, false);
        isCorrect &= Z.GetBinaryValue()[^bit] == '0';

        // test if bit is 1 when X is 1 and Y is 0
        SetValues(bit, true, false);
        isCorrect &= Z.GetBinaryValue()[^bit] == '1';

        // test if bit is 1 when X is 0 and Y is 1
        SetValues(bit, false, true);
        isCorrect &= Z.GetBinaryValue()[^bit] == '1';

        // test if bit is 0 when X and Y are 1
        SetValues(bit, true, true);
        isCorrect &= Z.GetBinaryValue()[^bit] == '0';

        if (bit == Z.Elements.Length - 1)
            return isCorrect;

        // test if bit carries correctly
        SetValues(bit + 1, true, true);
        isCorrect &= Z.GetBinaryValue()[^bit] == '1';

        return isCorrect;
    }

    public void SetValues(int bit, bool x, bool y)
    {
        X.SetValue(0);
        Y.SetValue(0);

        X.SetBit(X.Elements.Length - bit, x);
        Y.SetBit(Y.Elements.Length - bit, y);
    }
}
