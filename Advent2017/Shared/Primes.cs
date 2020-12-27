using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.Shared
{
    public static class Primes
    {
        public static List<long> primesSoFar;

        public static bool CheckPrime(long numToCheck)
        {
            if (primesSoFar == null) primesSoFar = new List<long>() { 2, 3, 5 };

            long numRoot = (long)Math.Ceiling(Math.Sqrt(numToCheck));

            GenerateAllPrimes(primesSoFar, numRoot);

            return CheckPrime(primesSoFar, numToCheck);
        }

        public static bool IsPrime(this long numToCheck) => CheckPrime(numToCheck);
        public static bool IsPrime(this int numToCheck) => CheckPrime(numToCheck);

        private static void GenerateAllPrimes(List<long> primesSoFar, long upperBound)
        {
            long numToCheck = 7;
            while(true)
            {
                CheckPrime(primesSoFar, numToCheck);
                numToCheck += 4;
                CheckPrime(primesSoFar, numToCheck);
                numToCheck += 2;

                if (numToCheck > upperBound) break;
            }
        }

        private static bool CheckPrime(List<long> primesSoFar, long numToCheck)
        {
            long numRoot = (long)Math.Ceiling(Math.Sqrt(numToCheck));
            var n = 2;

            bool notPrime = false;
            for (var primeToCheck = primesSoFar[n]; primeToCheck <= numRoot; primeToCheck = primesSoFar[++n])
            {
                if (numToCheck % primeToCheck == 0) { notPrime = true; break; }
            }
            if (!notPrime)
            {
                primesSoFar.Add(numToCheck);
                return true;
            }
            return false;
        }
    }
}
