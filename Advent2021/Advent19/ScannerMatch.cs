using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Advent19
{
    public class ScannerMatch
    {
        public Scanner otherScanner;
        public List<ProbeMatch> probeMatches;

        public int rotation;
        public bool xFlipped;
        public bool yFlipped;
        public bool zFlipped;

        public ScannerMatch(Scanner ownScanner, Scanner otherScanner, List<ProbeMatch> probeMatches)
        {
            this.otherScanner = otherScanner;
            this.probeMatches = probeMatches;

            rotation = probeMatches.Select(p => p.rotation).Distinct().Single(); // works
        }
    }

    public class ProbeMatch
    {
        public long myProbe;
        public long theirProbe;

        // rotationData
        public int rotation;

        public ProbeMatch(long myProbe, long theirProbe, int rotation)
        {
            this.myProbe = myProbe;
            this.theirProbe = theirProbe;
            this.rotation = rotation;
        }

        public override string ToString()
        {
            return $"my Probe: {myProbe}, their probe: {theirProbe}, rotation: {rotation}";
        }
    }
}
