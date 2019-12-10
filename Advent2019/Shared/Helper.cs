using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2019.Shared
{
    public static class Helper
    {
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
                for (int n = 0; n < input.Length; n++)
                {
                    var inputWithoutNth = input.WithoutNth(n);

                    var nextLevel = GetPermutations(inputWithoutNth);

                    foreach(var perm in nextLevel)
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

        public static int Factorial(int n)
        {
            if (n == 1) return 1;
            return n + Factorial(n - 1);
        }

        public static T[] WithoutNth<T>(this T[] input, int n)
        {
            var inputWithoutNth = new T[input.Length - 1];
            Array.Copy(input, 0, inputWithoutNth, 0, n);
            Array.Copy(input, n + 1, inputWithoutNth, n, input.Length - n - 1);

            return inputWithoutNth;
        }

        public static int GCD(int first, int second)
        {
            if (first == 0) return second;
            if (second == 0) return first;
            if (first == 1 || second == 1) return 1;

            if (first == 2) return Non1GCD(second, first);

            return Non1GCD(first, second);
        }

        public static int Non1GCD(int first, int second)
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

        public static double GetAngle((int x, int y) from, (int x, int y) target)
        {
            return GetAngle(target.x - from.x, target.y - from.y);
        }

        public static double GetAngle(int X, int Y)
        {
            var angle = ((Math.Atan2(Y, X) + 0.5 * Math.PI) + 2.0d * Math.PI) % (2.0d * Math.PI);
            return angle;
        }
    }
}
