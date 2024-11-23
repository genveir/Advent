namespace Advent2024.Shared.Mathemancy;

/// <summary>
/// A linear equation of the form Output = a * Input + b, with ways to parse it, and get values for inputs
/// and outputs based on the other
/// </summary>
public class LinearEquation
{
    public Fraction A { get; }
    public Fraction B { get; }

    /// <summary>
    /// Construct a linear equation in the form Output = a * Input + b
    /// </summary>
    /// <param name="a">the amount by which Output increases when Input increases by 1</param>
    /// <param name="b">the value of Output when Input is zero</param>
    public LinearEquation(Fraction a, Fraction b)
    {
        A = a;
        B = b;
    }

    /// <summary>
    /// Construct a linear equation in the form Output = a * Input + b
    /// </summary>
    /// <param name="outputVal">The value of Output at a certain coordinate</param>
    /// <param name="inputVal">The value of Input at a certain coordinate</param>
    /// <param name="vector">the amount by which Output increases when Input increases by 1</param>
    public LinearEquation(long outputVal, long inputVal, Fraction vector) : this(vector, -vector * inputVal + outputVal)
    { }

    /// <summary>
    /// Construct a linear equation in the form Output = a * Input + b
    /// </summary>
    /// <param name="point">A point, where Input is first and Output is second</param>
    /// <param name="vector">the amount by which Output increases when Input increases by 1</param>
    public LinearEquation(Coordinate2D point, Fraction vector) : this(point.Y, point.X, vector)
    { }

    /// <summary>
    /// Construct a linear equation in the form Output = a * Input + b
    /// </summary>
    /// <param name="first">A point, where Input is first and Output is second</param>
    /// <param name="second">A point, where Input is first and Output is second</param>
    public LinearEquation(Coordinate2D first, Coordinate2D second) :
        this(first, new Fraction(first.Y - second.Y, first.X - second.X))
    { }

    /// <summary>
    /// Return the value of Output at a value for Input
    /// </summary>
    /// <param name="input">The input value</param>
    /// <returns>The output value</returns>
    public Fraction ValueAt(long input) => ValueAt(new Fraction(input, 1));

    /// <summary>
    /// Return the value of Output at a value for Input
    /// </summary>
    /// <param name="input">The input value</param>
    /// <returns>The output value</returns>
    public Fraction ValueAt(Fraction input) => A * input + B;

    /// <summary>
    /// Return the value the input should take to get a certain output
    /// </summary>
    /// <param name="output">the expected output</param>
    /// <returns>the value of the input, null if none exists</returns>
    public Fraction InputFor(Fraction output) =>
        (A.Top == 0) ? null : (output - B) / A;

    /// <summary>
    /// Tries to find an intersection between two equations with the same in- and outputs
    /// </summary>
    /// <param name="other">the equation to intersect with</param>
    /// <param name="sameEquation">true if these equations are the same</param>
    /// <param name="input">the value of the input-parameter at the point of intersection, 0 if the equations are the same</param>
    /// <param name="output">the value of the output-parameter at the point of intersection, ValueAt(0) if the equations are the same</param>
    /// <returns>true when an intersection exists</returns>
    public bool TryFindIntersection(LinearEquation other, out bool sameEquation, out Fraction input, out Fraction output)
    {
        sameEquation = false;

        if (A == other.A)
        {
            if (B == other.B)
            {
                sameEquation = true;
                input = 0;
                output = ValueAt(0);
                return true;
            }
            input = null;
            output = null;
            return false;
        }

        input = (B - other.B) / (other.A - A);
        output = ValueAt(input);

        return true;
    }

    public static bool operator ==(LinearEquation left, LinearEquation right)
    {
        if (ReferenceEquals(left, right)) return true;
        return left is not null
            && left.Equals(right);
    }

    public static bool operator !=(LinearEquation left, LinearEquation right) => !(left == right);

    public override int GetHashCode() => A.GetHashCode() + 17 * B.GetHashCode();

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        return obj is LinearEquation other
            && A.Equals(other.A)
            && B.Equals(other.B);
    }

    public override string ToString() => $"out = {A} in + {B}";
}