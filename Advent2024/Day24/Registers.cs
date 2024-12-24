using System.Text;

namespace Advent2024.Day24;

public abstract class Register
{
    public string Name { get; set; }

    public abstract NetworkElement[] Elements { get; set; }

    public string GetBinaryValue()
    {
        var sb = new StringBuilder();
        foreach (var element in Elements)
        {
            if (element.GetValue() == true)
                sb.Append("1");
            else if (element.GetValue() == false)
                sb.Append("0");
            else
                throw new Exception("Element has no value");
        }
        return sb.ToString();
    }

    public long GetValue()
    {
        var binary = GetBinaryValue();

        return Convert.ToInt64(binary, 2);
    }

    public override string ToString()
    {
        return $"{Name} {GetBinaryValue()}";
    }
}

public class ReadonlyRegister : Register
{
    public override NetworkElement[] Elements { get; set; }
}

public class SettableRegister : Register
{
    public Pin[] Pins { get; set; }
    public override NetworkElement[] Elements
    {
        get => Pins;
        set => Pins = value.Cast<Pin>().ToArray();
    }

    public void SetValue(long value)
    {
        var asBinary = Convert.ToString(value, 2);

        if (asBinary.Length > Elements.Length)
            throw new Exception("Value too large for register");

        asBinary = asBinary.PadLeft(Elements.Length, '0');

        for (int i = 0; i < asBinary.Length; i++)
        {
            Pins[i].Value = asBinary[i] == '1';
        }
    }

    public void SetBit(int bit, bool value) =>
        Pins[bit].Value = value;

    public void SetToMaxValue()
    {
        foreach (var pin in Pins)
        {
            pin.Value = true;
        }
    }
}
