using Advent2021.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent19
{
    public class Scanner
    {
        public long requiredMatches;

        public long number;
        public ScannerCoordinates scannerCoordinates;
        public ScannerNormalizer scannerNormalizer;

        public Coordinate[] Coordinates => scannerCoordinates.coordinates;
        public Coordinate[][] RelativeCoordinates => scannerCoordinates.relativeCoordinates;
        public Coordinate[][] AbsoluteRelativeCoordinates => scannerCoordinates.absoluteRelativeCoordinates;
        public Coordinate[][][] RotatedAbsoluteRelativeCoordinates => scannerCoordinates.rotatedAbsoluteRelativeCoordinates;

        public Coordinate[] normalizedCoordinates => scannerNormalizer.normalizedCoordinates;

        public bool HasBeenNormalized;
        public Scanner NormalizedAgainst;

        public Coordinate OwnPosition()
        {
            Coordinate position;
            if (NormalizedAgainst != null) position = NormalizedAgainst.OwnPosition();
            else position = new Coordinate(0, 0, 0);

            position.X += scannerNormalizer.normalizationShifts[0];
            position.Y += scannerNormalizer.normalizationShifts[1];
            position.Z += scannerNormalizer.normalizationShifts[2];

            return position;
        }

        public List<ScannerMatch> scannerMatches;

        [ComplexParserConstructor]
        public Scanner(long number, Coordinate[] coordinates, long requiredMatches = 12)
        {
            this.number = number;
            this.scannerCoordinates = new ScannerCoordinates(coordinates);
            this.scannerNormalizer = new ScannerNormalizer(this);

            this.requiredMatches = requiredMatches;
        }

        public void NormalizeAgainst(Scanner scanner)
        {
            scannerNormalizer.NormalizeAgainst(scanner);

            NormalizedAgainst = scanner;
            HasBeenNormalized = true;
        }

        public void FindMatchingScanners(IEnumerable<Scanner> allScanners)
        {
            scannerMatches = new List<ScannerMatch>();
            foreach (var other in allScanners)
            {
                if (this == other) continue;

                var match = MatchWith(other);

                if (match != null) scannerMatches.Add(match);
            }
        }

        public ScannerMatch MatchWith(Scanner scanner)
        {
            var theirRotAbsCoordinates = scanner.RotatedAbsoluteRelativeCoordinates;

            var probeMatches = new List<ProbeMatch>();
            for (int theirProbe = 0; theirProbe < theirRotAbsCoordinates.Length; theirProbe++)
            {
                var probeMatch = MatchProbe(theirProbe, theirRotAbsCoordinates[theirProbe]);
                if (probeMatch != null) probeMatches.Add(probeMatch);
            }

            if (probeMatches.Count > 0) return new ScannerMatch(this, scanner, probeMatches);
            else return null;
        }

        public ProbeMatch MatchProbe(int theirProbeId, Coordinate[][] theirRotAbsCoordinates)
        {
            for (int myProbeId = 0; myProbeId < Coordinates.Length; myProbeId++)
            {
                for (int rotation = 0; rotation < 6; rotation++)
                {
                    var matches = MatchAbsRotated(AbsoluteRelativeCoordinates[myProbeId], theirRotAbsCoordinates[rotation]);

                    if (matches.Length >= requiredMatches)
                    {
                        return new ProbeMatch(myProbeId, theirProbeId, rotation);
                    }
                }
            }

            return null;
        }

        public static Coordinate[] MatchAbsRotated(Coordinate[] myCoords, Coordinate[] theirCoords)
        {
            return myCoords.Intersect(theirCoords).ToArray();
        }

        public override string ToString()
        {
            var normalized = HasBeenNormalized ? "Normalized " : "DISCONNECTED ";
            var pos = OwnPosition();
            return $"{normalized} Scanner {number} at {pos}";
        }
    }
}
