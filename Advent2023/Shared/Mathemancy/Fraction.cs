using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023.Shared.Mathemancy;

/// <summary>
/// A Normalized Fraction
/// </summary>
public class Fraction
{
    private long _top;
    public long Top
    { 
        get { return _top; }
        set { _top = value; Normalize(); } 
    }

    private long _bottom;
    public long Bottom
    {
        get { return _bottom; }
        set { _bottom = value; Normalize();}
    }

    public long Numerator { get => Top; set => Top = value; }
    public long Denominator { get => Bottom; set => Bottom = value; }

    /// <summary>
    /// Creates a fraction from two input values and normalizes it
    /// </summary>
    /// <param name="top">The numerator</param>
    /// <param name="bottom">The denominator</param>
    /// <exception cref="ArgumentException">Bottom can not be 0</exception>
    public Fraction(long top, long bottom)
    {
        if (bottom == 0)
            throw new ArgumentException("fraction cannot have a 0 divider");

        _top = top;
        _bottom = bottom;

        Normalize();
    }

    private void Normalize()
    {
        if (_top == 0)
        {
            _bottom = 1;
        }
        else
        {
            var gcd = Helper.GCD(Math.Abs(_top), Math.Abs(_bottom));
            var makeNegative = (_top < 0) ^ (_bottom < 0);

            _top = Math.Abs(_top / gcd);
            if (makeNegative) _top = -_top;

            _bottom = Math.Abs(_bottom / gcd);
        }
    }

    public bool IsInteger => Bottom == 1;

    public long ToLong() => Top / Bottom;

    public double ToDouble() => (double)Top / Bottom;

    #region operators
    public static implicit operator Fraction(long value) => new(value, 1);

    public static explicit operator long(Fraction value) => value.ToLong();
    public static explicit operator double(Fraction value) => value.ToDouble();

    public static Fraction operator -(Fraction toNegate) =>
        new Fraction(-toNegate.Top, toNegate.Bottom);

    public static Fraction operator *(Fraction first, Fraction second) =>
        new(first.Top * second.Top, first.Bottom * second.Bottom);

    public static Fraction operator /(Fraction first, Fraction second) =>
        first * new Fraction(second.Bottom, second.Top);

    public static Fraction operator -(Fraction first, Fraction second) =>
        first + -second;

    public static Fraction operator +(Fraction first, Fraction second) =>
        new(first.Top * second.Bottom + second.Top * first.Bottom, first.Bottom * second.Bottom);

    public static bool operator ==(Fraction first, Fraction second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null)) return false;
        return first.Equals(second);
    }

    public static bool operator !=(Fraction first, Fraction second) => !(first == second);

    public static bool operator >(Fraction first, Fraction second) => first.Top * second.Bottom > second.Top * first.Bottom;

    public static bool operator <(Fraction first, Fraction second) => first.Top * second.Bottom < second.Top * first.Bottom;

    public static bool operator >=(Fraction first, Fraction second) => first == second || first > second;

    public static bool operator <=(Fraction first, Fraction second) => first == second || first < second;

    #endregion

    public override int GetHashCode()
    {
        return Top.GetHashCode() + 7247 * Bottom.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        var other = obj as Fraction;
        if (ReferenceEquals(other, null)) return false;
        
        return other.Top == this.Top && 
            other.Bottom == this.Bottom;
    }

    public override string ToString() => $"{Top} / {Bottom}";
}
