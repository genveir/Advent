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
    }
}
