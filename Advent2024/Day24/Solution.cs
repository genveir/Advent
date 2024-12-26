using FluentAssertions;

namespace Advent2024.Day24;

public class Solution
{
    public Dictionary<string, Connection> ConnectionLookup = [];
    public List<Gate> Gates = [];

    public SettableRegister X { get; set; }
    public SettableRegister Y { get; set; }
    public ReadonlyRegister Z { get; set; }

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        var pins = blocks[0];
        var gates = blocks[1];

        foreach (var pin in new InputParser<Connection>("pin").Parse(pins))
        {
            ConnectionLookup[pin.Name] = pin;
        }

        var gateParser = new InputParser<string, string, string, string>("left op right -> out");

        var parsedGates = gateParser.Parse(gates);

        foreach (var (left, op, right, outName) in parsedGates)
        {
            if (!ConnectionLookup.TryGetValue(left, out var leftElement))
            {
                leftElement = new Connection()
                {
                    Name = left
                };

                ConnectionLookup[left] = leftElement;
            }

            if (!ConnectionLookup.TryGetValue(right, out var rightElement))
            {
                rightElement = new Connection()
                {
                    Name = right
                };
                ConnectionLookup[right] = rightElement;
            }

            Gate gate = op switch
            {
                "AND" => new AndGate { Left = leftElement, Right = rightElement },
                "OR" => new OrGate { Left = leftElement, Right = rightElement },
                "XOR" => new XorGate { Left = leftElement, Right = rightElement },
                _ => throw new Exception("Unknown gate type")
            };
            Gates.Add(gate);
            leftElement.FeedsInto.Add(gate);
            rightElement.FeedsInto.Add(gate);

            if (!ConnectionLookup.TryGetValue(outName, out var outElement))
            {
                outElement = new Connection()
                {
                    Name = outName
                };
                ConnectionLookup[outName] = outElement;
            }
            outElement.Source = gate;
            gate.FeedsInto = outElement;

            X = CreateSettableRegister("x");
            Y = CreateSettableRegister("y");
            Z = CreateReadonlyRegister("z");
        }
    }
    public Solution() : this("Input.txt") { }

    public ReadonlyRegister CreateReadonlyRegister(string name) =>
        new() { Elements = GetElements(name), Name = name.ToUpper() };

    public SettableRegister CreateSettableRegister(string name) =>
        new() { Elements = GetElements(name), Name = name.ToUpper() };

    private NetworkElement[] GetElements(string name) =>
        ConnectionLookup.Keys.Where(x => x.StartsWith(name))
            .OrderByDescending(x => int.Parse(x[1..]))
            .Select(x => ConnectionLookup[x])
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
        List<string> swaps = [];

        List<Adder> adders = [];
        for (int n = 1; n < X.Elements.Length; n++)
        {
            var x = ConnectionLookup[$"x{n:00}"];
            var y = ConnectionLookup[$"y{n:00}"];
            var z = ConnectionLookup[$"z{n:00}"];

            adders.Add(new Adder(n, x, y, z));
        }
        adders.Last().CarryOut = ConnectionLookup[$"z{(Z.Elements.Length - 1):00}"];

        Adder last = null;
        for (int n = 1; n <= adders.Count; n++)
        {
            var adder = adders[^n];

            ConnectionPair foundSwap = null;
            do
            {
                foundSwap = adder.FindSwappedConnection(last);

                if (foundSwap != null)
                {
                    swaps.Add(foundSwap.One.Name);
                    swaps.Add(foundSwap.Two.Name);

                    foundSwap.Swap();
                }

            } while (foundSwap != null);

            last = adder;
        }

        var halfAdder = new HalfAdder(0, ConnectionLookup["x00"], ConnectionLookup["y00"], ConnectionLookup["z00"]);

        ConnectionPair halfAdderSwap = null;
        do
        {
            halfAdderSwap = halfAdder.FindSwappedConnection(adders.First());

            if (halfAdderSwap != null)
            {
                swaps.Add(halfAdderSwap.One.Name);
                swaps.Add(halfAdderSwap.Two.Name);
                halfAdderSwap.Swap();
            }
        } while (halfAdderSwap != null);

        return string.Join(",", swaps.OrderBy(x => x));
    }

    public class ConnectionPair
    {
        public Connection One { get; set; }
        public Connection Two { get; set; }

        public void Swap()
        {
            One.Source.FeedsInto = Two;
            Two.Source.FeedsInto = One;

            (Two.Source, One.Source) = (One.Source, Two.Source);
            Console.WriteLine($"Swapped {One.Name} <--> {Two.Name}");
        }
    }

    public class HalfAdder
    {
        public int Index { get; set; }

        public Connection X { get; set; }
        public Connection Y { get; set; }

        public XorGate InputXorGate { get; set; }
        public AndGate InputAndGate { get; set; }

        public Connection Sum { get; set; }

        public Connection CarryOut { get; set; }

        public HalfAdder(int index, Connection x, Connection y, Connection z)
        {
            Index = index;
            X = x;
            Y = y;
            Sum = z;

            InputXorGate = X.FeedsInto.OfType<XorGate>().Single();
            InputAndGate = X.FeedsInto.OfType<AndGate>().Single();
        }

        public ConnectionPair FindSwappedConnection(Adder next)
        {
            // if InputXorGate has a wrong output, it's not Sum
            var inputXorConnection = InputXorGate.FeedsInto;
            if (inputXorConnection != Sum)
            {
                return new ConnectionPair { One = inputXorConnection, Two = Sum };
            }

            var inputAndConnection = InputAndGate.FeedsInto;
            if (inputAndConnection != next.Carry)
            {
                return new ConnectionPair { One = inputAndConnection, Two = next.Carry };
            }

            return null;
        }

        public override string ToString() => $"HalfAdder {Index}";
    }

    public class Adder
    {
        public int Index { get; set; }

        public Connection X { get; set; }
        public Connection Y { get; set; }
        public Connection Carry { get; set; }

        public XorGate InputXorGate { get; set; }
        public AndGate InputAndGate { get; set; }

        public Connection InputSumConnection { get; set; }

        public XorGate OutputXorGate { get; set; }

        public AndGate CarryAndGate { get; set; }

        public Connection InputCarryAndConnection { get; set; }
        public Connection CarryAndCarryConnection { get; set; }

        public OrGate CarryGate { get; set; }

        public Connection Sum { get; set; }

        public Connection CarryOut { get; set; }

        public Adder(int index, Connection x, Connection y, Connection z)
        {
            Index = index;

            X = x;
            Y = y;
            Sum = z;

            InputXorGate = X.FeedsInto.OfType<XorGate>().Single();
            InputAndGate = X.FeedsInto.OfType<AndGate>().Single();
        }

        // Assume only one connection is swapped per adder
        public ConnectionPair FindSwappedConnection(Adder next)
        {
            var outputXorGateResult = AnalyzeOutputXorGate();
            if (outputXorGateResult != null)
                return outputXorGateResult;

            var carryGateResult = AnalyzeCarryGate(next);
            if (carryGateResult != null)
                return carryGateResult;

            var inputSumConnectionOk = CheckInputSumConnection();
            var inputCarryConnectionOk = CheckInputCarryAndConnection();

            if (inputSumConnectionOk && inputCarryConnectionOk)
            {
                AnalyzeCarryAndGate();
            }
            else
            {
                if (!inputCarryConnectionOk && !inputSumConnectionOk)
                {
                    // if there's only one swap and both of these lines are wrong, they must be swapped
                    return new ConnectionPair() { One = InputXorGate.FeedsInto, Two = InputAndGate.FeedsInto };
                }
                else if (inputCarryConnectionOk)
                {
                    return AnalyzeFaultySumGate();
                }
                else if (inputSumConnectionOk)
                {
                    return AnalyzeFaultyInputAndGate();
                }
            }

            return null;
        }

        public ConnectionPair AnalyzeOutputXorGate()
        {
            var sumGate = Sum.Source;

            if (sumGate is not XorGate)
                return FindSwappedSumGate();

            OutputXorGate = sumGate as XorGate;

            return null;
        }

        public ConnectionPair FindSwappedSumGate()
        {
            InputSumConnection = InputXorGate.FeedsInto;
            OutputXorGate = InputSumConnection.FeedsInto.OfType<XorGate>().Single();

            return new ConnectionPair() { One = Sum, Two = OutputXorGate.FeedsInto };
        }

        public ConnectionPair AnalyzeCarryGate(Adder next)
        {
            CarryOut ??= next.Carry;

            var carryGate = CarryOut.Source;

            if (carryGate is not OrGate)
                return new ConnectionPair() { One = CarryOut, Two = next.Carry };

            CarryGate = carryGate as OrGate;

            return null;
        }

        public bool CheckInputSumConnection()
        {
            var inputSumConnection = InputXorGate.FeedsInto;

            if (OutputXorGate.Left == inputSumConnection)
            {
                InputSumConnection = OutputXorGate.Left;
                return true;
            }
            else if (OutputXorGate.Right == inputSumConnection)
            {
                InputSumConnection = OutputXorGate.Right;
                return true;
            }

            return false;
        }

        public bool CheckInputCarryAndConnection()
        {
            var inputCarryAndConnection = InputAndGate.FeedsInto;

            if (CarryGate.Left == inputCarryAndConnection)
            {
                InputCarryAndConnection = CarryGate.Left;
                return true;
            }
            else if (CarryGate.Right == inputCarryAndConnection)
            {
                InputCarryAndConnection = CarryGate.Right;
                return true;
            }

            return false;
        }

        public ConnectionPair AnalyzeCarryAndGate()
        {
            CarryAndGate = InputSumConnection.FeedsInto.OfType<AndGate>().Single();
            Carry = CarryAndGate.Left == InputSumConnection ? CarryAndGate.Right : CarryAndGate.Left;

            var carryAndCarryConnection = CarryAndGate.FeedsInto;

            if (CarryGate.Left == InputCarryAndConnection)
            {
                if (CarryGate.Right != carryAndCarryConnection)
                {
                    return new ConnectionPair() { One = CarryGate.Right, Two = carryAndCarryConnection };
                }
            }
            else if (CarryGate.Right == InputCarryAndConnection)
            {
                if (CarryGate.Left != carryAndCarryConnection)
                {
                    return new ConnectionPair() { One = CarryGate.Left, Two = carryAndCarryConnection };
                }
            }

            CarryAndCarryConnection = carryAndCarryConnection;
            return null;
        }

        public ConnectionPair AnalyzeFaultySumGate()
        {
            ;

            return null;
        }

        public ConnectionPair AnalyzeFaultyInputAndGate()
        {
            var carryAndGate = InputSumConnection.FeedsInto.OfType<AndGate>().Single();
            CarryAndCarryConnection = carryAndGate.FeedsInto;

            var faultyConnection = CarryGate.Left == CarryAndCarryConnection ? CarryGate.Right : CarryGate.Left;

            return new ConnectionPair { One = InputAndGate.FeedsInto, Two = faultyConnection };
        }

        public override string ToString() => $"Adder {Index}";
    }
}
