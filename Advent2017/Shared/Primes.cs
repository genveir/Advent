﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.Shared
{
    public static class Primes
    {
        public static bool CheckPrime(long numToCheck)
        {
            List<long> primesSoFar = FirstLongPrimes.PrimeLookup.ToList();

            long numRoot = (long)Math.Ceiling(Math.Sqrt(numToCheck));

            GenerateAllPrimes(primesSoFar, numRoot);

            return CheckPrime(primesSoFar, numToCheck);
        }

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

            using (var writer = new StreamWriter(@"d:\temp\primes.txt"))
            {
                for (int i = 0; i < primesSoFar.Count; i++)
                {
                    writer.WriteLine(primesSoFar[i] +",");
                }
            }
            Console.WriteLine("done generating primes to " + upperBound);
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
