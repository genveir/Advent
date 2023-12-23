using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Advent2023.Shared;

namespace Advent2023.Advent21;

public class Solution : ISolution
{
    public char[][] grid;

    public long Start;
    public HashSet<long> Plots;

    public HashSet<long>[] Front;

    public long StepsNum = 64;
    public long LoopStepsNum = 26501365;
    public bool Quadratic = true;

    public Solution(string input)
    {
        grid = Input.GetLetterGrid(input);
    }
    public Solution() : this("Input.txt") { }

    public void Reset()
    {
        Plots = new();

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] is 'S' or '.')
                {
                    var coord = FromCoordinates(x, y);
                    Plots.Add(coord);

                    if (grid[y][x] == 'S') Start = coord;
                }
            }
        }

        Front = new HashSet<long>[2];
        Front[0] = new() { Start };
        Front[1] = new();
    }

    // two factors of 10 higher than the large number in the assignment
    public static long YFactor = 1_000_000_000L;
    public static long XPlus = 500_000_000L;
    public static long FromCoordinates(long x, long y) => YFactor * y + x + XPlus;

    public static long Mod(long input, long modulo) => (input % modulo + modulo) % modulo;

    long modCoord;
    public bool IsPlot(long coordinate, bool loop)
    {
        if (loop)
        {
            var yPart = coordinate / YFactor;
            if (yPart < 0) yPart--;

            var xPart = coordinate - yPart * YFactor - XPlus;

            var x = Mod(xPart, grid[0].Length);
            var y = Mod(yPart, grid.Length);

            modCoord = FromCoordinates(x, y);
        }
        else modCoord = coordinate;

        return Plots.Contains(modCoord);
    }

    public long DoWalkies(bool loop)
    {
        var stepsToTake = loop ? LoopStepsNum : StepsNum;
        var neighbours = new long[4];

        long numOdd = 0;
        long numEven = 1; // we never revisit the original spot

        HashSet<long> active = Front[0];
        HashSet<long> passive = Front[1];
        HashSet<long> buffer = new();
        HashSet<long> swap;

        List<long> values = new();

        var modDiff = LoopStepsNum % grid.Length - 1;
        var stepsToSave = new List<long>()
        {
            modDiff,
            modDiff + grid.Length,
            modDiff + 2 * grid.Length
        };
        var step = 0;

        bool odd = true;
        for (int n = 0; n < stepsToTake; n++)
        {
            foreach (var node in active)
            {
                neighbours[0] = node - 1; // left
                neighbours[1] = node - YFactor; // up
                neighbours[2] = node + 1; // right
                neighbours[3] = node + YFactor; // down

                neighbours[0] = passive.Contains(neighbours[0]) ? 0 : IsPlot(neighbours[0], loop) ? neighbours[0] : 0;
                neighbours[1] = passive.Contains(neighbours[1]) ? 0 : IsPlot(neighbours[1], loop) ? neighbours[1] : 0;
                neighbours[2] = passive.Contains(neighbours[2]) ? 0 : IsPlot(neighbours[2], loop) ? neighbours[2] : 0;
                neighbours[3] = passive.Contains(neighbours[3]) ? 0 : IsPlot(neighbours[3], loop) ? neighbours[3] : 0;

                if (neighbours[0] != 0) buffer.Add(neighbours[0]);
                if (neighbours[1] != 0) buffer.Add(neighbours[1]);
                if (neighbours[2] != 0) buffer.Add(neighbours[2]);
                if (neighbours[3] != 0) buffer.Add(neighbours[3]);
            }

            if (odd) numOdd += buffer.Count;
            else numEven += buffer.Count;

            if (Quadratic)
            {
                if (stepsToSave[step] == n)
                {
                    var value = odd ? numOdd : numEven;
                    Debug.WriteLine($"value at {n} is {value}");

                    values.Add(value);
                    step++;
                }
                if (step == 3) return CalculateQuadratic(values, stepsToTake);
            }

            odd = !odd;

            swap = passive;
            passive = active;
            active = buffer;
            buffer = swap;
            buffer.Clear();
        }

        return stepsToTake % 2 == 0 ? numEven : numOdd;
    }

    // well this could have been simpler
    public long CalculateQuadratic(List<long> values, long stepsToTake)
    {
        var c = values[0];

        // values[0] = a * 0 + b * 0 + 1 * c
        var firstQFD = new QuadFuncDinges(values[0], 0, 0);
        // values[1] = a * 1 + b * 1 + 1 * c
        var secondQFD = new QuadFuncDinges(values[1], 1, 1);
        // values[2] = a * 4 + b * 2 + 1 * c
        var thirdQFD = new QuadFuncDinges(values[2], 4, 2);

        var negFirst = QuadFuncDinges.Negate(firstQFD);

        var NoC1 = QuadFuncDinges.Add(negFirst, secondQFD);
        var NoC2 = QuadFuncDinges.Add(negFirst, thirdQFD);

        var EqB1 = QuadFuncDinges.Product(NoC1, NoC2.bMult);
        var EqB2 = QuadFuncDinges.Product(NoC2, NoC1.bMult);

        var NegEqB1 = QuadFuncDinges.Negate(EqB1);

        var NoBC = QuadFuncDinges.Add(NegEqB1, EqB2);
        long a = NoBC.yVal / NoBC.aMult;

        var EqA1 = QuadFuncDinges.Product(NoC2, NoC1.aMult);
        var EqA2 = QuadFuncDinges.Product(NoC1, NoC2.aMult);

        var NegEqA1 = QuadFuncDinges.Negate(EqA1);

        var NoAC = QuadFuncDinges.Add(NegEqA1, EqA2);
        long b = NoAC.yVal / NoAC.bMult;

        Func<long, long> Y = (x) => a * x * x + b * x + c;

        var steps = stepsToTake / grid.Length;

        return Y(steps);
    }

    private class QuadFuncDinges
    {
        public QuadFuncDinges(long yVal, long aMult, long bMult)
        {
            this.yVal = yVal;
            this.aMult = aMult;
            this.bMult = bMult;
            cMult = 1;
        }

        public long yVal;
        public long aMult;
        public long bMult;
        public long cMult;

        public void Flip()
        {
            aMult = -aMult;
            bMult = -bMult;
            cMult = -cMult;
        }

        public static QuadFuncDinges Negate(QuadFuncDinges input)
        {
            return new(-input.yVal, -input.aMult, -input.bMult)
            {
                cMult = -input.cMult
            };
        }

        public static QuadFuncDinges Add(QuadFuncDinges first, QuadFuncDinges second)
        {
            return new(first.yVal + second.yVal, first.aMult + second.aMult, first.bMult + second.bMult)
            {
                cMult = first.cMult + second.cMult
            };
        }

        public static QuadFuncDinges Product(QuadFuncDinges input, long mult)
        {
            return new(input.yVal * mult, input.aMult * mult, input.bMult * mult)
            {
                cMult = input.cMult * mult
            };
        }

        public static QuadFuncDinges Divide(QuadFuncDinges input, long divisor)
        {
            return new(input.yVal / divisor, input.aMult / divisor, input.bMult / divisor)
            {
                cMult = input.cMult / divisor
            };
        }

        public override string ToString()
        {
            return $"{yVal} = {aMult}a + {bMult}b + {cMult}c";
        }
    }

    public List<long> GetDifferenceSequence(List<long> original, int stepSize = 1)
    {
        List<long> differences = new List<long>();
        for (int n = stepSize; n < original.Count; n += stepSize)
        {
            differences.Add(original[n] - original[n - stepSize]);
        }
        return differences;
    }

    public object GetResult1()
    {
        Reset();

        return DoWalkies(false);
    }

    public object GetResult2()
    {
        Reset();

        return DoWalkies(true);
    }
}
