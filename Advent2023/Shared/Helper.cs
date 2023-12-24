using Advent2023.Shared.InputParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Advent2023.Shared;

public static class Helper
{
    public static string StrReverse(this string input)
    {
        return new string(ArrReverse<char>(input.ToCharArray()));
    }

    public static T[] ArrReverse<T>(this T[] input)
    {
        var result = new T[input.Length];
        for (int n = 0; n < input.Length; n++)
        {
            result[n] = input[input.Length - n - 1];
        }
        return result;
    }

    public static T[] Segment<T>(this T[] input, int startIndex, int length)
    {
        var result = new T[length];
        Array.Copy(input, startIndex, result, 0, length);
        return result;
    }

    public static string[] Parts(this string input, int elementLength, int dividerLength)
    {
        List<string> parts = new();
        for (int n = 0; n < input.Length;)
        {
            parts.Add(input.Substring(n, elementLength));

            n += elementLength + dividerLength;
        }

        return parts.ToArray();
    }

    public static T Parse<T>(this string input, string pattern) => new InputParser<T>(pattern).Parse(input);
    public static (T1, T2) Parse<T1, T2>(this string input, string pattern) => new InputParser<T1, T2>(pattern).Parse(input);
    public static (T1, T2, T3) Parse<T1, T2, T3>(this string input, string pattern) => new InputParser<T1, T2, T3>(pattern).Parse(input);
    public static (T1, T2, T3, T4) Parse<T1, T2, T3, T4>(this string input, string pattern) => new InputParser<T1, T2, T3, T4>(pattern).Parse(input);
    public static (T1, T2, T3, T4, T5) Parse<T1, T2, T3, T4, T5>(this string input, string pattern) => new InputParser<T1, T2, T3, T4, T5>(pattern).Parse(input);
    public static (T1, T2, T3, T4, T5, T6) Parse<T1, T2, T3, T4, T5, T6>(this string input, string pattern) => new InputParser<T1, T2, T3, T4, T5, T6>(pattern).Parse(input);
    public static (T1, T2, T3, T4, T5, T6, T7) Parse<T1, T2, T3, T4, T5, T6, T7>(this string input, string pattern) => new InputParser<T1, T2, T3, T4, T5, T6, T7>(pattern).Parse(input);

    public static T Parse<T>(this string input, string pattern, Func<T> defaultOnEmpty)
    {
        if (string.IsNullOrWhiteSpace(input)) return defaultOnEmpty();

        else return input.Parse<T>(pattern);
    }

    public static T[] Rotate<T>(this T[] input, int rotation)
    {
        var positiveRotation = rotation % input.Length;
        positiveRotation += input.Length;
        positiveRotation = positiveRotation % input.Length;

        var result = new T[input.Length];
        Array.Copy(input, 0, result, positiveRotation, input.Length - positiveRotation);
        Array.Copy(input, input.Length - 1 - positiveRotation, result, 0, positiveRotation);

        return result;
    }

    public static T[] DeepCopy<T>(this T[] input)
    {
        var result = new T[input.Length];
        input.CopyTo(result, 0);

        return result;
    }

    public static T[][] GetPermutations<T>(this T[] input)
    {
        var result = new List<T[]>();

        if (input.Length == 1) result.Add(input);
        else
        {
            for (long n = 0; n < input.Length; n++)
            {
                var inputWithoutNth = input.WithoutNth(n);

                var nextLevel = GetPermutations(inputWithoutNth);

                foreach (var perm in nextLevel)
                {
                    var singleResult = new T[input.Length];
                    singleResult[0] = input[n];
                    Array.Copy(perm, 0, singleResult, 1, perm.Length);

                    result.Add(singleResult);
                }
            }
        }

        return result.ToArray();
    }

    public static long Factorial(long n)
    {
        if (n == 1) return 1;
        return n + Factorial(n - 1);
    }

    public static T[] WithoutNth<T>(this T[] input, long n)
    {
        var inputWithoutNth = new T[input.Length - 1];
        Array.Copy(input, 0, inputWithoutNth, 0, n);
        Array.Copy(input, n + 1, inputWithoutNth, n, input.Length - n - 1);

        return inputWithoutNth;
    }

    public static long LCM(long first, long second)
    {
        BigInteger product = first * second;
        BigInteger gcd = GCD(first, second);
        var result = product / gcd;

        return (long)result;
    }

    public static long GCD(long first, long second)
    {
        if (first == 0) return second;
        if (second == 0) return first;
        if (first == 1 || second == 1) return 1;

        if (first == 2) return Non1GCD(second, first);

        return Non1GCD(first, second);
    }

    public static long Non1GCD(long first, long second)
    {
        while (true)
        {
            if (first > second)
            {
                first = first % second;
                if (first == 0) break;
            }
            else
            {
                second = second % first;
                if (second == 0) break;
            }
        }

        if (second == 0) return first;
        return second;
    }

    public static double GetAngle((long x, long y) from, (long x, long y) target)
    {
        return GetAngle(target.x - from.x, target.y - from.y);
    }

    public static double GetAngle(long X, long Y)
    {
        var angle = ((Math.Atan2(Y, X) + 0.5 * Math.PI) + 2.0d * Math.PI) % (2.0d * Math.PI);
        return angle;
    }

    public static long[] AsDigits(this string input)
    {
        return input.Select(c => (long)(c - 48)).ToArray();
    }

    public static List<List<T>> Pivot<T>(this IEnumerable<IEnumerable<T>> input)
    {
        T[][] arrayInput = input.Select(i => i.ToArray()).ToArray();

        var output = new List<List<T>>();
        for (int originalColumn = 0; originalColumn < arrayInput[0].Length; originalColumn++)
        {
            var columnList = new List<T>();
            for (int originalRow = 0; originalRow < arrayInput.Length; originalRow++)
            {
                columnList.Add(arrayInput[originalRow][originalColumn]);
            }
            output.Add(columnList);
        }

        return output;
    }

    public static void InitAndUpdate<KeyType, ValueType>(this Dictionary<KeyType, ValueType> dict,
        KeyType key, Func<ValueType, ValueType> action, ValueType defaultValue = default(ValueType))
    {
        if (!dict.ContainsKey(key)) dict[key] = defaultValue;
        dict[key] = action(dict[key]);
    }

    public static void PrintGrid(IEnumerable<Coordinate2D> coordinates, bool flipX = false, bool flipY = false) =>
        PrintGrid(coordinates, c => c, c => cBLOCK, ' ', flipX, flipY);

    public static void PrintGrid<T>(IEnumerable<T> toPrint, Func<T, Coordinate2D> convertToCoordinate, Func<T, char> convertToChar,
        char emptyChar = ' ', bool flipX = false, bool flipY = false)
    {
        var lookup = toPrint.ToDictionary(c => convertToCoordinate(c), c => c);
        var coordinates = lookup.Keys;

        var minX = (int)coordinates.Min(c => c.X);
        var maxX = (int)coordinates.Max(c => c.X);
        var minY = (int)coordinates.Min(c => c.Y);
        var maxY = (int)coordinates.Max(c => c.Y);

        var fromX = flipX ? maxX : minX;
        Func<int, bool> xCondition = (int x) => flipX ? x >= minX : x <= maxX;
        Func<int, int> xIncrement = (int x) => flipX ? x - 1 : x + 1;

        var fromY = flipY ? minY : maxY;
        Func<int, bool> yCondition = (int y) => flipY ? y <= maxY : y >= minY;
        Func<int, int> yIncrement = (int y) => flipY ? y + 1 : y - 1;

        for (int y = fromY; yCondition(y); y = yIncrement(y))
        {
            for (int x = fromX; xCondition(x); x = xIncrement(x))
            {
                var coord = new Coordinate2D(x, y);
                var cToPrint = lookup.TryGetValue(coord, out T value) ? convertToChar(value) : emptyChar;

                Console.Write(cToPrint);
            }
            Console.WriteLine();
        }
    }

    public const char cBLOCK = '\U00002588';
    public const string BLOCK = "\U00002588";
}
