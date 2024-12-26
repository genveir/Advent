namespace Advent2024.Day24;
public abstract class NetworkElement
{
    public static int _id = 0;
    public int Id { get; set; } = _id++;


    public abstract bool? GetValue();
}

public interface INamedElement
{
    public string Name { get; set; }
}

public class Connection : NetworkElement, INamedElement
{
    public Connection() { }

    [ComplexParserTarget("name: value")]
    public Connection(string name, string value)
    {
        Name = name;
        Value = value == "1";
    }

    public bool? Value { get; set; }

    public string Name { get; set; }

    public Gate Source { get; set; }

    public List<Gate> FeedsInto { get; set; } = [];

    public override bool? GetValue()
    {
        return Value ?? Source.GetValue();
    }

    public override string ToString() => $"[{Id}: {Name}]";
}

public abstract class Gate : NetworkElement
{
    public Connection Left { get; set; }
    public Connection Right { get; set; }

    public Connection FeedsInto { get; set; }
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

    public override string ToString() => $"{Id}: {Left} And {Right} -> {FeedsInto} ";
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

    public override string ToString() => $"{Id}: {Left} Or {Right} -> {FeedsInto} ";
}

public class XorGate : Gate
{
    public override bool? GetValue()
    {
        if (Left.GetValue() == Right.GetValue())
            return false;

        return true;
    }

    public override string ToString() => $"{Id}: {Left} Xor {Right} -> {FeedsInto} ";
}
