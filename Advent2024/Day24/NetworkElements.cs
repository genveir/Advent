namespace Advent2024.Day24;
public abstract class NetworkElement
{
    public List<NetworkElement> FeedsInto { get; set; } = [];

    public abstract bool? GetValue();

    public abstract IEnumerable<Connection> GetWires();

    public abstract bool Validate();
    public abstract bool Validate(NetworkElement feedsInto);
}

public abstract class NamedElement : NetworkElement
{
    public string Name { get; set; }
}

public class Pin : NamedElement
{
    public bool Value { get; set; }

    [ComplexParserTarget("name: value")]
    public Pin(string name, string value)
    {
        Name = name;
        Value = value == "1";
    }

    public override bool? GetValue()
    {
        return Value;
    }

    public override IEnumerable<Connection> GetWires() => [];

    public override string ToString() => $"Pin {Name} {Value}";

    public override bool Validate() => true;

    public override bool Validate(NetworkElement feedsInto) => feedsInto is XorGate or AndGate;
}

public class Output : Connection
{
    public override bool Validate() => base.Validate(this);
}

public class Connection : NamedElement
{
    public static HashSet<string> faultyConnections = [];

    public NetworkElement Source { get; set; }

    public override bool? GetValue()
    {
        return Source.GetValue();
    }

    public override IEnumerable<Connection> GetWires() => Source.GetWires().Append(this);

    public override string ToString() => $"Connection {Name}";

    public override bool Validate() => Source.Validate(this);

    public override bool Validate(NetworkElement feedsInto)
    {
        var isValid = Source.Validate(feedsInto);
        if (!isValid)
        {
            faultyConnections.Add(Name);
        }

        return isValid;
    }
}

public abstract class Gate : NetworkElement
{
    public NamedElement Left { get; set; }
    public NamedElement Right { get; set; }

    public override IEnumerable<Connection> GetWires() => Left.GetWires().Concat(Right.GetWires());

    public override bool Validate() => Left.Validate(this) && Right.Validate(this);
}

public class AndGate : Gate
{
    public override bool? GetValue()
    {
        if (Left.GetValue() == true && Right.GetValue() == true)
            return true;

        if (Left.GetValue() == false || Right.GetValue() == false)
            return false;

        return null;
    }

    public override string ToString() => $"AndGate {Left} {Right}";

    public override bool Validate(NetworkElement feedsInto) => feedsInto is OrGate;
}

public class OrGate : Gate
{
    public override bool? GetValue()
    {
        if (Left.GetValue() == true || Right.GetValue() == true)
            return true;

        if (Left.GetValue() == false && Right.GetValue() == false)
            return false;

        return null;
    }

    public override string ToString() => $"OrGate {Left} {Right}";

    public override bool Validate(NetworkElement feedsInto) => feedsInto is AndGate or XorGate;
}

public class XorGate : Gate
{
    public override bool? GetValue()
    {
        if (Left.GetValue() == Right.GetValue())
            return false;

        return true;
    }

    public override string ToString() => $"XorGate {Left} {Right}";

    public override bool Validate(NetworkElement feedsInto) => feedsInto is Output or XorGate or AndGate;
}
