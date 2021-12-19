using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent19
{
    public class ScannerNormalizer
    {
        public Scanner myScanner;

        public Coordinate[] normalizedCoordinates;

        public int normalizationRotation;
        public bool[] normalizationFlips;
        public long[] normalizationShifts;

        public ScannerNormalizer(Scanner scanner)
        {
            this.myScanner = scanner;

            var coordinates = myScanner.Coordinates;

            normalizedCoordinates = new Coordinate[coordinates.Length];
            for (int n = 0; n < coordinates.Length; n++) normalizedCoordinates[n] =
                    new Coordinate(coordinates[n].X, coordinates[n].Y, coordinates[n].Z);

            this.normalizationRotation = 0;
            this.normalizationFlips = new bool[] { false, false, false };
            this.normalizationShifts = new long[] { 0, 0, 0 };
        }

        public void NormalizeAgainst(Scanner scanner)
        {
            var scannerMatch = myScanner.scannerMatches.Single(sm => sm.otherScanner == scanner);

            NormalizeRotation(scannerMatch.rotation);
            NormalizeFlips(scannerMatch);
            NormalizeShifts(scannerMatch);

            IterateNormalization(scanner);
        }

        public void IterateNormalization(Scanner normalizationStep)
        {
            while (normalizationStep != null)
            {
                DoRotation(normalizationStep.scannerNormalizer.normalizationRotation);
                DoFlips(normalizationStep.scannerNormalizer.normalizationFlips);
                DoShifts(normalizationStep.scannerNormalizer.normalizationShifts);

                normalizationStep = normalizationStep.NormalizedAgainst;
            }
        }

        public void NormalizeRotation(int rotation)
        {
            normalizationRotation = rotation;

            DoRotation(normalizationRotation);
        }

        public void DoRotation(int rotation)
        {
            for (int n = 0; n < normalizedCoordinates.Length; n++)
            {
                var coord = normalizedCoordinates[n];

                normalizedCoordinates[n] = rotation switch
                {
                    0 => new Coordinate(coord.X, coord.Y, coord.Z),
                    1 => new Coordinate(coord.Y, coord.Z.Value, coord.X),
                    2 => new Coordinate(coord.Z.Value, coord.X, coord.Y),

                    3 => new Coordinate(coord.X, coord.Z.Value, coord.Y),
                    4 => new Coordinate(coord.Z.Value, coord.Y, coord.X),
                    5 => new Coordinate(coord.Y, coord.X, coord.Z.Value),
                    _ => null
                };
            }
        }

        public void NormalizeFlips(ScannerMatch scannerMatch)
        {
            (int first, int second, Coordinate myRotationNormalizedDiff) = GetRotationNormalizedDiff(scannerMatch);

            var theirP1 = scannerMatch.probeMatches[first].theirProbe;
            var theirP2 = scannerMatch.probeMatches[second].theirProbe;
            var theirDiff = scannerMatch.otherScanner.RelativeCoordinates[theirP1][theirP2];

            normalizationFlips = new bool[3];
            if (myRotationNormalizedDiff.X != theirDiff.X) normalizationFlips[0] = true;
            if (myRotationNormalizedDiff.Y != theirDiff.Y) normalizationFlips[1] = true;
            if (myRotationNormalizedDiff.Z != theirDiff.Z) normalizationFlips[2] = true;

            DoFlips(normalizationFlips);
        }

        public void DoFlips(bool[] flips)
        {
            if (flips[0]) foreach (var coord in normalizedCoordinates) coord.X = -coord.X;
            if (flips[1]) foreach (var coord in normalizedCoordinates) coord.Y = -coord.Y;
            if (flips[2]) foreach (var coord in normalizedCoordinates) coord.Z = -coord.Z;
        }

        public void NormalizeShifts(ScannerMatch scannerMatch)
        {
            var pair = scannerMatch.probeMatches.First();
            var mine = normalizedCoordinates[pair.myProbe];
            var theirs = scannerMatch.otherScanner.Coordinates[pair.theirProbe];

            normalizationShifts = new long[3];
            normalizationShifts[0] = theirs.X - mine.X;
            normalizationShifts[1] = theirs.Y - mine.Y;
            normalizationShifts[2] = theirs.Z.Value - mine.Z.Value;

            DoShifts(normalizationShifts);
        }

        public void DoShifts(long[] shifts)
        {
            foreach (var coord in normalizedCoordinates)
            {
                coord.X += shifts[0];
                coord.Y += shifts[1];
                coord.Z += shifts[2];
            }
        }

        public (int first, int second, Coordinate myDiff) GetRotationNormalizedDiff(ScannerMatch scannerMatch)
        {
            for (int n = 0; n < scannerMatch.probeMatches.Count; n++)
            {
                for (int i = 0; i < scannerMatch.probeMatches.Count; i++)
                {
                    var myRotationNormalizedDiff = GetRotationNormalizedDiff(scannerMatch, n, i, scannerMatch.rotation);

                    if (myRotationNormalizedDiff != null) return (n, i, myRotationNormalizedDiff);
                }
            }

            return (-1, -1, null);
        }

        public Coordinate GetRotationNormalizedDiff(ScannerMatch scannerMatch, int probe1, int probe2, int rotation)
        {
            var myP1 = scannerMatch.probeMatches[probe1].myProbe;
            var myP2 = scannerMatch.probeMatches[probe2].myProbe;
            var myRotationNormalizedDiff = myScanner.RelativeCoordinates[myP1][myP2];
            var result = rotation switch
            {
                0 => new Coordinate(myRotationNormalizedDiff.X, myRotationNormalizedDiff.Y, myRotationNormalizedDiff.Z),
                1 => new Coordinate(myRotationNormalizedDiff.Y, myRotationNormalizedDiff.Z.Value, myRotationNormalizedDiff.X),
                2 => new Coordinate(myRotationNormalizedDiff.Z.Value, myRotationNormalizedDiff.X, myRotationNormalizedDiff.Y),

                3 => new Coordinate(myRotationNormalizedDiff.X, myRotationNormalizedDiff.Z.Value, myRotationNormalizedDiff.Y),
                4 => new Coordinate(myRotationNormalizedDiff.Z.Value, myRotationNormalizedDiff.Y, myRotationNormalizedDiff.X),
                5 => new Coordinate(myRotationNormalizedDiff.Y, myRotationNormalizedDiff.X, myRotationNormalizedDiff.Z.Value),
                _ => null
            };

            if (result.X == 0) return null;
            if (result.Y == 0) return null;
            if (result.Z == 0) return null;
            return result;
        }
    }
}
