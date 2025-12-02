using System.Numerics;

namespace Advent2025.Shared;

public class ModNum
{
    public long number;
    public long modulo;
    public bool hasPrimeModulo;

    public ModNum(long number, long modulo, bool hasPrimeModulo)
    {
        number = number % modulo;
        if (number < 0) number += modulo;
        this.number = number;

        this.modulo = modulo;
        this.hasPrimeModulo = hasPrimeModulo;
    }
    public ModNum(long number, long modulo) : this(number, modulo, Primes.CheckPrime(modulo)) { }

    // addition
    public static ModNum operator +(ModNum mn, long other)
    {
        long newVal = (mn.number + other) % mn.modulo;

        return new ModNum(newVal, mn.modulo, mn.hasPrimeModulo);
    }
    public static ModNum operator -(ModNum mn, long other) => mn + -other;
    public static ModNum operator +(ModNum mn, ModNum other) => mn + other.number;
    public static ModNum operator -(ModNum mn, ModNum other) => mn - other.number;

    public static long operator +(long other, ModNum mn) => other + mn.number;
    public static long operator -(long other, ModNum mn) => other - mn.number;
    public static long operator *(long other, ModNum mn) => other * mn.number;
    public static long operator /(long other, ModNum mn) => other / mn.number;

    public static ModNum operator ++(ModNum mn) => mn + 1L;
    public static ModNum operator --(ModNum mn) => mn + -1L;

    // multiplication
    public static ModNum operator *(ModNum mn, long other)
    {
        BigInteger inbetween = mn.number * other;
        long newVal = (long)(inbetween % mn.modulo);

        return new ModNum(newVal, mn.modulo, mn.hasPrimeModulo);
    }

    public override string ToString()
    {
        return number.ToString();
    }
}
