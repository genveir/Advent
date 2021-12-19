﻿using Advent2021.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2021.Advent19
{
    class Tests
    {
        [TestCase(example, 79)]
        public void Test1(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult1());
        }

        [TestCase(example, 3621)]
        public void Test2(string input, object output)
        {
            var sol = new Solution(input);

            Assert.AreEqual(output, sol.GetResult2());
        }

        [Test]
        public void CheckPossibleMatches1and2()
        {
            var sol = new Solution(example);

            var s1 = sol.scanners[0];
            var s2 = sol.scanners[1];

            var matched = s1.MatchWith(s2);

            Assert.AreEqual(12, matched.probeMatches.Count);
        }

        [Test]
        public void CheckOne()
        {
            var sol = new Solution(example);

            var s1 = sol.scanners[0];
            var s2 = sol.scanners[1];

            var match = s2.MatchProbe(9, s1.RotatedAbsoluteRelativeCoordinates[9]);

            Assert.IsNotNull(match);
        }

        [Test]
        public void CheckRelative()
        {
            var sol = new Solution(test1);

            var scanner = sol.scanners[0];

            var relativePositions = scanner.RelativeCoordinates;

            Assert.AreEqual(2, relativePositions.Length);
            Assert.AreEqual(new Coordinate(0,0,0), relativePositions[0][0]);
            Assert.AreEqual(new Coordinate(1, 2, 3), relativePositions[0][1]);
            Assert.AreEqual(new Coordinate(-1, -2, -3), relativePositions[1][0]);
            Assert.AreEqual(new Coordinate(0, 0, 0), relativePositions[1][1]);
        }

//        [Test]
//        public void CheckNormalizeRotation()
//        {
//            var sol = new Solution(testRot, 3);

//            var scanners = sol.scanners;

//            scanners[1].NormalizeAgainst(scanners[0]);

//            Assert.AreEqual(new Coordinate(6, 6, 6), scanners[1].normalizedCoordinates[0]);
//            Assert.AreEqual(new Coordinate(3, 3, 3), scanners[1].normalizedCoordinates[1]);
//            Assert.AreEqual(new Coordinate(4, 5, 6), scanners[1].normalizedCoordinates[2]);
//            Assert.AreEqual(new Coordinate(19, 17, 18), scanners[1].normalizedCoordinates[3]);
//        }

//        public const string testRot = @"--- scanner 0 ---
//1,1,1
//2,3,4
//4,4,4

//--- scanner 1 ---
//6,6,6
//3,3,3
//5,6,4
//17,18,19";

//        [Test]
//        public void CheckNormalizedFlips()
//        {
//            var sol = new Solution(testFlip, 3);

//            var scanners = sol.scanners;

//            scanners[1].NormalizeAgainst(scanners[0]);

//            Assert.AreEqual(new Coordinate(-6, 6, 6), scanners[1].normalizedCoordinates[0]);
//            Assert.AreEqual(new Coordinate(-3, 3, 3), scanners[1].normalizedCoordinates[1]);
//            Assert.AreEqual(new Coordinate(-4, 5, 6), scanners[1].normalizedCoordinates[2]);
//            Assert.AreEqual(new Coordinate(-19, 17, 18), scanners[1].normalizedCoordinates[3]);
//        }

        public const string testFlip = @"--- scanner 0 ---
-1,1,1
-2,3,4
-4,4,4

--- scanner 1 ---
6,6,6
3,3,3
5,6,4
17,18,19";

        [Test]
        public void CheckNormalized()
        {
            var sol = new Solution(testFlip, 3);

            var scanners = sol.scanners;

            scanners[1].NormalizeAgainst(scanners[0]);

            Assert.AreEqual(new Coordinate(-4, 4, 4), scanners[1].normalizedCoordinates[0]);
            Assert.AreEqual(new Coordinate(-1, 1, 1), scanners[1].normalizedCoordinates[1]);
            Assert.AreEqual(new Coordinate(-2, 3, 4), scanners[1].normalizedCoordinates[2]);
            Assert.AreEqual(new Coordinate(-17, 15, 16), scanners[1].normalizedCoordinates[3]);
        }

        [Test]
        public void CheckIteratedMatches()
        {
            var sol = new Solution(testIterative, 3);

            var scanners = sol.scanners;

            Assert.AreEqual(1, scanners[0].scannerMatches.Count);
            Assert.AreEqual(2, scanners[1].scannerMatches.Count);
            Assert.AreEqual(1, scanners[2].scannerMatches.Count);

            Assert.That(scanners[0].scannerMatches.Any(sm => sm.otherScanner == scanners[1]));

            Assert.That(scanners[1].scannerMatches.Any(sm => sm.otherScanner == scanners[0]));
            Assert.That(scanners[1].scannerMatches.Any(sm => sm.otherScanner == scanners[2]));

            Assert.That(scanners[2].scannerMatches.Any(sm => sm.otherScanner == scanners[1]));
        }

        [Test]
        public void CheckIteratedNormalized()
        {
            var sol = new Solution(testIterative, 3);

            var scanners = sol.scanners;

            scanners[1].NormalizeAgainst(scanners[0]);
            scanners[2].NormalizeAgainst(scanners[1]);

            Assert.AreEqual(new Coordinate(-17, 15, 16), scanners[1].normalizedCoordinates[3]);
            Assert.AreEqual(new Coordinate(-29, 25, 27), scanners[1].normalizedCoordinates[4]);
            Assert.AreEqual(new Coordinate(-129, 125, 127), scanners[1].normalizedCoordinates[5]);

            Assert.AreEqual(new Coordinate(-17, 15, 16), scanners[2].normalizedCoordinates[0]);
            Assert.AreEqual(new Coordinate(-29, 25, 27), scanners[2].normalizedCoordinates[1]);
            Assert.AreEqual(new Coordinate(-129, 125, 127), scanners[2].normalizedCoordinates[2]);
        }

        public const string testIterative = @"--- scanner 0 ---
-1,1,1
-2,3,4
-4,4,4

--- scanner 1 ---
6,6,6
3,3,3
5,6,4
17,18,19
27,29,31
127,129,131

--- scanner 2 ---
0,0,0
-10,11,-12
-110,111,-112";

        [Test]
        public void CheckAbsoluteRelative()
        {
            var sol = new Solution(test1);

            var scanner = sol.scanners[0];

            var absRelPositions = scanner.AbsoluteRelativeCoordinates;

            Assert.AreEqual(2, absRelPositions.Length);
            Assert.AreEqual(new Coordinate(0, 0, 0), absRelPositions[0][0]);
            Assert.AreEqual(new Coordinate(1, 2, 3), absRelPositions[0][1]);
            Assert.AreEqual(new Coordinate(1, 2, 3), absRelPositions[1][0]);
            Assert.AreEqual(new Coordinate(0, 0, 0), absRelPositions[1][1]);
        }

        [Test]
        public void CheckRotatedAbsoluteRelative()
        {
            var sol = new Solution(test1);

            var scanner = sol.scanners[0];

            var rotAbsRelPositions = scanner.RotatedAbsoluteRelativeCoordinates;

            Assert.AreEqual(2, rotAbsRelPositions.Length);
            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[0][0][0]);
            Assert.AreEqual(new Coordinate(1, 2, 3), rotAbsRelPositions[0][0][1]);
            Assert.AreEqual(new Coordinate(1, 2, 3), rotAbsRelPositions[1][0][0]);
            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[1][0][1]);

            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[0][1][0]);
            Assert.AreEqual(new Coordinate(3, 1, 2), rotAbsRelPositions[0][1][1]);
            Assert.AreEqual(new Coordinate(3, 1, 2), rotAbsRelPositions[1][1][0]);
            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[1][1][1]);

            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[0][2][0]);
            Assert.AreEqual(new Coordinate(2, 3, 1), rotAbsRelPositions[0][2][1]);
            Assert.AreEqual(new Coordinate(2, 3, 1), rotAbsRelPositions[1][2][0]);
            Assert.AreEqual(new Coordinate(0, 0, 0), rotAbsRelPositions[1][2][1]);
        }

        [Test]
        public void ExampleMatchesAreFound()
        {
            var sol = new Solution(example);

            var s0 = sol.scanners[0];
            var s1 = sol.scanners[1];
            var s2 = sol.scanners[2];
            var s3 = sol.scanners[3];
            var s4 = sol.scanners[4];

            Assert.That(s0.scannerMatches.Any(s => s.otherScanner == s1));
            Assert.That(s1.scannerMatches.Any(s => s.otherScanner == s4));

            Assert.That(s0.scannerMatches.Any());
            Assert.That(s1.scannerMatches.Any());
            Assert.That(s2.scannerMatches.Any());
            Assert.That(s3.scannerMatches.Any());
            Assert.That(s4.scannerMatches.Any());
        }

        [Test]
        public void ExampleBeaconsAreFound()
        {
            var sol = new Solution(example);

            var allFoundBeacons = sol.GetAllBeacons();
            var exampleBeacons = Input.GetInputLines(allExampleBeacons)
                .Select(line => new Coordinate(line.Split(',').Select(long.Parse).ToArray())).ToList();

            var missingBeacons = new List<Coordinate>();

            for (int n = 0; n < exampleBeacons.Count; n++)
            {
                if (!allFoundBeacons.Contains(exampleBeacons[n])) missingBeacons.Add(exampleBeacons[n]);
            }

            Assert.AreEqual(0, missingBeacons.Count, "missing beacons: " + string.Join(Environment.NewLine, missingBeacons));
        }

        [Test]
        public void ExampleScannersAreInTheRightPlaces()
        {
            var sol = new Solution(example);

            sol.GetAllBeacons();

            Assert.AreEqual(new Coordinate(0, 0, 0), sol.scanners[0].OwnPosition());
            Assert.AreEqual(new Coordinate(68, -1246, -43), sol.scanners[1].OwnPosition());
            Assert.AreEqual(new Coordinate(-20, -1133, 1061), sol.scanners[4].OwnPosition());

            Assert.AreEqual(new Coordinate(1105, -1205, 1229), sol.scanners[2].OwnPosition());
            Assert.AreEqual(new Coordinate(-92, -2380, -20), sol.scanners[3].OwnPosition());
            
        }

        public const string missingBeacons = @"1135,-1161,1235
1243,-1093,1063
1660,-552,429
1693,-557,386
1735,-437,1738
1749,-1800,1813
1772,-405,1572
1776,-675,371
1779,-442,1789
1780,-1548,337
1786,-1538,337
1847,-1591,415
1889,-1729,1762
1994,-1805,1792";

        public const string test1 = @"--- scanner 0 ---
0,0,0
1,2,3";

        public const string example = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";

        public const string allExampleBeacons = @"-892,524,684
-876,649,763
-838,591,734
-789,900,-551
-739,-1745,668
-706,-3180,-659
-697,-3072,-689
-689,845,-530
-687,-1600,576
-661,-816,-575
-654,-3158,-753
-635,-1737,486
-631,-672,1502
-624,-1620,1868
-620,-3212,371
-618,-824,-621
-612,-1695,1788
-601,-1648,-643
-584,868,-557
-537,-823,-458
-532,-1715,1894
-518,-1681,-600
-499,-1607,-770
-485,-357,347
-470,-3283,303
-456,-621,1527
-447,-329,318
-430,-3130,366
-413,-627,1469
-345,-311,381
-36,-1284,1171
-27,-1108,-65
7,-33,-71
12,-2351,-103
26,-1119,1091
346,-2985,342
366,-3059,397
377,-2827,367
390,-675,-793
396,-1931,-563
404,-588,-901
408,-1815,803
423,-701,434
432,-2009,850
443,580,662
455,729,728
456,-540,1869
459,-707,401
465,-695,1988
474,580,667
496,-1584,1900
497,-1838,-617
527,-524,1933
528,-643,409
534,-1912,768
544,-627,-890
553,345,-567
564,392,-477
568,-2007,-577
605,-1665,1952
612,-1593,1893
630,319,-379
686,-3108,-505
776,-3184,-501
846,-3110,-434
1135,-1161,1235
1243,-1093,1063
1660,-552,429
1693,-557,386
1735,-437,1738
1749,-1800,1813
1772,-405,1572
1776,-675,371
1779,-442,1789
1780,-1548,337
1786,-1538,337
1847,-1591,415
1889,-1729,1762
1994,-1805,1792";
    }
}
